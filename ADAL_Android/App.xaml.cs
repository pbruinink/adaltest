using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ADAL_Android.Services;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace ADAL_Android
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            _ = checkPermissions();
        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private async Task checkPermissions()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);

            if (permissionStatus == PermissionStatus.Granted)
            {
                MainPage = new MainPage();
            }
        }
    }
}
