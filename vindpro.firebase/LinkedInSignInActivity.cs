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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Auth;
using Firebase.Auth;
using vindpro.firebase.shared;

namespace vindpro.firebase
{
    [Activity(Label = "LinkedInSignInActivity")]
    public class LinkedInSignInActivity : BaseActivity
    {
        TextView mStatusTextView;
        TextView mDetailTextView;
        OAuth2Authenticator auth;
        private FirebaseAuth mAuth;
        //var tokenHandler = new JwtSecurityTokenHandler();
        const string TAG = "LinkedInSignInActivity";

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

           SetContentView(Resource.Layout.activity_linkedin);
            mStatusTextView = FindViewById<TextView>(Resource.Id.status);
            mDetailTextView = FindViewById<TextView>(Resource.Id.detail);
            mAuth = FirebaseAuth.Instance;
            auth = new OAuth2Authenticator(
                clientId: "REPLACE_ME",
                clientSecret: "REPLACE_ME",
                scope: "r_basicprofile r_emailaddress",
                authorizeUrl: new Uri("https://www.linkedin.com/uas/oauth2/authorization"),
                redirectUrl: new Uri("http://localhost/"),
                accessTokenUrl: new Uri("https://www.linkedin.com/uas/oauth2/accessToken")

            );
            // If authorization succeeds or is canceled, .Completed will be fired.
            // Button listeners
            FindViewById(Resource.Id.button_linkedin_login).Click += delegate
            {
                SignIn();
            };
            FindViewById(Resource.Id.button_linkedin_signout).Click += delegate
            {
                SignOut();
            };
            auth.AllowCancel = true;
            auth.Completed += Auth_Completed;
            //StartActivity(auth.GetUI(this.ApplicationContext));
        }

        private void SignOut()
        {
            //var access_token = values["access_token"];
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage("Not Implemented");
            builder.SetPositiveButton("Ok", (o, e) => { });
            builder.Create().Show();
        }

        private void SignIn()
        {
            StartActivity(auth.GetUI(this.ApplicationContext));

        }

        private void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                
                FindViewById(Resource.Id.button_linkedin_login).Visibility = ViewStates.Gone;
                FindViewById(Resource.Id.button_linkedin_signout).Visibility = ViewStates.Visible;
                var values = e.Account.Properties;
                var access_token = values["access_token"];
                try
                {

                    var request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v1/people/~?oauth2_access_token=" + access_token + "&format=json", ""));//summary,educations,three-current-positions,honors-awards,site-standard-profile-request,location,api-standard-profile-request,phone-numbers,picture-url
                    request.ContentType = "application/json";
                    request.Method = "GET";

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            var content = reader.ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(content))
                            {
                                var result = JsonConvert.DeserializeObject<LinkedInUser>(content);
                                mDetailTextView.Text = "Name:" + result.firstName + " " + result.lastName + "/ ID:" + result.id;
                                CreateFirebaseToken(result.id);
                            }

                            
                        }
                    }
                }
                catch (Exception exx)
                {
                    FindViewById(Resource.Id.button_linkedin_login).Visibility = ViewStates.Visible;
                    FindViewById(Resource.Id.button_linkedin_signout).Visibility = ViewStates.Gone;
                    mDetailTextView.Text = exx.ToString();
                }
            }
        }

       
        private void CreateFirebaseToken(string linkedinUID)
        {
            var uid = linkedinUID;
            var claims = new Dictionary<string, object> { { "premium_account", true } };
            var token = Firebasetoken.EncodeToken(uid, claims);
            mAuth.SignInWithCustomToken(token);
            //var token = tokenHandler.CreateToken(
        }
    }
    public class SiteStandardProfileRequest
    {
        public string url { get; set; }
    }

    public class LinkedInUser
    {
        public string firstName { get; set; }
        public string headline { get; set; }
        public string id { get; set; }
        public string lastName { get; set; }
        public SiteStandardProfileRequest siteStandardProfileRequest { get; set; }
    }
}