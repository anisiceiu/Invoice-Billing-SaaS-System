using InvoiceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceSystem.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
    }
}
