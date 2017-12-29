using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using EasyChefDemo.Web.Models;
using EasyChefDemo.Web.Infrastructure.Helpers;
using System.Net;
using System.Security.Authentication;

namespace net.authorize
{

    public class ChargeCreditCard
    {
        //string ApiLoginID = Constants.API_LOGIN_ID;
        //string ApiTransactionKey = Constants.TRANSACTION_KEY;
        public static ANetApiResponse Run(PaymentViewModel PaymentVM)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = Constants.API_LOGIN_ID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = Constants.TRANSACTION_KEY,
            };

            var creditCard = new creditCardType
            {
                cardNumber = PaymentVM.sCreditCardNumber,
                expirationDate = PaymentVM.sExpiryMonth.ToString() + PaymentVM.sExpiryYear.ToString(),
                cardCode = PaymentVM.sCCV.ToString()
            };

            var billingAddress = new customerAddressType
            {
                firstName = PaymentVM.sCardName,
                lastName = "",
                address = PaymentVM.BillingAddressVM[0].StreetName,
                city = PaymentVM.BillingAddressVM[0].City,
                zip = PaymentVM.BillingAddressVM[0].Zip
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            // Add line Items
            var lineItems = new lineItemType[1];
            lineItems[0] = new lineItemType
            {
                itemId = PaymentVM.iPlanID.ToString(),
                name = "Test",
                quantity = 1,
                unitPrice = PaymentVM.dAmount
            };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                amount = PaymentVM.dAmount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems
            };
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();



            return response;
        }
    }
}
