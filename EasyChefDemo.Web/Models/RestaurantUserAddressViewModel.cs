using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

using EasyChefDemo.Web.Infrastructure.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EasyChefDemo.Web.Models
{
    [Bind(Exclude = "Image")]
    public class RestaurantUserAddressViewModel : IValidatableObject
    {
        public RestaurantUserAddressViewModel()
        {
            RestaurantAddressVM = new List<AddressViewModel>();
            // RestaurantUserVM = new List<UserViewModel>();
            RestaurantContactsVM = new List<ContactsViewModel>();
            SubcriptionVM = new List<SubcriptionPlanViewModel>();

        }


        public int ID { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public bool GlobalBar { get; set; }
        public bool AutoOrder { get; set; }
        public bool NoQty { get; set; }


        public bool Status { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }

        public IList<AddressViewModel> RestaurantAddressVM { get; set; }
        public UserViewModel RestaurantUserVM { get; set; }
        //public IList<UserViewModel> RestaurantUserVM { get; set; }
        // public IList<RestaurantAddressViewModel> RestaurantAddressList { get; set; }

        public IList<SubcriptionPlanViewModel> SubcriptionVM { get; set; }


        //Added on 08302016
        public IList<ContactsViewModel> RestaurantContactsVM { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new RestaurantUserAddressViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }

    }
}