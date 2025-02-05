using FluentAssertions;
using Moq;
using QualityManager.Domain.DTOs;
using QualityManager.Infrastructure.Repository;
using QualityManager.Application.Services;
using Xunit;
using System;
using System.Threading.Tasks;

public class FoodBatchServiceTests
{
    private readonly Mock<IFoodBatchRepository> _mockRepository;
    private readonly FoodBatchService _service;

    public FoodBatchServiceTests()
    {
        _mockRepository = new Mock<IFoodBatchRepository>();
        _service = new FoodBatchService(_mockRepository.Object);
    }

    public static IEnumerable<object[]> InvalidFoodBatchData()
    {
        yield return new object[] { null, 1 }; // Null FoodName
        yield return new object[] { "Apple", default(int) }; // Null AnalysisType (int default is 0)
    }

    [Theory]
    [MemberData(nameof(InvalidFoodBatchData))]
    public async Task ProcessFoodBatchAsync_ShouldThrowArgumentException_WhenInputIsNull(string foodName, int analysisType)
    {
        // Arrange
        var request = new FoodBatchRequestDto
        {
            FoodName = foodName,
            DateSubmitted = DateTime.UtcNow,
            SerialNumber = "12345",
            AnalysisType = analysisType
        };

        // Act
        Func<Task> act = async () => await _service.ProcessFoodBatchAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Food name and analysis type cannot be null or empty.");
    }
}
