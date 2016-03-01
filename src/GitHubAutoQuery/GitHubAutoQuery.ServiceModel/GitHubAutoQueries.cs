using GitHubAutoQuery.ServiceModel.Types;
using ServiceStack;

namespace GitHubAutoQuery.ServiceModel
{
    [Route("/repos")]
    [AutoQueryViewer(Title = "ServiceStack Repositories", Description = "Browse list of different ServiceStack repositories",
        DefaultSearchField = "Language", DefaultSearchType = "=", DefaultSearchText = "C#",
        IconUrl = "octicon:repo",
        DefaultFields = "Id,Name,Description:500,Homepage,Language,Watchers_Count,Stargazes_Count,Forks_Count,Open_Issues_Count,Size,Full_Name,Created_At,Updated_At,Has_Issues,Has_Downloads,Has_Wiki,Has_Pages,Fork,Private")]
    public class QueryRepos : QueryBase<GithubRepo> {}

    [Route("/commits")]
    [AutoQueryViewer(Title = "ServiceStack Commits", Description = "Browse latest 1000 commits",
        DefaultSearchField = "Message", DefaultSearchType = "Contains", DefaultSearchText = "AutoQuery",
        IconUrl = "octicon:history")]
    public class QueryRepoCommits : QueryBase<GithubCommit> { }

    [Route("/contents")]
    [AutoQueryViewer(Title = "ServiceStack Files", Description = "Browse ServiceStack top-level files and folders",
        DefaultSearchField = "Type", DefaultSearchType = "=", DefaultSearchText = "file",
        IconUrl = "octicon:file-directory")]
    public class QueryRepoContent : QueryBase<GithubContent> { }

    [Route("/contributors")]
    [AutoQueryViewer(Title = "ServiceStack Contributors", Description = "Browse ServiceStack Contributors",
        DefaultSearchField = "Contributions", DefaultSearchType = ">=", DefaultSearchText = "5",
        IconUrl = "octicon:organization")]
    public class QueryRepoContributors : QueryBase<GithubContributor> { }

    [Route("/subscribers")]
    [AutoQueryViewer(Title = "ServiceStack Subscribers", Description = "Browse ServiceStack Subscribers",
        DefaultSearchField = "Type", DefaultSearchType = "=", DefaultSearchText = "User",
        IconUrl = "octicon:eye")]
    public class QueryRepoSubscribers : QueryBase<GithubSubscriber> { }

    [Route("/comments")]
    [AutoQueryViewer(Title = "ServiceStack Comments", Description = "Browse ServiceStack Subscribers",
        DefaultSearchField = "Id", DefaultSearchType = ">", DefaultSearchText = "0",
        IconUrl = "octicon:comment")]
    public class QueryRepoComments : QueryBase<GithubComment> { }

    [Route("/releases")]
    [AutoQueryViewer(Title = "ServiceStack Releases", Description = "Browse ServiceStack Releases",
        DefaultSearchField = "Name", DefaultSearchType = "Starts With", DefaultSearchText = "v4",
        IconUrl = "octicon:tag")]
    public class QueryRepoReleases : QueryBase<GithubRelease> { }
}