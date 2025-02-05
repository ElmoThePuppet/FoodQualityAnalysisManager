using Contract.Messages;
using MassTransit;

public class FoodBatchPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public FoodBatchPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAnalysisRequestAsync(AnalysisRequest request)
    {
        try
        {
            await _publishEndpoint.Publish(request, ctx =>
            {
                ctx.SetRoutingKey("foodbatch.response");
            });
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to publish analysis request", ex);
        }
    }
}
