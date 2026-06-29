using Bookanizer.REST.DAL;
using Bookanizer.REST.Middleware;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Authentication & Authorization using JWT
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Jwt:Audience is not configured.");
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();
logger.Info("JWT Authentication and Authorization added.");

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
// EXCEPTION HANDLING
// -----------------------

logger.Info("=== Enabling exception handling middleware ===");
app.UseExceptionHandling();

logger.Info("=== Enabling status code pages ===");
app.UseStatusCodePages(async statusCodeContext =>
{
    var context = statusCodeContext.HttpContext;
    var statusCode = context.Response.StatusCode;

    // Only synthesize a body for error statuses that arrived without one.
    if (statusCode >= 400)
    {
        await ProblemWriter.WriteAsync(context, statusCode, ProblemWriter.ReasonFor(statusCode));
    }
});

// -----------------------
// HTTP REQUEST PIPELINE
// -----------------------

logger.Info("=== Configuring HTTP Request Pipeline ===");

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
