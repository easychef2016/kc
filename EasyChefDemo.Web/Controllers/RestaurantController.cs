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
    [RoutePrefix("api/restaurant")]
    public class RestaurantController : ApiControllerBase
    {

        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Restaurant> _restaurantRepository;
        private readonly IEntityBaseRepository<UserRestaurant> _userrestaurantRepository;
        private readonly IEntityBaseRepository<RestaurantAddress> _restaurantAddressRepository;
        private readonly IEntityBaseRepository<Address> _addressRepository;


        public RestaurantController(IEntityBaseRepository<User> userRepository,
                IEntityBaseRepository<Restaurant> restaurantRepository,
                IEntityBaseRepository<UserRestaurant> userrestaurantRepository,
                IEntityBaseRepository<RestaurantAddress> restaurantAddressRepository,
                IEntityBaseRepository<Address> addressRepository,
                IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork


            )
            : base(_errorsRepository, _unitOfWork)
        {

            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _userrestaurantRepository = userrestaurantRepository;
            _restaurantAddressRepository = restaurantAddressRepository;
            _addressRepository = addressRepository;
        }


        [Route("details/{filter?}")]
        [CacheFilter(TimeDuration = 100)]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                RestaurantUserAddressViewModel restusrvm = new RestaurantUserAddressViewModel();
                if (!string.IsNullOrEmpty(filter))
                {
                    var existingUserDb = _userRepository.GetSingleByUsername(filter);

                    if (existingUserDb != null)
                    {
                        var userrestaurantDb = _userrestaurantRepository.GetAll().Where(userrest => userrest.UserId == existingUserDb.ID).ToList();
                        foreach (var userrestaurant in userrestaurantDb)
                        {
                            // var restaurantDb = _restaurantRepository.AllIncluding().Where(r => r.UserRestaurants.Any(ur => ur.User.ID == existingUserDb.ID)).AsQueryable();
                            var restaurantDb = _restaurantRepository.GetSingle(userrestaurant.RestaurantId);

                            restusrvm.ID = restaurantDb.ID;
                            restusrvm.Name = restaurantDb.Name.ToString();
                            restusrvm.Description = restaurantDb.Description.ToString();
                            restusrvm.AutoOrder = restaurantDb.AutoOrder;
                            restusrvm.GlobalBar = restaurantDb.GlobalBar;
                            restusrvm.RestaurantAddressVM = GetRestuarantDetail(restusrvm.ID);
                            restusrvm.RestaurantUserVM = GetUserDetail(userrestaurant.UserId);

                        }

                        response = request.CreateResponse<RestaurantUserAddressViewModel>(HttpStatusCode.OK, restusrvm);
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

        private List<AddressViewModel> GetRestuarantDetail(int restuarantId)
        {
            List<AddressViewModel> listRestaurantAddress = new List<AddressViewModel>();

            var restaurantAddressdb = _restaurantAddressRepository.GetAll().Where(r => r.Restaurant.ID == restuarantId).ToList();

            if (restaurantAddressdb.Count > 0)
            {

                foreach (var restaurantAddress in restaurantAddressdb)
                {
                    var addressDb = _addressRepository.GetSingle(restaurantAddress.AddressId);
                    AddressViewModel listaddressDb = new AddressViewModel()
                    {
                        ID = addressDb.ID,
                        Email = addressDb.Email,
                        AddressDetails = addressDb.AddressDetails,
                        StreetName = addressDb.StreetName,
                        City = addressDb.City,
                        State = addressDb.State,
                        Zip = addressDb.Zip,
                        Country = addressDb.Country,
                        PrimaryPhone = addressDb.PrimaryPhone,
                        CreatedDate = addressDb.CreatedDate.HasValue ? addressDb.CreatedDate : null


                    };


                    listRestaurantAddress.Add(listaddressDb);
                }

                //  listRestaurantAddress.Sort((r1, r2) => r2.CreatedDate.CompareTo(r1.CreatedDate));
            }

            return listRestaurantAddress;
        }

        private UserViewModel GetUserDetail(int userId)
        {
            // List<UserViewModel> lstUser = new List<UserViewModel>();

            var userdb = _userRepository.GetAll().Where(r => r.ID == userId).ToList();
            UserViewModel uvm = new UserViewModel();
            if (userdb.Count > 0)
            {

                foreach (var user in userdb)
                {
                    uvm.Username = user.Username;
                    uvm.Email = user.Email;
                    uvm.ID = user.ID;
                }
            }

            return uvm;
        }


        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, RestaurantUserAddressViewModel restaurantuseraddressvm)
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
                    Restaurant newRestaurant = new Restaurant();
                    Address newaddress = new Address();
                    newRestaurant.UpdateRestaurantEdit(restaurantuseraddressvm, restaurantuseraddressvm.RestaurantUserVM.ID);
                    _restaurantRepository.Edit(newRestaurant);
                    _unitOfWork.Commit();
                    foreach (var restaurantAddress in restaurantuseraddressvm.RestaurantAddressVM)
                    {
                        newaddress.UpdateAddressEdit(restaurantAddress, restaurantuseraddressvm.RestaurantUserVM.Email, restaurantuseraddressvm.RestaurantUserVM.ID);
                        _addressRepository.Edit(newaddress);
                        _unitOfWork.Commit();
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });

                }

                return response;
            });
        }


    }


    [Authorize(Roles = "Admin")]
    [RoutePrefix("mobileapi/restaurant")]
    public class MobileRestaurantController : ApiControllerBase
    {

        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Restaurant> _restaurantRepository;
        private readonly IEntityBaseRepository<UserRestaurant> _userrestaurantRepository;
        private readonly IEntityBaseRepository<RestaurantAddress> _restaurantAddressRepository;
        private readonly IEntityBaseRepository<Address> _addressRepository;


        public MobileRestaurantController(IEntityBaseRepository<User> userRepository,
                IEntityBaseRepository<Restaurant> restaurantRepository,
                IEntityBaseRepository<UserRestaurant> userrestaurantRepository,
                IEntityBaseRepository<RestaurantAddress> restaurantAddressRepository,
                IEntityBaseRepository<Address> addressRepository,
                IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork


            )
            : base(_errorsRepository, _unitOfWork)
        {

            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _userrestaurantRepository = userrestaurantRepository;
            _restaurantAddressRepository = restaurantAddressRepository;
            _addressRepository = addressRepository;
        }


        [AllowAnonymous]
        [Route("details/{filter?}")]
        [CacheFilter(TimeDuration = 100)]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                RestaurantUserAddressViewModel restusrvm = new RestaurantUserAddressViewModel();
                if (!string.IsNullOrEmpty(filter))
                {
                    var existingUserDb = _userRepository.GetSingleByUsername(filter);

                    if (existingUserDb != null)
                    {
                        var userrestaurantDb = _userrestaurantRepository.GetAll().Where(userrest => userrest.UserId == existingUserDb.ID).ToList();
                        foreach (var userrestaurant in userrestaurantDb)
                        {
                            // var restaurantDb = _restaurantRepository.AllIncluding().Where(r => r.UserRestaurants.Any(ur => ur.User.ID == existingUserDb.ID)).AsQueryable();
                            var restaurantDb = _restaurantRepository.GetSingle(userrestaurant.RestaurantId);

                            restusrvm.ID = restaurantDb.ID;
                            restusrvm.Name = restaurantDb.Name.ToString();
                            restusrvm.Description = restaurantDb.Description.ToString();
                            restusrvm.AutoOrder = restaurantDb.AutoOrder;
                            restusrvm.GlobalBar = restaurantDb.GlobalBar;
                            restusrvm.RestaurantAddressVM = GetRestuarantDetail(restusrvm.ID);

                        }

                        response = request.CreateResponse<RestaurantUserAddressViewModel>(HttpStatusCode.OK, restusrvm);
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

        private List<AddressViewModel> GetRestuarantDetail(int restuarantId)
        {
            List<AddressViewModel> listRestaurantAddress = new List<AddressViewModel>();

            var restaurantAddressdb = _restaurantAddressRepository.GetAll().Where(r => r.Restaurant.ID == restuarantId).ToList();

            if (restaurantAddressdb.Count > 0)
            {

                foreach (var restaurantAddress in restaurantAddressdb)
                {
                    var addressDb = _addressRepository.GetSingle(restaurantAddress.AddressId);
                    AddressViewModel listaddressDb = new AddressViewModel()
                    {

                        Email = addressDb.Email,
                        AddressDetails = addressDb.AddressDetails,
                        StreetName = addressDb.StreetName,
                        City = addressDb.City,
                        State = addressDb.State,
                        Zip = addressDb.Zip,
                        Country = addressDb.Country,
                        PrimaryPhone = addressDb.PrimaryPhone,
                        CreatedDate = addressDb.CreatedDate.HasValue ? addressDb.CreatedDate : null


                    };


                    listRestaurantAddress.Add(listaddressDb);
                }

                //  listRestaurantAddress.Sort((r1, r2) => r2.CreatedDate.CompareTo(r1.CreatedDate));
            }

            return listRestaurantAddress;
        }


    }
}
