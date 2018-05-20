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

            return callers.Distinct().ToArray();
        }

        private async Task<IEnumerable<SymbolCallerInfo>> FindCallersRecursively(ISymbol symbol, Solution solution)
        {
            var callers = (await SymbolFinder.FindCallersAsync(symbol, solution)).ToList();

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

