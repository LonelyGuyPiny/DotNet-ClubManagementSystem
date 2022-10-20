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

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    var envVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    if (envVar == "Production")
    {
        //postgres://<username>:<password>@<host>/<dbname>
        //Host=my_host;Database=my_db;Username=my_user;Password=my_pw
        var m = Regex.Match(envVar, @"postgres://(.+):(\w+)@(.+)/(.+)");
        var conStr = $"Host={m.Groups[3]};Database={m.Groups[4]};Username={m.Groups[1]};Password={m.Groups[2]};";

        options.UseNpgsql(conStr);
    }   
    else 
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    }   
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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();;
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