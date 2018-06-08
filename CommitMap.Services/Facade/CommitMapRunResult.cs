using System;
using System.Linq;

namespace CommitMap.Services.Facade
{
    public class CommitMapRunResult
    {
        public DateTime CompletedAt { get; internal set; }

        public Endpoint[] Controllers { get; internal set; }
    }

    public class Endpoint
    {
        public string Name { get; internal set; }

        public Method Method { get; internal set; }

        public string Caller { get; internal set; }

        public override string ToString()
        {
            return $"{Name} - {Method} called by {Caller} {Environment.NewLine}";
        }
    }

    public class Method
    {
        public string Name { get; internal set; }

        public Parameter[] Params { get; internal set; }

        public override string ToString()
        {
            return $"{Name}({string.Join(",", Params.SelectMany(p => p.ToString()))})";
        }
    }

    public class Parameter
    {
        public string Name { get; internal set; }

        public string Type { get; internal set; }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }
}
