using System.ComponentModel.DataAnnotations;

namespace QualityManager.Domain.Models
{
    public class FoodBatch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Food name cannot be longer than 30 characters.")]
        public string FoodName { get; set; } = string.Empty;

        [Required]
        [StringLength(30, ErrorMessage = "Serial number cannot be longer than 30 characters.")]
        public string SerialNumber { get; set; } = string.Empty;

        [Range(1, 3, ErrorMessage = "AnalysisType must be 1, 2, or 3.")]
        public int AnalysisType { get; set; }

        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
