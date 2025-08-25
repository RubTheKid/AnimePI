using AnimePI.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();

// Get connection string
var connectionString = builder.Configuration.GetConnectionStringForEnvironment();

// Configure application services
builder.Services
    .AddDatabaseContexts(connectionString)
    .AddApplicationServices()
    .AddExternalServices()
    .AddMediatRServices()
    .AddCustomAuthentication(builder.Configuration);

var app = builder.Build();

// Initialize database and configure pipeline
await app.InitializeDatabaseAsync();
app.ConfigurePipeline();

app.Run();
