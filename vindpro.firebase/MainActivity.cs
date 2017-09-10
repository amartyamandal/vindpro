using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace vindpro.firebase
{
    [Activity(Label = "vindpro firebase auth", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            FindViewById(Resource.Id.button_google_signin).Click += delegate
            {
                SignInGoogle();
            };
            FindViewById(Resource.Id.button_fb_signin).Click += delegate
            {
                SignInFacebook();
            };
            FindViewById(Resource.Id.button_linkedin_signin).Click += delegate
            {
                SignInLinkedIn();
            };

        }

        private void SignInLinkedIn()
        {
            StartActivity(typeof(LinkedInSignInActivity));
        }

        private void SignInFacebook()
        {
            StartActivity(typeof(FacebookSignInActivity));
        }

        private void SignInGoogle()
        {
            StartActivity(typeof(GoogleSignInActivity));
        }
    }
}

