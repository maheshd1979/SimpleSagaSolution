using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] OrderRequest order)
        {
            if (order.Price <= 1000) // Example payment condition
                return Ok();
            return BadRequest("Payment failed.");
        }

        [HttpPost("refund")]
        public IActionResult RefundPayment([FromBody] OrderRequest order)
        {
            Console.WriteLine($"Payment refund for Order ID: {order.OrderId}");
            return Ok();
        }
    }

}
