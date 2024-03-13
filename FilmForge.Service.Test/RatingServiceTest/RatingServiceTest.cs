namespace FilmForge.Service.Test.RatingServiceTest;

public class RatingServiceTest : BaseRatingServiceTest
{
    [Fact]
    public async Task CreateAsync_ShouldReturnRatingDto_WhenRatingIsCreated()
    {
        // Arrange
        var ratingDto = ArrangeRatingDtos(1).FirstOrDefault();
        var rating = ArrangeRatingEntities(1).FirstOrDefault();

        RatingRepositoryMock
            .Setup(r => r.CreateAsync(rating, ratingDto))
            .ReturnsAsync(ratingDto);

        MapperMock
            .Setup(m => m.Map<Rating>(It.IsAny<RatingDto>()))
            .Returns(rating);

        LoggerMock
            .Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        InitService();

        // Act
        var result = await RatingService.CreateAsync(ratingDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RatingDto>(result);
        RatingRepositoryMock.Verify(r => r.CreateAsync(rating, ratingDto), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenRatingIsDeleted()
    {
        // Arrange
        int ratingId = 1;
        RatingRepositoryMock
            .Setup(r => r.DeleteByIdAsync(ratingId)).
            ReturnsAsync(true);

        InitService();

        // Act
        var result = await RatingService.DeleteByIdAsync(ratingId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnArrayOfUserDtos()
    {
        // Arrange
        var ratingDto = ArrangeRatingDtos(10);

        RatingRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(ratingDto);

        InitService();

        // Act
        var result = await RatingService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ratingDto.Length, result.Length);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRatingDto_WhenRatingExists()
    {
        // Arrange
        int ratingId = 1;
        var ratingDto = ArrangeRatingDtos(1).FirstOrDefault();

        RatingRepositoryMock
            .Setup(r => r.GetByIdAsync(ratingId))
            .ReturnsAsync(ratingDto);

        InitService();

        // Act
        var result = await RatingService.GetByIdAsync(ratingId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RatingDto>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedRatingDto_WhenRatingIsUpdated()
    {
        // Arrange
        int ratingId = 1;
        var ratingDto = ArrangeRatingDtos(1).FirstOrDefault();
        var rating = ArrangeRatingEntities(1).FirstOrDefault();

        MapperMock
            .Setup(m => m.Map<Rating>(It.IsAny<RatingDto>()))
            .Returns(rating);

        RatingRepositoryMock
            .Setup(r => r.UpdateAsync(ratingId, rating))
            .ReturnsAsync(ratingDto);

        InitService();

        // Act
        var result = await RatingService.UpdateAsync(ratingId, ratingDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RatingDto>(result);
        RatingRepositoryMock.Verify(r => r.UpdateAsync(ratingId, rating), Times.Once);
    }
}
