using System;

namespace CommitMap.Services.Facade
{
    public class AnalysisResult
    {
        public DateTime CompletedAt { get; internal set; }

        public Endpoint[] AffectedEndpoints { get; internal set; }
    }

    public class Endpoint
    {
        public string ApplicationName { get; internal set; }

        public string Url { get; internal set; }

        public string HttpMethod { get; internal set; }

        public string Controller { get; internal set; }

        public string Method { get; internal set; }

        public bool IsLabelled { get; internal set; }

        public string Namespace { get; internal set; }

        public override string ToString()
        {
            return $"{HttpMethod} {Url}";
        }

        public Endpoint WithAppName(string appName)
        {
            this.ApplicationName = appName;
            return this;
        }
    }
}
