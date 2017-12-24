using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyChefDemo.Web.Models
{
    public class SubcriptionPlanViewModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Currency { get; set; }

        public int IntervalId { get; set; }
        //public virtual SubscriptionIntervalViewModel Interval { get; set; }

        
        public bool Disabled { get; set; }
        public int TrialPeriodInDays { get; set; }


        public bool Status { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
    }
}