using System.Threading.Tasks;

namespace CommitMap.Services.Changes
{
    public interface ICommitScanner
    {
        Task<string[]> GetModifiedDocuments(string fromCommit, string toCommit);
    }
}
