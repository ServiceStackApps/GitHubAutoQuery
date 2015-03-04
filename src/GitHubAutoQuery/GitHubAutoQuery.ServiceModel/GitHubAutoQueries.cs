using GitHubAutoQuery.ServiceModel.Types;
using ServiceStack;

namespace GitHubAutoQuery.ServiceModel
{
    [Route("/repos")]
    [AutoQueryViewer(Title = "ServiceStack Repositories", Description = "Browse list of different ServiceStack repositories",
        DefaultSearchField = "Language", DefaultSearchType = "=", DefaultSearchText = "C#",
        IconUrl = "/img/app/repos-inverted-75.png")]
    public class QueryRepos : QueryBase<GithubRepo> {}

    [Route("/commits")]
    [AutoQueryViewer(Title = "ServiceStack Commits", Description = "Browse latest 1000 commits",
        DefaultSearchField = "Message", DefaultSearchType = "Contains", DefaultSearchText = "AutoQuery",        
        IconUrl = "/img/app/commits-inverted-75.png")]
    public class QueryRepoCommits : QueryBase<GithubCommit> { }

    [Route("/contents")]
    [AutoQueryViewer(Title = "ServiceStack Files", Description = "Browse ServiceStack top-level files and folders",
        DefaultSearchField = "Type", DefaultSearchType = "=", DefaultSearchText = "file",
        IconUrl = "/img/app/contents-inverted-75.png")]
    public class QueryRepoContent : QueryBase<GithubContent> { }

    [Route("/contributors")]
    [AutoQueryViewer(Title = "ServiceStack Contributors", Description = "Browse ServiceStack Contributors",
        DefaultSearchField = "Contributions", DefaultSearchType = ">=", DefaultSearchText = "5",
        IconUrl = "/img/app/contributors-inverted-75.png")]
    public class QueryRepoContributors : QueryBase<GithubContributor> { }

    [Route("/subscribers")]
    [AutoQueryViewer(Title = "ServiceStack Subscribers", Description = "Browse ServiceStack Subscribers",
        DefaultSearchField = "Type", DefaultSearchType = "=", DefaultSearchText = "User",
        IconUrl = "/img/app/subscribers-inverted-75.png")]
    public class QueryRepoSubscribers : QueryBase<GithubSubscriber> { }

    [Route("/comments")]
    [AutoQueryViewer(Title = "ServiceStack Comments", Description = "Browse ServiceStack Subscribers",
        //DefaultSearchField = "Id", DefaultSearchType = ">", DefaultSearchText = "0",
        IconUrl = "/img/app/comments-inverted-75.png")]
    public class QueryRepoComments : QueryBase<GithubComment> { }

    [Route("/releases")]
    [AutoQueryViewer(Title = "ServiceStack Releases", Description = "Browse ServiceStack Releases",
        DefaultSearchField = "Name", DefaultSearchType = "Starts With", DefaultSearchText = "v4",
        IconUrl = "/img/app/releases-inverted-75.png")]
    public class QueryRepoReleases : QueryBase<GithubRelease> { }
}