using AutoMapper;
using Bogus;
using FilmForge.Common.Enum;
using FilmForge.Entities.EntityModels;
using FilmForge.Models.Dtos;
using FilmForge.Repository.UserRepository;
using US = FilmForge.Service.UserService;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmForge.Service.Test.UserServiceTests;

public class BaseUserServiceTests
{
    protected readonly Mock<IUserRepository> UserRepositoryMock = new();
    protected readonly Mock<ILogger<US.UserService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected US.UserService UserService;

    private protected void InitService()
    {
        UserService = new US.UserService(UserRepositoryMock.Object, LoggerMock.Object, MapperMock.Object);
    }


    private protected UserDto[] ArrangeUserDtos(int amount)
    {
        return new Faker<UserDto>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .RuleFor(x => x.Role, y => y.PickRandom<UserRole>())
            .RuleFor(x => x.Password, y => y.Internet.Password())
            .RuleFor(x => x.Salt, y => string.Empty)
            .RuleFor(x => x.CreatedOn, y => y.Date.Past(1))
            .RuleFor(x => x.ModifiedOn, (y, x) => y.Date.Between(x.CreatedOn, DateTime.UtcNow))
            .Generate(amount)
            .ToArray();
    }

    private protected User[] ArrangeUserEntities(int amount)
    {
        return new Faker<User>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .RuleFor(x => x.Role, y => y.PickRandom<UserRole>())
            .RuleFor(x => x.Password, y => y.Random.Bytes(42))
            .RuleFor(x => x.Salt, y => y.Random.Bytes(42))
            .RuleFor(x => x.CreatedOn, y => y.Date.Past(1))
            .RuleFor(x => x.ModifiedOn, (y, x) => y.Date.Between(x.CreatedOn, DateTime.UtcNow))
            .Generate(amount)
            .ToArray();
    }
}
