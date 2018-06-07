using System.IO;
using System.Threading.Tasks;

namespace CommitMap.Services.Changes.Bitbucket
{
    public interface IBitbucketApiClient
    {
        Task<TResponse> Get<TResponse>(string url);

        Task<Stream> LoadFile(string url);
    }
}
