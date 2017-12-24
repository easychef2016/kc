using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyChefDemo.Web.Infrastructure.Validators;
using System.ComponentModel.DataAnnotations;

namespace EasyChefDemo.Web.Models
{
    public class SubscriptionIntervalViewModel
    {

        public int ID { get; set; }

        public string Name { get; set; }
        public int? NumberofDays { get; set; }
        public bool Status { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }

       // public int? RestaurantId { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var validator = new SubscriptionIntervalViewModelValidator();
        //    var result = validator.Validate(this);
        //    return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        //}

    }
}