using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EasyChefDemo.Entities;
using EasyChefDemo.Web.Models;

namespace EasyChefDemo.Web.Mappings
{

    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Recipe, RecipiesViewModel>()
                .ForMember(vm => vm.Restaurant, map => map.MapFrom(r => r.Restaurant.Name))
                .ForMember(vm => vm.NumberOfRecipeIngrdients, map => map.MapFrom(r => r.RecipeIngredients.Count))
                .ForMember(vm => vm.RecipeIngredients, map => map.MapFrom(r => r.RecipeIngredients))
                .ForMember(vm => vm.InventoryItems, opt => opt.MapFrom(x => x.RecipeIngredients.Select(y => y.InventoryItem).ToList()))
                .ForMember(vm => vm.Image, map => map.MapFrom(r => string.IsNullOrEmpty(r.Image) == true ? "unknown.jpg" : r.Image));



            Mapper.CreateMap<InventoryItem, InventoryItemViewModel>()
                .ForMember(vm => vm.Category, map => map.MapFrom(it => it.Category.Name))
                .ForMember(vm => vm.Vendor, map => map.MapFrom(it => it.Vendor.Name))
                .ForMember(vm => vm.IngredientType, map => map.MapFrom(it => it.IngredientType.Name))
                .ForMember(vm => vm.Unit, map => map.MapFrom(it => it.Unit.Name))
                .ForMember(vm => vm.Image, map => map.MapFrom(it => string.IsNullOrEmpty(it.Image) == true ? "unknown.jpg" : it.Image));



            Mapper.CreateMap<RecipeIngredient, RecipeIngredientViewModel>();
            // .ForMember(vm => vm.InventoryItems, map => map.MapFrom(it => it.InventoryItem));


            Mapper.CreateMap<Category, CategoryViewModel>()
                .ForMember(vm => vm.NumberOfInventoryItems, map => map.MapFrom(g => g.IneventoryItems.Count()));


            //Vendor Details
            Mapper.CreateMap<Vendor, VendorViewModel>()
                  .ForMember(vm => vm.ID, map => map.MapFrom(vr => vr.ID))
                  .ForMember(vm => vm.Name, map => map.MapFrom(vr => vr.Name))
                  .ForMember(vm => vm.Description, map => map.MapFrom(vr => vr.Description))
                  .ForMember(vm => vm.SalesAgent, map => map.MapFrom(vr => vr.SalesAgent))
                  .ForMember(vm => vm.Status, map => map.MapFrom(vr => vr.Status))
                  .ForMember(vm => vm.CreatedBy, map => map.MapFrom(vr => vr.CreatedBy))
                  .ForMember(vm => vm.CreatedDate, map => map.MapFrom(vr => vr.CreatedDate))

                  .ForMember(vm => vm.VendorAddresses, opt => opt.MapFrom(x => x.VendorAddresses.Select(y => y.Address).ToList()))
                  .ForMember(vm => vm.VendorContacts, map => map.MapFrom(vr => vr.VendorContacts))

                 .ForMember(vm => vm.Image, map => map.MapFrom(vr => string.IsNullOrEmpty(vr.Image) == true ? "unknown.jpg" : vr.Image));




            Mapper.CreateMap<Vendor, VendorUserViewModel>()
                  .ForMember(vm => vm.ID, map => map.MapFrom(vr => vr.ID))
                  .ForMember(vm => vm.Name, map => map.MapFrom(vr => vr.Name))
                  .ForMember(vm => vm.Description, map => map.MapFrom(vr => vr.Description))
                  .ForMember(vm => vm.SalesAgent, map => map.MapFrom(vr => vr.SalesAgent))
                 .ForMember(vm => vm.Image, map => map.MapFrom(vr => string.IsNullOrEmpty(vr.Image) == true ? "unknown.jpg" : vr.Image));



            Mapper.CreateMap<Unit, UnitViewModel>();


            Mapper.CreateMap<Address, AddressViewModel>()
                 .ForMember(vm => vm.ID, map => map.MapFrom(vr => vr.ID))
                 .ForMember(vm => vm.Email, map => map.MapFrom(vr => vr.Email))
                 .ForMember(vm => vm.WebsiteURL, map => map.MapFrom(vr => vr.WebsiteURL))
                 .ForMember(vm => vm.PrimaryPhone, map => map.MapFrom(vr => vr.PrimaryPhone))
                 .ForMember(vm => vm.SecondaryPhone, map => map.MapFrom(vr => vr.SecondaryPhone))
                 .ForMember(vm => vm.Fax, map => map.MapFrom(vr => vr.Fax))

