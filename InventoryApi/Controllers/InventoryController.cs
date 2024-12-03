
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        [HttpPost("check")]
        public IActionResult CheckInventory([FromBody] OrderRequest order)
        {
            if (order.Quantity <= 100) // Example inventory condition
                return Ok();
            return BadRequest("Insufficient inventory.");
        }

        [HttpPost("rollback")]
        public IActionResult RollbackInventory([FromBody] OrderRequest order)
        {
            Console.WriteLine($"Inventory rollback for Order ID: {order.OrderId}");
            return Ok();
        }
    }

}
