namespace OutboxPatternOrders
{
    public class Order: Entity
    {
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
