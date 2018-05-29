using Newtonsoft.Json;

namespace CommitMap.Services.Changes.Bitbucket
{
    public class CommitDiffValueDto
    {
        [JsonProperty("type", Required = Required.Always)]
        public string ChangeType { get; set; }

        [JsonProperty("nodeType", Required = Required.Always)]
        public string NodeType { get; set; }

        [JsonProperty("path", Required = Required.Always)]
        public CommitDiffComponentDto Document { get; set; }
    }
}