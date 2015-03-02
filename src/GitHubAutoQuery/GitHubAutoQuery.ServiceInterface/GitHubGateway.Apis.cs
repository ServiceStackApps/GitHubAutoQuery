using System.Collections.Generic;
using System.Linq;
using GitHubAutoQuery.ServiceModel.Types;

namespace GitHubAutoQuery.ServiceInterface
{
    public partial class GithubGateway
    {       
        public List<GithubOrg> GetUserOrgs(string githubUsername)
        {
            return GetJson<List<GithubOrg>>("users/{0}/orgs", githubUsername);
        }

        public List<GithubRepo> GetUserRepos(string githubUsername)
        {
            return GetJson<List<GithubRepo>>("users/{0}/repos", githubUsername);
        }

        public List<GithubRepo> GetOrgRepos(string githubOrgName)
        {
            return GetJson<List<GithubRepo>>("orgs/{0}/repos", githubOrgName);
        }

        public List<GithubRepo> GetAllUserAndOrgsReposFor(string githubUsername)
        {
            var map = new Dictionary<int, GithubRepo>();
            GetUserRepos(githubUsername).ForEach(x => map[x.Id] = x);
            GetUserOrgs(githubUsername).ForEach(org =>
                GetOrgRepos(org.Login)
                    .ForEach(repo => map[repo.Id] = repo));

            return map.Values.ToList();
        }

        public IEnumerable<GithubCommitResult> GetRepoCommits(string githubUser, string githubRepo)
        {
            return StreamJsonCollection<GithubCommitResult>("repos/{0}/{1}/commits", githubUser, githubRepo);
        }

        public List<GithubContent> GetRepoContents(string githubUser, string githubRepo)
        {
            return GetJson<List<GithubContent>>("repos/{0}/{1}/contents", githubUser, githubRepo);
        }

        public List<GithubContributor> GetRepoContributors(string githubUser, string githubRepo)
        {
            return GetJson<List<GithubContributor>>("repos/{0}/{1}/contributors", githubUser, githubRepo);
        }

        public List<GithubSubscriber> GetRepoSubscribers(string githubUser, string githubRepo)
        {
            return GetJson<List<GithubSubscriber>>("repos/{0}/{1}/subscribers", githubUser, githubRepo);
        }

        public List<GithubComment> GetRepoComments(string githubUser, string githubRepo)
        {
            return GetJson<List<GithubComment>>("repos/{0}/{1}/comments", githubUser, githubRepo);
        }

        public List<GithubRelease> GetRepoReleases(string githubUser, string githubRepo)
        {
            return GetJson<List<GithubRelease>>("repos/{0}/{1}/releases", githubUser, githubRepo);
        }
    }


}