using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyChefDemo.Entities
{
    public class SubscriptionInterval : IEntityBase, IAuditable
    {
        public SubscriptionInterval()
        {
            SubscriptionPlans = new List<SubscriptionPlan>();
        }


        public int ID { get; set; }

        public string Name { get; set; }
        public int ? NumberofDays { get; set; }
        public virtual ICollection<SubscriptionPlan> SubscriptionPlans { get; set; }
        public bool Status { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }

       // public int? RestaurantId { get; set; }

    }
}
