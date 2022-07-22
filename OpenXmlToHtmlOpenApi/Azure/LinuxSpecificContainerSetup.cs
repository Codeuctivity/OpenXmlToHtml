using Codeuctivity.OpenXmlToHtml.Tooling;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace OpenXmlToHtmlOpenApi.Azure
{
    /// <summary>
    /// Setup dependencies on azure linux containers
    /// </summary>w
    public class AzureAndWslSpecificContainerSetup : IHostedService
    {
        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Linux.SetupDependencies();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            { return Task.CompletedTask; }
        }
    }
}