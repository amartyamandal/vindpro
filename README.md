# Firebase & Xamarin Authentication Sample
Facebook &amp; LinkedIn login with Firebase and Xamarin

![Facebook Auth](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAA1oAAAAJGRiZjVlMjhjLTU5OTYtNGE3OS04MjRjLWVmOWI0NDg3NTcxNQ.png?raw=true "Facebook Auth")
![LinkedIn Auth](https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAtMAAAAJGUxMTZhMjZkLTBmMTgtNGEwMC1hZTU1LThmOWNmYjlkZjZlMg.png?raw=true "LinkedIn Auth")

To start with have an account with firebase it’s best to have an account with google cloud to get access to google cloud control and then to firebase.

Go ahead and create/import your android app project and get “google-services.json” folks who do not know what it is you have lot to catch. Replace the Json file of your own! 

My Project has a sample “google-services.json” file copied from FirebaseAuthQuickStart example -have a look at this url to have the basic clear https://components.xamarin.com/gettingstarted/firebase-auth.

FirebaseAuthQuickStart  sample already has  google sign in implemented with firebase.

Firebase has few major authentication providers which are managed identity provider through firebase but so far Instagram, LinkedIn has not been included which are unmanaged identity providers and can be maintained by firebase with the use of custom firebase token.
When you have decided to use firebase to manage app users and broker authentication it’s better if we can manage unmanaged authentication providers too with firebase

The source code will provide a best practice to follow while implementing Facebook sign in with firebase -implemented with Xamarin Facebook sdk and Firebase sdk.

As well as a way to maintain LinkedIn users with firebase with xamarin.auth and firebase sdk.
Both of the implementation has different flavors for managed authentication providers like google and Facebook respective auth sdk will handle the heavy lifting- like in case of Facebook -specific activity should implement IFacebookCallback (Xamarin.Facebook) and IOnCompleteListener (Android.Gms.Tasks) and then firebase sdk will do the rest it has to use “AuthStateChanged” to check if an user logged in or not and handle the Facebook access token just like following simple example once a authentication credential is created it will make an entry to firebase table.
