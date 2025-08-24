
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OutBoxPatternShipments
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddMassTransit(x =>
            {
                // Adiciona consumidores (caso tenha)
                 x.AddConsumer<OrderCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("user");
                        h.Password("password");
                    });

                    cfg.ReceiveEndpoint("order-created-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedConsumer>(context);
                    });
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.EnableTryItOutByDefault();
                c.DisplayRequestDuration();
            });


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", () => "Welcome to the Outbox Pattern Shipments API!");

            app.MapGet("/shipments", async (AppDbContext context) =>
            {

                return Results.Ok(await context.Shipments.ToListAsync());
            });

            app.MapPost("/shipments/{orderId}", async ([FromRoute] Guid orderId, AppDbContext context) =>
            {
                var shipment = await context.Shipments.FirstOrDefaultAsync(item => item.OrderId.Equals(orderId) );

                if(shipment is null)
                {
                    return Results.NotFound("Shipment not found for the given OrderId.");   
                }

                shipment.Status = EShipmentStatus.Shipped;
                await context.SaveChangesAsync();

                return Results.Ok(shipment);
            });


            app.MapControllers();

            app.Run();
        }
    }
}
