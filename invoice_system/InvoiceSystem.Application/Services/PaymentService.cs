using InvoiceSystem.Application.Dtos;
using InvoiceSystem.Application.Interfaces;
using InvoiceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceSystem.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<PaymentDto> CreatePaymentAsync(PaymentDto paymentDto)
        {
            var payment = new Payment
            {
                StripePaymentIntentId = paymentDto.StripePaymentIntentId,
                StripeCheckoutSessionId = paymentDto.StripeCheckoutSessionId,
                Amount = paymentDto.Amount,
                Currency = paymentDto.Currency,
                Status = paymentDto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdPayment = await _paymentRepository.AddAsync(payment);

            return new PaymentDto
            {
                Id = createdPayment.Id,
                StripePaymentIntentId = createdPayment.StripePaymentIntentId,
                StripeCheckoutSessionId = createdPayment.StripeCheckoutSessionId,
                Amount = createdPayment.Amount,
                Currency = createdPayment.Currency,
                Status = createdPayment.Status,
                CreatedAt = createdPayment.CreatedAt,
                UpdatedAt = createdPayment.UpdatedAt
            };  
              
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _paymentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            var paymentDtos = new List<PaymentDto>();

            foreach (var payment in payments)
            {
                paymentDtos.Add(new PaymentDto
                {
                    Id = payment.Id,
                    StripePaymentIntentId = payment.StripePaymentIntentId,
                    StripeCheckoutSessionId = payment.StripeCheckoutSessionId,
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    Status = payment.Status,
                    CreatedAt = payment.CreatedAt,
                    UpdatedAt = payment.UpdatedAt
                });
            }

            return paymentDtos;
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return null!;

            return new PaymentDto
            {
                Id = payment.Id,
                StripePaymentIntentId = payment.StripePaymentIntentId,
                StripeCheckoutSessionId = payment.StripeCheckoutSessionId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }

        

        public async Task<PaymentDto> UpdatePaymentAsync(int id, PaymentDto paymentDto)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return null!;

            payment.StripePaymentIntentId = paymentDto.StripePaymentIntentId;
            payment.StripeCheckoutSessionId = paymentDto.StripeCheckoutSessionId;
            payment.Amount = paymentDto.Amount;
            payment.Currency = paymentDto.Currency;
            payment.Status = paymentDto.Status;
            payment.UpdatedAt = DateTime.UtcNow;

            var updatedPayment = await _paymentRepository.UpdateAsync(payment);

            return new PaymentDto
            {
                Id = updatedPayment.Id,
                StripePaymentIntentId = updatedPayment.StripePaymentIntentId,
                StripeCheckoutSessionId = updatedPayment.StripeCheckoutSessionId,
                Amount = updatedPayment.Amount,
                Currency = updatedPayment.Currency,
                Status = updatedPayment.Status,
                CreatedAt = updatedPayment.CreatedAt,
                UpdatedAt = updatedPayment.UpdatedAt
            };
        }
    }
}
