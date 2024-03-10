using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FilmForge.Entities.Context;

public class FilmForgeDbContextFactory : IDesignTimeDbContextFactory<FilmForgeDbContext>
{
    public FilmForgeDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "../FilmForge/appsettings.json"), optional: false)
            .Build();

        var builder = new DbContextOptionsBuilder<FilmForgeDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString);

        return new FilmForgeDbContext(builder.Options);
    }
}
