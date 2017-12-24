using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyChefDemo.Entities
{
    public class SubscriptionPlan : IEntityBase, IAuditable
    {

        public SubscriptionPlan()
        {
            Subscriptions = new List<Subscription>();
            
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public double ? Price { get; set; }
        public string Currency { get; set; }


        public int IntervalId { get; set; }
        public virtual SubscriptionInterval Interval { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public bool Disabled { get; set; }  
        public int TrialPeriodInDays { get; set; }


        public bool Status { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
    }
}
