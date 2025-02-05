using Contract.Messages;
using QualityManager.Application.Interfaces;
using QualityManager.Domain.Models;
using QualityManager.Infrastructure.Repository;

namespace QualityManager.Application.Services
{
    public class AnalysisResultService : IAnalysisResultService
    {
        private readonly IAnalysisRepository _analysisRepository;

        public AnalysisResultService(IAnalysisRepository analysisRepository)
        {
            _analysisRepository = analysisRepository;
        }

        public async Task SaveAnalysisResultAsync(AnalysisResponse analysisResult)
        {
            if (analysisResult == null)
                throw new ArgumentNullException(nameof(analysisResult), "Analysis result cannot be null");

            await _analysisRepository.SaveAnalysisAsync(analysisResult);
        }

        public async Task<AnalysisResultDto> GetAnalysisResultAsync(string foodBatchSerialNumber)
        {
            return await _analysisRepository.GetAnalysisAsync(foodBatchSerialNumber);
        }
    }
}
