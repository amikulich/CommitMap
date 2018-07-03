using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommitMap.Services.Changes;
using CommitMap.Services.Changes.Bitbucket.Solution;
using CommitMap.Services.Semantics;

namespace CommitMap.Services.Facade
{
    public interface ICommitMapEngine
    {
        Task<AnalysisResult> Run(string firstCommit, string lastCommit);
    }

    public class CommitMapEngine : ICommitMapEngine
    {
        private readonly ISolutionProvider _solutionProvider;

        private readonly ICommitScanner _commitScanner;

        private readonly IAnalyzer _analyzer;

        /// <summary>
        /// Initializes an instance of <see cref="CommitMapEngine"/>
        /// </summary>
        public CommitMapEngine(ISolutionProvider solutionProvider, ICommitScanner commitScanner, IAnalyzer analyzer)
        {
            _solutionProvider = solutionProvider;
            _commitScanner = commitScanner;
            _analyzer = analyzer;
        }

        public async Task<AnalysisResult> Run(string firstCommit, string lastCommit)
        {
            var solution = await _solutionProvider.GetSolution();

            var modifiedDocumentsNames = await _commitScanner.GetModifiedDocuments(firstCommit, lastCommit);

            var documentsAffected = solution.Projects
                .SelectMany(p => p.Documents.Where(doc => modifiedDocumentsNames.Contains(doc.Name)));

            var usages = await _analyzer.FindAffectedEndpoints(documentsAffected, solution);

            return new AnalysisResult()
                       {
                           CompletedAt = DateTime.UtcNow,
                           AffectedEndpoints = usages.ToArray(),
            };
        }
    }
}
