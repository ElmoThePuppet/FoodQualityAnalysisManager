namespace Contract.Messages
{
    public class AnalysisRequest
    {
        public string FoodName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public int AnalysisType { get; set; }
        public DateTime DateSubmitted { get; set; }
    }
}
