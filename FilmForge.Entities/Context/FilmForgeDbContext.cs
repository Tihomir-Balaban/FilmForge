using FilmForge.Entities.EntityModels;
using FilmForge.Entities.EntityModels.Base;
using Microsoft.EntityFrameworkCore;

namespace FilmForge.Entities.Context;

public class FilmForgeDbContext : DbContext
{
    public FilmForgeDbContext(DbContextOptions<FilmForgeDbContext> options)
        : base(options) { }

    public DbSet<Director> Directors { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<User> Users { get; set; }
}
