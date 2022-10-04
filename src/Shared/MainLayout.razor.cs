using ClubManagementSystem.Resources;
using ClubManagementSystem.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace ClubManagementSystem.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        protected MudThemeProvider MudThemeProvider { get; set; } = null!;
        protected DefaultTheme DefaultTheme { get; set; } = new();
        protected bool IsDrawerOpened { get; set; }
        protected bool IsDarkMode { get; set;}

        [Inject]
        protected IStringLocalizer<Common> Localizer { get; set; } = null!;

        protected void DrawerToggle()
         => IsDrawerOpened = !IsDrawerOpened;

        protected void DarkModeToggle()
         => IsDarkMode = !IsDarkMode;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsDarkMode = await MudThemeProvider.GetSystemPreference();
                StateHasChanged();
            }
        }
    }
}
