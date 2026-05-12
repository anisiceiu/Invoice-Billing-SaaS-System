using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceSystem.Application.Dtos;
using InvoiceSystem.Application.Interfaces;
using InvoiceSystem.Domain;

namespace InvoiceSystem.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerRepository _customerRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, ICustomerRepository customerRepository)
        {
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
        }

        public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null) return null;

            return new InvoiceDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status.ToString(),
                Subtotal = invoice.Subtotal,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                CreatedAt = invoice.CreatedAt,
                UpdatedAt = invoice.UpdatedAt,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.Name,
                Items = invoice.Items.Select(i => new InvoiceItemDto
                {
                    Id = i.Id,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            var invoiceDtos = new List<InvoiceDto>();

            foreach (var invoice in invoices)
            {
                invoiceDtos.Add(new InvoiceDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoiceDate = invoice.InvoiceDate,
                    DueDate = invoice.DueDate,
                    Status = invoice.Status.ToString(),
                    Subtotal = invoice.Subtotal,
                    TaxAmount = invoice.TaxAmount,
                    TotalAmount = invoice.TotalAmount,
                    CreatedAt = invoice.CreatedAt,
                    UpdatedAt = invoice.UpdatedAt,
                    CustomerId = invoice.CustomerId,
                    CustomerName = invoice.Customer?.Name,
                    Items = invoice.Items.Select(i => new InvoiceItemDto
                    {
                        Id = i.Id,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                });
            }

            return invoiceDtos;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto)
        {
            var invoice = new Invoice
            {
                InvoiceNumber = GenerateInvoiceNumber(),
                InvoiceDate = invoiceDto.InvoiceDate,
                DueDate = invoiceDto.DueDate,
                Status = InvoiceStatus.Pending,
                Subtotal = invoiceDto.Subtotal,
                TaxAmount = invoiceDto.TaxAmount,
                TotalAmount = invoiceDto.TotalAmount,
                CustomerId = invoiceDto.CustomerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add invoice items
            foreach (var itemDto in invoiceDto.Items)
            {
                invoice.Items.Add(new InvoiceItem
                {
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                });
            }

            var createdInvoice = await _invoiceRepository.AddAsync(invoice);

            // Return the created invoice as DTO
            return await GetInvoiceByIdAsync(createdInvoice.Id);
        }

        public async Task<InvoiceDto> UpdateInvoiceAsync(int id, InvoiceDto invoiceDto)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null) return null!;

            invoice.InvoiceNumber = invoiceDto.InvoiceNumber;
            invoice.InvoiceDate = invoiceDto.InvoiceDate;
            invoice.DueDate = invoiceDto.DueDate;
            // Note: Status update should be done via a separate method for security
            invoice.Subtotal = invoiceDto.Subtotal;
            invoice.TaxAmount = invoiceDto.TaxAmount;
            invoice.TotalAmount = invoiceDto.TotalAmount;
            invoice.CustomerId = invoiceDto.CustomerId;
            invoice.UpdatedAt = DateTime.UtcNow;

            // Clear existing items and add new ones
            invoice.Items.Clear();
            foreach (var itemDto in invoiceDto.Items)
            {
                invoice.Items.Add(new InvoiceItem
                {
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                });
            }

            var updatedInvoice = await _invoiceRepository.UpdateAsync(invoice);

            return await GetInvoiceByIdAsync(updatedInvoice.Id);
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            await _invoiceRepository.DeleteAsync(id);
        }

        public async Task UpdateInvoiceStatusAsync(int id, string status)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null) return;

            if (Enum.TryParse<InvoiceStatus>(status, true, out var parsedStatus))
            {
                invoice.Status = parsedStatus;
                invoice.UpdatedAt = DateTime.UtcNow;
                await _invoiceRepository.UpdateAsync(invoice);
            }
        }

        private string GenerateInvoiceNumber()
        {
            // Simple invoice number generation: INV-YYYYMM-####
            var yearMonth = DateTime.UtcNow.ToString("yyyyMM");
            // In a real system, you would query the database for the last invoice number of the month
            // For simplicity, we'll use a random number or a static one for now.
            // This should be replaced with a proper sequence.
            return $"INV-{yearMonth}-{new Random().Next(1000, 9999)}";
        }
    }
}