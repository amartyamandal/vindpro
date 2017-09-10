using Jose;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace vindpro.firebase.shared
{
    class Firebasetoken
    {
        public static string firebasePrivateKey = @"-----BEGIN PRIVATE KEY-----\nREPLACE_ME\n-----END PRIVATE KEY-----\n";

        // Same for everyone
        public static string firebasePayloadAUD = "https://identitytoolkit.googleapis.com/google.identity.identitytoolkit.v1.IdentityToolkit";

        // client_email from the Service Account JSON file
        public static string firebasePayloadISS = "REPLACE_ME.iam.gserviceaccount.com";
        public static string firebasePayloadSUB = "REPLACE_ME.iam.gserviceaccount.com";

        // the token 'exp' - max 3600 seconds - see https://firebase.google.com/docs/auth/server/create-custom-tokens
        public static int firebaseTokenExpirySecs = 3600;

        private static RsaPrivateCrtKeyParameters _rsaParams;
        private static object _rsaParamsLocker = new object();
        public static string EncodeToken(string uid, Dictionary<string, object> claims)
        {
            // Get the RsaPrivateCrtKeyParameters if we haven't already determined them
            if (_rsaParams == null)
            {
                lock (_rsaParamsLocker)
                {
                    if (_rsaParams == null)
                    {
                        StreamReader sr = new StreamReader(GenerateStreamFromString(firebasePrivateKey.Replace(@"\n", "\n")));
                        var pr = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                        _rsaParams = (RsaPrivateCrtKeyParameters)pr.ReadObject();
                    }
                }
            }

            var payload = new Dictionary<string, object> {
            {"claims", claims}
            ,{"uid", uid}
            ,{"iat", SecondsSinceEpoch(DateTime.UtcNow)}
            ,{"exp", SecondsSinceEpoch(DateTime.UtcNow.AddSeconds(firebaseTokenExpirySecs))}
            ,{"aud", firebasePayloadAUD}
            ,{"iss", firebasePayloadISS}
            ,{"sub", firebasePayloadSUB}
            };

            return Jose.JWT.Encode(payload, Org.BouncyCastle.Security.DotNetUtilities.ToRSA(_rsaParams), JwsAlgorithm.RS256);
        }

        private static long SecondsSinceEpoch(DateTime dt)
        {
            TimeSpan t = dt - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }

        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        
    }
}
