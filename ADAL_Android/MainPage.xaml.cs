using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADAL_Android.Services;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace ADAL_Android
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private AuthenticationResult authResult = null;

        public MainPage()
        {
            InitializeComponent();

            checkPermissions();

        }

        private async Task checkPermissions()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);

            if (permissionStatus == PermissionStatus.Granted)
            {
                LoginButton.Text = "Granted Contacts Permission";
                _ = getTokenAsync();
            }
            else
            {
                LoginButton.Text = "You haven't granted the Contacts Permission";
            }
        }


        private void OnClicked(object sender, EventArgs e)
        {
            (sender as Button).Text = "I was just clicked!";
            TextLabel.Text = "";

            _ = getTokenAsync();
        }

        private async Task getTokenAsync()
        {
            string authority = "https://login.microsoftonline.com/< tenantid >";
            string clientId = "< appID >";
            string redirectUri = "msauth://< packageIDofApp >/< base64key >";
            string resource = "< url of resource >";

            var auth = DependencyService.Get<IAuthenticator>();
            try
            {
                authResult = await auth.Authenticate(authority, resource, clientId, redirectUri);

                if (authResult != null)
                {
                    Debug.WriteLine("Successfully retrieved token:" + authResult.AccessToken);
                    TextLabel.Text = "Successfully retrieved token for user:" + authResult.UserInfo.DisplayableId + "\naccessToken:" + authResult.AccessToken;
                }
                else
                {
                    TextLabel.Text = "Failed to obtain an accessToken!";
                }
            }
            catch (Microsoft.IdentityModel.Clients.ActiveDirectory.AdalException ex)
            {
                TextLabel.Text = ex.ToString();
            }

        }
    }


}
