namespace QualityManager.Domain.Models
{
    public class AnalysisResultDto
    {
        public string FoodBatchSerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
    }
}
