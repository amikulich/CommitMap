using System.Configuration;

namespace CommitMap.Services.Facade
{
    public class CMSolutionProviderConfiguration : ConfigurationSection
    {
        public static CMSolutionProviderConfiguration Current { get; } = (CMSolutionProviderConfiguration)ConfigurationManager.GetSection("CMSolutionProviderConfiguration");

        [ConfigurationProperty("useLocal")]
        public bool UseLocal => (bool)this["useLocal"];

        [ConfigurationProperty("solutionLocalPath")]
        public string LocalPath => this["solutionLocalPath"] as string;

        [ConfigurationProperty("solutionName")]
        public string SolutionName => this["solutionName"] as string;

        [ConfigurationProperty("enableCache")]
        public bool EnableCache => (bool)this["enableCache"];

        [ConfigurationProperty("cacheAgeMinutes")]
        public int CacheAgeMinutes => (int)this["cacheAgeMinutes"];
    }
}