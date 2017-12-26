using AutoMapper;

using EasyChefDemo.Data.Infrastructure;
using EasyChefDemo.Data.Repositories;
using EasyChefDemo.Entities;
using EasyChefDemo.Services.Abstract;
using EasyChefDemo.Services.Utilities;
using EasyChefDemo.Web.Infrastructure.Core;
using EasyChefDemo.Web.Models;
using EasyChefDemo.Data.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EasyChefDemo.Web.Infrastructure.Extensions;

using EasyChefDemo.Web.Infrastructure.Filters;


namespace EasyChefDemo.Web.Controllers
{
    [DeflateCompression]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Restaurant> _restaurant;
        private readonly IEntityBaseRepository<Address> _address;
        private readonly IEntityBaseRepository<RestaurantAddress> _restaurantaddress;
        private readonly IEntityBaseRepository<UserRestaurant> _userrestaurant;
        private readonly IEntityBaseRepository<SubscriptionInterval> _subscriptionIntervalRepository;
        private readonly IEntityBaseRepository<Subscription> _subscriptionRepository;
        public AccountController(IMembershipService membershipService,
           IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Restaurant> restaurant, IEntityBaseRepository<Address> address,
             IEntityBaseRepository<RestaurantAddress> restaurantaddress,
            IEntityBaseRepository<UserRestaurant> userrestaurant,
             IEntityBaseRepository<SubscriptionInterval> subscriptionIntervalRepository,
             IEntityBaseRepository<Subscription> subscriptionRepository
            )
            : base(_errorsRepository, _unitOfWork)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _restaurant = restaurant;
            _address = address;
            _restaurantaddress = restaurantaddress;
            _userrestaurant = userrestaurant;
            _subscriptionIntervalRepository = subscriptionIntervalRepository;
            _subscriptionRepository = subscriptionRepository;
        }




        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]

        public HttpResponseMessage Login(HttpRequestMessage request, LoginViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    MembershipContext _userContext = _membershipService.ValidateUser(user.Username, user.Password);

                    if (_userContext.User != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                    }
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }


        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage Register(HttpRequestMessage request, RestaurantUserAddressViewModel restaurantuseraddressvm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                UserViewModel uservm = new UserViewModel();

                if (!ModelState.IsValid)
                {
                    // response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {

                    if (restaurantuseraddressvm.RestaurantUserVM != null)
                    {


                        var existingUser = _userRepository.GetSingleByUsername(restaurantuseraddressvm.RestaurantUserVM.Username);

                        if (existingUser != null)
                        {
                            //throw new Exception("Username is already in use");
                            // ModelState.AddModelError("Invalid user", "Email or User number already exists");
                            //response = request.CreateResponse(HttpStatusCode.BadRequest,
                            //ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                            //      .Select(m => m.ErrorMessage).ToArray());
                            response = request.CreateErrorResponse(HttpStatusCode.Ambiguous, "Email or User number already exists");


                        }
                        else
                        {
                            //Add Address details
                            Address newaddress = new Address();
                            Restaurant newRestaurant = new Restaurant();
                            RestaurantAddress newrestaurantAddress = new RestaurantAddress();
                            UserRestaurant newuserestaurant = new UserRestaurant();
                            //Note : -
                            //Username   --> Email
                            Entities.User _user = _membershipService.CreateUser(restaurantuseraddressvm.RestaurantUserVM.Username, restaurantuseraddressvm.RestaurantUserVM.Username, restaurantuseraddressvm.RestaurantUserVM.Password, new int[] { 1 });

                            newRestaurant.UpdateRestaurant(restaurantuseraddressvm, _user.ID);
                            _restaurant.Add(newRestaurant);
                            _unitOfWork.Commit();

                            newuserestaurant.RestaurantId = newRestaurant.ID;
                            newuserestaurant.UserId = _user.ID;

                            _userrestaurant.Add(newuserestaurant);
                            _unitOfWork.Commit();


                            //// Update view model
                            //customer = Mapper.Map<Customer, CustomerViewModel>(newCustomer);
                            //response = request.CreateResponse<CustomerViewModel>(HttpStatusCode.Created, customer);

                            foreach (var restaurantAddress in restaurantuseraddressvm.RestaurantAddressVM)
                            {
                                newaddress.UpdateAddress(restaurantAddress, restaurantuseraddressvm.RestaurantUserVM.Username, _user.ID);
                                _address.Add(newaddress);
                                _unitOfWork.Commit();

                                newrestaurantAddress.RestaurantId = newRestaurant.ID;
                                newrestaurantAddress.AddressId = newaddress.ID;

                                _restaurantaddress.Add(newrestaurantAddress);
                                _unitOfWork.Commit();

                                //int i = restaurantuseraddressvm.PlanID;
                                foreach (var resturantsubscription in restaurantuseraddressvm.SubcriptionVM)
                                {
                                    Subscription newsubscription = new Subscription();
                                    newsubscription.SubscriptionPlanId = resturantsubscription.ID;
                                    newsubscription.StartDate = DateTime.UtcNow;
                                    newsubscription.TrialStartDate = DateTime.UtcNow;
                                    newsubscription.EndDate = GetPlanIntervalEnddate(resturantsubscription.IntervalId);
                                    newsubscription.EndDate = GetPlanIntervalEnddate(resturantsubscription.IntervalId);
                                    newsubscription.RestaurantId = newRestaurant.ID;
                                    newsubscription.TransId = "";
                                    newsubscription.Status = true;
                                    _subscriptionRepository.Add(newsubscription);
                                    _unitOfWork.Commit();
                                }
                            }



                            if (_user != null)
                            {


                                uservm = Mapper.Map<User, UserViewModel>(_user);
                                response = request.CreateResponse<UserViewModel>(HttpStatusCode.OK, uservm);





                                // response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                            }
                            else
                            {
                                response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Registration failed. Try again.");
                            }

                        }





                    }


                    //restaurantuseraddressvm.RestaurantUserVM.Add(uservm);
                    // response = request.CreateResponse<RestaurantUserAddressViewModel>(HttpStatusCode.OK, restaurantuseraddressvm);



                    // response = request.CreateResponse(HttpStatusCode.OK, new { success = true });


                }

                return response;
            });
        }


        [AllowAnonymous]
        [Route("checkusername")]
        [HttpPost]
        public HttpResponseMessage Checkuser(HttpRequestMessage request, LoginViewModel loginuser)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {

                }
                else
                {
                    var existingUser = _userRepository.GetSingleByUsername(loginuser.Username);
                    if (existingUser != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                }

                return response;
            });
        }


        [AllowAnonymous]
        [Route("forgotpassword")]
        [HttpPost]
        public HttpResponseMessage ForgotPassword(HttpRequestMessage request, ForgotPasswordViewModel forgotpassworduser)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var existingUser = _userRepository.GetSingleByUsername(forgotpassworduser.Username);

                    if (existingUser != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });
                    }
                }
                else
                {

                    response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });

                }


                return response;
            });
        }

        [AllowAnonymous]
        [Route("resetpassword")]
        [HttpPost]
        public HttpResponseMessage ResetPassword(HttpRequestMessage request, ResetPasswordViewModel resetpassworduser)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var existingUser = _userRepository.GetSingleByUsername(resetpassworduser.Username);

                    if (existingUser != null)
                    {

                        Entities.User _user = _membershipService.UpdateUser(resetpassworduser.Username, resetpassworduser.Username, resetpassworduser.Password, new int[] { 1 });
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });
                    }
                }
                else
                {

                    response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });

                }


                return response;
            });
        }
        //[AllowAnonymous]
        //[Route("register")]
        //[HttpPost]
        //public HttpResponseMessage Register(HttpRequestMessage request, RestaurantUserViewModel restaurantuser)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;

        //        if (!ModelState.IsValid)
        //        {
        //            response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
        //        }
        //        else
        //        {

        //             var existingUser = _userRepository.GetSingleByUsername(restaurantuser.UserEmail);

        //            if (existingUser != null)
        //            {
        //                //throw new Exception("Username is already in use");

        //                ModelState.AddModelError("Invalid user", "Email or User number already exists");
        //                response = request.CreateResponse(HttpStatusCode.BadRequest,
        //                ModelState.Keys.SelectMany(k => ModelState[k].Errors)
        //                      .Select(m => m.ErrorMessage).ToArray());
        //            }

        //            else
        //            {
        //                Restaurant newRestaurant = new Restaurant();
        //                newRestaurant.UpdateRestaurant(restaurantuser);
        //                _restaurant.Add(newRestaurant);

        //                _unitOfWork.Commit();

        //                //// Update view model
        //                //customer = Mapper.Map<Customer, CustomerViewModel>(newCustomer);
        //                //response = request.CreateResponse<CustomerViewModel>(HttpStatusCode.Created, customer);

        //                //Add Address details

        //                Address newaddress = new Address();
        //                RestaurantAddress newrestaurantAddress = new RestaurantAddress();

        //                UserRestaurant newuserestaurant = new UserRestaurant();

        //                newaddress.UpdateAddress(restaurantuser);
        //                _address.Add(newaddress);
        //                _unitOfWork.Commit();

        //                newrestaurantAddress.RestaurantId = newRestaurant.ID;
        //                newrestaurantAddress.AddressId = newaddress.ID;

        //                _restaurantaddress.Add(newrestaurantAddress);
        //                _unitOfWork.Commit();

        //                Entities.User _user = _membershipService.CreateUser(restaurantuser.UserEmail, restaurantuser.UserEmail, restaurantuser.Password, new int[] { 1 });

        //                newuserestaurant.RestaurantId = newRestaurant.ID;
        //                newuserestaurant.UserId = _user.ID;

        //                _userrestaurant.Add(newuserestaurant);
        //                _unitOfWork.Commit();


        //                if (_user != null)
        //                {
        //                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
        //                }
        //                else
        //                {
        //                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
        //                }

        //            }




        //        }

        //        return response;
        //    });
        //}




        //public HttpResponseMessage Register(HttpRequestMessage request, RegistrationViewModel user)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;

        //        if (!ModelState.IsValid)
        //        {
        //            response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
        //        }
        //        else
        //        {
        //            Entities.User _user = _membershipService.CreateUser(user.Username, user.Email, user.Password, new int[] { 1 });

        //            if (_user != null)
        //            {
        //                response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
        //            }
        //            else
        //            {
        //                response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
        //            }
        //        }

        //        return response;
        //    });
        //}

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

    }



    [Authorize(Roles = "Admin")]
    [RoutePrefix("mobileapi/Account")]
    public class MobileAccountController : ApiControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Restaurant> _restaurant;
        private readonly IEntityBaseRepository<Address> _address;
        private readonly IEntityBaseRepository<RestaurantAddress> _restaurantaddress;
        private readonly IEntityBaseRepository<UserRestaurant> _userrestaurant;

        public MobileAccountController(IMembershipService membershipService,
           IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Restaurant> restaurant, IEntityBaseRepository<Address> address,
             IEntityBaseRepository<RestaurantAddress> restaurantaddress,
            IEntityBaseRepository<UserRestaurant> userrestaurant
            )
            : base(_errorsRepository, _unitOfWork)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _restaurant = restaurant;
            _address = address;
            _restaurantaddress = restaurantaddress;
            _userrestaurant = userrestaurant;
        }




        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, LoginViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    MembershipContext _userContext = _membershipService.ValidateUser(user.Username, user.Password);

                    if (_userContext.User != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                    }
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }



        [AllowAnonymous]
        [Route("authenticatemobile")]
        [HttpPost]

        public HttpResponseMessage LoginMobile(HttpRequestMessage request, LoginViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                LoginResponseViewModel loginResponseVM = new LoginResponseViewModel();

                if (ModelState.IsValid)
                {
                    MembershipContext _userContext = _membershipService.ValidateUser(user.Username, user.Password);

                    if (_userContext.User != null)
                    {
                        //  response = request.CreateResponse(HttpStatusCode.OK, new { success = true });

                        loginResponseVM = Mapper.Map<User, LoginResponseViewModel>(_userContext.User);
                        response = request.CreateResponse<LoginResponseViewModel>(HttpStatusCode.OK, loginResponseVM);
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.Unauthorized, new { success = false });
                    }
                }
                else
                    response = request.CreateResponse(HttpStatusCode.Unauthorized, new { success = false });

                return response;
            });
        }


        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage Register(HttpRequestMessage request, RestaurantUserAddressViewModel restaurantuseraddressvm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                UserViewModel uservm = new UserViewModel();

                if (!ModelState.IsValid)
                {
                    // response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {

                    if (restaurantuseraddressvm.RestaurantUserVM != null)
                    {


                        var existingUser = _userRepository.GetSingleByUsername(restaurantuseraddressvm.RestaurantUserVM.Username);

                        if (existingUser != null)
                        {
                            //throw new Exception("Username is already in use");
                            // ModelState.AddModelError("Invalid user", "Email or User number already exists");
                            //response = request.CreateResponse(HttpStatusCode.BadRequest,
                            //ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                            //      .Select(m => m.ErrorMessage).ToArray());
                            response = request.CreateErrorResponse(HttpStatusCode.Ambiguous, "Email or User number already exists");


                        }
                        else
                        {
                            //Add Address details
                            Address newaddress = new Address();
                            Restaurant newRestaurant = new Restaurant();
                            RestaurantAddress newrestaurantAddress = new RestaurantAddress();
                            UserRestaurant newuserestaurant = new UserRestaurant();



                            //Note : -
                            //Username   --> Email
                            Entities.User _user = _membershipService.CreateUser(restaurantuseraddressvm.RestaurantUserVM.Username, restaurantuseraddressvm.RestaurantUserVM.Username, restaurantuseraddressvm.RestaurantUserVM.Password, new int[] { 1 });

                            newRestaurant.UpdateRestaurant(restaurantuseraddressvm, _user.ID);
                            _restaurant.Add(newRestaurant);
                            _unitOfWork.Commit();

                            newuserestaurant.RestaurantId = newRestaurant.ID;
                            newuserestaurant.UserId = _user.ID;

                            _userrestaurant.Add(newuserestaurant);
                            _unitOfWork.Commit();


                            //// Update view model
                            //customer = Mapper.Map<Customer, CustomerViewModel>(newCustomer);
                            //response = request.CreateResponse<CustomerViewModel>(HttpStatusCode.Created, customer);

                            foreach (var restaurantAddress in restaurantuseraddressvm.RestaurantAddressVM)
                            {
                                newaddress.UpdateAddress(restaurantAddress, restaurantuseraddressvm.RestaurantUserVM.Username, _user.ID);
                                _address.Add(newaddress);
                                _unitOfWork.Commit();

                                newrestaurantAddress.RestaurantId = newRestaurant.ID;
                                newrestaurantAddress.AddressId = newaddress.ID;

                                _restaurantaddress.Add(newrestaurantAddress);
                                _unitOfWork.Commit();
                            }



                            if (_user != null)
                            {


                                uservm = Mapper.Map<User, UserViewModel>(_user);
                                response = request.CreateResponse<UserViewModel>(HttpStatusCode.OK, uservm);




                            }
                            else
                            {
                                response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Registration failed. Try again.");
                            }

                        }





                    }


                    //restaurantuseraddressvm.RestaurantUserVM.Add(uservm);
                    // response = request.CreateResponse<RestaurantUserAddressViewModel>(HttpStatusCode.OK, restaurantuseraddressvm);



                    // response = request.CreateResponse(HttpStatusCode.OK, new { success = true });


                }

                return response;
            });
        }




        [AllowAnonymous]
        [Route("checkusername")]
        [HttpPost]
        public HttpResponseMessage Checkuser(HttpRequestMessage request, LoginViewModel loginuser)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {

                }
                else
                {
                    var existingUser = _userRepository.GetSingleByUsername(loginuser.Username);
                    if (existingUser != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                }

                return response;
            });
        }


        [AllowAnonymous]
        [Route("forgotpassword")]
        [HttpPost]
        public HttpResponseMessage ForgotPassword(HttpRequestMessage request, ForgotPasswordViewModel forgotpassworduser)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var existingUser = _userRepository.GetSingleByUsername(forgotpassworduser.Username);

                    if (existingUser != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });
                    }
                }
                else
                {

                    response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });

                }


                return response;
            });
        }

        [AllowAnonymous]
        [Route("resetpassword")]
        [HttpPost]
        public HttpResponseMessage ResetPassword(HttpRequestMessage request, ResetPasswordViewModel resetpassworduser)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var existingUser = _userRepository.GetSingleByUsername(resetpassworduser.Username);

                    if (existingUser != null)
                    {

                        Entities.User _user = _membershipService.UpdateUser(resetpassworduser.Username, resetpassworduser.Username, resetpassworduser.Password, new int[] { 1 });
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });
                    }
                }
                else
                {

                    response = request.CreateResponse(HttpStatusCode.NotAcceptable, new { success = false });

                }


                return response;
            });
        }
        //[AllowAnonymous]
        //[Route("register")]
        //[HttpPost]
        //public HttpResponseMessage Register(HttpRequestMessage request, RestaurantUserViewModel restaurantuser)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;

        //        if (!ModelState.IsValid)
        //        {
        //            response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
        //        }
        //        else
        //        {

        //             var existingUser = _userRepository.GetSingleByUsername(restaurantuser.UserEmail);

        //            if (existingUser != null)
        //            {
        //                //throw new Exception("Username is already in use");

        //                ModelState.AddModelError("Invalid user", "Email or User number already exists");
        //                response = request.CreateResponse(HttpStatusCode.BadRequest,
        //                ModelState.Keys.SelectMany(k => ModelState[k].Errors)
        //                      .Select(m => m.ErrorMessage).ToArray());
        //            }

        //            else
        //            {
        //                Restaurant newRestaurant = new Restaurant();
        //                newRestaurant.UpdateRestaurant(restaurantuser);
        //                _restaurant.Add(newRestaurant);

        //                _unitOfWork.Commit();

        //                //// Update view model
        //                //customer = Mapper.Map<Customer, CustomerViewModel>(newCustomer);
        //                //response = request.CreateResponse<CustomerViewModel>(HttpStatusCode.Created, customer);

        //                //Add Address details

        //                Address newaddress = new Address();
        //                RestaurantAddress newrestaurantAddress = new RestaurantAddress();

        //                UserRestaurant newuserestaurant = new UserRestaurant();

        //                newaddress.UpdateAddress(restaurantuser);
        //                _address.Add(newaddress);
        //                _unitOfWork.Commit();

        //                newrestaurantAddress.RestaurantId = newRestaurant.ID;
        //                newrestaurantAddress.AddressId = newaddress.ID;

        //                _restaurantaddress.Add(newrestaurantAddress);
        //                _unitOfWork.Commit();

        //                Entities.User _user = _membershipService.CreateUser(restaurantuser.UserEmail, restaurantuser.UserEmail, restaurantuser.Password, new int[] { 1 });

        //                newuserestaurant.RestaurantId = newRestaurant.ID;
        //                newuserestaurant.UserId = _user.ID;

        //                _userrestaurant.Add(newuserestaurant);
        //                _unitOfWork.Commit();


        //                if (_user != null)
        //                {
        //                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
        //                }
        //                else
        //                {
        //                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
        //                }

        //            }




        //        }

        //        return response;
        //    });
        //}




        //public HttpResponseMessage Register(HttpRequestMessage request, RegistrationViewModel user)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;

        //        if (!ModelState.IsValid)
        //        {
        //            response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
        //        }
        //        else
        //        {
        //            Entities.User _user = _membershipService.CreateUser(user.Username, user.Email, user.Password, new int[] { 1 });

        //            if (_user != null)
        //            {
        //                response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
        //            }
        //            else
        //            {
        //                response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
        //            }
        //        }

        //        return response;
        //    });
        //}



    }
}
