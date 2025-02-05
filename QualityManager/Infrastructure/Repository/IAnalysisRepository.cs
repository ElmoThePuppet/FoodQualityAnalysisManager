using Contract.Messages;
using QualityManager.Domain.Models;

namespace QualityManager.Infrastructure.Repository
{
    public interface IAnalysisRepository
    {
        public Task<AnalysisResultDto> GetAnalysisAsync(string foodBatchId);
        public Task SaveAnalysisAsync(AnalysisResponse analysisResult);
    }
}
