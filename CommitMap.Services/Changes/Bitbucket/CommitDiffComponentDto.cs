using Newtonsoft.Json;

namespace CommitMap.Services.Changes.Bitbucket
{
    public class CommitDiffComponentDto
    {
        [JsonProperty("parent", Required = Required.Always)]
        public string Parent { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("toString", Required = Required.Always)]
        public string FullPath { get; set; }

    }
}