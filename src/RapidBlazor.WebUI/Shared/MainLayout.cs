using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.ThemeManager;
using RapidBlazor.WebUI.Theme;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RapidBlazor.WebUI.Shared
{
    public partial class MainLayout : IDisposable
    {
        private ThemeManagerTheme _themeManager = new ThemeManagerTheme();

        public bool _drawerOpen = true;
        public bool _themeManagerOpen = false;

        [Inject]
        public NavigationManager NavigationManager { get; set; }
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
            NavigationManager.LocationChanged += LocationChanged;

        }

        // TODO : Manage this from pages or dynamically set
        public List<BreadcrumbItem> BreadcrumbItems { get; set; } = new List<BreadcrumbItem>();
        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();
            NavigationManager.NavigateTo("authentication/logout");
        }

        private void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            BreadcrumbItems = new List<BreadcrumbItem>()
            {
                new BreadcrumbItem("Home", href: "/"),
            };
        }

        void IDisposable.Dispose()
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}
