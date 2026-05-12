using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InvoiceSystem.Domain;
using Stripe;
using InvoiceSystem.Application.Interfaces;

namespace InvoiceSystem.Api.Controllers
{
   

    [ApiController]
    [Route("api/webhooks")]
    public class WebhookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;
        private readonly IInvoiceService _invoiceService;   

        public WebhookController(
            IConfiguration configuration,
            IPaymentService paymentService,
            IInvoiceService invoiceService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _invoiceService = invoiceService;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json =
                await new StreamReader(HttpContext.Request.Body)
                    .ReadToEndAsync();

            var stripeSignature =
                Request.Headers["Stripe-Signature"];

            var endpointSecret =
                _configuration["Stripe:WebhookSecret"];

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                endpointSecret
            );

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session =
                    stripeEvent.Data.Object as Stripe.Checkout.Session;

                var invoiceId =
                    int.Parse(session.Metadata["invoiceId"]);

                var invoice =
                    await _invoiceService.GetInvoiceByIdAsync(invoiceId);

                if (invoice != null)
                {
                    invoice.Status = "Paid";

                    var payment = new Payment
                    {
                        InvoiceId = invoice.Id,
                        Amount = invoice.TotalAmount,
                        StripeCheckoutSessionId = session.Id,
                        StripePaymentIntentId =
                            session.PaymentIntentId,

                        Status = "Paid",
                        CreatedAt = DateTime.UtcNow
                    };

                    await _paymentService.CreatePaymentAsync(new Application.Dtos.PaymentDto
                    {
                        InvoiceId = payment.InvoiceId,
                        Amount = payment.Amount,
                        Currency = "usd",
                        StripeCheckoutSessionId =
                            payment.StripeCheckoutSessionId,
                        StripePaymentIntentId =
                            payment.StripePaymentIntentId,
                        Status = payment.Status
                    });

                }
            }

            return Ok();
        }
    }
}
