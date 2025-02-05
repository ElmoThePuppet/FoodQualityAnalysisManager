using MassTransit;
using QualityManager.Domain.Models;
using QualityManager.Application.Interfaces;
using Contract.Messages;
public class AnalysisResultConsumer : IConsumer<AnalysisResponse>
{
    private readonly IAnalysisResultService _analysisResultService;
    private readonly IFoodBatchService _foodBatchService;

    public AnalysisResultConsumer(IAnalysisResultService analysisResultService, IFoodBatchService foodBatchService)
    {
        _analysisResultService = analysisResultService;
        _foodBatchService = foodBatchService;
    }

    public async Task Consume(ConsumeContext<AnalysisResponse> context)
    {
        var analysisResult = context.Message;
        Console.WriteLine("Quality Manager message received!");
        await _analysisResultService.SaveAnalysisResultAsync(analysisResult);
        await _foodBatchService.UpdateFoodBatchStatus(analysisResult);
    }
}
