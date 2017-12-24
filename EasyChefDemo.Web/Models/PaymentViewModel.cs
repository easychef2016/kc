using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyChefDemo.Web.Models
{
    public class PaymentViewModel
    {
        public PaymentViewModel()
        {
            BillingAddressVM = new List<AddressViewModel>();
            
        }
        public string sloginUserInfo { get; set; }
        public string sCreditCardNumber { get; set; }
        public int iPlanID { get; set; }
        public string sExpiryMonth { get; set; }
        public string sExpiryYear { get; set; }
        public string sCCV { get; set; }
        public string sCardName { get; set; }
        public decimal dAmount { get; set; }
        public IList<AddressViewModel> BillingAddressVM { get; set; }
        public IList<SubcriptionPlanViewModel> SubscriptionPlanVM { get; set; }
    }
}