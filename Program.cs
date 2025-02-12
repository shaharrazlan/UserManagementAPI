using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Text;
using UserManagementAPI.Repositories;
using UserManagementAPI.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
    ContentRootPath = AppContext.BaseDirectory
});

// ✅ Get the absolute path for .env
var basePath = Directory.GetCurrentDirectory();
var envFilePath = Path.Combine(basePath, ".env");
if (File.Exists(envFilePath))
{
    foreach (var line in File.ReadAllLines(envFilePath))
    {
        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
    Console.WriteLine("✅ Environment variables loaded from .env");
}
else
{
    Console.WriteLine("⚠️ Warning: .env file not found, using system environment variables.");
}

// ✅ Get Variables from Environment
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME");
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
var clientAppUrl = Environment.GetEnvironmentVariable("CLIENT_APP_URL") ?? "http://localhost:3000";

if (string.IsNullOrEmpty(mongoConnectionString) || string.IsNullOrEmpty(mongoDatabaseName))
{
    throw new InvalidOperationException("❌ MongoDB ConnectionString or DatabaseName is missing. Check .env file.");
}
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("❌ JWT Secret key is missing. Check .env file.");
}

Console.WriteLine($"✅ MongoDB Connection String: {mongoConnectionString}");
Console.WriteLine($"✅ MongoDB Database Name: {mongoDatabaseName}");
Console.WriteLine($"✅ Allowed CORS Origin: {clientAppUrl}");

// ✅ Configure MongoDB Connection
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
builder.Services.AddScoped<IUserRepository, UserRepository>(); 
builder.Services.AddScoped<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});

// ✅ Register Repositories & Services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// ✅ Configure JWT Authentication
Console.WriteLine("✅ JWT Secret key loaded successfully.");
Console.WriteLine($"🔍 JWT Secret Length: {jwtSecret.Length} characters");

var keyBytes = Encoding.UTF8.GetBytes(jwtSecret);
var key = new SymmetricSecurityKey(keyBytes);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Configure CORS to Allow React Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins(clientAppUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()); // Allow cookies, headers, etc.
});

var app = builder.Build();

// ✅ Enable Middleware
app.UseCors("AllowReactApp"); // Enable CORS
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("🚀 Application is running...");

app.Run();

public partial class Program { } // ✅ Make 'Program' accessible for tests

