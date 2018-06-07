using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Linq;
using System.Text;

using CommitMap.Services.Changes.Bitbucket.Urls;
using CommitMap.Services.Facade;

using Microsoft.CodeAnalysis.MSBuild;

namespace CommitMap.Services.Changes.Bitbucket.Solution
{
    public interface ISolutionProvider
    {
        Task<Microsoft.CodeAnalysis.Solution> GetSolution();
    }

    public class SolutionProvider : ISolutionProvider
    {
        private readonly IBitbucketApiClient _apiClient;

        /// <summary>
        /// Initializes an instance of <see cref="SolutionProvider"/>
        /// </summary>
        public SolutionProvider(IBitbucketApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Microsoft.CodeAnalysis.Solution> GetSolution()
        {
            string solutionFilePath;
            if (!CMSolutionProviderConfiguration.Current.UseLocal)
            {
                await DownloadSolution();
                solutionFilePath = GetSolutionFilePath(ConfigurationManager.AppSettings["WorkingFolder"]);
            }
            else
            {
                solutionFilePath = GetSolutionFilePath(CMSolutionProviderConfiguration.Current.LocalPath);
            }

            return await OpenSolution(solutionFilePath);
        }

        private string GetSolutionFilePath(string folderPath)
        {
            var solutionName = CMSolutionProviderConfiguration.Current.SolutionName.Replace(".sln","");

            return Directory.GetFiles(folderPath, $"{solutionName}.sln", SearchOption.AllDirectories).SingleOrDefault();
        }

        private async Task<Microsoft.CodeAnalysis.Solution> OpenSolution(string solutionPath)
        {
            var workspace = MSBuildWorkspace.Create();

            workspace.WorkspaceFailed += (sender, args) =>
                {
                    //Console.WriteLine(args.Diagnostic.Message);
                    //Console.WriteLine("-----------------------");
                };

            return await workspace.OpenSolutionAsync(solutionPath);
        }

        private async Task DownloadSolution()
        {
            var archiveFile = new ArchiveFilePath();
            try
            {
                if (!File.Exists(archiveFile)
                    || CMSolutionProviderConfiguration.Current.EnableCache
                    && File.GetLastWriteTimeUtc(archiveFile) < DateTime.UtcNow.AddMinutes(-CMSolutionProviderConfiguration.Current.CacheAgeMinutes))
                {
                    using (var stream = await _apiClient.LoadFile(new BitbucketDownloadRepoArchiveUrl()))
                    {
                        using (var fileStream = File.Open(archiveFile, FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }

                var repositoryFolder = new RepositoryFolderPath();
                if (Directory.Exists(repositoryFolder))
                {
                    Directory.Delete(repositoryFolder, true);
                }

                Directory.CreateDirectory(repositoryFolder);
                ZipFile.ExtractToDirectory(archiveFile, repositoryFolder, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                File.Delete(archiveFile);
                throw;
            }
        }
    }
}
