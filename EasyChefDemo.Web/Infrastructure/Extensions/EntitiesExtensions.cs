using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EasyChefDemo.Entities;
using EasyChefDemo.Web.Models;

namespace EasyChefDemo.Web.Infrastructure.Extensions
{
    public static class EntitiesExtensions
    {

        //public static void UpdateRestaurant(this Restaurant restaurant, RestaurantUserViewModel restaurantVm)
        //{
        //    restaurant.Name = restaurantVm.Name;
        //    restaurant.Description = restaurantVm.Description;
        //    restaurant.GlobalBar = false;
        //    restaurant.AutoOrder = false;
        //    restaurant.NoQty =false;
        //    restaurant.Status=true;
        //    restaurant.CreatedBy=restaurantVm.UserEmail;
        //    restaurant.CreatedDate = DateTime.Now;



        //}

        public static void UpdateRestaurant(this Restaurant restaurant, RestaurantUserAddressViewModel restaurantVm, int userId)
        {
            restaurant.Name = restaurantVm.Name;
            restaurant.Description = restaurantVm.Description;
            restaurant.GlobalBar = false;
            restaurant.AutoOrder = false;
            restaurant.NoQty = false;
            restaurant.Status = true;
            restaurant.CreatedBy = Convert.ToString(userId);
            restaurant.CreatedDate = DateTime.Now;



        }

        public static void UpdateRestaurantEdit(this Restaurant restaurant, RestaurantUserAddressViewModel restaurantVm, int userId)
        {
            restaurant.ID = restaurantVm.ID;
            restaurant.Name = restaurantVm.Name;
            restaurant.Description = restaurantVm.Description;
            restaurant.GlobalBar = false;
            restaurant.AutoOrder = false;
            restaurant.NoQty = false;
            restaurant.Status = true;
            restaurant.CreatedBy = Convert.ToString(userId);
            restaurant.CreatedDate = DateTime.Now;



        }

        //public static void UpdateAddress(this Address address, RestaurantUserViewModel restaurantVm)
        //{
        //    address.AddressDetails = restaurantVm.AddressDetails;
        //    address.Email = restaurantVm.UserEmail;
        //    address.WebsiteURL = restaurantVm.WebsiteURL;
        //    address.PrimaryPhone = restaurantVm.PrimaryPhone;
        //    address.StreetName = restaurantVm.StreetName;
        //    address.City = restaurantVm.City;
        //    address.State = restaurantVm.State;
        //    address.Country = restaurantVm.Country;

        //    address.Zip = restaurantVm.Zip;
        //    address.Status = true;
        //    address.CreatedBy = restaurantVm.UserEmail;
        //    address.CreatedDate = DateTime.Now;



        //}


        public static void UpdateAddress(this Address address, AddressViewModel restaurantVm, string UserEmail, int userId)
        {
            address.AddressDetails = restaurantVm.AddressDetails;
            address.Email = UserEmail;
            address.WebsiteURL = restaurantVm.WebsiteURL;
            address.PrimaryPhone = restaurantVm.PrimaryPhone;
            address.StreetName = restaurantVm.StreetName;
            address.City = restaurantVm.City;
            address.State = restaurantVm.State;
            address.Country = restaurantVm.Country;

            address.Zip = restaurantVm.Zip;
            address.Status = true;
            address.CreatedBy = Convert.ToString(userId);
            address.CreatedDate = DateTime.Now;



        }

        public static void UpdateAddressEdit(this Address address, AddressViewModel restaurantVm, string UserEmail, int userId)
        {
            address.ID = restaurantVm.ID;
            address.AddressDetails = restaurantVm.AddressDetails;
            address.Email = UserEmail;
            address.WebsiteURL = restaurantVm.WebsiteURL;
            address.PrimaryPhone = restaurantVm.PrimaryPhone;
            address.StreetName = restaurantVm.StreetName;
            address.City = restaurantVm.City;
            address.State = restaurantVm.State;
            address.Country = restaurantVm.Country;

            address.Zip = restaurantVm.Zip;
            address.Status = true;
            address.CreatedBy = Convert.ToString(userId);
            address.CreatedDate = DateTime.Now;



        }
        public static void UpdateInventoryItem(this InventoryItem inventoryItem, InventoryItem inventoryItemVm)
        {
            inventoryItem.Name = inventoryItemVm.Name;
            inventoryItem.ItemDescription = inventoryItemVm.ItemDescription;
            inventoryItem.Quantity = inventoryItemVm.Quantity;
            inventoryItem.Price = inventoryItemVm.Price;

            inventoryItem.PriceDate = inventoryItemVm.PriceDate;
            inventoryItem.VendorId = inventoryItemVm.VendorId;
            inventoryItem.UnitId = inventoryItemVm.UnitId;
            inventoryItem.CategoryId = inventoryItemVm.CategoryId;
            inventoryItem.IngredientTypeId = 39;
            inventoryItem.CreatedDate = DateTime.Now;

        }


