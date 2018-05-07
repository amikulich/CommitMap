
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
            return await workspace.OpenSolutionAsync("C:/Users/Dell/Documents/Visual Studio 2015/Projects/SharpMind/SharpMind.sln");
        }
    }
}
