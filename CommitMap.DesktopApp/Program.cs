using System;
using System.Linq;

using CommitMap.DataAccess;
using CommitMap.Services;
using CommitMap.Services.Changes;
using CommitMap.Services.Semantics;

namespace CommitMap.DesktopApp
{
    class Program
    {
        private static ICommitScanner _commitScanner = new CommitScanner(new BitBucketApiClient());

        private static ISolutionProvider _solutionProvider = new SolutionProvider();

        private static IAnalyzer analyzer = new Analyzer();

        static void Main(string[] args)
        {
            var solution = _solutionProvider.GetSolution().Result;

            var modifiedDocumentsNames = _commitScanner.GetModifiedDocuments("2575aaf49418d86bae0e98a0f6a4613da0d6cfb6", "5f0fbaabbea1e9f7b1cf3caba568441ab3967684").Result;

            var documentsAffected = solution.Projects
                .SelectMany(p => p.Documents.Where(doc => modifiedDocumentsNames.Contains(doc.Name)));

            var usages = analyzer.FindAllCallers(documentsAffected, solution).Result;

            var endPoints = usages.Where(u => u.CallingSymbol.ContainingType.Name.EndsWith("Controller"));

            Console.WriteLine("Results:");
            int i = 1;
            foreach (var endPoint in endPoints.Distinct(new SymbolCallerInfoEqualityComparer()))
            {
                Console.WriteLine($"{i++}. {endPoint.CallingSymbol}");
            }
        }
    }
}
