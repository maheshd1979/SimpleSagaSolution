using Models;

namespace OrderAPI
{
    public class SagaOrchestrator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SagaOrchestrator(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> OrchestrateOrderAsync(OrderRequest order)
        {
            SagaState state = SagaState.Created;

            try
            {
                // Step 1: Check and Reserve Inventory
                if (!await CheckInventory(order))
                    throw new Exception("Inventory check failed.");
                state = SagaState.InventoryChecked;

                // Step 2: Process Payment
                if (!await ProcessPayment(order))
                    throw new Exception("Payment processing failed.");
                state = SagaState.PaymentProcessed;

                // Step 3: Mark order as completed
                state = SagaState.Completed;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Saga failed at state: {state}. Error: {ex.Message}");
                await Compensate(state, order);
                return false;
            }
        }

        private async Task<bool> CheckInventory(OrderRequest order)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7297/api/inventory/check", order);
             return response.IsSuccessStatusCode;
        }

        private async Task<bool> ProcessPayment(OrderRequest order)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7105/api/payment/process", order);
            return response.IsSuccessStatusCode;
        }

        private async Task Compensate(SagaState failedState, OrderRequest order)
        {
            var client = _httpClientFactory.CreateClient();

            if (failedState == SagaState.InventoryChecked)
            {
                // Rollback inventory
                await client.PostAsJsonAsync("https://localhost:7297/api/inventory/rollback", order);
            }

            if (failedState == SagaState.PaymentProcessed)
            {
                // Rollback payment
                await client.PostAsJsonAsync("https://localhost:7105/api/payment/refund", order);
            }
        }
    }

}
