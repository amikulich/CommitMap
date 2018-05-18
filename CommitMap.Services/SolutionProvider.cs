
using System;
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

            workspace.WorkspaceFailed += (sender, args) =>
            {
                Console.WriteLine(args.Diagnostic.Message);
                Console.WriteLine("-----------------------");
            };

            return await workspace.OpenSolutionAsync("c:\\temp\\TestSol\\TestSol.sln");
        }
    }
}
