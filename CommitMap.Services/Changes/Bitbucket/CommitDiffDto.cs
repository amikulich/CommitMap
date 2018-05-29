using Newtonsoft.Json;

namespace CommitMap.Services.Changes.Bitbucket
{
    public class CommitDiffDto
    {
        [JsonProperty("values", Required = Required.Always)]
        public CommitDiffValueDto[] Values { get; set; }

        [JsonProperty("isLastPage", Required = Required.Always)]
        public bool IsLastPage { get; set; }

        [JsonProperty("nextPageStart", Required = Required.AllowNull)]
        public int? NextPageStart { get; set; }
    }
}