using AutoPartsPM.Application;
using AutoPartsPM.Application.Common.Interfaces;
using AutoPartsPM.Infrastructure;
using AutoPartsPM.Web.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;
using MudBlazor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// Windows Authentication
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("PM_Admins"));
    options.AddPolicy("ManagerOrAbove", policy => policy.RequireRole("PM_Admins", "PM_Managers"));
    options.AddPolicy("MemberOrAbove", policy => policy.RequireRole("PM_Admins", "PM_Managers", "PM_Members"));

    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddCascadingAuthenticationState();

// Application & Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Current User Service
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<AutoPartsPM.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
