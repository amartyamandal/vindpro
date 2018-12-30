# Firebase & Xamarin Authentication Sample
Facebook &amp; LinkedIn login with Firebase and Xamarin

![Facebook Auth](https://www.codeproject.com/KB/android/1205582/AAEAAQAAAAAAAA1oAAAAJGRiZjVlMjhjLTU5OTYtNGE3OS04MjRjLWVmOWI0NDg3NTcxNQ.png)
![LinkedIn Auth](https://www.codeproject.com/KB/android/1205582/AAEAAQAAAAAAAAtMAAAAJGUxMTZhMjZkLTBmMTgtNGEwMC1hZTU1LThmOWNmYjlkZjZlMg.png)

To start with have an account with firebase it’s best to have an account with google cloud to get access to google cloud control and then to firebase.

Go ahead and create/import your android app project and get “google-services.json” folks who do not know what it is you have lot to catch. Replace the Json file of your own! 

My Project has a sample “google-services.json” file copied from FirebaseAuthQuickStart example -have a look at this url to have the basic clear https://components.xamarin.com/gettingstarted/firebase-auth.

FirebaseAuthQuickStart  sample already has  google sign in implemented with firebase.

Firebase has few major authentication providers which are managed identity provider through firebase but so far Instagram, LinkedIn has not been included which are unmanaged identity providers and can be maintained by firebase with the use of custom firebase token.
When you have decided to use firebase to manage app users and broker authentication it’s better if we can manage unmanaged authentication providers too with firebase

The source code will provide a best practice to follow while implementing Facebook sign in with firebase -implemented with Xamarin Facebook sdk and Firebase sdk.

As well as a way to maintain LinkedIn users with firebase with xamarin.auth and firebase sdk.
Both of the implementation has different flavors for managed authentication providers like google and Facebook respective auth sdk will handle the heavy lifting- like in case of Facebook -specific activity should implement IFacebookCallback (Xamarin.Facebook) and IOnCompleteListener (Android.Gms.Tasks) and then firebase sdk will do the rest it has to use “AuthStateChanged” to check if an user logged in or not and handle the Facebook access token just like following simple example once a authentication credential is created it will make an entry to firebase table.

//
// handleFacebookAccessToken
//

private void handleFacebookAccessToken(AccessToken accessToken)
{
    AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken.Token);
    mAuth.SignInWithCredential(credential).AddOnCompleteListener(this, this);
}

But in case of LinkedIn its simple oAuth2 implementation complexity lies in creating the custom firebase token.

I have kept the token creation process in a separate shared project with a single class “Firebasetoken” make sure you have shared project template installed in your visual studio.

Remember this is just an example of how to do it – do not include this code or implement this at client side it involves a service account and other secrets which are potential security variabilities this supposed to be implemented at server end.

Following are the steps to follow.
In the Firebase console click the setting icon which is top left, next to the project name, and click 'Permissions'.

![Firebase Console](https://www.codeproject.com/KB/android/1205582/AAEAAQAAAAAAAApRAAAAJDdjZmE1ZTFmLWNhODAtNDNmNi04ZTBlLTU2YzQ4YTY4YjRlOA.png")

At the IAM and Admin page, click 'Service Accounts' on the left
Click 'Create Service Account' at the top, enter a 'Service Account Name', select 'Project->Editor' in the Role selection, tick the 'Furnish a new private key' checkbox and select JSON.
Click 'Create' and download the Service Account JSON file and keep it safe.
Open the Service Account JSON file in a suitable text editor and put the values into Firebasetoken class

![Firebase Token](https://www.codeproject.com/KB/android/1205582/AAEAAQAAAAAAAAz1AAAAJDU3MTZiNTkxLWM5ZGYtNDZhNS1hMDE4LWZjMWU2NmE2MzdjMg.png)

Remember to include BouncyCastle reference, In fact, here is a screenshot of the references you require to build this project correctly 

![References](https://www.codeproject.com/KB/android/1205582/AAEAAQAAAAAAAAw3AAAAJGUwNjdmOTJiLTQ1MmYtNDk4ZS05MmU2LTBlNDlkOWViYWVkMw.png)
