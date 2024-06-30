using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Poslasticarnica.Configuration;
using Poslasticarnica.Core;
using Poslasticarnica.Model;
using Poslasticarnica.Model.dto;
using Poslasticarnica.Repository;
using Stripe;
using Stripe.Checkout;

namespace Poslasticarnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly string _stripeSecretKey;
        private readonly string _stripePublishableKey;

        public StripeController(IConfiguration config)
        {
            _stripeSecretKey = config["StripeSettings:SecretKey"];
            _stripePublishableKey = config["StripeSettings:PublishableKey"];

            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession([FromBody] CreateCheckoutSessionDTO request)
        {
            List<SessionLineItemOptions> sessionLineItemsOptions = new List<SessionLineItemOptions>();

            foreach (CheckoutDto item in request.Items)
            {
                sessionLineItemsOptions.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        UnitAmount = item.ProductPrice,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                            Description = item.ProductDescription,
                        },
                    },
                    Quantity = item.Quantity,
                });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = sessionLineItemsOptions,
                Metadata = new Dictionary<string, string>
                {
                    { "korisnikId", request.KorisnikId.ToString() },
                    { "datumVremePorudzbine", DateTime.Now.ToString() }

                },
                Mode = "payment",
                SuccessUrl = "http://localhost:5173/payment/success",
                CancelUrl = "http://localhost:5173/payment/failed",
            };

            var service = new SessionService();

            Session session = service.Create(options);

            return Ok(new { sessionId = session.Id });
        }
    }
}
