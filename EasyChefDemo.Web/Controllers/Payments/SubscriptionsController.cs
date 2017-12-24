using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//-----------------------------//

using AutoMapper;

using EasyChefDemo.Data.Infrastructure;
using EasyChefDemo.Data.Repositories;
using EasyChefDemo.Entities;
using EasyChefDemo.Services.Abstract;
using EasyChefDemo.Services.Utilities;
using EasyChefDemo.Web.Infrastructure.Core;
using EasyChefDemo.Web.Models;
using EasyChefDemo.Data.Extensions;
using net.authorize;
using EasyChefDemo.Web.Infrastructure.Extensions;
using Newtonsoft.Json;
using AuthorizeNet.Api.Contracts.V1;
namespace EasyChefDemo.Web.Controllers
{
    //  [Authorize(Roles = "Admin, SuperAdmin")]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Payments")]
    public class SubscriptionsController : ApiControllerBase
    {

        private readonly IEntityBaseRepository<Subscription> _subscriptionRepository;
        private readonly IEntityBaseRepository<SubscriptionPlan> _subscriptionPlanRepository;
        private readonly IEntityBaseRepository<SubscriptionInterval> _subscriptionIntervalRepository;


        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Restaurant> _restaurantRepository;

        private readonly IEntityBaseRepository<UserRestaurant> _userrestaurantRepository;

        public SubscriptionsController(
           IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
             IEntityBaseRepository<Subscription> subscriptionRepository,
             IEntityBaseRepository<SubscriptionPlan> subscriptionPlanRepository,
             IEntityBaseRepository<SubscriptionInterval> subscriptionIntervalRepository,
             IEntityBaseRepository<UserRestaurant> userrestaurantRepository,
            IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Restaurant> restaurantRepository

            )
            : base(_errorsRepository, _unitOfWork)
        {

            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _userrestaurantRepository = userrestaurantRepository;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _subscriptionIntervalRepository = subscriptionIntervalRepository;



        }

        //--------------------------------------------------------------------------------------------------
        // SUBSCRIPTION INTERVAL
        //--------------------------------------------------------------------------------------------------


        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, List<SubscriptionInterval> subscriptionIntervalList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    SubscriptionInterval newsubscriptionIntervalItem = new SubscriptionInterval();

                    if (subscriptionIntervalList != null)
                    {
                        foreach (var subscriptionInterval in subscriptionIntervalList)
                        {
                            //newcategorytem.UpdateCategoryItem(category);
                            //_categoriesRepository.Add(newcategorytem);
                            //_unitOfWork.Commit();
                        }
                    }

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }

