using System;
using System.IO;
using System.Linq;
using System.Net;
using Funq;
using GitHubAutoQuery.ServiceInterface;
using GitHubAutoQuery.ServiceModel;
using GitHubAutoQuery.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Razor;

namespace GitHubAutoQuery
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("GitHub AutoQuery", typeof(MyServices).Assembly)
        {
            var customSettings = new FileInfo(@"~/appsettings.txt".MapHostAbsolutePath());
            AppSettings = customSettings.Exists
                ? (IAppSettings)new TextFileSettings(customSettings.FullName)
                : new AppSettings();
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());

            SetConfig(new HostConfig
            {
                DebugMode = AppSettings.Get("DebugMode", false),
                AddRedirectParamsToQueryString = true
            });

            this.Plugins.Add(new RazorFormat());

            InitDatabase(container, AppSettings.GetString("GitHubUser"), AppSettings.GetString("GitHubRepo"));
        }

        private void InitDatabase(Container container, string githubUser, string githubRepo)
        {
            if (githubUser.IsNullOrEmpty() || githubRepo.IsNullOrEmpty())
                throw new ArgumentException("userName and repoName are required");

            var dbPath = "~/App_Data/{0}-{1}.sqlite".Fmt(githubUser, githubRepo).MapHostAbsolutePath();

            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(dbPath, SqliteDialect.Provider));

            container.Register(c => new GithubGateway
            {
                Username = AppSettings.GetString("GitHubAuthUsername"),
                Password = AppSettings.GetString("GitHubAuthPassword"),
            });

            if (!File.Exists(dbPath) || AppSettings.Get("RecreateDatabase", false))
            {
                using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
                {
                    db.DropAndCreateTable<GithubRepo>();
                    db.DropAndCreateTable<GithubCommit>();
                    db.DropAndCreateTable<GithubContent>();
                    db.DropAndCreateTable<GithubContributor>();
                    db.DropAndCreateTable<GithubSubscriber>();
                    db.DropAndCreateTable<GithubComment>();
                    db.DropAndCreateTable<GithubRelease>();

                    var gateway = container.Resolve<GithubGateway>();

                    var allRepos = gateway.GetAllUserAndOrgsReposFor(githubUser);

                    db.InsertAll(allRepos);

                    var commitResponses = gateway.GetRepoCommits(githubUser, githubRepo)
                        .Take(1000)
                        .ToList();

                    var commits = commitResponses.Select(x => {
                        x.Commit.Id = x.Sha;
                        x.Commit.Date = x.Commit.Committer.Date;
                        x.Commit.Name = x.Commit.Committer.Name;
                        return x.Commit;
                    });
                    db.InsertAll(commits);

                    var repoContents = gateway.GetRepoContents(githubUser, githubRepo);
                    db.InsertAll(repoContents);

                    var repoContributors = gateway.GetRepoContributors(githubUser, githubRepo);
                    db.InsertAll(repoContributors);

                    var repoSubscribers = gateway.GetRepoSubscribers(githubUser, githubRepo);
                    db.InsertAll(repoSubscribers);

                    var repoComments = gateway.GetRepoComments(githubUser, githubRepo);
                    db.InsertAll(repoComments);

                    var repoReleases = gateway.GetRepoReleases(githubUser, githubRepo);
                    db.InsertAll(repoReleases);
                }
            }

            Plugins.Add(new AutoQueryFeature
            {
                LoadFromAssemblies = { typeof(QueryRepos).Assembly },
                MaxLimit = 200,
                AutoQueryViewerConfig =
                {
                    ServiceDescription = "Browse ServiceStack on GitHub",
                    ServiceIconUrl = "/img/app/autoquery-75.png",
                    BackgroundColor = "#253D48",
                    TextColor = "#fff",
                    LinkColor = "#ffff8d",
                    BrandImageUrl = "/img/app/brand-inverted.png",
                    BrandUrl = "https://github.com/ServiceStack/ServiceStack/",
                    BackgroundImageUrl = "/img/app/bg.png",
                    IsPublic = true,
                }
            });
        }
    }
}