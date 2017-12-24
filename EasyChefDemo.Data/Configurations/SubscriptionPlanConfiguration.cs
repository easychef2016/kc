using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data.Entity.ModelConfiguration;
using EasyChefDemo.Entities;

namespace EasyChefDemo.Data.Configurations
{
    public class SubscriptionPlanConfiguration : EntityBaseConfiguration<SubscriptionPlan>
    {

        public  SubscriptionPlanConfiguration()
        {

            Property(subscriptionplan => subscriptionplan.Name).IsRequired();

            Property(subscriptionplan => subscriptionplan.Price).IsOptional();
            Property(subscriptionplan => subscriptionplan.Currency).IsOptional();
            Property(subscriptionplan => subscriptionplan.IntervalId).IsOptional();



            Property(subscriptionplan => subscriptionplan.Disabled).IsOptional();
            Property(subscriptionplan => subscriptionplan.TrialPeriodInDays).IsOptional();


            HasMany(subscriptionplan => subscriptionplan.Subscriptions).WithOptional().HasForeignKey(subscriptions => subscriptions.RestaurantId);






            Property(subscriptionplan => subscriptionplan.Status).IsOptional();
            Property(subscriptionplan => subscriptionplan.CreatedBy).IsOptional().HasMaxLength(50);
            Property(subscriptionplan => subscriptionplan.CreatedDate).IsOptional();
            Property(subscriptionplan => subscriptionplan.UpdatedBy).IsOptional().HasMaxLength(50);
            Property(subscriptionplan => subscriptionplan.UpdatedDate).IsOptional();

            HasMany(subscriptionplan => subscriptionplan.Subscriptions).WithRequired(subscriptions => subscriptions.SubscriptionPlan).HasForeignKey(subscriptions => subscriptions.SubscriptionPlanId);
        }
    }
}
