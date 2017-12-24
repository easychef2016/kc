using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using EasyChefDemo.Entities;
using EasyChefDemo.Data.Configurations;

namespace EasyChefDemo.Data
{


    public class EasyChefDemoContext : DbContext
    {
        public EasyChefDemoContext()
            : base("EasyChefDemo")
        {
            Database.SetInitializer<EasyChefDemoContext>(null);
        }


        #region Entity Sets
        public IDbSet<User> UserSet { get; set; }
        public IDbSet<Role> RoleSet { get; set; }
        public IDbSet<UserRole> UserRoleSet { get; set; }
        public IDbSet<Restaurant> RestaurantSet { get; set; }
        public IDbSet<Address> AddressSet { get; set; }
        public IDbSet<RestaurantAddress> RestaurantAddressSet { get; set; }
       
        public IDbSet<Category> CategorySet { get; set; }

        public IDbSet<UserRestaurant> UserRestaurantSet { get; set; }
        public IDbSet<Vendor> VendorSet { get; set; }
        public IDbSet<VendorAddress> VendorAddressSet { get; set; }
        public IDbSet<Error> ErrorSet { get; set; }
        public IDbSet<IngredientType> IngredientTypeSet { get; set; }

        public IDbSet<Unit> UnitSet { get; set; }
        public IDbSet<Location> LocationSet { get; set; }
        public IDbSet<InventoryItem> InventoryItemSet { get; set; }
        public IDbSet<InventoryItemLocation> InventoryItemLocationSet { get; set; }
        public IDbSet<InventoryItemRestaurant> InventoryItemRestaurantSet { get; set; }

        public IDbSet<Recipe> RecipeSet { get; set; }
        public IDbSet<RecipeIngredient> RecipeIngredientSet { get; set; }
        public IDbSet<Inventory> InventorySet { get; set; }
        public IDbSet<ApprovedInventory> ApprovedInventorySet { get; set; }
       
        //Added on 08//08/2016
       public IDbSet<StatusMaster> StatusMasterSet { get; set; }
    
        

        
        #endregion


        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new UserRestaurantConfiguration ());
            

            modelBuilder.Configurations.Add(new RestaurantAddressConfiguration());
            modelBuilder.Configurations.Add(new RestaurantConfiguration());

            modelBuilder.Configurations.Add(new UnitConfiguration());
            modelBuilder.Configurations.Add(new VendorAddressConfiguration());
            modelBuilder.Configurations.Add(new VendorConfiguration());
            modelBuilder.Configurations.Add(new LocationConfiguration());

            modelBuilder.Configurations.Add(new IngredientTypeConfiguration());
            modelBuilder.Configurations.Add(new InventoryItemConfiguration());
            modelBuilder.Configurations.Add(new RecipeConfiguration());
            modelBuilder.Configurations.Add(new RecipeIngredientConfiguration());

            modelBuilder.Configurations.Add(new InventoryItemLocationConfiguration());
            modelBuilder.Configurations.Add(new InventoryItemRestaurantConfiguration());

            modelBuilder.Configurations.Add(new InventoryConfiguration());
            modelBuilder.Configurations.Add(new ApprovedInventoryConfiguration());
            modelBuilder.Configurations.Add(new StatusMasterConfiguration());

            
            //Added on 01/11/2017

            modelBuilder.Configurations.Add(new SubscriptionIntervalConfiguration());
            modelBuilder.Configurations.Add(new SubscriptionPlanConfiguration());
            modelBuilder.Configurations.Add(new SubscriptionConfiguration());

            
           
        }


     }
}
