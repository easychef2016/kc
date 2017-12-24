using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EasyChefDemo.Web.Infrastructure.Validators;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace EasyChefDemo.Web.Models
{
    public class SubscriptionViewModel 
    {
        public SubscriptionViewModel()
        {
            SubscriptionPlanVm = new List<SubcriptionPlanViewModel>();
            SubscriptionIntervalVm = new List<SubscriptionIntervalViewModel>();
            // RestaurantUserVM = new List<UserViewModel>();
            //subscriptionintervalvm = new List<SubscriptionIntervalViewModel>();

        }
        

        public int ID { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime? TrialStartDate { get; set; }

        public DateTime? TrialEndDate { get; set; }

        public int SubscriptionPlanId { get; set; }

        // public virtual SubcriptionPlanViewModel subscriptionPlanVm { get; set; }
        public IList<SubcriptionPlanViewModel> SubscriptionPlanVm { get; set; }
        public IList<SubscriptionIntervalViewModel> SubscriptionIntervalVm { get; set; }

        public int RestaurantId { get; set; }
        public string TransId { get; set; }
        public virtual RestaurantViewModel Restaurant { get; set; }


        public bool Status { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
    }
}