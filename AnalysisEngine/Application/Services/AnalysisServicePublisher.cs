using Contract.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;

public class AnalysisServicePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AnalysisServicePublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAnalysisResultAsync(AnalysisResponse result)
    {
        try
        {
            await _publishEndpoint.Publish(result, ctx =>
            {
                ctx.SetRoutingKey("analysis.response");
            });

            Console.WriteLine($"Published analysis result for batch: {result.FoodBatchSerialNumber}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to publish analysis result: {ex.Message}");
        }
    }
}
