using OutboxPatternOrders;

namespace OutBoxPatternShipments
{
    public class Shipment : Entity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public EShipmentStatus Status { get; set; }
    }
}
