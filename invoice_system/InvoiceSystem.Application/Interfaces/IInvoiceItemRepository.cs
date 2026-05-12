using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceSystem.Domain;

namespace InvoiceSystem.Application.Interfaces
{
    public interface IInvoiceItemRepository
    {
        Task<InvoiceItem?> GetByIdAsync(int id);
        Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId);
        Task<InvoiceItem> AddAsync(InvoiceItem invoiceItem);
        Task<InvoiceItem> UpdateAsync(InvoiceItem invoiceItem);
        Task DeleteAsync(int id);
    }
}