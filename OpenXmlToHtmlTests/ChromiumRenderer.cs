using PuppeteerSharp;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OpenXmlToHtmlTests
{
    // todo move to nuget package
    internal class ChromiumRenderer : IAsyncDisposable
    {
        private Browser Browser { get; set; } = default!;
        private int LastProgressValue { get; set; }
        public BrowserFetcher BrowserFetcher { get; private set; } = default!;

        public static Task<ChromiumRenderer> CreateAsync()
        {
            var html2Pdf = new ChromiumRenderer();
            return html2Pdf.InitializeAsync();
        }

        private async Task<ChromiumRenderer> InitializeAsync()
        {
            BrowserFetcher = new BrowserFetcher();
            BrowserFetcher.DownloadProgressChanged += DownloadProgressChanged;

            await BrowserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision).ConfigureAwait(false);
            Browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }).ConfigureAwait(false);
            return this;
        }

        public async Task ConvertHtmlToPdf(string sourceHtmlFilePath, string destinationPdfFilePath)
        {
            if (!File.Exists(sourceHtmlFilePath))
            {
                throw new FileNotFoundException(sourceHtmlFilePath);
            }

            var absolutePath = Path.GetFullPath(sourceHtmlFilePath);
            var page = await Browser.NewPageAsync().ConfigureAwait(false);
            await page.GoToAsync($"file://{absolutePath}").ConfigureAwait(false);
            await page.PdfAsync(destinationPdfFilePath).ConfigureAwait(false);
        }

        public async Task ConvertHtmlToPng(string sourceHtmlFilePath, string destinationPdfFilePath)
        {
            if (!File.Exists(sourceHtmlFilePath))
            {
                throw new FileNotFoundException(sourceHtmlFilePath);
            }

            var absolutePath = Path.GetFullPath(sourceHtmlFilePath);
            var page = await Browser.NewPageAsync().ConfigureAwait(false);
            await page.GoToAsync($"file://{absolutePath}").ConfigureAwait(false);
            await page.ScreenshotAsync(destinationPdfFilePath, new ScreenshotOptions() { FullPage = true }).ConfigureAwait(false);
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (LastProgressValue != e.ProgressPercentage)
            {
                LastProgressValue = e.ProgressPercentage;
            }
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            Browser.CloseAsync().ConfigureAwait(false);
            return ((IAsyncDisposable)Browser).DisposeAsync();
        }
    }
}