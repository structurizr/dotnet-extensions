using System;
using System.Text;

namespace Structurizr.Api
{
    internal class HmacAuthorizationHeader
    {

        private string apiKey;
        private string hmac;

        public HmacAuthorizationHeader(string apiKey, string hmac)
        {
            this.apiKey = apiKey;
            this.hmac = hmac;
        }

        public override string ToString()
        {
            return apiKey + ":" + Convert.ToBase64String(Encoding.UTF8.GetBytes(hmac));
        }

    }
}
