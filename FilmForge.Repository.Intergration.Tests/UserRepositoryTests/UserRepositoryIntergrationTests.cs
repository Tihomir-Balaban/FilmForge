using FilmForge.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Security;
using UR = FilmForge.Repository.UserRepository;

namespace FilmForge.Repository.Intergration.Tests.UserRepositoryTests
{
    public class UserRepositoryIntergrationTests : BaseUserRepositoryIntergrationsTest<UR.UserRepository>
    {
        private readonly UR.UserRepository userRepository;
        private readonly ISecurityService securityService;
        private readonly Mock<IConfiguration> configurationMock = new();

        public UserRepositoryIntergrationTests() 
                : base ()
        {
            securityService = new SecurityService(configurationMock.Object);

            userRepository = new UR.UserRepository(
                filmForgeDbContext,
                securityService,
                loggerMock.Object,
                mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddUser()
        {
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123"
            };

            var user = mapper.Map<User>(userDto);

            var result = await userRepository.CreateAsync(user, userDto);

            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldRemoveUser()
        {
            byte[] password, salt;

            (password, salt) = securityService.HashPassword("password123");

            var user = new User
            {
                Name = "Delete Test",
                Email = "delete@example.com",
                Password = password,
                Salt = salt
            };


            var userDto = mapper.Map<UserDto>(user);

            var createdUser = await userRepository.CreateAsync(user, userDto);

            var result = await userRepository.DeleteByIdAsync(createdUser.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsers()
        {
            var users = await userRepository.GetAllAsync();
            Assert.NotNull(users);
            Assert.True(users.Length > 0);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            var users = await userRepository.GetAllAsync();
            var user = users.FirstOrDefault();
            Assert.NotNull(user);

            var result = await userRepository.GetByIdAsync(user.Id);
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyUser()
        {
            var user = filmForgeDbContext.Users.First();
            user.Name = "Updated Name";
            var updatedUser = await userRepository.UpdateAsync(user.Id, user);

            Assert.NotNull(updatedUser);
            Assert.Equal("Updated Name", updatedUser.Name);
        }
    }
}