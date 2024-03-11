using AutoMapper;
using FilmForge.Entities.EntityModels;
using FilmForge.Models.Dtos;
using FilmForge.Repository.UserRepository;
using FilmForge.Service.UserService;
using Microsoft.Extensions.Logging;
using Moq;

namespace FilmForge.Service.Test.UserServiceTests;

public class UserServiceTests : BaseUserServiceTests
{

    [Fact]
    public async Task CreateAsync_ShouldReturnUserDto_WhenUserIsCreated()
    {
        // Arrange
        var userDto = ArrangeUserDtos(1).FirstOrDefault();
        var user = ArrangeUserEntities(1).FirstOrDefault();

        UserRepositoryMock
            .Setup(r => r.CreateAsync(user, userDto))
            .ReturnsAsync(userDto);

        MapperMock
            .Setup(m => m.Map<User>(It.IsAny<UserDto>()))
            .Returns(user);

        LoggerMock
            .Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        InitService();

        // Act
        var result = await UserService.CreateAsync(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserDto>(result);
        UserRepositoryMock.Verify(r => r.CreateAsync(user, userDto), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenUserIsDeleted()
    {
        // Arrange
        int userId = 1;
        UserRepositoryMock
            .Setup(r => r.DeleteByIdAsync(userId)).
            ReturnsAsync(true);

        InitService();

        // Act
        var result = await UserService.DeleteByIdAsync(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnArrayOfUserDtos()
    {
        // Arrange
        var usersDto = ArrangeUserDtos(10);

        UserRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(usersDto);

        InitService();

        // Act
        var result = await UserService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(usersDto.Length, result.Length);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        int userId = 1;
        var userDto = ArrangeUserDtos(1).FirstOrDefault();

        UserRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(userDto);

        InitService();

        // Act
        var result = await UserService.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserDto>(result);
    }

    [Fact]
    public async Task GetUserByEmailAndPassword_ShouldReturnUserDto_WhenCredentialsAreValid()
    {
        // Arrange
        var userDto = ArrangeUserDtos(1).FirstOrDefault();

        UserRepositoryMock
            .Setup(r => r.GetUserByEmailAndPassord(userDto.Email, userDto.Password))
            .ReturnsAsync(userDto);

        InitService();

        // Act
        var result = await UserService.GetUserByEmailAndPassord(userDto.Email, userDto.Password);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserDto>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedUserDto_WhenUserIsUpdated()
    {
        // Arrange
        int userId = 1;
        var userDto = ArrangeUserDtos(1).FirstOrDefault();
        var user = ArrangeUserEntities(1).FirstOrDefault();

        MapperMock
            .Setup(m => m.Map<User>(It.IsAny<UserDto>()))
            .Returns(user);

        UserRepositoryMock
            .Setup(r => r.UpdateAsync(userId, user))
            .ReturnsAsync(userDto);

        InitService();

        // Act
        var result = await UserService.UpdateAsync(userId, userDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserDto>(result);
        UserRepositoryMock.Verify(r => r.UpdateAsync(userId, user), Times.Once);
    }
}
