using System.Collections.Generic;

using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMap.Services.Semantics
{
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