                 .ForMember(vm => vm.AddressDetails, map => map.MapFrom(vr => vr.AddressDetails))
                 .ForMember(vm => vm.StreetName, map => map.MapFrom(vr => vr.StreetName))
                 .ForMember(vm => vm.City, map => map.MapFrom(vr => vr.City))
                 .ForMember(vm => vm.State, map => map.MapFrom(vr => vr.State))
                 .ForMember(vm => vm.Zip, map => map.MapFrom(vr => vr.Zip))
                 .ForMember(vm => vm.Country, map => map.MapFrom(vr => vr.Country));

            Mapper.CreateMap<Contacts, ContactsViewModel>()
              .ForMember(vm => vm.ID, map => map.MapFrom(vr => vr.ID))
              .ForMember(vm => vm.Email, map => map.MapFrom(vr => vr.Email))
              .ForMember(vm => vm.CellPhone, map => map.MapFrom(vr => vr.CellPhone))
              .ForMember(vm => vm.WebsiteURL, map => map.MapFrom(vr => vr.WebsiteURL))
              .ForMember(vm => vm.Vendor, map => map.MapFrom(vr => vr.Vendor))
              .ForMember(vm => vm.VendorId, map => map.MapFrom(it => it.Vendor.ID))

              .ForMember(vm => vm.Restaurant, map => map.MapFrom(it => it.Restaurant.Name))
              .ForMember(vm => vm.RestaurantId, map => map.MapFrom(it => it.Restaurant.ID))

              .ForMember(vm => vm.User, map => map.MapFrom(it => it.User.Username))
              .ForMember(vm => vm.UserId, map => map.MapFrom(it => it.User.ID));





            Mapper.CreateMap<Inventory, InventoryViewModel>();
            //  .ForMember(InventoryVm => InventoryVm.SenttoVendor, map => map.MapFrom(inventory => inventory.SenttoVendor == 1 ? 1 :0));

            Mapper.CreateMap<ApprovedInventory, ApprovedInventoryViewModel>()
                   .ForMember(ai => ai.InventoryItems, map => map.MapFrom(it => it.InventoryItem))
                   .ForMember(ai => ai.InventoryItemName, map => map.MapFrom(it => it.InventoryItem.Name))
                   .ForMember(ai => ai.InventoryItemDescription, map => map.MapFrom(it => it.InventoryItem.ItemDescription))
                   .ForMember(ai => ai.Category, map => map.MapFrom(it => it.InventoryItem.Category.Name))
                   .ForMember(ai => ai.CategoryId, map => map.MapFrom(it => it.InventoryItem.Category.ID))

                   //.ForMember(ai => ai.Vendor, map => map.MapFrom(it => it.InventoryItem.Vendor.Name))
                   //.ForMember(ai => ai.VendorId, map => map.MapFrom(it => it.InventoryItem.Vendor.ID))

                   .ForMember(ai => ai.Vendor, map => map.MapFrom(it => it.Vendor.Name))
                   .ForMember(ai => ai.VendorId, map => map.MapFrom(it => it.Vendor.ID))
                   .ForMember(ai => ai.Vendordetails, map => map.MapFrom(it => it.Vendor))

                   .ForMember(ai => ai.InventoryItemUnit, map => map.MapFrom(it => it.InventoryItem.Unit.Name));

            Mapper.CreateMap<ApprovedInventory, InventoryInvoiceViewModel>()
                 .ForMember(vm => vm.ApprovedMinOrder, map => map.MapFrom(Ai => Ai.MinOrder))
                 .ForMember(vm => vm.ApprovedOrder, map => map.MapFrom(Ai => Ai.Order))
                 .ForMember(vm => vm.ApprovedParValue, map => map.MapFrom(Ai => Ai.ParValue))
                 .ForMember(vm => vm.ApprovedQuantity, map => map.MapFrom(Ai => Ai.Quantity))
                 .ForMember(vm => vm.ApprovedPrice, map => map.MapFrom(Ai => Ai.Price))
                 .ForMember(vm => vm.InventoryItemName, map => map.MapFrom(Ai => Ai.InventoryItem.Name))
                 .ForMember(vm => vm.InventoryItemDescription, map => map.MapFrom(Ai => Ai.InventoryItem.ItemDescription))

