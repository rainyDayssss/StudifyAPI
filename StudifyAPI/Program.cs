using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudifyAPI.Features.Auth;
using StudifyAPI.Features.FriendRequests.Repository;
using StudifyAPI.Features.FriendRequests.Service;
using StudifyAPI.Features.Friends.Repository;
using StudifyAPI.Features.Friends.Service;
using StudifyAPI.Features.Pomodoro.Service;
using StudifyAPI.Features.Tasks.Repository;
using StudifyAPI.Features.Tasks.Service;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Features.Users.Services;
using StudifyAPI.Features.UserStreaks.Repository;
using StudifyAPI.Features.UserStreaks.Service;
using StudifyAPI.Shared.Database;
using StudifyAPI.Shared.Mapping;
using StudifyAPI.Shared.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// CORS for React
// ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ----------------------
// Register DbContext
// ----------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StudifyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// ----------------------
// Register Services / Repositories
// ----------------------
// Auth
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Users
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Friends
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IFriendService, FriendService>();

// Friend Requests
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();

// Tasks
builder.Services.AddScoped<IUserTaskRepository, UserTaskRepository>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();

// Streaks
builder.Services.AddScoped<IUserStreakRepository, UserStreakRepository>();
builder.Services.AddScoped<IUserStreakService, UserStreakService>();

// Pomodoro (Only Service exists)
builder.Services.AddScoped<IPomodoroService, PomodoroService>();

// ----------------------
// Authentication & JWT
// ----------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// ----------------------
// Add Controllers (API) & AutoMapper
// ----------------------
builder.Services.AddControllers();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

// ----------------------
// Swagger / OpenAPI
// ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudifyAPI", Version = "v1" });
    
    // Configure Swagger to use JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ----------------------
// Middleware
// ----------------------

// 1. Global Exception Handling
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS
app.UseCors("AllowReactApp");

// 2. Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// ----------------------
// Map API Controllers
// ----------------------
app.MapControllers();

app.Run();