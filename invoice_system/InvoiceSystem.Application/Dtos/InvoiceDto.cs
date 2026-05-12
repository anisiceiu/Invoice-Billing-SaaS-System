using System;
using System.Collections.Generic;

namespace InvoiceSystem.Application.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Customer details (optional, for nested representation)
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        // Invoice items
        public ICollection<InvoiceItemDto> Items { get; set; } = new List<InvoiceItemDto>();
    }
}