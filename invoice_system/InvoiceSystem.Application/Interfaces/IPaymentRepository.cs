using InvoiceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceSystem.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment> AddAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task DeleteAsync(int id);
    }
}
