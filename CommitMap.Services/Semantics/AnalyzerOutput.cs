using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace CommitMap.Services.Semantics
{
    public class AnalyzerOutput
    {
        /// <summary>
        /// Initializes an instance of <see cref="AnalyzerOutput"/>
        /// </summary>
        public AnalyzerOutput()
        {
        }

        public IEnumerable<AffectedController> Controllers { get; internal set; }

        public bool AreConfigsAffected { get; internal set; }
    }

    public class AffectedController
    {
        public AffectedController(string name, ImmutableArray<SymbolDisplayPart>[] methods)
        {
            Name = name;
            Methods = methods;
        }

        /// <summary>
        /// Fully qualified controller name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Controller methods names
        /// </summary>
        public ImmutableArray<SymbolDisplayPart>[] Methods { get; }
    }
}
