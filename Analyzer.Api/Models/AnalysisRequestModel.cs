namespace Analyzer.Api.Models
{
    public class AnalysisRequestModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public string CallbackUrl { get; set; }
    }
}