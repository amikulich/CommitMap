using System.Linq;
using System.Threading.Tasks;

using CommitMap.Services.Changes.Bitbucket;
using CommitMap.Services.Changes.Bitbucket.Urls;

namespace CommitMap.Services.Changes
{
    public class CommitScanner : ICommitScanner
    {
        private readonly IBitbucketApiClient _apiClient;

        /// <summary>
        /// Initializes an instance of <see cref="CommitScanner"/>
        /// </summary>
        public CommitScanner(IBitbucketApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string[]> GetModifiedDocuments(string firstCommit, string lastCommit)
        {
            var url = new BitbucketCommitDiffUrl(firstCommit, lastCommit);

            var commitDiff = await _apiClient.Get<CommitDiffDto>(url);

            return commitDiff
                .Values
                .Select(v => v.Document.FullPath.Replace("/", "\\"))
                .ToArray();
        }
    }
}
