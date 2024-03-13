namespace FilmForge.Service.Test.ReviewServiceTest;

public class ReviewServiceTest : BaseReviewServiceTest
{
    [Fact]
    public async Task CreateAsync_ShouldReturnReviewDto_WhenReviewIsCreated()
    {
        // Arrange
        var reviewDto = ArrangeReviewDtos(1).FirstOrDefault();
        var review = ArrangeReviewEntities(1).FirstOrDefault();

        ReviewRepositoryMock
            .Setup(r => r.CreateAsync(review, reviewDto))
            .ReturnsAsync(reviewDto);

        MapperMock
            .Setup(m => m.Map<Review>(It.IsAny<ReviewDto>()))
            .Returns(review);

        LoggerMock
            .Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        InitService();

        // Act
        var result = await ReviewService.CreateAsync(reviewDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ReviewDto>(result);
        ReviewRepositoryMock.Verify(r => r.CreateAsync(review, reviewDto), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenReviewIsDeleted()
    {
        // Arrange
        int reviewId = 1;
        ReviewRepositoryMock
            .Setup(r => r.DeleteByIdAsync(reviewId)).
            ReturnsAsync(true);

        InitService();

        // Act
        var result = await ReviewService.DeleteByIdAsync(reviewId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnArrayOfUserDtos()
    {
        // Arrange
        var reviewDto = ArrangeReviewDtos(10);

        ReviewRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(reviewDto);

        InitService();

        // Act
        var result = await ReviewService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reviewDto.Length, result.Length);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnReviewDto_WhenReviewExists()
    {
        // Arrange
        int reviewId = 1;
        var reviewDto = ArrangeReviewDtos(1).FirstOrDefault();

        ReviewRepositoryMock
            .Setup(r => r.GetByIdAsync(reviewId))
            .ReturnsAsync(reviewDto);

        InitService();

        // Act
        var result = await ReviewService.GetByIdAsync(reviewId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ReviewDto>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedReviewDto_WhenReviewIsUpdated()
    {
        // Arrange
        int reviewId = 1;
        var reviewDto = ArrangeReviewDtos(1).FirstOrDefault();
        var review = ArrangeReviewEntities(1).FirstOrDefault();

        MapperMock
            .Setup(m => m.Map<Review>(It.IsAny<ReviewDto>()))
            .Returns(review);

        ReviewRepositoryMock
            .Setup(r => r.UpdateAsync(reviewId, review))
            .ReturnsAsync(reviewDto);

        InitService();

        // Act
        var result = await ReviewService.UpdateAsync(reviewId, reviewDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ReviewDto>(result);
        ReviewRepositoryMock.Verify(r => r.UpdateAsync(reviewId, review), Times.Once);
    }
}
