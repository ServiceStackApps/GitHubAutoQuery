using System.Collections.Generic;
using GitHubAutoQuery.ServiceInterface;
using NUnit.Framework;

namespace GitHubAutoQuery.Tests
{
    [TestFixture]
    public class GitHubGatewayTests
    {
        [Test]
        public void Can_parse_link_urls()
        {
            var linkHeader = "<https://api.github.com/repositories/1339922/commits?page=101>; rel=\"next\", <https://api.github.com/repositories/1339922/commits?page=1>; rel=\"first\", <https://api.github.com/repositories/1339922/commits?page=99>; rel=\"prev\"";

            var map = GithubGateway.ParseLinkUrls(linkHeader);

            Assert.That(map, Is.EquivalentTo(new Dictionary<string,string>
            {
                {"next", "https://api.github.com/repositories/1339922/commits?page=101"},
                {"first", "https://api.github.com/repositories/1339922/commits?page=1"},
                {"prev", "https://api.github.com/repositories/1339922/commits?page=99"},
            }));
        } 
    }
}