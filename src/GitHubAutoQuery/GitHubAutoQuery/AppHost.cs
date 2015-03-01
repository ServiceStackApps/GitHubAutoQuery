using System;
using System.IO;
using System.Net;
using Funq;
using GitHubAutoQuery.ServiceInterface;
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
            : base("GitHubAutoQuery", typeof(MyServices).Assembly)
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

            //Return default.cshtml home page for all 404 requests so we can handle routing on the client
            base.CustomErrorHttpHandlers[HttpStatusCode.NotFound] = new RazorHandler("/default.cshtml");

            SetConfig(new HostConfig
            {
                DebugMode = AppSettings.Get("DebugMode", false),
                AddRedirectParamsToQueryString = true
            });

            this.Plugins.Add(new RazorFormat());

            this.Plugins.Add(new AutoQueryFeature());

            CreateDatabaseIfNotExists(container, AppSettings.GetString("GitHubUser"), AppSettings.GetString("GitHubRepo"));
        }

        private void CreateDatabaseIfNotExists(Container container, string githubUser, string githubRepo)
        {
            if (githubUser.IsNullOrEmpty() || githubRepo.IsNullOrEmpty())
                throw new ArgumentException("userName and repoName are required");

            var dbPath = "~/App_Data/{0}-{1}.sqlite".Fmt(githubUser, githubRepo).MapHostAbsolutePath();

            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(dbPath, SqliteDialect.Provider));

            container.Register(c => new GithubGateway());

            if (!File.Exists(dbPath) || AppSettings.Get("RecreateDatabase", false))
            {
                using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
                {
                    db.DropAndCreateTable<GithubUser>();
                    db.DropAndCreateTable<GithubRepo>();

                    var gateway = container.Resolve<GithubGateway>();

                    var allRepos = gateway.GetAllUserAndOrgsReposFor(githubUser);

                    db.InsertAll(allRepos);
                }
            }
        }
    }
}