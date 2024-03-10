using FilmForge.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Service.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISecurityService, SecurityService>();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FilmForgeDbContext>(
    options =>
        options.UseSqlServer(connectionString)
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
