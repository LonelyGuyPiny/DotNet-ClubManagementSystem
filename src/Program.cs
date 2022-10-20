using ClubManagementSystem.Areas.Identity;
using ClubManagementSystem.Data;
using ClubManagementSystem.Data.Entities;
using ClubManagementSystem.Interfaces;
using ClubManagementSystem.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    var envVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    if (envVar == "Production")
    {
        var uri = new Uri(envVar);

        var username = uri.UserInfo.Split(':')[0];
        var password = uri.UserInfo.Split(':')[1];
    
        var connectionString =
        "; Database=" + uri.AbsolutePath.Substring(1) +
        "; Username=" + username +
        "; Password=" + password +
        "; Port=" + uri.Port +
        "; SSL Mode=Require; Trust Server Certificate=true;";
        options.UseNpgsql(connectionString);
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