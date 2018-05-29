using System;

namespace CommitMap.Services.Changes.Bitbucket
{
    public class BitbucketIntegrationException : Exception
    {
        /// <summary>
        /// Initializes an instance of <see cref="BitbucketIntegrationException"/>
        /// </summary>
        public BitbucketIntegrationException(string message, Exception e) 
            :base(message, e)
        {
        }

        public BitbucketIntegrationException(string message)
            : base(message)
        {
        }
    }
}
