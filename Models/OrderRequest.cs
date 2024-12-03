namespace Models
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public enum SagaState
    {
        Created,
        InventoryChecked,
        PaymentProcessed,
        Completed,
        Failed
    }
}
