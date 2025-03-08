using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiTenantTaskManager.Data;
using MultiTenantTaskManager.Models;

var builder = WebApplication.CreateBuilder(args);

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add MVC with Views
builder.Services.AddControllersWithViews();

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new ArgumentNullException(nameof(jwtSecret), "JWT secret key is not configured.");
}

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Enable API controllers
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the SeedData service
//builder.Services.AddHostedService<SeedData>();

var app = builder.Build();

// Enforce HTTPS redirection
app.UseHttpsRedirection();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Enable Sessions
app.UseSession();

// Enable Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// Middleware to extract JWT from cookies and set User.Identity
app.Use(async (context, next) =>
{
    // Skip JWT validation for the registration page
    if (context.Request.Path.StartsWithSegments("/Account/Register"))
    {
        await next();
        return;
    }

    var token = context.Request.Cookies["AuthToken"];
    if (!string.IsNullOrEmpty(token))
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSecret);

        var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        context.User = claimsPrincipal;
    }

    await next();
});

// Configure default route (MVC support)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the app
app.Run();



/*
  {
    "id": "04e83cc0-0e51-4392-ac35-d62b8641743b",
    "fullName": "Admin",
    "email": "Hus1@gmail.com",
    "userName": "Hus1@gmail.com",
    "roles": [
      "Employee"
    ]
  },
  {
    "id": "083fbb5a-aa65-495a-a36d-e1faa9932368",
    "fullName": "Admin",
    "email": "Admin@gmail.com",
    "userName": "Admin@gmail.com",
    "roles": [
      "Employee"
    ]
  }
]*/