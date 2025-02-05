using Contract.Messages;
using QualityManager.Domain.DTOs;
using QualityManager.Domain.Models;
namespace QualityManager.Infrastructure.Repository
{
    public interface IFoodBatchRepository
    {
        public Task AddFoodBatchAsync(FoodBatchRequestDto foodBatch);
        public Task<FoodBatch> GetFoodBatchByIdAsync(int foodBatchId);
        public Task<FoodBatch> GetFoodBatchBySerialNumberAsync(string serialNumber);
        public Task UpdateFoodBatchAsync(AnalysisResponse analysisResponse);
    }
}