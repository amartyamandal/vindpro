using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Facebook;
using Android.Gms.Tasks;
using Firebase;
using Firebase.Auth;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Login;

[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/facebook_app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace vindpro.firebase
{
    [Activity(Label = "Facebook SignIn")]
    public class FacebookSignInActivity : BaseActivity, IFacebookCallback, IOnCompleteListener
    {
        private ICallbackManager mCallbackManager;
        private FirebaseAuth mAuth;
        TextView mStatusTextView;
        TextView mDetailTextView;
        const string TAG = "FacebookActivity";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            SetContentView(Resource.Layout.activity_facebook);
            // Views
            mStatusTextView = FindViewById<TextView>(Resource.Id.status);
            mDetailTextView = FindViewById<TextView>(Resource.Id.detail);
            // Button listeners
            FirebaseApp.InitializeApp(this);
            mAuth = FirebaseAuth.Instance;
            LoginButton fblogin = FindViewById<LoginButton>(Resource.Id.button_facebook_login);
            fblogin.Click += delegate
            {
                mCallbackManager = CallbackManagerFactory.Create();
                fblogin.RegisterCallback(mCallbackManager, this);
            };
            FindViewById(Resource.Id.button_facebook_signout).Click += delegate
            {
                SignOut();
            };

        }

        void AuthStateChanged(object sender, FirebaseAuth.AuthStateEventArgs e)
        {
            var user = e.Auth.CurrentUser;
            if (user != null)
            {
                // User is signed in
                Android.Util.Log.Debug(TAG, "onAuthStateChanged:signed_in:" + user.Uid);
            }
            else
            {
                // User is signed out
                Android.Util.Log.Debug(TAG, "onAuthStateChanged:signed_out");
            }
            // [START_EXCLUDE]
            UpdateUI(user);
            // [END_EXCLUDE]
        }

        // [START on_start_add_listener]
        protected override void OnStart()
        {
            base.OnStart();
            mAuth.AuthState += AuthStateChanged;
        }
        // [END on_start_add_listener]

        //[START on_stop_remove_listener]
        protected override void OnStop()
        {
            base.OnStop();
            mAuth.AuthState -= AuthStateChanged;
        }
        // [END on_stop_remove_listener]
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            var resultCodeNum = 0;
            switch (resultCode)
            {
                case Result.Ok:
                    resultCodeNum = -1;
                    break;

                case Result.Canceled:
                    resultCodeNum = 0;
                    break;

                case Result.FirstUser:
                    resultCodeNum = 1;
                    break;
            }
            mCallbackManager.OnActivityResult(requestCode, resultCodeNum, data);
        }

        private void UpdateUI(FirebaseUser user)
        {
            HideProgressDialog();
            if (user != null)
            {
                mStatusTextView.Text = GetString(Resource.String.facebook_status_fmt, user.DisplayName);
                mDetailTextView.Text = GetString(Resource.String.firebase_status_fmt, user.Uid);

                FindViewById(Resource.Id.button_facebook_login).Visibility = ViewStates.Gone;
                FindViewById(Resource.Id.button_facebook_signout).Visibility = ViewStates.Visible;
            }
            else
            {
                mStatusTextView.SetText(Resource.String.signed_out);
                mDetailTextView.Text = null;

                FindViewById(Resource.Id.button_facebook_login).Visibility = ViewStates.Visible;
                FindViewById(Resource.Id.button_facebook_signout).Visibility = ViewStates.Gone;
            }
        }

        private void handleFacebookAccessToken(AccessToken accessToken)
        {

            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken.Token);
            mAuth.SignInWithCredential(credential).AddOnCompleteListener(this, this);
        }
        private void SignOut()
        {
            // Firebase sign out
            mAuth.SignOut();

            UpdateUI(null);
        }


        public void OnCancel()
        {
            UpdateUI(null);
        }



        public void OnError(FacebookException error)
        {
            throw new NotImplementedException();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            LoginResult loginResult = result as LoginResult;
            handleFacebookAccessToken(loginResult.AccessToken);
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                FirebaseUser user = mAuth.CurrentUser;
            }
            else
            {
                Toast.MakeText(this, "Authentication failed.", ToastLength.Short).Show();
            }
        }
    }
}