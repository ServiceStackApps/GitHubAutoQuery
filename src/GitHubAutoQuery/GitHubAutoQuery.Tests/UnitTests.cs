using System;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using GitHubAutoQuery.ServiceModel;
using GitHubAutoQuery.ServiceInterface;

namespace GitHubAutoQuery.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }
    }
}
