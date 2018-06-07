using System.Configuration;

namespace CommitMap.Services.Changes.Bitbucket.Solution
{
    internal class RepositoryFolderPath
    {
        private readonly string _solutionName;

        private string _workingFolder;

        /// <summary>
        /// Initializes an instance of <see cref="RepositoryFolderPath"/>
        /// </summary>
        public RepositoryFolderPath()
        {
            _solutionName = ConfigurationManager.AppSettings["BitbucketRepositoryName"];
            _workingFolder = ConfigurationManager.AppSettings["WorkingFolder"];
        }

        public override string ToString()
        {
            return $"{_workingFolder}/{_solutionName}";
        }

        public static implicit operator string (RepositoryFolderPath folder)
        {
            return folder.ToString();
        }
    }
}
