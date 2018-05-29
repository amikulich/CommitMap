using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using CommitMap.Services.Changes;
using CommitMap.Services.Changes.Bitbucket;

using Newtonsoft.Json;

namespace CommitMap.DataAccess
{
    public class BitBucketApiClient : IBitbucketApiClient
    {
        private const string AuthorizationPrefix = "Basic";

        private readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Initializes an instance of <see cref="BitBucketApiClient"/>
        /// </summary>
        public BitBucketApiClient()
        {
            httpClient.Timeout = new TimeSpan(0, 1, 0);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var authToken = ConfigurationManager.AppSettings["BitbucketAuthToken"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationPrefix, authToken);
        }

        public async Task<TResponse> Get<TResponse>(string url)
        {
            string responseBody = string.Empty;

            var response = await httpClient.GetAsync(url);

            if (response.Content != null)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return JsonConvert.DeserializeObject<TResponse>(responseBody);
                }
                catch (Exception e)
                {
                    var exception = new BitbucketIntegrationException("failed to extract info from Jira call", e);
                    throw exception;
                }
            }

            var oasysIntegrationException = new BitbucketIntegrationException("Jira GET call failed");

            throw oasysIntegrationException;
        }


        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
