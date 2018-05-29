using System;
using System.Linq;

using CommitMap.Services.Changes;
using CommitMap.Services.Semantics;

namespace CommitMap.Services.Facade
{
    public interface ICommitMapEngine
    {
        void Run();
    }

    public class CommitMapEngine : ICommitMapEngine
    {
        private readonly ISolutionProvider _solutionProvider;

        private readonly ICommitScanner _commitScanner;

        private readonly IAnalyzer _analyzer;

        /// <summary>
        /// Initializes an instance of <see cref="CommitMapEngine"/>
        /// </summary>
        public CommitMapEngine(ISolutionProvider solutionProvider, ICommitScanner commitScanner, IAnalyzer analyzer)
        {
            _solutionProvider = solutionProvider;
            _commitScanner = commitScanner;
            _analyzer = analyzer;
        }

        public void Run()
        {
            var solution = _solutionProvider.GetSolution().Result;

            var modifiedDocumentsNames = _commitScanner.GetModifiedDocuments("2575aaf49418d86bae0e98a0f6a4613da0d6cfb6", "5f0fbaabbea1e9f7b1cf3caba568441ab3967684").Result;

            var documentsAffected = solution.Projects
                .SelectMany(p => p.Documents.Where(doc => modifiedDocumentsNames.Contains(doc.Name)));

            var usages = _analyzer.FindAllCallers(documentsAffected, solution).Result;

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
