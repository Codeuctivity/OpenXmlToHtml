using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Codeuctivity.OpenXmlToHtml.Tooling
{
    public class Linux
    {
        public static readonly string ChromiumInstallCommand = "export DEBIAN_FRONTEND=noninteractive && apt update && apt upgrade -y && apt install mc libgconf-2-4 libatk1.0-0 libatk-bridge2.0-0 libgdk-pixbuf2.0-0 libgtk-3-0 libgbm-dev libasound2 libnss3 -y";

        public static void SetupDependencies()
        {
            var azureLinuxAppChromeDependencies = ChromiumInstallCommand;

            var escapedArgs = azureLinuxAppChromeDependencies.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Failed to execute '{ChromiumInstallCommand}'");
            }
        }

        public static bool IsRunningOnAzureLinux()
        {
            var websiteSku = Environment.GetEnvironmentVariable("WEBSITE_SKU");

            if (string.IsNullOrEmpty(websiteSku))
            {
                return false;
            }

            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && websiteSku.Contains("Linux");
        }
    }
}