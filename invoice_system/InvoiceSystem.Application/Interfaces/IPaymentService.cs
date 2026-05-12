using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceSystem.Application.Dtos;

namespace InvoiceSystem.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task<PaymentDto> CreatePaymentAsync(PaymentDto paymentDto);
        Task<PaymentDto> UpdatePaymentAsync(int id, PaymentDto paymentDto);
        Task DeletePaymentAsync(int id);
    }
}