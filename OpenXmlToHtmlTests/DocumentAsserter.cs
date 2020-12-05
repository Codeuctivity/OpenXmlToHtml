using Codeuctivity;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    internal static class DocumentAsserter
    {
        internal static async Task EqualFileContentAsync(string actualFilePath, string expectReferenceFilePath)
        {
            var actualFullPath = Path.GetFullPath(actualFilePath);
            var expectFullPath = Path.GetFullPath(expectReferenceFilePath);

            Assert.True(File.Exists(actualFullPath), $"actualFilePath not found {actualFullPath}");
            Assert.True(File.Exists(expectFullPath), $"ExpectReferenceFilePath not found \n{expectFullPath}\n copy over \n{actualFullPath}\n if this is a new test case.");
            var actualHtmlContent = File.ReadAllBytesAsync(actualFullPath);
            var expectedHtmlContent = File.ReadAllBytesAsync(expectFullPath);
            await Task.WhenAll(actualHtmlContent, expectedHtmlContent);

            Assert.True(actualHtmlContent.Result == expectedHtmlContent.Result, $"Expected {expectFullPath}\ndiffers to actual {actualFullPath}");
        }

        internal static void AssertImageIsEqual(string actualFilePath, string expectReferenceFilePath)
        {
            var actualFullPath = Path.GetFullPath(actualFilePath);
            var expectFullPath = Path.GetFullPath(expectReferenceFilePath);

            Assert.True(File.Exists(actualFullPath), $"actualFilePath not found {actualFullPath}");
            Assert.True(File.Exists(expectFullPath), $"ExpectReferenceFilePath not found \n{expectFullPath}\n copy over \n{actualFullPath}\n if this is a new test case.");
            Assert.True(ImageSharpCompare.ImageAreEqual(actualFilePath, expectFullPath), $"Expected {expectFullPath}\ndiffers to actual {actualFullPath}");
        }

        internal static async Task AssertRenderedHtmlIsEqual(string actualFilePath, string expectReferenceFilePath)
        {
            var actualFullPath = Path.GetFullPath(actualFilePath);
            var expectFullPath = Path.GetFullPath(expectReferenceFilePath);

            Assert.True(File.Exists(actualFullPath), $"actualFilePath not found {actualFullPath}");
            await using var chromiumRenderer = await ChromiumRenderer.CreateAsync();
            var pathRasterizedHtml = actualFilePath + ".png";
            await chromiumRenderer.ConvertHtmlToPng(actualFilePath, pathRasterizedHtml);

            Assert.True(File.Exists(expectFullPath), $"ExpectReferenceFilePath not found \n{expectFullPath}\n copy over \n{pathRasterizedHtml}\n if this is a new test case.");

            AssertImageIsEqual(pathRasterizedHtml, expectReferenceFilePath);
        }
    }
}