using Bookanizer.REST.DAL;
using Microsoft.EntityFrameworkCore;

// -----------------------
// BUILD APPLICATION
// -----------------------

// Builder
var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DB Configuration
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(Configuration.PostgresConnectionString);
});

// Build
var app = builder.Build();

// -----------------------
// HTTP REQUEST PIPELINE
// -----------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); // Endpoints annotated with [Authorize] will require authorization
app.MapControllers();

// -----------------------
// RUN
// -----------------------
app.Run();
