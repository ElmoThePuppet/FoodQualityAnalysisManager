using MassTransit;
using Xunit;
using Contract.Messages;

public class MessagingIntegrationTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IBus _bus;

    public MessagingIntegrationTests()
    {
        var services = new ServiceCollection();

        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ReceiveEndpoint("analysis-response-queue", e =>
                {
                    e.Handler<AnalysisResponse>(context =>
                    {
                        Console.WriteLine($"Received AnalysisResponse for batch: {context.Message.FoodBatchSerialNumber}");
                        return Task.CompletedTask;
                    });
                });
            });
        });

        _serviceProvider = services.BuildServiceProvider();
        _bus = _serviceProvider.GetRequiredService<IBus>();
    }

    [Fact]
    public async Task Should_Send_And_Receive_AnalysisResponse()
    {
        // Arrange
        var response = new AnalysisResponse
        {
            FoodBatchSerialNumber = "TEST123",
            Result = "Test result!",
            DateProcessed = DateTime.UtcNow
        };

        // Act
        await _bus.Publish(response);

        await Task.Delay(1000);

        // Assert - manually check console output for verification (for now)

    }
}
