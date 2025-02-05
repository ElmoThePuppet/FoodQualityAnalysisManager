using Contract.Messages;
using FoodQualityAnalysis.Infrastructure;
using Microsoft.EntityFrameworkCore;
using QualityManager.Domain.DTOs;
using QualityManager.Domain.Models;

namespace QualityManager.Infrastructure.Repository
{
    public class FoodBatchRepository : IFoodBatchRepository
    {
        private readonly FoodQualityContext _context;

        public FoodBatchRepository(FoodQualityContext context)
        {
            _context = context;
        }

        public async Task AddFoodBatchAsync(FoodBatchRequestDto foodBatchDto)
        {
            var foodBatch = new FoodBatch
            {
                FoodName = foodBatchDto.FoodName,
                SerialNumber = foodBatchDto.SerialNumber,
                AnalysisType = foodBatchDto.AnalysisType,
                DateSubmitted = foodBatchDto.DateSubmitted,
                Status = "Analysis Pending"
            };
            await _context.FoodBatches.AddAsync(foodBatch);
            await _context.SaveChangesAsync();
        }

        public async Task<FoodBatch> GetFoodBatchByIdAsync(int foodBatchId)
        {
            return await _context.FoodBatches
                           .FirstOrDefaultAsync(fb => fb.Id == foodBatchId);
        }

        public async Task<FoodBatch> GetFoodBatchBySerialNumberAsync(string serialNumber)
        {
            return await _context.FoodBatches
                .FirstOrDefaultAsync(fb => fb.SerialNumber == serialNumber);
        }

        public async Task UpdateFoodBatchAsync(AnalysisResponse analysisResponse)
        {
            var dbFoodBatch = await _context.FoodBatches.FirstOrDefaultAsync(x => x.SerialNumber == analysisResponse.FoodBatchSerialNumber);

            if (dbFoodBatch == null)
            {
                throw new KeyNotFoundException($"FoodBatch with ID {analysisResponse.FoodBatchSerialNumber} not found.");
            }
            dbFoodBatch.Status = analysisResponse.Result;
            _context.FoodBatches.Update(dbFoodBatch);

            await _context.SaveChangesAsync();
        }
    }
}
