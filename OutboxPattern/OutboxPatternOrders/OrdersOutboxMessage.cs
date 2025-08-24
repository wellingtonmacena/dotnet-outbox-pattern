namespace OutboxPatternOrders
{
    public class OrdersOutboxMessage: Entity
    {
        public string Type { get; init; }
        public string Content { get; init; }
        public DateTime? ProcessedOnUtc { get; set; }
        public string? Error { get; init; }
        public Guid OrderId { get; init; }
    }
}
