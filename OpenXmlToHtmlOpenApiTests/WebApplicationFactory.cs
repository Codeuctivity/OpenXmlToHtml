using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace OpenXmlToHtmlOpenApiTests
{
    public class OpenXmlToHtmlOpenApiTestFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // see https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1 for things you could place here

            builder?.ConfigureServices(services =>
            {
                // Build the service provider.
                services.BuildServiceProvider();
            });
        }
    }
}