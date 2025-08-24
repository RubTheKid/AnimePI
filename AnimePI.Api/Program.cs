using AnimePI.Application.Interfaces;
using AnimePI.Application.Services;
using AnimePI.Domain.Aggregates.AnimeAggregate.Interfaces;
using AnimePI.Domain.Aggregates.FavoriteAggregate.Interfaces;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Infra.Context;
using AnimePI.Infra.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Se rodando em Docker, usar o nome do container SQL Server
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    if (connectionString.Contains("localhost"))
    {
        connectionString = connectionString.Replace("localhost", "sqlserver");
        Console.WriteLine($"Running in Docker container, using connection string with sqlserver");
    }
    else if (connectionString.Contains("host.docker.internal"))
    {
        connectionString = connectionString.Replace("host.docker.internal", "sqlserver");
        Console.WriteLine($"Running in Docker container, using connection string with sqlserver");
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();

builder.Services.AddHttpClient<JikanApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.jikan.moe/v4/");
    client.DefaultRequestHeaders.Add("User-Agent", "AnimePI/1.0");
    client.Timeout = TimeSpan.FromSeconds(30);
});


// Configurar MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AnimePI.Application.AssemblyReference).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Só usar HTTPS redirection em produção, não em Docker/Development
if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
