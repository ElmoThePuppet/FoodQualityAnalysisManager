using System;
using System.ComponentModel.DataAnnotations;
using QualityManager.Domain.Enums;

namespace QualityManager.Domain.DTOs
{
    public class FoodBatchRequestDto
    {
        [Required(ErrorMessage = "Food name is required.")]
        public string FoodName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Serial number is required.")]
        public string SerialNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Analysis type must be a valid positive integer.")]
        public int AnalysisType { get; set; }

        [Required(ErrorMessage = "Date submitted is required.")]
        public DateTime DateSubmitted { get; set; }
    }
}
