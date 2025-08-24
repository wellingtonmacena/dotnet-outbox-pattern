
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;

namespace OutboxPatternOrders
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob(
        AppDbContext dbContext,
        IPublishEndpoint publisher) : IJob
    {
       
        public async Task Execute(IJobExecutionContext context)
        {
            var integrationEventsAssembly = typeof(ProcessOutboxMessagesJob).Assembly;
            List<OrdersOutboxMessage> messages = await dbContext
                .Set<OrdersOutboxMessage>()
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(i => i.CreatedAt)
                .Take(1)
                
                .ToListAsync(context.CancellationToken);

            foreach (OrdersOutboxMessage message in messages)
            {
                var messageType = Messaging.Contracts.AssembyReference.Assembly.GetType(message.Type);
                var deserializedMessage = JsonSerializer.Deserialize(message.Content, messageType);

                if (message is null)
                {
                    continue;
                }

                await publisher.Publish(deserializedMessage, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}