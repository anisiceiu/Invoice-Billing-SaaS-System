using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceSystem.Application.Dtos;

namespace InvoiceSystem.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto?> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto);
        Task<InvoiceDto> UpdateInvoiceAsync(int id, InvoiceDto invoiceDto);
        Task DeleteInvoiceAsync(int id);
        Task UpdateInvoiceStatusAsync(int id, string status);
    }
}