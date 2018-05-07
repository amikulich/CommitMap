using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMap.Services
{
    public interface ISemanticAnalyzer
    {
        Task<SymbolCallerInfo[]> FindAllCallers(IEnumerable<Document> documentsAffected, Solution solution);
    }

    public class SemanticAnalyzer : ISemanticAnalyzer
    {
        public async Task<SymbolCallerInfo[]> FindAllCallers(IEnumerable<Document> documentsAffected,
            Solution solution)
        {
            var callers = new List<SymbolCallerInfo>();
            foreach (var document in documentsAffected)
            {
                var semanticModel = document.GetSemanticModelAsync().Result;

                foreach (var item in semanticModel.SyntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>())
                {
                    var symbol = semanticModel.GetDeclaredSymbol(item);

                    if (symbol != null)
                    {
                        var newCallers = FindCallersRecursively(symbol, solution).Result;
                        callers.AddRange(newCallers);
                    }
                }
            }

            return callers.Distinct().ToArray();
        }

        private async Task<IEnumerable<SymbolCallerInfo>> FindCallersRecursively(ISymbol symbol, Solution solution)
        {
            var callers = (SymbolFinder.FindCallersAsync(symbol, solution).Result).ToList();

            var nestedCallers = new List<SymbolCallerInfo>();
            foreach (var symbolCallerInfo in callers)
            {
                nestedCallers.AddRange(FindCallersRecursively(symbolCallerInfo.CallingSymbol, solution).Result);
            }

            callers.AddRange(nestedCallers);

            return callers;
        }
    }
}

