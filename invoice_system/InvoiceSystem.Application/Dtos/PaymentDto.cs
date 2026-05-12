using System;

namespace InvoiceSystem.Application.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string StripePaymentIntentId { get; set; } = string.Empty;
        public string StripeCheckoutSessionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Invoice details (optional, for nested representation)
        public int InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
    }
}