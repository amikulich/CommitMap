using System;
using System.Collections.Generic;
using System.Linq;
using CommitMap.Services;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMapPrototype
{
    class Program
    {
        private static ICommitScanner _commitScanner = new CommitScanner();

        private static ISolutionProvider _solutionProvider = new SolutionProvider();

        private static ISemanticAnalyzer _semanticAnalyzer = new SemanticAnalyzer();

        static void Main(string[] args)
        {
            var solution = _solutionProvider.GetSolution().Result;

            var modifiedDocumentsNames = _commitScanner.GetModifiedDocuments("somehash");
            var documentsAffected = solution.Projects
                .SelectMany(p => p.Documents.Where(doc => modifiedDocumentsNames.Contains(doc.Name))); 

            var usages = _semanticAnalyzer.FindAllCallers(documentsAffected, solution).Result;

            var endPoints = usages.Where(u => u.CallingSymbol.ContainingType.Name.EndsWith("Controller"));

            Console.WriteLine("Results:");
            foreach (var endPoint in endPoints)
            {
                Console.WriteLine(endPoint.CallingSymbol);
            }
        }
    }
}
