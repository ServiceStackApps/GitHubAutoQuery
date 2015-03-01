using GitHubAutoQuery.ServiceModel.Types;
using ServiceStack;

namespace GitHubAutoQuery.ServiceModel
{
    [Route("/repos")]
    public class QueryRepos : QueryBase<GithubRepo> {} 
}