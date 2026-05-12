using Microsoft.AspNetCore.Mvc;
using InvoiceSystem.Application.Dtos;
using InvoiceSystem.Application.Interfaces;

namespace InvoiceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IStripeService _stripeService;
        public PaymentController(IPaymentService paymentService, IStripeService stripeService)
        {
            _paymentService = paymentService;
            _stripeService = stripeService;
        }


        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession(
        CreateCheckoutSessionRequest request)
        {
            var url =
                await _stripeService.CreateCheckoutSessionAsync(
                    request.InvoiceId);

            return Ok(new
            {
                url
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment(PaymentDto paymentDto)
        {
            var createdPayment = await _paymentService.CreatePaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPayment), new { id = createdPayment.Id }, createdPayment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentDto paymentDto)
        {
            if (id != paymentDto.Id)
            {
                return BadRequest();
            }

            var updatedPayment = await _paymentService.UpdatePaymentAsync(id, paymentDto);
            if (updatedPayment == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }
    }
}