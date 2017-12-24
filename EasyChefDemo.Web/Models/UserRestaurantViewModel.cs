using EasyChefDemo.Web.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyChefDemo.Web.Models
{
    public class UserRestaurantViewModel : IValidatableObject
    {
        public int ID { get; set; }

        public int RestaurantId { get; set; }
        public int UserId { get; set; }

        public  UserViewModel UserDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new UserRestaurantViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }

    }
}