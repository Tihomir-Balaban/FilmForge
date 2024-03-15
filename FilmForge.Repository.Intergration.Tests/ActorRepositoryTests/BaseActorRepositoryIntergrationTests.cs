using FilmForge.Repository.Intergration.Tests.Base;
using System.Security.Cryptography;
using System.Text;

namespace FilmForge.Repository.Intergration.Tests.ActorRepositoryTests;

public class BaseActorRepositoryIntergrationTests<T> : BaseRepositoryIntergrationsTest<T>
{
    public User[] users;
    public Actor[] actors;

    public BaseActorRepositoryIntergrationTests()
            : base()
    {
        users = GenerateUsers();
        actors = GenerateActors(users);
    }
}