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
    public class RestaurantUserViewModel
    {
        //Restuarant Details
        public int ID { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public bool GlobalBar { get; set; }
        public bool AutoOrder { get; set; }
        public bool NoQty { get; set; }

        //Adddress Details

       // public string RestaurantEmail { get; set; }
        public string WebsiteURL { get; set; }
        public string PrimaryPhone { get; set; }
       // public string SecondaryPhone { get; set; }
       // public string Fax { get; set; }


        public string AddressDetails { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        // User Details
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new RestaurantUserViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }

    }
}