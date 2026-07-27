using LeaveManagement.API.Authorization;
using LeaveManagement.API.Configurations;
using LeaveManagement.API.Data;
using LeaveManagement.API.Middleware;
using LeaveManagement.API.Models;
using LeaveManagement.API.Repositories;
using LeaveManagement.API.Services;
using LeaveMangement.API.Interfaces;
using LeaveMangement.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


// ======================================
// Controllers
// ======================================

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
        .Add(new JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Leave Management API";
    config.Version = "v1";
    config.Description = "Leave Management System API";

    config.AddSecurity(
        "JWT",
        Enumerable.Empty<string>(),
        new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Enter: Bearer {your JWT token}"
        });

    config.OperationProcessors.Add(
        new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});


// ======================================
// Database
// ======================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


// ======================================
// ASP.NET Identity
// ======================================

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// ======================================
// Dependency Injection
// ======================================

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
builder.Services.AddScoped<IFiscalYearRepository, FiscalYearRepository>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IFiscalYearService, FiscalYearService>();
builder.Services.AddScoped<IFiscalYearManagementService, FiscalYearManagementService>();

builder.Services.AddScoped<
    IFiscalYearSettingsRepository,
    FiscalYearSettingsRepository>();

builder.Services.AddScoped<
    IFiscalYearSettingsService,
    FiscalYearSettingsService>();

builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ======================================
// JWT Authentication
// ======================================

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });


// ======================================
// Authorization + Dynamic Permissions
// ======================================

builder.Services.AddAuthorization();


builder.Services.AddSingleton
    <IAuthorizationPolicyProvider, PermissionPolicyProvider>();


builder.Services.AddScoped
    <IAuthorizationHandler, PermissionAuthorizationHandler>();


//=================================
// Email
//=================================

builder.Services
    .Configure<EmailSettings>(
        builder.Configuration
        .GetSection("EmailSettings"));


builder.Services
    .AddScoped<IEmailService, EmailService>();


// ======================================
// Build
// ======================================

var app = builder.Build();


// ======================================
// Pipeline
// ======================================

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();

    app.UseSwaggerUi(settings =>
    {
        settings.DocumentTitle = "Leave Management API";
    });
}


app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();


// ======================================
// Seed Database
// ======================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager =
        services.GetRequiredService<UserManager<ApplicationUser>>();

    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var context =
        services.GetRequiredService<ApplicationDbContext>();

    await IdentitySeeder.SeedAsync(
        userManager,
        roleManager,
        context);
}


// ======================================
// Controllers
// ======================================

app.MapControllers();


app.Run();