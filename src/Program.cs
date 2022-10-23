using ClubManagementSystem.Areas.Identity;
using ClubManagementSystem.Data;
using ClubManagementSystem.Data.Entities;
using ClubManagementSystem.Interfaces;
using ClubManagementSystem.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    var conStr = builder.Configuration.GetConnectionString("Default");
    if (builder.Environment.IsEnvironment("Production"))
    {
        //postgres://:@/
        var matches = Regex.Match(Environment.GetEnvironmentVariable("DATABASE_URL")!, @"postgres://(.+):(\w+)@(.+)/(.+)");
        conStr = $"Host={matches.Groups[3]};Database={matches.Groups[4]};Username={matches.Groups[1]};Password={matches.Groups[2]};";
    }

    options.UseNpgsql(conStr);
    //else
    //{
    //    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    //            providerOptions => providerOptions.EnableRetryOnFailure());
    //}
});

builder.Services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddLocalization();

//builder.Services.AddHostedService<MembershipNotificationService>();
builder.Services.AddTransient<UserManager<User>>();
builder.Services.AddTransient<IMemberService, MemberService>();
builder.Services.AddTransient<IMembershipService, MembershipService>();
builder.Services.AddTransient<IUploadFileService, UploadFileService>();

builder.Services.AddScoped<AuthenticationStateProvider,
    RevalidatingIdentityAuthenticationStateProvider<User>>();

builder.Logging.AddFile("Logs/log-{Date}.txt", LogLevel.Information,
    new Dictionary<string, LogLevel>
    { {"Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error } });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); ;
app.UseAuthorization();

var supportedCultures = new[] { "en-US", "bg-BG" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await DbInitializer.ApplyMigrationsAndSeedData(app.Services);

app.Run();