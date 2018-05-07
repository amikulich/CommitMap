
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CommitMap.Services
{
    public interface ISolutionProvider
    {
        Task<Solution> GetSolution();
    }

    public class SolutionProvider : ISolutionProvider
    {
        public async Task<Solution> GetSolution()
        {
            var workspace = MSBuildWorkspace.Create();
            return await workspace.OpenSolutionAsync("C:\\Videology\\DSP-UI\\DSP\\Code\\DSP\\DSP.sln");
        }
    }
}
