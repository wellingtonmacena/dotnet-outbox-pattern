using MassTransit;
using Messaging.Contracts;
using OutBoxPatternShipments;

public class OrderCreatedConsumer(AppDbContext dbContext) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        await dbContext.Shipments.AddAsync(new Shipment()
        {
            Status = EShipmentStatus.Pending,
            OrderId = message.Id,
            

        });

        Console.WriteLine($"Recebido evento OrderCreated: {message.Id}");

        // Aqui você pode salvar no banco ou processar a lógica
        await dbContext.SaveChangesAsync();
    }
}
