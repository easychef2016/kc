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
    [RoutePrefix("api/Users")]
    public class UsersController : ApiControllerBase
    {

        private readonly IMembershipService _membershipService;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Restaurant> _restaurant;
        private readonly IEntityBaseRepository<Address> _address;
        private readonly IEntityBaseRepository<RestaurantAddress> _restaurantaddress;
        private readonly IEntityBaseRepository<UserRestaurant> _userrestaurant;
        private readonly IEntityBaseRepository<Contacts> _usercontacts;


        public UsersController(IMembershipService membershipService,
           IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Restaurant> restaurant, IEntityBaseRepository<Address> address,
             IEntityBaseRepository<RestaurantAddress> restaurantaddress,
            IEntityBaseRepository<UserRestaurant> userrestaurant,
            IEntityBaseRepository<Contacts> usercontacts
            )
            : base(_errorsRepository, _unitOfWork)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _restaurant = restaurant;
            _address = address;
            _restaurantaddress = restaurantaddress;
            _userrestaurant = userrestaurant;
            _usercontacts = usercontacts;
        }



        [AllowAnonymous]
        [Route("latest/{filter?}")]
        [CacheFilter(TimeDuration = 100)]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;



                if (!string.IsNullOrEmpty(filter))
                {
                    var existingUserDb = _userRepository.GetSingleByUsername(filter);


                    var userrestaurantDb = _userRepository.GetAll().ToList();

                    IEnumerable<UserViewModel> uservm = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(userrestaurantDb);

                    response = request.CreateResponse<IEnumerable<UserViewModel>>(HttpStatusCode.OK, uservm);
                    return response;

                }

                else
                {
                    response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid User");

                }



                return response;
            });
        }

        [AllowAnonymous]
        [Route("add")]
        public HttpResponseMessage Register(HttpRequestMessage request, UserRestaurantViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.UserDetails.Username))
                    {
                        var existingUserDb = _userRepository.GetSingleByUsername(user.UserDetails.Username);
                        if (existingUserDb != null)// && existingUserDb.RestaurantId == restaurantdetail.ID)
                        {
                            ModelState.AddModelError("Invalid User", "User name already exists");
                            response = request.CreateResponse(HttpStatusCode.BadRequest,
                            ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                  .Select(m => m.ErrorMessage).ToArray());
                        }
                        else
                        {
                            Entities.User _user = _membershipService.CreateUser(user.UserDetails.Username, user.UserDetails.Username, user.UserDetails.Password, new int[] { 1 });
                            UserRestaurant newuserestaurant = new UserRestaurant();
                            newuserestaurant.RestaurantId = user.ID;
                            newuserestaurant.UserId = _user.ID;
                            _userrestaurant.Add(newuserestaurant);
                            _unitOfWork.Commit();
                            response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                        }

                    }
                }
                return response;

            });
        }


        [AllowAnonymous]
        [Route("details/{filter?}")]
        //[CacheFilter(TimeDuration = 100)]
        public HttpResponseMessage GetUsers(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<UserViewModel> usrvm = new List<UserViewModel>();

                if (!string.IsNullOrEmpty(filter))
                {
                    var existingUserDb = _userRepository.GetSingleByUsername(filter);
                    if (existingUserDb != null)
                    {
                        var userrestaurantDb = _userrestaurant.GetAll().Where(userrest => userrest.UserId == existingUserDb.ID).ToList();
                        foreach (var userrestaurant in userrestaurantDb)
                        {
                            // var restaurantDb = _restaurantRepository.AllIncluding().Where(r => r.UserRestaurants.Any(ur => ur.User.ID == existingUserDb.ID)).AsQueryable();
                            var userDb = _userrestaurant.GetAll().Where(usr => usr.RestaurantId == userrestaurant.RestaurantId);

                            if (userDb.Count() > 0)
                            {
                                foreach (var us in userDb)
                                {
                                    var users = _userRepository.GetAll().Where(u => u.ID == us.UserId)
                                       .OrderByDescending(it => it.DateCreated).ToList();
                                    foreach (var u in users)
                                    {
                                        UserViewModel uv = new UserViewModel();
                                        uv.Username = u.Username;
                                        uv.IsLocked = u.IsLocked;
                                        if (uv.Username != filter)
                                            usrvm.Add(uv);
                                    }
                                    //IEnumerable<UserViewModel> userVM = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users);
                                    response = request.CreateResponse<List<UserViewModel>>(HttpStatusCode.OK, usrvm);
                                }
                            }
                        }

                        //  response = request.CreateResponse<RestaurantUserAddressViewModel>(HttpStatusCode.OK, usrvm);
                        return response;
                    }
                    else
                    {
                        response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid User");
                    }

                    //var userrestaurantDb = _userRepository.GetAll().ToList();


                    return response;

                }

                else
                {
                    response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid User");

                }



                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, UserViewModel uservm)
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
                    try
                    {
                        Entities.User _user = _membershipService.UpdateUserEdit(uservm.Username, !uservm.IsLocked);

                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
                return response;
            });
        }



    }



    [Authorize(Roles = "Admin")]
    [RoutePrefix("mobileapi/Users")]
    public class MobileUsersController : ApiControllerBase
    {

        private readonly IMembershipService _membershipService;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Restaurant> _restaurant;
        private readonly IEntityBaseRepository<Address> _address;
        private readonly IEntityBaseRepository<RestaurantAddress> _restaurantaddress;
        private readonly IEntityBaseRepository<UserRestaurant> _userrestaurant;
        private readonly IEntityBaseRepository<Contacts> _usercontacts;


        public MobileUsersController(IMembershipService membershipService,
           IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Restaurant> restaurant, IEntityBaseRepository<Address> address,
             IEntityBaseRepository<RestaurantAddress> restaurantaddress,
            IEntityBaseRepository<UserRestaurant> userrestaurant,
            IEntityBaseRepository<Contacts> usercontacts
            )
            : base(_errorsRepository, _unitOfWork)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _restaurant = restaurant;
            _address = address;
            _restaurantaddress = restaurantaddress;
            _userrestaurant = userrestaurant;
            _usercontacts = usercontacts;
        }



        [AllowAnonymous]
        [Route("latest/{filter?}")]
        [CacheFilter(TimeDuration = 100)]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;



                if (!string.IsNullOrEmpty(filter))
                {
                    var existingUserDb = _userRepository.GetSingleByUsername(filter);


                    var userrestaurantDb = _userRepository.GetAll().ToList();

                    IEnumerable<UserViewModel> uservm = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(userrestaurantDb);

                    response = request.CreateResponse<IEnumerable<UserViewModel>>(HttpStatusCode.OK, uservm);
                    return response;

                }

                else
                {
                    response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid User");

                }



                return response;
            });
        }

    }
}
