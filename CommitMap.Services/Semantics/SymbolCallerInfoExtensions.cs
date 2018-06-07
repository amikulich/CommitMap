using System.Collections.Generic;

using CommitMap.Services.Facade;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMap.Services.Semantics
{
    public static class SymbolCallerInfoExtensions
    {
        public static Endpoint ToEndpoint(this SymbolCallerInfo symbolCallerInfo)
        {
            var callingSymbol = symbolCallerInfo.CallingSymbol;
            return new Endpoint()
                       {
                           Name = callingSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                           Method = new Method()
                                        {
                                            Name = callingSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                                            Params = new Parameter[0]
                                        },
                           Caller = symbolCallerInfo.CalledSymbol.ToDisplayString()
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
