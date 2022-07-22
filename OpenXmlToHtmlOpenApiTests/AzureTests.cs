using OpenXmlToHtmlOpenApi.Azure;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Xunit;

namespace OpenXmlToHtmlOpenApiTests
{
    public class AzureTests
    {
        [SkippableFact]
        public async void ShoulSetupChromeiumDependencies()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Linux), "Setup code is designed for Ubuntu");
            Skip.If(System.Environment.UserName != "root");
            var azureSpecificContainerSetup = new AzureAndWslSpecificContainerSetup();
            await azureSpecificContainerSetup.StartAsync(new CancellationToken());
        }

        public static bool IsRunningOnWsl()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return false;
            }

            var version = File.ReadAllText("/proc/version");
            var IsWsl = version.Contains("WSL");
            return IsWsl;
        }
    }
}