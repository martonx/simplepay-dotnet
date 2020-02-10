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
            var keyByteArray = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmac = new HMACSHA384(keyByteArray))
            {
                hmac.ComputeHash(messageBytes);
                return Convert.ToBase64String(hmac.Hash);
            }

        }
    }
}
