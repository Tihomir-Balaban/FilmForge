using FilmForge.Common.Enum;
using FilmForge.Repository.Intergration.Tests.Base;
using System.Security.Cryptography;
using System.Text;
using UR = FilmForge.Repository.UserRepository;

namespace FilmForge.Repository.Intergration.Tests.UserRepositoryTests
{
    public class BaseUserRepositoryIntergrationsTest<T> : BaseRepositoryIntergrationsTest<T>, IIntergrationTests
    {
        public BaseUserRepositoryIntergrationsTest()
            : base()
        {
            PopulateTestData();
        }

        public void PopulateTestData()
        {
            filmForgeDbContext.Users.RemoveRange(filmForgeDbContext.Users);

            using (var hmac = new HMACSHA512())
            {
                var users = new Faker<User>()
                    .RuleFor(x => x.CreatedOn, y => DateTime.Now)
                    .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
                    .RuleFor(x => x.Name, y => y.Person.FullName)
                    .RuleFor(x => x.Email, y => y.Person.Email)
                    .RuleFor(
                        x => x.Password, y => hmac
                        .ComputeHash(
                            Encoding
                            .UTF8
                            .GetBytes(y.Internet.Password())))
                    .RuleFor(x => x.Salt, y => hmac.Key)
                    .RuleFor(x => x.Role, y => y.PickRandom<UserRole>())
                    .Generate(10)
                    .ToArray();

                filmForgeDbContext.Users.AddRange(users);
                filmForgeDbContext.SaveChanges();
            }
        }
    }
}