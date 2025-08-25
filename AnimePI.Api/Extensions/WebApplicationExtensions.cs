using AnimePI.Auth.Models;
using AnimePI.Infra.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnimePI.Api.Extensions;

public static class WebApplicationExtensions
{
    public static bool IsDevOrDockerEnvironment(this WebApplication app)
        => app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker");

    public static bool IsProductionEnvironment(this WebApplication app)
        => !app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Docker");

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        if (!app.IsDevOrDockerEnvironment()) return;

        using var scope = app.Services.CreateScope();

        await ApplyMigrationsAsync(scope.ServiceProvider);
        await ApplyAuthMigrationsAsync(scope.ServiceProvider);
        await CreateDefaultUserAsync(scope.ServiceProvider);
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.IsDevOrDockerEnvironment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // HTTPS redirection in production
        if (app.IsProductionEnvironment())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    private static async Task CreateDefaultUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        const string userEmail = "admin@gmail.com";
        const string userPassword = "Senha123!";
        var userId = Guid.Parse("8ED6CC7D-2F43-4CFB-6781-08DC6EC36900");

        var existingUser = await userManager.FindByEmailAsync(userEmail);
        if (existingUser != null)
        {
            Console.WriteLine($"Usuário {userEmail} já existe.");
            return;
        }

        var user = new ApplicationUser
        {
            UserName = userEmail,
            Email = userEmail,
            UserId = userId
        };

        var result = await userManager.CreateAsync(user, userPassword);

        if (result.Succeeded)
        {
            Console.WriteLine($"User {userEmail} created successfully.");
        }
        else
        {
            Console.WriteLine($"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    private static async Task ApplyMigrationsAsync(IServiceProvider serviceProvider)
    {
        try
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
            Console.WriteLine("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro applying AppDbContext migrations: {ex.Message}");
        }
    }

    private static async Task ApplyAuthMigrationsAsync(IServiceProvider serviceProvider)
    {
        try
        {
            var context = serviceProvider.GetRequiredService<AuthDbContext>();
            await context.Database.MigrateAsync();
            Console.WriteLine("AuthDbContext migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AuthDbContext migrations failed: {ex.Message}");
        }
    }
}
