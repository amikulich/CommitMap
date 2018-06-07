using System.Configuration;

namespace CommitMap.Services.Changes.Bitbucket.Urls
{
    internal class BitbucketCommitDiffUrl
    {
        public string FirstCommit { get; }

        public string LastCommit { get; }

        /// <summary>
        /// Initializes an instance of <see cref="BitbucketCommitDiffUrl"/>
        /// </summary>
        public BitbucketCommitDiffUrl(string firstCommit, string lastCommit)
        {
            FirstCommit = firstCommit;
            LastCommit = lastCommit;
        }

        public override string ToString()
        {
            var url = ConfigurationManager.AppSettings["BitbucketUrl"];
            var projectName = ConfigurationManager.AppSettings["BitbucketProjectName"];
            var repoName = ConfigurationManager.AppSettings["BitbucketRepositoryName"];

            return $"{url}/rest/api/1.0/projects/{projectName}/repos/{repoName}/compare/changes?&limit=1000&until=master&from={LastCommit}&to={FirstCommit}";
        }

        public static implicit operator string(BitbucketCommitDiffUrl url)
        {
            return url.ToString();
        }
    }
}