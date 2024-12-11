using H4SoftwareTestOgSikkerhed.Components;
using H4SoftwareTestOgSikkerhed.Components.Account;
using H4SoftwareTestOgSikkerhed.Components.Interfaces;
using H4SoftwareTestOgSikkerhed.Components.Services;
using H4SoftwareTestOgSikkerhed.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<SymetricEncryptionService>();
builder.Services.AddScoped<AsymetricEncryptionService>();

builder.Services.AddSingleton<ICustomEmailSender, EmailSender>();
builder.Services.AddSingleton<IHashingHelper, HashingHelper>();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();



// if os is linux use sqlite
var connectionString = string.Empty;
var connectionString2 = string.Empty;

if (OperatingSystem.IsLinux())
{

    connectionString2 = builder.Configuration.GetConnectionString("MockToDoConnection") ?? throw new InvalidOperationException("Connection string 'ToDoConnection' not found.");
    builder.Services.AddDbContext<ToDoDBContext>(options =>
        options.UseSqlite(connectionString2));

    // Use For MockDB With WSL - And SQL Lite
    connectionString = builder.Configuration.GetConnectionString("MockDBConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
else
{
    string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    userFolder = Path.Combine(userFolder, ".aspnet");
    userFolder = Path.Combine(userFolder, "https");
    userFolder = Path.Combine(userFolder, "BitBendersCert.pfx");
    builder.Configuration.GetSection("Kestrel:EndPoints:Https:Certificate:Path").Value = userFolder;

    string kestrelPassword = builder.Configuration.GetValue<string>("KestrelPassword");
    builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate:Password").Value = kestrelPassword;

    builder.WebHost.UseKestrel((context, serverOptions) =>
    {
        serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
            .Endpoint("HTTPS", listenOptions =>
            {
                listenOptions.HttpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            });
    });

    // Use for Https And SQL
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    connectionString2 = builder.Configuration.GetConnectionString("ToDoConnection") ?? throw new InvalidOperationException("Connection string 'ToDoConnection' not found.");
    builder.Services.AddDbContext<ToDoDBContext>(options =>
        options.UseSqlServer(connectionString2));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}



builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();


// adds admin role to authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthentication", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
}
);

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
