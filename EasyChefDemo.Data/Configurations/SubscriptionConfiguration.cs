using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data.Entity.ModelConfiguration;
using EasyChefDemo.Entities;


namespace EasyChefDemo.Data.Configurations
{
    public class SubscriptionConfiguration : EntityBaseConfiguration<Subscription>
    {

        public   SubscriptionConfiguration()
        {

            Property(subscription => subscription.StartDate).IsOptional();
            Property(subscription => subscription.EndDate).IsOptional();

            Property(subscription => subscription.TrialStartDate).IsOptional();
            Property(subscription => subscription.TrialEndDate).IsOptional();

            //Property(subscription => subscription.SubscriptionPlanId).IsOptional();
            HasRequired(subscription => subscription.SubscriptionPlan).WithMany(subcriptionplan => subcriptionplan.Subscriptions).HasForeignKey(subscription => subscription.SubscriptionPlanId);
            
            
            Property(subscription => subscription.RestaurantId).IsOptional();


            Property(subscription => subscription.Status).IsOptional();
            Property(subscription => subscription.CreatedBy).IsOptional().HasMaxLength(50);
            Property(subscription => subscription.CreatedDate).IsOptional();
            Property(subscription => subscription.UpdatedBy).IsOptional().HasMaxLength(50);
            Property(subscription => subscription.UpdatedDate).IsOptional();
        
        }
    }
}
