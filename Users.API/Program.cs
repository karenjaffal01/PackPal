using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TravelApp.Business.Interfaces;
using TravelApp.Business.Services;
using TravelApp.Persistence.Data;
using TravelApp.Persistence.Interfaces;
using TravelApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Configure Serilog
// --------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/travelapp-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// --------------------
// Add services to DI
// --------------------
builder.Services.AddDbContext<TravelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers(); // For minimal API endpoints or controllers

// --------------------
// Configure Authentication (JWT)
// --------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateLifetime = true
        };
    });

// --------------------
// Configure Swagger (with JWT Auth)
// --------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Travel App API",
        Version = "v1",
        Description = "API documentation for the Travel App"
    });

    // Add JWT authorization to Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            new string[] {}
        }
    });
});

// --------------------
// Build the app
// --------------------
var app = builder.Build();
// --------------------
// Middleware pipeline
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel App API v1");
        c.RoutePrefix = string.Empty; // So you can access it at https://localhost:5000/
    });
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
