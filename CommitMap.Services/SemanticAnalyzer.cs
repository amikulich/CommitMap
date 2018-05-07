using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
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
                    var symbol = semanticModel.GetSymbolInfo(item);

                    if (symbol.Symbol != null)
                    {
                        callers.AddRange(await FindCallersRecursively(symbol.Symbol, solution).ConfigureAwait(false));
                    }
                }
            }

            return callers.ToArray();
        }

        private async Task<IEnumerable<SymbolCallerInfo>> FindCallersRecursively(ISymbol symbol, Solution solution)
        {
            var callers = (SymbolFinder.FindCallersAsync(symbol, solution).Result).ToList();

            IEnumerable<SymbolCallerInfo> nestedCallers = new List<SymbolCallerInfo>();
            foreach (var symbolCallerInfo in callers)
            {
                nestedCallers = await FindCallersRecursively(symbolCallerInfo.CallingSymbol, solution);
            }

            callers.AddRange(nestedCallers);

            return callers;
        }
    }
}
