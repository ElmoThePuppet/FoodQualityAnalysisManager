using Contract.Messages;
using MassTransit;

public class AnalysisServiceConsumer : IConsumer<AnalysisRequest>
{
    private readonly AnalysisServicePublisher _publisher;

    public AnalysisServiceConsumer(AnalysisServicePublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Consume(ConsumeContext<AnalysisRequest> context)
    {
        try
        {
            var request = context.Message;
            Console.WriteLine($"Received analysis request for batch: {request.SerialNumber}");
            var analysisResult = string.Empty;
            switch (request.AnalysisType)
            {
                case 1: analysisResult = "Processing complete: Microorganisms are within limits"; break;
                case 2: analysisResult = "Processing complete: No harmful chemical compounds found!"; break;
                case 3: analysisResult = "Processing complete: Physical integrity intact!"; break;
                default:
                    break;
            }
            var result = new AnalysisResponse
            {
                FoodBatchSerialNumber = request.SerialNumber,
                Result = analysisResult,
                DateProcessed = DateTime.UtcNow,
            };

            Console.WriteLine($"Processing analysis for batch: {result.FoodBatchSerialNumber}...");
            await Task.Delay(3000);
            Console.WriteLine($"PROCESSED: {result.FoodBatchSerialNumber}");

            await _publisher.PublishAnalysisResultAsync(result);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing analysis for batch: {ex.Message}");
        }
    }
}
