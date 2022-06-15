using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
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
            if (IsRunningOnAzureLinux())
                new Azure().SetupChromeiumDependencies();

            CreateHostBuilder(args).Build().Run();
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