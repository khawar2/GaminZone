using Braintree;

namespace GamingZone.Services
{
    public class PaymentGateway : IGateway
    {
        private readonly BraintreeGateway _gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "8tph2x85qws9kw9w",
            PublicKey = "66xtwkzyg2xr8vbh",
            PrivateKey = "ab0f34388b28f10b36dd8beccc8b76d7"
        };

        public PaymentResult ProcessPayment(ViewModels.CheckoutViewModel model)
        {
            var request = new TransactionRequest()
            {
                Amount = model.Total,
                CreditCard = new TransactionCreditCardRequest()
                {
                    Number = model.CardNumber,
                    CVV = model.Cvv,
                    ExpirationMonth = model.Month,
                    ExpirationYear = model.Year
                },
                Options = new TransactionOptionsRequest()
                {
                    SubmitForSettlement = true
                }
            };

            var result = _gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                return new PaymentResult(result.Target.Id, true, null);
            }

            return new PaymentResult(null, false, result.Message);
        }
    }
}
