using System.Configuration;

namespace CommitMap.Services.Changes.Bitbucket.Solution
{
    internal class ArchiveFilePath
    {
        private readonly string _fileName;

        private readonly ArchiveExtensions _extension = ArchiveExtensions.Zip;

        private string _workingFolder;

        /// <summary>
        /// Initializes an instance of <see cref="ArchiveFilePath"/>
        /// </summary>
        public ArchiveFilePath()
        {
            _workingFolder = ConfigurationManager.AppSettings["WorkingFolder"];
            _fileName = ConfigurationManager.AppSettings["BitbucketRepositoryName"];
        }

        public override string ToString()
        {
            return $"{_workingFolder}/{_fileName}.{_extension.ToString()}";
        }

        public static implicit operator string (ArchiveFilePath path)
        {
            return path.ToString();
        }
    }
}
