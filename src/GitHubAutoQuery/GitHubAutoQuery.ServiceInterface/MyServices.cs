using ServiceStack;

namespace GitHubAutoQuery.ServiceInterface
{
    [FallbackRoute("/{PathInfo*}")]
    public class FallbackToClientRoutes
    {
        public string PathInfo { get; set; }
    }

    public class MyServices : Service
    {
        public object Any(FallbackToClientRoutes request)
        {
            return SinglePageApp();
        }

        public object SinglePageApp()
        {
            //Return default.cshtml for unmatched requests so routing can be handled on the client
            return new HttpResult
            {
                View = "/default.cshtml"
            };
        }
    }
}