using Bookanizer.REST.DAL;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;

// -----------------------
// LOG
// -----------------------

XmlConfigurator.Configure(new FileInfo("log4net.config"));
var logger = LogManager.GetLogger(typeof(Program));

// -----------------------
// BUILD
// -----------------------

logger.Info("=== Building Application ===");

// Builder
var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddLog4Net();

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();
logger.Info("Controllers added to builder.");

// DB Configuration
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(Configuration.PostgresConnectionString);
});
logger.Info("DbContext added to builder.");

// Health Check
builder.Services.AddHealthChecks();
logger.Info("Health Check added to builder.");

// Build
var app = builder.Build();
logger.Info("Application built.");

// -----------------------
// DATABASE MIGRATIONS
// -----------------------

logger.Info("=== Applying migrations to database ===");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        logger.Info("Applying database migrations...");
        var dataContext = services.GetRequiredService<DataContext>();
        dataContext.Database.Migrate();
        logger.Info("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.Error("Database migrations failed to apply.", ex);
        throw;
    }
}

// -----------------------
// HTTP REQUEST PIPELINE
// -----------------------

logger.Info("=== Configuring HTTPS Request Pipeline ===");

// OpenApi
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    logger.Info("OpenApi mapped.");
}

// Authentication
app.UseAuthentication();
logger.Info("Authentication added.");
app.UseAuthorization(); // Endpoints annotated with [Authorize] will require authorization
logger.Info("Authorization added.");

// Controllers
app.MapControllers();
logger.Info("Controllers mapped.");

// Health Check
app.MapHealthChecks("/health").AllowAnonymous();
logger.Info("Health Check mapped.");

// -----------------------
// RUN
// -----------------------

logger.Info("=== Running Application ===");

// Run the application
app.Run();
