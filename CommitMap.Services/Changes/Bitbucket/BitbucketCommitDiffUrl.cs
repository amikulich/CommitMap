using System.Configuration;

namespace CommitMap.Services.Changes.Bitbucket
{
    public class BitbucketCommitDiffUrl
    {
        public string FromCommit { get; }

        public string ToCommit { get; }

        /// <summary>
        /// Initializes an instance of <see cref="BitbucketCommitDiffUrl"/>
        /// </summary>
        public BitbucketCommitDiffUrl(string fromCommit, string toCommit)
        {
            FromCommit = fromCommit;
            ToCommit = toCommit;
        }

        public override string ToString()
        {
            var url = ConfigurationManager.AppSettings["BitbucketUrl"];
            var projectName = ConfigurationManager.AppSettings["BitbucketProjectName"];
            var repoName = ConfigurationManager.AppSettings["BitbucketRepositoryName"];

            return $"{url}/rest/api/1.0/projects/{projectName}/repos/{repoName}/compare/changes?&limit=1000&until=master&from={FromCommit}&to={ToCommit}";
        }

        public static implicit operator string(BitbucketCommitDiffUrl url)
        {
            return url.ToString();
        }
    }
}