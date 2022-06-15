using OpenXmlToHtmlOpenApi;
using System.Runtime.InteropServices;
using Xunit;

namespace OpenXmlToHtmlOpenApiTests
{
    public class AzureTests
    {
        [SkippableFact]
        public void ShoulSetupChromeiumDependencies()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Linux));
            new Azure().SetupChromeiumDependencies();
        }
    }
}