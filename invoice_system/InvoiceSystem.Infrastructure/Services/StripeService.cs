using System;
using System.Collections.Generic;
using System.Text;
using InvoiceSystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace InvoiceSystem.Infrastructure.Services
{
    

    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly IInvoiceRepository _invoiceRepository;

        public StripeService(
            IConfiguration configuration,
            IInvoiceRepository invoiceRepository)
        {
            _configuration = configuration;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<string> CreateCheckoutSessionAsync(int invoiceId)
        {
            Stripe.StripeConfiguration.ApiKey =
                _configuration["Stripe:SecretKey"];

            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

            if (invoice == null)
                throw new Exception("Invoice not found");

            var domain = "http://localhost:4200";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl =
                    $"{domain}/payment/success?session_id={{CHECKOUT_SESSION_ID}}",

                CancelUrl =
                    $"{domain}/payment/cancel",

                Mode = "payment",

                Metadata = new Dictionary<string, string>
            {
                { "invoiceId", invoice.Id.ToString() }
            },

                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,

                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",

                        UnitAmountDecimal = invoice.TotalAmount * 100,

                        ProductData =
                            new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Invoice {invoice.InvoiceNumber}"
                            }
                    }
                }
            }
            };

            var service = new Stripe.Checkout.SessionService();

            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
