using Autofac;

using CommitMap.DataAccess;
using CommitMap.Services;
using CommitMap.Services.Changes;
using CommitMap.Services.Changes.Bitbucket.Solution;
using CommitMap.Services.Facade;
using CommitMap.Services.Semantics;

namespace CommitMap.DI
{
    public class CommitMapDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BitbucketApiClient>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SolutionProvider>().AsImplementedInterfaces();
            builder.RegisterType<CommitScanner>().AsImplementedInterfaces();
            builder.RegisterType<Analyzer>().AsImplementedInterfaces();

            builder.RegisterType<CommitMapEngine>().AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}
