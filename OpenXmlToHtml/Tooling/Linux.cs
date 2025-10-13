using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Codeuctivity.OpenXmlToHtml.Tooling
{
    /// <summary>
    /// Utility class for Linux-specific operations and Azure Linux environment detection
    /// </summary>
    public static class Linux
    {
        /// <summary>
        /// Command to install Chromium dependencies on Linux systems
        /// </summary>
        public static readonly string ChromiumInstallCommand = "export DEBIAN_FRONTEND=noninteractive && apt update && apt upgrade -y && apt install mc libgconf-2-4 libatk1.0-0 libatk-bridge2.0-0 libgdk-pixbuf2.0-0 libgtk-3-0 libgbm-dev libasound2 libnss3 -y";

        /// <summary>
        /// Sets up Linux dependencies required for Chromium operation
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when dependency installation fails</exception>
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
                throw new InvalidOperationException($"Failed to execute '{ChromiumInstallCommand}'");
            }
        }

        /// <summary>
        /// Determines whether the current environment is running on Azure Linux
        /// </summary>
        /// <returns>True if running on Azure Linux, false otherwise</returns>
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