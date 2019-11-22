using System;
using System.Security.Cryptography;
using System.Text;

namespace Structurizr.Api
{
    internal class HashBasedMessageAuthenticationCode
    {

        private string apiSecret;

        internal HashBasedMessageAuthenticationCode(string apiSecret)
        {
            this.apiSecret = apiSecret;
        }

        public string Generate(string content)
        {
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            byte[] hash = hmac.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

    }
}
