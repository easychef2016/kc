using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EasyChefDemo.Web.Infrastructure.Validators;

namespace EasyChefDemo.Web.Models
{
    public class UserViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public int RestaurantID { get; set; }

        public bool IsLocked { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new UserViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}