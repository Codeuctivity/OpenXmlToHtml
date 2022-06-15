using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OpenXmlToHtmlOpenApi
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //if (IsRunningOnAzureLinux())
            //    SetupChromeiumDependencies();

            CreateHostBuilder(args).Build().Run();
        }

        private static void SetupChromeiumDependencies()
        {
            var azureLinuxAppChromeDependencies = "export DEBIAN_FRONTEND=noninteractive && bash -c 'apt update &&apt upgrade -y apt install libgconf-2-4 libatk1.0-0 libatk-bridge2.0-0 libgdk-pixbuf2.0-0 libgtk-3-0 libgbm-dev libasound2 libnss3 -y >/dev/null 2>&1 & disown'";

            Process.Start(azureLinuxAppChromeDependencies);
        }

        private static bool IsRunningOnAzureLinux()
        {
            var websiteSku = Environment.GetEnvironmentVariable("WEBSITE_SKU");

            if (string.IsNullOrEmpty(websiteSku))
            {
                return false;
            }
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && websiteSku.Contains("Linux", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Called on start
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}