        public static void UpdateInventoryItemEdit(this InventoryItem inventoryItem, InventoryItemViewModel inventoryItemVm)
        {
            inventoryItem.Name = inventoryItemVm.Name;
            inventoryItem.ItemDescription = inventoryItemVm.ItemDescription;
            inventoryItem.Quantity = inventoryItemVm.Quantity;
            inventoryItem.Price = inventoryItemVm.Price;

            inventoryItem.PriceDate = inventoryItemVm.PriceDate;
            inventoryItem.VendorId = inventoryItemVm.VendorId;
            inventoryItem.UnitId = inventoryItemVm.UnitId;
            inventoryItem.CategoryId = inventoryItemVm.CategoryId;
            inventoryItem.IngredientTypeId = 39;
            inventoryItem.UpdatedDate = DateTime.Now;

        }

        public static void UpdateRecipe(this Recipe recipe, RecipiesViewModel recipeVm)
        {
            recipe.Name = recipeVm.Name;
            recipe.SellingPrice = recipeVm.SellingPrice;
            recipe.DesiredMargin = recipeVm.DesiredMargin;
            recipe.Ratio = recipeVm.Ratio;
            recipe.Description = recipeVm.Description;
            recipe.PlateCost = recipeVm.PlateCost;
            recipe.DesiredPlateCost = recipeVm.DesiredPlateCost;

            recipe.Difference = recipeVm.Difference;
            recipe.ActualMargin = recipeVm.ActualMargin;
            recipe.Margin = recipeVm.Margin;





        }

        public static void UpdateCategoryItem(this Category category, Category categoryItemVm)
        {
            category.Name = categoryItemVm.Name;
            category.Status = categoryItemVm.Status;



        }

        public static void UpdateCategoryItemEdit(this Category category, CategoryViewModel categoryItemVm)
        {
            category.Name = categoryItemVm.Name;
            category.Status = Convert.ToBoolean(categoryItemVm.Status);


        }

        public static void UpdateUnitItemAdd(this Unit unit, Unit unitItemVm)
        {
            unit.Name = unitItemVm.Name;
            unit.Status = unitItemVm.Status;


        }

        public static void UpdateUnitItemEdit(this Unit unit, UnitViewModel unitVM)
        {
            unit.Name = unitVM.Name;
            unit.Status = Convert.ToBoolean(unitVM.Status);


        }


        public static void UpdateInventory(this Inventory inventory, InventoryViewModel inventoryVm, int? restaurantID)
        {
            inventory.Name = inventoryVm.Name;
            inventory.InventoryDate = inventoryVm.InventoryDate;
            inventory.Status = inventoryVm.Status;
            inventory.RestaurantId = restaurantID;
            inventory.CreatedDate = DateTime.Now;

        }


        public static void UpdateInventoryVendor(this Inventory inventory, InventoryViewModel inventoryVm)
        {
            //inventory.Name = inventoryVm.Name;
            //  inventory.InventoryDate = inventoryVm.InventoryDate;
            //   inventory.Status = inventoryVm.Status;
            inventory.VendorId = inventoryVm.VendorId;
            inventory.UpdatedDate = DateTime.Now;

        }

        public static void UpdateInventoryVendorStatus(this Inventory inventory, InventoryViewModel inventoryVm)
        {
            //inventory.Name = inventoryVm.Name;
            //  inventory.InventoryDate = inventoryVm.InventoryDate;
            //   inventory.Status = inventoryVm.Status;
            inventory.SenttoVendor = 1;
            inventory.SenttoVendorDate = DateTime.Now;

        }

        public static void UpdateApprovedInventory(this ApprovedInventory approvedinventoryItem, ApprovedInventoryViewModel approvedinventoryVm, int intInventoryID)
        {
            approvedinventoryItem.UniqueKey = Guid.NewGuid();
            approvedinventoryItem.InventoryId = intInventoryID;
            approvedinventoryItem.InventoryItemId = approvedinventoryVm.InventoryItemId;
            approvedinventoryItem.Quantity = approvedinventoryVm.Quantity;
            approvedinventoryItem.Price = approvedinventoryVm.Price;
            approvedinventoryItem.ParValue = approvedinventoryVm.ParValue;
            approvedinventoryItem.MinOrder = approvedinventoryVm.MinOrder;
            approvedinventoryItem.Order = approvedinventoryVm.Order;
            approvedinventoryItem.Status = approvedinventoryVm.Status;
            approvedinventoryItem.VendorId = approvedinventoryVm.VendorId;
            approvedinventoryItem.CreatedDate = DateTime.Now;


        }

