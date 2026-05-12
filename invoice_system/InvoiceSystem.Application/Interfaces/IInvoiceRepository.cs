using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceSystem.Domain;

namespace InvoiceSystem.Application.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetByIdAsync(int id);
        Task<IEnumerable<Invoice>> GetAllAsync();
        Task<Invoice> AddAsync(Invoice invoice);
        Task<Invoice> UpdateAsync(Invoice invoice);
        Task DeleteAsync(int id);
    }
}