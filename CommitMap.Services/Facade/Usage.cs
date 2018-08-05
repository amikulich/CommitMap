 using System;

namespace CommitMap.Services.Facade
{
    public class Usage : Attribute
    {
        public Usage(string url, string method) 
            :this(url, method, string.Empty)
        {
        }

        public Usage(string url, string method, string tag)
        {
            Url = url;
            Method = method;
            Tag = tag;
        }

        public string Url { get; }

        public string Method { get; }

        public string Tag { get; }
    }
}
