namespace Contract.Messages
{
    public class AnalysisResponse
    {
        public string FoodBatchSerialNumber { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public DateTime DateProcessed { get; set; }
    }
}
