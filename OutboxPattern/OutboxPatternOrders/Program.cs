
using MassTransit;
using Messaging.Contracts;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Text.Json;

namespace OutboxPatternOrders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
     });


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DbConnectionString")));

            builder.Services.AddQuartz(configure =>
            {
                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            builder.Services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });


            builder.Services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(
                        trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                            schedule => schedule.WithIntervalInSeconds(10).WithRepeatCount(3)));

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            builder.Services.AddMassTransit(x =>
            {
                // Adiciona consumidores (caso tenha)
             

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("user");
                        h.Password("password");
                    });

                 
                });
            });
            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c =>
            {
                c.EnableTryItOutByDefault();
                c.DisplayRequestDuration();
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", () => "Welcome to the Outbox Pattern Orders API!");

            app.MapGet("/orders", async (AppDbContext context) =>
            {

                return Results.Ok(await context.Orders.ToListAsync());
            });

            app.MapPost("/orders", async (AppDbContext context) =>
            {
                var random = new Random().Next(100);
                await context.Database.BeginTransactionAsync();
                Order order = new()
                {
                    ProductName = "teste",
                    CustomerName = "teste",
                    Quantity = random,
                    TotalPrice = (decimal)(new Random().NextDouble() * random)

                };

                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Order> createdOrder = await context.Orders.AddAsync(order);
                var createdEvent = new OrderCreatedEvent(createdOrder.Entity.Id);


                await context.OrdersOutboxMessages.AddAsync(new OrdersOutboxMessage
                {
                    OrderId = createdEvent.Id,
                    Type = createdEvent.GetType().FullName,
                    Content = JsonSerializer.Serialize(order),
                   
                });

                await context.SaveChangesAsync();
                await context.Database.CommitTransactionAsync();
                return Results.Created($"/orders/{createdOrder.Entity.Id}", createdOrder.Entity);
            });


            app.MapControllers();

            app.Run();
        }
    }
}
