using System.Text;
using Bookanizer.REST.Configuration;
using Bookanizer.REST.DAL;
using Bookanizer.REST.DAL.Repositories;
using Bookanizer.REST.DAL.Repositories.Interfaces;
using Bookanizer.REST.Middleware;
using Bookanizer.REST.Services;
using Bookanizer.REST.Services.Interfaces;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

JwtSettings jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt configuration section is missing.");

if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
    throw new InvalidOperationException("Jwt:Secret is not configured.");
if (string.IsNullOrWhiteSpace(jwtSettings.Issuer))
    throw new InvalidOperationException("Jwt:Issuer is not configured.");
if (string.IsNullOrWhiteSpace(jwtSettings.Audience))
    throw new InvalidOperationException("Jwt:Audience is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
logger.Info("JWT Authentication and Authorization added.");

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
