using System.Diagnostics;

namespace OpenXmlToHtmlOpenApi
{
    /// <summary>
    /// Setup dependecies on Azure App Service with Linux plan
    /// </summary>
    public class Azure
    {
        /// <summary>
        /// Installs every known dependency of chromium used to render html to pdf
        /// </summary>
        public void SetupChromeiumDependencies()
        {
            var azureLinuxAppChromeDependencies = "export DEBIAN_FRONTEND=noninteractive && bash -c 'apt update && apt upgrade -y apt install mc libgconf-2-4 libatk1.0-0 libatk-bridge2.0-0 libgdk-pixbuf2.0-0 libgtk-3-0 libgbm-dev libasound2 libnss3 -y >/dev/null 2>&1 & disown'";

            var escapedArgs = azureLinuxAppChromeDependencies.Replace("\"", "\\\"");

            using var process = new Process()
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
        }
    }
}