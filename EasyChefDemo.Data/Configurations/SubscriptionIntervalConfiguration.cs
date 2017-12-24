using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.ModelConfiguration;
using EasyChefDemo.Entities;

namespace EasyChefDemo.Data.Configurations
{
    public class SubscriptionIntervalConfiguration : EntityBaseConfiguration<SubscriptionInterval>
    {

        public SubscriptionIntervalConfiguration()
        {
            Property(subscriptioninterval => subscriptioninterval.Name).IsRequired().HasMaxLength(50);
            Property(subscriptioninterval => subscriptioninterval.NumberofDays).IsOptional();

            Property(subscriptioninterval => subscriptioninterval.Status).IsOptional();
            Property(subscriptioninterval => subscriptioninterval.CreatedBy).IsOptional().HasMaxLength(50);
            Property(subscriptioninterval => subscriptioninterval.CreatedDate).IsOptional();
            Property(subscriptioninterval => subscriptioninterval.UpdatedBy).IsOptional().HasMaxLength(50);
            Property(subscriptioninterval => subscriptioninterval.UpdatedDate).IsOptional();


            HasMany(subscriptioninterval => subscriptioninterval.SubscriptionPlans).WithOptional().HasForeignKey(subscriptionplans => subscriptionplans.IntervalId);
        
        }
    }
}
