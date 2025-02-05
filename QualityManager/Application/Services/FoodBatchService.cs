using Contract.Messages;
using QualityManager.Application.Interfaces;
using QualityManager.Domain.DTOs;
using QualityManager.Infrastructure.Repository;

public class FoodBatchService: IFoodBatchService
{
    private readonly IFoodBatchRepository _foodBatchRepository;
    private readonly FoodBatchPublisher _foodBatchPublisher;

    public FoodBatchService(IFoodBatchRepository foodBatchRepository)
    {
        _foodBatchRepository = foodBatchRepository;
    }
    public FoodBatchService(IFoodBatchRepository foodBatchRepository, FoodBatchPublisher foodBatchPublisher)
    {
        _foodBatchRepository = foodBatchRepository;
        _foodBatchPublisher = foodBatchPublisher;
    }

    public async Task<FoodBatchResponseDto> GetFoodBatchStatusBySerialNumberAsync(string serialNumber)
    {
        if (string.IsNullOrEmpty(serialNumber))
        {
            throw new ArgumentException("Serial number is required.");
        }

        var foodBatch = await _foodBatchRepository.GetFoodBatchBySerialNumberAsync(serialNumber);

        if (foodBatch == null)
        {
            return new FoodBatchResponseDto
            {
                SerialNumber = serialNumber,
                Status = "Not Found"
            };
        }

        return new FoodBatchResponseDto
        {
            SerialNumber = foodBatch.SerialNumber,
            Status = foodBatch.Status
        };
    }

    public async Task<FoodBatchResponseDto> ProcessFoodBatchAsync(FoodBatchRequestDto foodBatch)
    {
        if (string.IsNullOrWhiteSpace(foodBatch.FoodName) || foodBatch.AnalysisType == 0)
        {
            throw new ArgumentException("Food name and analysis type cannot be null or empty.");
        }

        await _foodBatchRepository.AddFoodBatchAsync(foodBatch);

        var analysisRequest = new AnalysisRequest
        {
            FoodName = foodBatch.FoodName,
            SerialNumber = foodBatch.SerialNumber,
            AnalysisType = foodBatch.AnalysisType,
            DateSubmitted = foodBatch.DateSubmitted,
        };
        await _foodBatchPublisher.PublishAnalysisRequestAsync(analysisRequest);
        return new FoodBatchResponseDto
        {
            SerialNumber = foodBatch.SerialNumber,
            Status = "Processing started",
            Message = "The analysis request has been submitted."
        };
    }

    public async Task UpdateFoodBatchStatus(AnalysisResponse analysisResponse)
    {
        await _foodBatchRepository.UpdateFoodBatchAsync(analysisResponse);
    }
}
