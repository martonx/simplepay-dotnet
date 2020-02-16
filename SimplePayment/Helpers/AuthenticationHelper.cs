using System;
using System.Security.Cryptography;
using System.Text;

namespace SimplePayment.Helpers
{
    public class AuthenticationHelper
    {
        //TODO it's not right
        public string HMACSHA384Encode(string key, string message)
        {
            var keyByteArray = Encoding.UTF32.GetBytes(key);
            var messageBytes = Encoding.UTF32.GetBytes(message);
            using (var hmac = new HMACSHA384(keyByteArray))
            {
                hmac.ComputeHash(messageBytes);
                return Convert.ToBase64String(hmac.Hash, Base64FormattingOptions.None);
            }
        }

        public bool IsMessageValid(string key, string message, string signature)
        {
            var reGenerateHash = HMACSHA384Encode(key, message);
            return signature == reGenerateHash;
        }

        public string GenerateSalt()
        {
            byte[] salt = new byte[32];
            var rngProvider = new RNGCryptoServiceProvider();
            rngProvider.GetBytes(salt);
            return Convert.ToBase64String(salt, Base64FormattingOptions.None);
        }
    }
}
