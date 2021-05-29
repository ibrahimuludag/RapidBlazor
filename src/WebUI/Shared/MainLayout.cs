using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.ThemeManager;
using RapidBlazor.WebUI.Theme;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Shared
{
    public partial class MainLayout
    {
        private ThemeManagerTheme _themeManager = new ThemeManagerTheme();

        public bool _drawerOpen = true;
        public bool _themeManagerOpen = false;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        void OpenThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        void UpdateTheme(ThemeManagerTheme value)
        {
            _themeManager = value;
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            _themeManager.Theme = new MudBlazorAdminDashboard();
            _themeManager.DrawerClipMode = DrawerClipMode.Always;
            _themeManager.FontFamily = "Montserrat";
            _themeManager.DefaultBorderRadius = 3;
        }

        // TODO : Manage this from pages or dynamically set
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Personal", href: "#"),
        new BreadcrumbItem("Dashboard", href: "#"),
    };

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }
    }
}
