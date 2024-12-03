using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace OrderAPI.Controllers
{
    
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly SagaOrchestrator _sagaOrchestrator;

        public OrderController(SagaOrchestrator sagaOrchestrator)
        {
            _sagaOrchestrator = sagaOrchestrator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest order)
        {
            var result = await _sagaOrchestrator.OrchestrateOrderAsync(order);
            return result ? Ok("Order processed successfully.") : StatusCode(500, "Order processing failed.");
        }
    }

}
