using Microsoft.EntityFrameworkCore;

// Create builder.
var builder = WebApplication.CreateBuilder(args);

// Get the connection string from the appsettings.json file
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Check if the connection string is valid
if (connectionString is null)
    throw new Exception("Connection string is null");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseMySQL(connectionString);
});

// Create app (API)
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run app
app.Run();