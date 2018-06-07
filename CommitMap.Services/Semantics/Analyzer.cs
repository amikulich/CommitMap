using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommitMap.Services.Facade;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMap.Services.Semantics
{
    public interface IAnalyzer
    {
        Task<IEnumerable<Endpoint>> FindAffectedEndpoints(IEnumerable<Document> documentsAffected, Solution solution);
    }

    public class Analyzer : IAnalyzer
    {
        public const byte MaxDepth = 10;

        public async Task<IEnumerable<Endpoint>> FindAffectedEndpoints(IEnumerable<Document> documentsAffected, Solution solution)
        {
            var callers = new List<SymbolCallerInfo>();
            var references = new List<ReferenceLocation>();
            foreach (var document in documentsAffected)
            {
                var semanticModel = await document.GetSemanticModelAsync();

                foreach (var item in semanticModel.SyntaxTree.GetRoot().DescendantNodes())
                {
                    switch (item)
                    {
                        case ClassDeclarationSyntax classDeclaration:
                            references.AddRange(await ProcessClass(semanticModel.GetDeclaredSymbol(item), solution));
                            break;
                        case ConstructorDeclarationSyntax ctor:
                        case MethodDeclarationSyntax method:
                        case PropertyDeclarationSyntax property:
                            callers.AddRange(await FindCallersRecursively(semanticModel.GetDeclaredSymbol(item), solution)); 
                            break;
                    }
                }
            }

            foreach (var referenceLocation in references)
            {
                var semanticModel = await referenceLocation.Document.GetSemanticModelAsync();

                foreach (var item in semanticModel.SyntaxTree.GetRoot().DescendantNodes())
                {
                    switch (item)
                    {
                        //case ClassDeclarationSyntax classDeclaration:
                            //references.AddRange(await ProcessClass(semanticModel.GetDeclaredSymbol(item), solution));
                          //  break;
                        case ConstructorDeclarationSyntax ctor:
                        case MethodDeclarationSyntax method:
                        case PropertyDeclarationSyntax property:
                            callers.AddRange(await FindCallersRecursively(semanticModel.GetDeclaredSymbol(item), solution));
                            break;
                    }
                }
            }

            return callers
                .Distinct(new SymbolCallerInfoEqualityComparer())
                .Where(u => u.CallingSymbol.ContainingType.Name.EndsWith("Controller"))
                .Select(c => c.ToEndpoint());
        }

        private async Task<IEnumerable<ReferenceLocation>> ProcessClass(ISymbol symbol, Solution solution)
        {
            if (symbol.Name.Contains("Attribute"))
            {
                var references = await SymbolFinder.FindReferencesAsync(symbol, solution);

                return references.SelectMany(r => r.Locations);
            }

            return new List<ReferenceLocation>();
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

