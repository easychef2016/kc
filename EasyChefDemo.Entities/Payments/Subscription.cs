using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyChefDemo.Entities
{
    public class Subscription : IEntityBase, IAuditable
    {
        public Subscription()
        {

        }

        public int ID { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime? TrialStartDate { get; set; }

        public DateTime? TrialEndDate { get; set; }

        public int SubscriptionPlanId { get; set; }

        public virtual SubscriptionPlan SubscriptionPlan { get; set; }

        public string TransId { get; set; }

        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }


        public bool Status { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
    }
}
