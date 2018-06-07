using System.Configuration;

namespace CommitMap.Services.Changes.Bitbucket.Urls
{
    public class BitbucketDownloadRepoArchiveUrl
    {
        public override string ToString()
        {
            var url = ConfigurationManager.AppSettings["BitbucketUrl"];
            var projectName = ConfigurationManager.AppSettings["BitbucketProjectName"];
            var repoName = ConfigurationManager.AppSettings["BitbucketRepositoryName"];

            return $"{url}/rest/api/1.0/projects/{projectName}/repos/{repoName}/archive";
        }

        public static implicit operator string(BitbucketDownloadRepoArchiveUrl url)
        {
            return url.ToString();
        }
    }
}
