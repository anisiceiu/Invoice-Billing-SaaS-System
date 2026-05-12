using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceSystem.Application.Interfaces;
using InvoiceSystem.Domain;
using InvoiceSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Infrastructure.Repositories
{
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private readonly InvoiceSystemDbContext _context;

        public InvoiceItemRepository(InvoiceSystemDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceItem?> GetByIdAsync(int id)
        {
            return await _context.InvoiceItems.FindAsync(id);
        }

        public async Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId)
        {
            return await _context.InvoiceItems
                .Where(ii => ii.InvoiceId == invoiceId)
                .ToListAsync();
        }

        public async Task<InvoiceItem> AddAsync(InvoiceItem invoiceItem)
        {
            _context.InvoiceItems.Add(invoiceItem);
            await _context.SaveChangesAsync();
            return invoiceItem;
        }

        public async Task<InvoiceItem> UpdateAsync(InvoiceItem invoiceItem)
        {
            _context.Entry(invoiceItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return invoiceItem;
        }

        public async Task DeleteAsync(int id)
        {
            var invoiceItem = await _context.InvoiceItems.FindAsync(id);
            if (invoiceItem != null)
            {
                _context.InvoiceItems.Remove(invoiceItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}