                return response;
            });
        }

        [AllowAnonymous]
        [Route("details/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                SubscriptionViewModel subvm = new SubscriptionViewModel();
                if (!string.IsNullOrEmpty(filter))
                {
                    var existingUserDb = _userRepository.GetSingleByUsername(filter);

                    if (existingUserDb != null)
                    {
                        var userrestaurantDb = _userrestaurantRepository.GetAll().Where(userrest => userrest.UserId == existingUserDb.ID).ToList();
                        foreach (var userrestaurant in userrestaurantDb)
                        {
                            var subscriptiondb = _subscriptionRepository.GetAll().Where(sub => sub.RestaurantId == userrestaurant.RestaurantId).ToList();

                            foreach (var subcription in subscriptiondb)
                            {
                                subvm.ID = subcription.ID;
                                subvm.StartDate = subcription.StartDate;
                                subvm.EndDate = subcription.EndDate;
                                subvm.TrialStartDate = subcription.TrialStartDate;
                                subvm.TrialEndDate = subcription.TrialEndDate;
                                subvm.RestaurantId = subcription.RestaurantId;
                                subvm.SubscriptionPlanId = subcription.SubscriptionPlanId;
                                subvm.SubscriptionPlanVm = GetPlanNameByPlanId(subcription.SubscriptionPlanId,subcription.RestaurantId);
                                subvm.SubscriptionIntervalVm = GetAllPlans();


                            }

                        }

                        response = request.CreateResponse<SubscriptionViewModel>(HttpStatusCode.OK, subvm);
                        return response;
                    }
                    else
                    {
                        response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid User");
                    }
                }

                else
                {
                    response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid User");

                }



                return response;
            });
        }


        [AllowAnonymous]
        [Route("plans")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var plans = _subscriptionPlanRepository.GetAll().Where(r => r.Name != "Trail").ToList();

                IEnumerable<SubcriptionPlanViewModel> plansVM = Mapper.Map<IEnumerable<SubscriptionPlan>, IEnumerable<SubcriptionPlanViewModel>>(plans);
                response = request.CreateResponse<IEnumerable<SubcriptionPlanViewModel>>(HttpStatusCode.OK, plansVM);

                return response;
            });
        }

        public List<SubcriptionPlanViewModel> GetPlanNameByPlanId(int PlanID,int ResturantID)
        {
            List<SubcriptionPlanViewModel> listSubscription = new List<SubcriptionPlanViewModel>();
            var subscriptionplandetails = _subscriptionPlanRepository.GetAll().Where(r => (r.IntervalId == PlanID)).ToList();
            if (subscriptionplandetails.Count > 0)
            {
                foreach (var subscriptionplan in subscriptionplandetails)
                {
                    SubcriptionPlanViewModel listsubscriptionplan = new SubcriptionPlanViewModel();
                    listsubscriptionplan.ID = subscriptionplan.ID;
                    listsubscriptionplan.Name = subscriptionplan.Name;
                    listsubscriptionplan.Price = subscriptionplan.Price;
                    listsubscriptionplan.Currency = subscriptionplan.Currency;
                    listsubscriptionplan.TrialPeriodInDays = subscriptionplan.TrialPeriodInDays;

                    listSubscription.Add(listsubscriptionplan);
                }
            }

            return listSubscription;

        }

        public DateTime GetPlanIntervalEnddate(int PlanID)
        {
            var subscriptionplans = _subscriptionIntervalRepository.GetSingle(PlanID);
            DateTime dt = new DateTime();
            if (subscriptionplans != null)
            {

                dt = DateTime.UtcNow.AddDays(Convert.ToDouble(subscriptionplans.NumberofDays));
            }
            return dt;

        }

        public List<SubscriptionIntervalViewModel> GetAllPlans()
        {
            List<SubscriptionIntervalViewModel> lstSubcriptionplan = new List<SubscriptionIntervalViewModel>();
            var subscriptionplans = _subscriptionIntervalRepository.GetAll().ToList();
            if (subscriptionplans.Count > 0)
            {
                foreach (var subscriptionplan in subscriptionplans)
                {
                    SubscriptionIntervalViewModel lstSubplan = new SubscriptionIntervalViewModel();
                    lstSubplan.ID = subscriptionplan.ID;
                    lstSubplan.Name = subscriptionplan.Name;

                    lstSubcriptionplan.Add(lstSubplan);
                }
            }
            return lstSubcriptionplan;
        }

        [HttpPost]
        [Route("addPay")]
        public HttpResponseMessage Add(HttpRequestMessage request, PaymentViewModel paymentlist)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage Httpresponse = null;
                if (!string.IsNullOrEmpty(paymentlist.sloginUserInfo))
                {
                    var existingUserDb = _userRepository.GetSingleByUsername(paymentlist.sloginUserInfo);
                    if (existingUserDb != null)
                    {
                        var userrestaurantDb = _userrestaurantRepository.GetAll().Where(userrest => userrest.UserId == existingUserDb.ID).ToList();
                        foreach (var userrestaurant in userrestaurantDb)
                        {
                            var subscriptiondb = _subscriptionRepository.GetSingle(userrestaurant.RestaurantId);
                            if (paymentlist != null)
                            {
                                var response = (createTransactionResponse)ChargeCreditCard.Run(paymentlist);
                                //validate
                                if (response != null)
                                {
                                    if (response.messages.resultCode == messageTypeEnum.Ok)
                                    {
                                        if (response.transactionResponse.messages != null)
                                        {

                                            var subscriptionDb = _subscriptionRepository.GetSingle(subscriptiondb.ID);

                                            if (subscriptionDb == null)
                                                Httpresponse = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Subscription.");
                                            else
                                            {
                                                Subscription newsubscription = new Subscription();
                                                SubscriptionViewModel subscriptionvm = new SubscriptionViewModel();
                                                subscriptionvm.SubscriptionPlanId = Convert.ToInt32(paymentlist.iPlanID.ToString());
                                                subscriptionvm.StartDate = DateTime.UtcNow;
                                                subscriptionvm.EndDate = GetPlanIntervalEnddate(paymentlist.iPlanID);
                                                subscriptionvm.TransId = response.transactionResponse.transId.ToString();
                                                subscriptionDb.UpdateSubscription(subscriptionvm);

                                                _subscriptionRepository.Edit(subscriptionDb);
                                                _unitOfWork.Commit();
                                                Httpresponse = request.CreateResponse(HttpStatusCode.OK, response.transactionResponse);
                                            }

                                        }
                                        else
                                        {
                                            if (response.transactionResponse.errors != null)
                                            {
                                                Httpresponse = request.CreateResponse(HttpStatusCode.BadRequest, response.transactionResponse.errors);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (response.transactionResponse != null && response.transactionResponse.errors != null)
                                        {
                                            Httpresponse = request.CreateResponse(HttpStatusCode.BadRequest, response.transactionResponse.errors);
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                    Httpresponse = request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Payment");
                                }
                            }
                        }
                    }
                }

                return Httpresponse;
            });
        }
        //--------------------------------------------------------------------------------------------------
        // SUBSCRIPTION INTERVAL     - END
        //--------------------------------------------------------------------------------------------------






    }
}
