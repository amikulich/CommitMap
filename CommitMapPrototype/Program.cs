using System;
using System.Linq;
using CommitMap.Services;
using CommitMap.Services.Semantics;

namespace CommitMapPrototype
{
    class Program
    {
        private static ICommitScanner _commitScanner = new CommitScanner();

        private static ISolutionProvider _solutionProvider = new SolutionProvider();

        private static IAnalyzer analyzer = new Analyzer();

        static void Main(string[] args)
        {
            var solution = _solutionProvider.GetSolution().Result;

            var modifiedDocumentsNames = _commitScanner.GetModifiedDocuments("somehash");
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
