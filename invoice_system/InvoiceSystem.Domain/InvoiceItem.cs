using System;

namespace InvoiceSystem.Domain
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;

        // Foreign key
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}