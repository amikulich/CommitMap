using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using CommitMap.Services.Changes.Bitbucket;

using Newtonsoft.Json;

namespace CommitMap.DataAccess
{
    public class BitbucketApiClient : IBitbucketApiClient
    {
        private const string AuthorizationPrefix = "Basic";

        private readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Initializes an instance of <see cref="BitbucketApiClient"/>
        /// </summary>
        public BitbucketApiClient()
        {
            httpClient.Timeout = new TimeSpan(0, 1, 0);
            //httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

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
                    var exception = new BitbucketIntegrationException("failed to extract info from Bitbucket API call", e);
                    throw exception;
                }
            }

            var oasysIntegrationException = new BitbucketIntegrationException("Bitbucket API GET call failed");

            throw oasysIntegrationException;
        }

        public async Task<Stream> LoadFile(string url)
        {
            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            if (response.Content != null)
            {
                return await response.Content.ReadAsStreamAsync();
            }

            var oasysIntegrationException = new BitbucketIntegrationException("Bitbucket API GET call failed");

            throw oasysIntegrationException;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
