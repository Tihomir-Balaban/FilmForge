using FilmForge.Service.InvitationService;

namespace FilmForge.Service.Test.InvitationServiceTest;

public class InvitationServiceTest : BaseInvitationServiceTest
{
    [Fact]
    public async Task CreateAsync_ReturnsInvitationDto_WhenValid()
    {
        // Arrange
        var invitationDto = ArrangInvitationDtos(1).First();
        var invitation = ArrangeInvitationEntities(1).First();

        MapperMock.Setup(m => m.Map<Invitation>(invitationDto)).Returns(invitation);
        InvitationRepositoryMock.Setup(repo => repo.CreateAsync(invitation)).ReturnsAsync(invitationDto);
        ActorServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ActorDto());
        MovieServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new MovieDto() { Actors = new List<ActorDto>() });

        InitService();

        // Act
        var result = await InvitationService.CreateAsync(invitationDto);

        // Assert
        Assert.NotNull(result);
        MapperMock.Verify(m => m.Map<Invitation>(invitationDto), Times.Once);
        InvitationRepositoryMock.Verify(repo => repo.CreateAsync(invitation), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ReturnsTrue_WhenSuccessful()
    {
        // Arrange
        int id = 1;
        InvitationRepositoryMock.Setup(repo => repo.DeleteByIdAsync(id)).ReturnsAsync(true);

        InitService();
        // Act
        var result = await InvitationService.DeleteByIdAsync(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetByActorIdAndMovieIdAsync_ReturnsInvitationDto_WhenFound()
    {
        // Arrange
        int actorId = 1, movieId = 2;
        var invitationDto = ArrangInvitationDtos(1).First();
        InvitationRepositoryMock.Setup(repo => repo.GetByActorIdAndMovieIdAsync(actorId, movieId)).ReturnsAsync(invitationDto);

        InitService();
        // Act
        var result = await InvitationService.GetByActorIdAndMovieIdAsync(actorId, movieId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByActorIdAsync_ReturnsArrayOfInvitationDto_WhenFound()
    {
        // Arrange
        int actorId = 1;
        var invitations = new[] { new InvitationDto(), new InvitationDto() };
        InvitationRepositoryMock.Setup(repo => repo.GetByActorIdAsync(actorId)).ReturnsAsync(invitations);

        InitService();
        // Act
        var result = await InvitationService.GetByActorIdAsync(actorId);

        // Assert
        Assert.Equal(invitations.Length, result.Length);
    }

    [Fact]
    public async Task GetByMovieIdAsync_ReturnsArrayOfInvitationDto_WhenFound()
    {
        // Arrange
        int movieId = 1;
        var invitations = new[] { new InvitationDto(), new InvitationDto() };
        InvitationRepositoryMock.Setup(repo => repo.GetByMovieIdAsync(movieId)).ReturnsAsync(invitations);

        InitService();
        // Act
        var result = await InvitationService.GetByMovieIdAsync(movieId);

        // Assert
        Assert.Equal(invitations.Length, result.Length);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedInvitationDto_WhenValid()
    {
        // Arrange
        var invitationDto = ArrangInvitationDtos(1).First();
        var invitation = ArrangeInvitationEntities(1).First()   ;
        MapperMock.Setup(m => m.Map<Invitation>(invitationDto)).Returns(invitation);
        InvitationRepositoryMock.Setup(repo => repo.UpdateAsync(invitationDto.Id, invitation)).ReturnsAsync(invitationDto);

        InitService();
        // Act
        var result = await InvitationService.UpdateAsync(invitationDto.Id, invitationDto);

        // Assert
        Assert.NotNull(result);
        MapperMock.Verify(m => m.Map<Invitation>(invitationDto), Times.Once);
        InvitationRepositoryMock.Verify(repo => repo.UpdateAsync(invitationDto.Id, invitation), Times.Once);
    }
}