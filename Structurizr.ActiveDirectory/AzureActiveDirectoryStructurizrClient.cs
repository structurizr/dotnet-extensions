using Structurizr.Api;
using System;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Globalization;
using System.Net.Http.Headers;

namespace Structurizr.ActiveDirectory
{

    /// <summary>
    /// An extension of the regular StructurizrClient that allows an Active Directory authentication context
    /// to be injected into the API requests. Use this if your Structurizr on-premises installation
    /// resides behind Active Directory.
    /// </summary>
    public class AzureActiveDirectoryStructurizrClient : StructurizrClient
    {

        private AuthenticationResult authenticationResult;

        public AzureActiveDirectoryStructurizrClient(string apiUrl, string apiKey, string apiSecret,
            string azureActiveDirectoryInstance, string activeDirectoryTenant, string clientId, string clientKey, string resourceId) : base(apiUrl, apiKey, apiSecret)
        {
            string authority = String.Format(CultureInfo.InvariantCulture, azureActiveDirectoryInstance, activeDirectoryTenant);
            AuthenticationContext authenticationContext = new AuthenticationContext(authority);
            ClientCredential clientCredential = new ClientCredential(clientId, clientKey);

            try
            {
                authenticationResult = authenticationContext.AcquireTokenAsync(resourceId, clientCredential).Result;
            }
            catch (AdalException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        protected override HttpClient createHttpClient()
        {
            HttpClient httpClient = base.createHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);

            return httpClient;
        }

    }
}
