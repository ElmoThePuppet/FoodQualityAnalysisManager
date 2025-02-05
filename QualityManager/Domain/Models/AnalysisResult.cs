using System.ComponentModel.DataAnnotations;

namespace QualityManager.Domain.Models
{
    public class AnalysisResult
    {
        public int Id { get; set; }
        [Required]
        public int FoodBatchId { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Result cannot be longer than 50 characters.")]
        public string Result { get; set; } = string.Empty;
        public DateTime DateProcessed { get; set; }
    }
}
