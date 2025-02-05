using Contract.Messages;
using QualityManager.Domain.DTOs;

namespace QualityManager.Application.Interfaces
{
    public interface IFoodBatchService
    {
        Task<FoodBatchResponseDto> ProcessFoodBatchAsync(FoodBatchRequestDto foodBatch);
        Task<FoodBatchResponseDto> GetFoodBatchStatusBySerialNumberAsync(string serialNumber);
        Task UpdateFoodBatchStatus(AnalysisResponse analysisResponse);
    }
}
