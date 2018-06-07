using System.Threading.Tasks;

namespace CommitMap.Services.Changes
{
    public interface ICommitScanner
    {
        Task<string[]> GetModifiedDocuments(string firstCommit, string lastCommit);
    }
}
