using Android.App;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.CurrentActivity;
using ADAL_Android.Droid.Services;
using ADAL_Android.Services;

[assembly: Dependency(typeof(Authenticator))]

namespace ADAL_Android.Droid.Services
{
    public class Authenticator : IAuthenticator
    {

        public async Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri)
        {

            AuthenticationContext ac = new AuthenticationContext(authority);
            AuthenticationResult result = null;
            try
            {
                result = await ac.AcquireTokenSilentAsync(resource, clientId);
            }
            catch (AdalException adalException)
            {
                if (adalException.ErrorCode == AdalError.FailedToAcquireTokenSilently
                    || adalException.ErrorCode == AdalError.InteractionRequired)
                {
                    result = await ac.AcquireTokenAsync(resource, clientId, new Uri(returnUri),
                                                        new PlatformParameters(CrossCurrentActivity.Current.Activity, true, PromptBehavior.Auto));
                }
                else
                {
                    result = null;
                }
            }

            return result;

        }

        public bool hasLoginData(string authority)
        {
            var authContext = new AuthenticationContext(authority);
            return authContext.TokenCache.ReadItems().Any();
        }

        public void Logout(string authority)
        {
            var authContext = new AuthenticationContext(authority);
            if (authContext.TokenCache.ReadItems().Any()) authContext.TokenCache.Clear();
        }

    }
}

