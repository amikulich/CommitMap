using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMap.Services.Semantics
{
    public interface IAnalyzer
    {
        Task<SymbolCallerInfo[]> FindAllCallers(IEnumerable<Document> documentsAffected, Solution solution);
    }

    public class Analyzer : IAnalyzer
    {
        public const byte MaxDepth = 10;

        public async Task<SymbolCallerInfo[]> FindAllCallers(IEnumerable<Document> documentsAffected,
            Solution solution)
        {
            var callers = new List<SymbolCallerInfo>();
            foreach (var document in documentsAffected)
            {
                var semanticModel = document.GetSemanticModelAsync().Result;

                foreach (var item in semanticModel.SyntaxTree.GetRoot().DescendantNodes())
                {
                    switch (item)
                    {
                        case ConstructorDeclarationSyntax ctor:
                        case MethodDeclarationSyntax method:
                        case PropertyDeclarationSyntax property:
                            callers.AddRange(await FindCallersRecursively(semanticModel.GetDeclaredSymbol(item), solution)); 
                            break;
                    }
                }
            }

            return callers.Distinct(new SymbolCallerInfoEqualityComparer()).ToArray();
        }

        private async Task<IEnumerable<SymbolCallerInfo>> FindCallersRecursively(ISymbol symbol, Solution solution, int currentDepth = 0)
        {
            var callers = (await SymbolFinder.FindCallersAsync(symbol, solution)).ToList();

            var nestedCallers = new List<SymbolCallerInfo>();
            foreach (var symbolCallerInfo in callers)
            {
                if (currentDepth < MaxDepth)
                {
                    nestedCallers.AddRange(FindCallersRecursively(symbolCallerInfo.CallingSymbol, solution, ++currentDepth).Result);
                }
                else
                {
                    Console.WriteLine("Reached the recursion limit");
                }
            }

            callers.AddRange(nestedCallers);

            return callers;
        }
    }
}

