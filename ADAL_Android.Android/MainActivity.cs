using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Accounts;
using Android.Content;
using Plugin.CurrentActivity;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Android.Support.V4.Content;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace ADAL_Android.Droid
{
    [Activity(Label = "ADAL_Android", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            RequestPermissions();

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.GetAccounts) == (int)Android.Content.PM.Permission.Granted)
            {
                enableBroker();
            }
            else
            {
                Toast.MakeText(this, "We need the contacts permission for authentication", ToastLength.Long).Show();
            }


            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();

        }

        private void enableBroker()
        {
            string WORK_AND_SCHOOL_TYPE = "com.microsoft.workaccount";
            // See the Android docs for customizing the UI https://developers.google.com/android/reference/com/google/android/gms/common/AccountPicker
            Intent intent = AccountManager.NewChooseAccountIntent(null, null, new[] { WORK_AND_SCHOOL_TYPE }, null, null, null, null);
            // Start an activity with this intent, e.g. 
            CrossCurrentActivity.Current.Activity.StartActivity(intent);

            // this variable should now have all the accounts in the broker
            var accManager = AccountManager.Get(Application.Context);
            var accounts = accManager.GetAccountsByType(WORK_AND_SCHOOL_TYPE);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }

        private void RequestPermissions()
        {
            this.RequestPermissions(new string[] { Manifest.Permission.GetAccounts }, 0);
            this.RequestPermissions(new string[] { Manifest.Permission.ManageAccounts }, 1);
            this.RequestPermissions(new string[] { Manifest.Permission.UseCredentials }, 2);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }
    }
}