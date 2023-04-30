using Invoices_Manager_API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Create builder.
var builder = WebApplication.CreateBuilder(args);

// Get the connection string from the appsettings.json file
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Check if the connection string is valid
if (connectionString is null)
    throw new Exception("Connection string is null");

// Add services to the builder.
builder.Services.AddControllers();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 33554432; // 32 MB
});
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdasdasdasddsa"))
        };
    });

// Will clear the Cache folder, since the files are no longer assigned to a thread anyway (zombie data)
await Task.Run(() => FileCore.ClearCacheFolder());

// Create app (API)
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

// Run app
app.Run();