                  // //Commented on 09/21/2016: to display vendor information with multiple contacts
                  // .ForMember(vm => vm.Vendor, map => map.MapFrom(Ai => Ai.Vendor.Name))




                  .ForMember(vm => vm.InventoryItemUnit, map => map.MapFrom(Ai => Ai.InventoryItem.Unit.Name));

            Mapper.CreateMap<Inventory, InventorySheetViewModel>()
                 .ForMember(invsht => invsht.ApprovedInventories, map => map.MapFrom(ai => ai.ApprovedInventories))
                 .ForMember(invsht => invsht.Vendor, map => map.MapFrom(vendor => vendor.Vendor.Name))
                 .ForMember(invsht => invsht.VendorId, map => map.MapFrom(vendor => vendor.Vendor.ID))


                     .ForMember(invsht => invsht.Restaurant, map => map.MapFrom(restaurant => restaurant.Restaurant.Name))
                 .ForMember(invsht => invsht.RestaurantId, map => map.MapFrom(restaurant => restaurant.Restaurant.ID))
                 .ForMember(invsht => invsht.InventoryItems, opt => opt.MapFrom(x => x.ApprovedInventories.Select(y => y.InventoryItem).ToList()));

            Mapper.CreateMap<User, UserViewModel>()
              .ForMember(vm => vm.Username, map => map.MapFrom(user => user.Username))
              .ForMember(vm => vm.Email, map => map.MapFrom(user => user.Email))
              .ForMember(vm => vm.Password, map => map.MapFrom(user => user.HashedPassword))
            .ForMember(vm => vm.IsLocked, map => map.MapFrom(user => user.IsLocked));
            //.ForMember(vm => vm.RestaurantID, map => map.MapFrom(user => user.Restaurant_ID));


            Mapper.CreateMap<Restaurant, RestaurantViewModel>()
            .ForMember(vm => vm.Name, map => map.MapFrom(restaurant => restaurant.Name))
            .ForMember(vm => vm.Description, map => map.MapFrom(restaurant => restaurant.Description))
            .ForMember(vm => vm.GlobalBar, map => map.MapFrom(restaurant => restaurant.GlobalBar))
            .ForMember(vm => vm.AutoOrder, map => map.MapFrom(restaurant => restaurant.AutoOrder))
            .ForMember(vm => vm.NoQty, map => map.MapFrom(restaurant => restaurant.NoQty));


            Mapper.CreateMap<User, LoginResponseViewModel>()
         .ForMember(vm => vm.EmailId, map => map.MapFrom(user => user.Email))
         .ForMember(vm => vm.Username, map => map.MapFrom(user => user.Username));



            Mapper.CreateMap<Subscription, SubscriptionViewModel>()
                .ForMember(vm => vm.StartDate, map => map.MapFrom(subscription => subscription.StartDate))
                .ForMember(vm => vm.EndDate, map => map.MapFrom(subscription => subscription.EndDate))
                .ForMember(vm => vm.TrialStartDate, map => map.MapFrom(subscription => subscription.TrialStartDate))
                .ForMember(vm => vm.TrialEndDate, map => map.MapFrom(subscription => subscription.TrialEndDate))
                .ForMember(vm => vm.SubscriptionPlanId, map => map.MapFrom(subscription => subscription.SubscriptionPlanId))
                .ForMember(vm => vm.RestaurantId, map => map.MapFrom(subscription => subscription.RestaurantId));

            Mapper.CreateMap<SubscriptionInterval, SubscriptionIntervalViewModel>()
                .ForMember(vm => vm.ID, map => map.MapFrom(sub => sub.ID))
                .ForMember(vm => vm.Name, map => map.MapFrom(sub => sub.Name));


            Mapper.CreateMap<SubscriptionPlan, SubcriptionPlanViewModel>()
               .ForMember(vm => vm.ID, map => map.MapFrom(subplan => subplan.ID))
               .ForMember(vm => vm.Name, map => map.MapFrom(subplan => subplan.Name))
               .ForMember(vm => vm.Price, map => map.MapFrom(subplan => subplan.Price))
               .ForMember(vm => vm.Currency, map => map.MapFrom(subplan => subplan.Currency))
               .ForMember(vm => vm.IntervalId, map => map.MapFrom(subplan => subplan.IntervalId))
               .ForMember(vm => vm.TrialPeriodInDays, map => map.MapFrom(subplan => subplan.TrialPeriodInDays));

        }
    }
}