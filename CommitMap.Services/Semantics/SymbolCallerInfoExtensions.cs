using System.Collections.Generic;
using System.Linq;
using CommitMap.Services.Facade;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMap.Services.Semantics
{
    public static class SymbolCallerInfoExtensions
    {
        public static Endpoint ToEndpoint(this SymbolCallerInfo symbolCallerInfo)
        {
            var action = symbolCallerInfo.CallingSymbol;

            var attribute = action
                    .GetAttributes()
                    .FirstOrDefault(a => a.AttributeClass.Name == "Usage");

            string url = string.Empty, method = string.Empty;

            var isLabeledWithAttribute = attribute != null;
            if (isLabeledWithAttribute)
            {
                var parameters = attribute.ApplicationSyntaxReference.GetSyntax()
                    .DescendantNodes()
                    .Where(n => n is AttributeArgumentSyntax)
                    .ToArray();

                url = parameters[0].ToString().Replace("\"", "");
                method = parameters[1].ToString().Replace("\"", "");
            }

            return new Endpoint()
                       {
                           Url = url,
                           HttpMethod = method,
                           Controller = action.ContainingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                           Method = action.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                           Namespace = action.ContainingSymbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                           IsLabelled = isLabeledWithAttribute
                       };
        }
    }

    public class SymbolCallerInfoEqualityComparer : IEqualityComparer<SymbolCallerInfo>
    {
        public bool Equals(SymbolCallerInfo x, SymbolCallerInfo y)
        {
            if (x.GetType() != y.GetType()) return false;
            return x.CallingSymbol.ToDisplayString() == y.CallingSymbol.ToDisplayString();
        }


        public int GetHashCode(SymbolCallerInfo obj)
        {
            unchecked
            {
                return (obj.CallingSymbol.GetHashCode() * 397);
            }
        }
    }
}
