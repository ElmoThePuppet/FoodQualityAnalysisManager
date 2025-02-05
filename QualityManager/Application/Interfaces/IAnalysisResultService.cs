using Contract.Messages;
using QualityManager.Domain.Models;

namespace QualityManager.Application.Interfaces
{
    public interface IAnalysisResultService
    {
        Task SaveAnalysisResultAsync(AnalysisResponse analysisResult);
        Task<AnalysisResultDto> GetAnalysisResultAsync(string foodBatchSerialNumber);
    }
}
