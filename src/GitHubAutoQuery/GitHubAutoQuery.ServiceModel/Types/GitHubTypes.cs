using System;

namespace GitHubAutoQuery.ServiceModel.Types
{
    public class GithubRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Homepage { get; set; }
        public string Language { get; set; }
        public int Watchers { get; set; }
        public int Forks { get; set; }
        public int Size { get; set; }
        public string Open_Issues { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime? Pushed_At { get; set; }
        public DateTime? Updated_At { get; set; }

        public bool Has_Wiki { get; set; }
        public bool Has_Downloads { get; set; }
        public bool Has_issues { get; set; }
        public bool Fork { get; set; }

        public GithubUser Owner { get; set; }
        public string Svn_Url { get; set; }
        public string Mirror_Url { get; set; }
        public string Url { get; set; }
        public string Ssh_Url { get; set; }
        public string Html_Url { get; set; }
        public string Clone_Url { get; set; }
        public string Git_Url { get; set; }
        public bool Private { get; set; }
    }

    public class GithubUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Avatar_Url { get; set; }
        public string Url { get; set; }
        public int? Followers { get; set; }
        public int? Following { get; set; }
        public string Type { get; set; }
        public int? Public_Gists { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string Html_Url { get; set; }
        public int? Public_Repos { get; set; }
        public DateTime? Created_At { get; set; }
        public string Blog { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool? Hireable { get; set; }
        public string Gravatar_Id { get; set; }
        public string Bio { get; set; }
    }

    public class GithubOrg
    {
        public int Id { get; set; }
        public string Avatar_Url { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
    }
}