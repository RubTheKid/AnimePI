using AnimePI.Application.Interfaces;
using AnimePI.Application.Services;
using AnimePI.Auth.Models;
using AnimePI.Domain.Aggregates.AnimeAggregate.Interfaces;
using AnimePI.Domain.Aggregates.FavoriteAggregate.Interfaces;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Infra.Context;
using AnimePI.Infra.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace AnimePI.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IAnimeRepository, AnimeRepository>();

        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddHttpClient<JikanApiService>(client =>
        {
            client.BaseAddress = new Uri("https://api.jikan.moe/v4/");
            client.DefaultRequestHeaders.Add("User-Agent", "AnimePI/1.0");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }

    public static IServiceCollection AddDatabaseContexts(this IServiceCollection services, string connectionString)
    {
        var sqlServerOptions = (Action<Microsoft.EntityFrameworkCore.Infrastructure.SqlServerDbContextOptionsBuilder>)(sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sqlServerOptions));

        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(connectionString, sqlServerOptions));

        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")))
            };
        });

        return services;
    }

    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AnimePI.Application.AssemblyReference).Assembly));

        return services;
    }

    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AnimePI API",
                Version = "v1",
                Description = "API para gerenciamento de animes e favoritos"
            });

            // Configuração JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando o esquema Bearer. \r\n\r\n" +
                            "Digite 'Bearer' [espaço] e então seu token na entrada de texto abaixo.\r\n\r\n" +
                            "Exemplo: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
}