        public static void UpdateVendor(this Vendor vendor, VendorViewModel vendorVm, int? RestaurantId, int? UserId)
        {
            vendor.Name = vendorVm.Name;
            vendor.SalesAgent = vendorVm.SalesAgent;
            vendor.Description = vendorVm.Description;
            vendor.DotNotCopy = vendorVm.DotNotCopy;
            vendor.RestaurantId = Convert.ToInt32(RestaurantId);
            vendor.Status = true;
            vendor.CreatedBy = Convert.ToString(UserId);
            vendor.CreatedDate = DateTime.Now;



        }

        public static void UpdateVendorAddress(this Address address, AddressViewModel vendorVm)
        {
            address.AddressDetails = vendorVm.AddressDetails;
            address.Email = vendorVm.Email;
            address.WebsiteURL = vendorVm.WebsiteURL;
            address.PrimaryPhone = vendorVm.PrimaryPhone;
            address.StreetName = vendorVm.StreetName;
            address.City = vendorVm.City;
            address.State = vendorVm.State;
            address.Country = vendorVm.Country;

            address.Zip = vendorVm.Zip;
            address.Status = true;
            //address.CreatedBy = restaurantVm.UserEmail;
            address.CreatedDate = DateTime.Now;



        }

        public static void UpdateVendorContacts(this Contacts contacts, ContactsViewModel contactsVm, int? VendorId, int? UserId)
        {
            contacts.Email = contactsVm.Email;
            contacts.CellPhone = contactsVm.CellPhone;

            contacts.VendorId = Convert.ToInt32(VendorId);
            contacts.RestaurantId = 1;  //default value
            contacts.UserId = 1;  //default
            contacts.CreatedBy = Convert.ToString(UserId);
            contacts.CreatedDate = DateTime.Now;

        }

        public static void UpdateVendorEdit(this Vendor vendor, VendorViewModel vendorVm)
        {
            vendor.Name = vendorVm.Name;
            vendor.SalesAgent = vendorVm.SalesAgent;
            vendor.Description = vendorVm.Description;
            vendor.DotNotCopy = vendorVm.DotNotCopy;

            vendor.Status = true;
            //vendor.CreatedBy = restaurantVm.UserEmail;
            vendor.UpdatedDate = DateTime.Now;



        }

        public static void UpdateVendorAddressEdit(this Address address, VendorUserViewModel vendorVm)
        {
            address.AddressDetails = vendorVm.AddressDetails;
            address.Email = vendorVm.Email;
            address.WebsiteURL = vendorVm.WebsiteURL;
            address.PrimaryPhone = vendorVm.PrimaryPhone;
            address.StreetName = vendorVm.StreetName;
            address.City = vendorVm.City;
            address.State = vendorVm.State;
            address.Country = vendorVm.Country;

            address.Zip = vendorVm.Zip;
            address.Status = true;
            //address.CreatedBy = restaurantVm.UserEmail;
            address.UpdatedDate = DateTime.Now;



        }

        public static void UpdateVendorContactsEdit(this Contacts contacts, ContactsViewModel contactsVm)
        {
            contacts.Email = contactsVm.Email;
            contacts.CellPhone = contactsVm.CellPhone;

            // contacts.VendorId = Convert.ToInt32(VendorId);
            contacts.RestaurantId = 1;  //default value
            contacts.UserId = 1;  //default
                                  //   contacts.CreatedBy = Convert.ToString(UserId);
            contacts.UpdatedDate = DateTime.Now;

        }


        public static void UpdateSubscription(this Subscription subscriptionItem, SubscriptionViewModel subscriptionvm)
        {
            subscriptionItem.StartDate = subscriptionvm.StartDate;
            subscriptionItem.EndDate = subscriptionvm.EndDate;
            subscriptionItem.SubscriptionPlanId = subscriptionvm.SubscriptionPlanId;
            subscriptionItem.RestaurantId = subscriptionvm.RestaurantId;
            subscriptionItem.TransId = subscriptionvm.TransId;
            subscriptionItem.UpdatedDate = DateTime.Now;

        }

    }
}