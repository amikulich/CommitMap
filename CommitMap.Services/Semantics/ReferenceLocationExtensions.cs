using System.Collections.Generic;

using Microsoft.CodeAnalysis.FindSymbols;

namespace CommitMap.Services.Semantics
{
    public class ReferenceLocationComparer : IEqualityComparer<ReferenceLocation>
    {
        public bool Equals(ReferenceLocation x, ReferenceLocation y)
        {
            if (x.GetType() != y.GetType()) return false;
            return x.Location.ToString() == y.Location.ToString();
        }


        public int GetHashCode(ReferenceLocation obj)
        {
            unchecked
            {
                return (obj.Location.GetHashCode() * 397);
            }
        }
    }
}
