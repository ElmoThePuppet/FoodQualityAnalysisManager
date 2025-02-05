using FoodQualityAnalysis.Infrastructure;
using Microsoft.EntityFrameworkCore;
using QualityManager.Domain.Models;
using Contract.Messages;

namespace QualityManager.Infrastructure.Repository
{
    public class AnalysisRepository : IAnalysisRepository
    {
        private readonly FoodQualityContext _context;

        public AnalysisRepository(FoodQualityContext context)
        {
            _context = context;
        }

        public async Task<AnalysisResultDto> GetAnalysisAsync(string foodBatchSerialNumber)
        {
            var result = await _context.FoodBatches
                .Where(f => f.SerialNumber == foodBatchSerialNumber)
                .Select(f => new AnalysisResultDto
                {
                    FoodBatchSerialNumber = f.SerialNumber,
                    Result = _context.AnalysisResults
                                .Where(a => a.FoodBatchId == f.Id)
                                .Select(a => a.Result)
                                .FirstOrDefault(),
                    Status = f.Status
                })
                .FirstOrDefaultAsync();

            return result ?? new AnalysisResultDto
            {
                FoodBatchSerialNumber = foodBatchSerialNumber,
                Result = "Not Available",
                Status = "Food batch not found or no analysis recorded"
            };
        }

        public async Task SaveAnalysisAsync(AnalysisResponse analysisResult)
        {
            if (analysisResult == null)
                throw new ArgumentNullException(nameof(analysisResult), "Analysis result cannot be null");
            var dbFoodBatch = await _context.FoodBatches.FirstOrDefaultAsync(x => x.SerialNumber == analysisResult.FoodBatchSerialNumber);
            Console.WriteLine(dbFoodBatch);
            await _context.AnalysisResults.AddAsync(new AnalysisResult
            {
                FoodBatchId = dbFoodBatch.Id,
                Result = analysisResult.Result,
                DateProcessed = analysisResult.DateProcessed
            });
            await _context.SaveChangesAsync();
        }
    }
}
