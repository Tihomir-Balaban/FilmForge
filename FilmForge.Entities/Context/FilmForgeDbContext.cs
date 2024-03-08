using FilmForge.Entities.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FilmForge.Entities.Context;

internal class FilmForgeDbContext : DbContext
{
    public FilmForgeDbContext(DbContextOptions<FilmForgeDbContext> options)
        : base(options) { }

    public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; }
}
