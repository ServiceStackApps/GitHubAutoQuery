using GitHubAutoQuery.ServiceModel.Types;
using ServiceStack;

namespace GitHubAutoQuery.ServiceModel
{
    [Route("/repos")]
    public class QueryRepos : QueryBase<GithubRepo> {}

    [Route("/commits")]
    public class QueryRepoCommits : QueryBase<GithubCommit> { }

    [Route("/contents")]
    public class QueryRepoContent : QueryBase<GithubContent> { }

    [Route("/contributors")]
    public class QueryRepoContributors : QueryBase<GithubContributor> { }

    [Route("/subscribers")]
    public class QueryRepoSubscribers : QueryBase<GithubSubscriber> { }

    [Route("/comments")]
    public class QueryRepoComments : QueryBase<GithubComment> { }

    [Route("/releases")]
    public class QueryRepoReleases : QueryBase<GithubRelease> { }
}