// -----------------------
// BUILDER
// -----------------------
var builder = WebApplication.CreateBuilder(args);

// -----------------------
// SERVICES
// -----------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// -----------------------
// BUILD
// -----------------------
var app = builder.Build();

// -----------------------
// HTTP REQUEST PIPELINE
// -----------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// -----------------------
// RUN
// -----------------------
app.Run();
