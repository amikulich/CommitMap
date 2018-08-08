using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommitMap.Services.Facade;
using Newtonsoft.Json;

namespace CommitMap.Services.Integration
{
    public interface IAnalysisResultsApiClient
    {
        Task PostAsync(Uri callbackUrl, AnalysisResult analysisResult);
    }

    public class AnalysisResultsApiClient : IAnalysisResultsApiClient
    {
        private readonly HttpClient _client;

        public AnalysisResultsApiClient()
        {
            _client = new HttpClient
            {
            };
        }

        public async Task PostAsync(Uri callbackUrl, AnalysisResult analysisResult)
        {
            var body = JsonConvert.SerializeObject(ToDto(analysisResult));

            await _client.PostAsync(callbackUrl, new StringContent(body, Encoding.UTF8, "application/json"));
        }

        AnalysisResultDto ToDto(AnalysisResult analysisResult)
        {
            return new AnalysisResultDto()
            {
                CompletedAt = analysisResult.CompletedAt,
                Endpoints = analysisResult.AffectedEndpoints.Select(e => new EndpointDto()
                {
                    ApplicationName = e.ApplicationName,
                    Controller = e.Controller,
                    HttpMethod = e.HttpMethod,
                    IsLabelled = e.IsLabelled,
                    Method = e.Method,
                    Namespace = e.Namespace,
                    Url = e.Url,
                }).ToArray()
            };
        }


        private class AnalysisResultDto
        {
            public DateTime CompletedAt { get; set; }

            public EndpointDto[] Endpoints { get; set; }
        }

        private class EndpointDto
        {
            public string Url { get; set; }

            public string HttpMethod { get; internal set; }

            public bool IsLabelled { get; internal set; }

            public string Namespace { get; internal set; }

            public string Controller { get; internal set; }

            public string Method { get; internal set; }

            public string ApplicationName { get; internal set; }
        }
    }
}
