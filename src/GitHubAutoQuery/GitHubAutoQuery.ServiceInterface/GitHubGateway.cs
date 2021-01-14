using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using GitHubAutoQuery.ServiceModel.Types;
using ServiceStack;

namespace GitHubAutoQuery.ServiceInterface
{
    public partial class GitHubGateway
    {
        public const string GithubApiBaseUrl = "https://api.github.com/";
        public static string UserAgent = typeof(GitHubGateway).Namespace.SplitOnFirst('.').First();

        /// <summary>
        /// AccessTokenSecret
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Intercept and override GitHub JSON API requests
        /// </summary>
        public Func<string, string> GetJsonFilter { get; set; }

        public GitHubGateway() {}
        public GitHubGateway(string accessToken) => AccessToken = accessToken;

        public virtual void ApplyRequestFilters(HttpWebRequest req)
        {
            if (!string.IsNullOrEmpty(AccessToken))
            {
                req.Headers["Authorization"] = "token " + AccessToken;
            }
            req.UserAgent = UserAgent;
        }

        public T GetJson<T>(string route, params object[] routeArgs)
        {
            return GithubApiBaseUrl.CombineWith(route.Fmt(routeArgs))
                .GetJsonFromUrl(ApplyRequestFilters)
                .FromJson<T>();
        }

        public IEnumerable<T> StreamJsonCollection<T>(string route, params object[] routeArgs)
        {
            List<T> results;
            var nextUrl = GithubApiBaseUrl.CombineWith(route.Fmt(routeArgs));

            do
            {
                results = nextUrl
                    .GetJsonFromUrl(
                        ApplyRequestFilters,
                        responseFilter: res => {
                            var links = ParseLinkUrls(res.Headers["Link"]);
                            links.TryGetValue("next", out nextUrl);
                        })
                    .FromJson<List<T>>();

                foreach (var result in results)
                {
                    yield return result;
                }

            } while (results.Count > 0 && nextUrl != null);
        }

        public static Dictionary<string, string> ParseLinkUrls(string linkHeader)
        {
            var map = new Dictionary<string, string>();
            var links = linkHeader;

            while (!string.IsNullOrEmpty(links))
            {
                var urlStartPos = links.IndexOf('<');
                var urlEndPos = links.IndexOf('>');

                if (urlStartPos == -1 || urlEndPos == -1)
                    break;

                var url = links.Substring(urlStartPos + 1, urlEndPos - urlStartPos - 1);
                var parts = links.Substring(urlEndPos).SplitOnFirst(',');

                var relParts = parts[0].Split(';');
                foreach (var relPart in relParts)
                {
                    var keyValueParts = relPart.SplitOnFirst('=');
                    if (keyValueParts.Length < 2)
                        continue;

                    var name = keyValueParts[0].Trim();
                    var value = keyValueParts[1].Trim().Trim('"');

                    if (name == "rel")
                    {
                        map[value] = url;
                    }
                }

                links = parts.Length > 1 ? parts[1] : null;
            }

            return map;
        }
    }


}