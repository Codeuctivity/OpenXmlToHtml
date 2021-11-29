using Codeuctivity.PuppeteerSharp;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    internal static class DocumentAsserter
    {
        internal static async Task AssertRenderedHtmlIsEqual(string actualFilePath, string expectReferenceFilePath, int allowedPixelErrorCount)
        {
            var actualFullPath = Path.GetFullPath(actualFilePath);
            var expectFullPath = Path.GetFullPath(expectReferenceFilePath);

            Assert.True(File.Exists(actualFullPath), $"actualFilePath not found {actualFullPath}");
            await using var chromiumRenderer = await Renderer.CreateAsync();
            var pathRasterizedHtml = actualFilePath + ".png";
            await chromiumRenderer.ConvertHtmlToPng(actualFilePath, pathRasterizedHtml);

            Assert.True(File.Exists(expectFullPath), $"ExpectReferenceFilePath not found \n{expectFullPath}\n copy over \n{pathRasterizedHtml}\n if this is a new test case.");

            await AssertImageIsEqualAsync(pathRasterizedHtml, expectReferenceFilePath, allowedPixelErrorCount);
        }

        internal static async Task AssertImageIsEqualAsync(string actualImagePath, string expectImageFilePath, int allowedPixelErrorCount)
        {
            var actualFullPath = Path.GetFullPath(actualImagePath);
            var expectFullPath = Path.GetFullPath(expectImageFilePath);

            Assert.True(File.Exists(actualFullPath), $"actualImagePath not found {actualFullPath}");
            Assert.True(File.Exists(expectFullPath), $"ExpectReferenceImagePath not found \n{expectFullPath}\n copy over \n{actualFullPath}\n if this is a new test case.");
            var base64fyedActualImage = Convert.ToBase64String(File.ReadAllBytes(actualFullPath));

            if (Codeuctivity.ImageSharpCompare.ImageAreEqual(actualFullPath, expectFullPath))
            {
                return;
            }

            var osSpecificDiffFileSuffix = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" : "win";

            var allowedDiffImage = $"{expectFullPath}.diff.{osSpecificDiffFileSuffix}.png";
            var newDiffImage = $"{actualFullPath}.diff.png";
            try
            {
                using (var maskImage = Codeuctivity.ImageSharpCompare.CalcDiffMaskImage(actualFullPath, expectFullPath))
                {
                    await maskImage.SaveAsync(newDiffImage);
                }
                var base64fyedImageDiff = Convert.ToBase64String(File.ReadAllBytes(newDiffImage));

                if (File.Exists(allowedDiffImage))
                {
                    var resultWithAllowedDiff = Codeuctivity.ImageSharpCompare.CalcDiff(actualFullPath, expectFullPath, allowedDiffImage);

                    Assert.True(resultWithAllowedDiff.PixelErrorCount <= allowedPixelErrorCount, $"Expected PixelErrorCount beyond {allowedPixelErrorCount} but was {resultWithAllowedDiff.PixelErrorCount}\nExpected {expectFullPath}\ndiffers to actual {actualFullPath}\n {base64fyedActualImage}\n \n Diff is {newDiffImage} \n {base64fyedImageDiff}\n");
                    return;
                }

                var result = Codeuctivity.ImageSharpCompare.CalcDiff(actualFullPath, expectFullPath);

                Assert.True(result.PixelErrorCount <= allowedPixelErrorCount, $"Expected PixelErrorCount beyond {allowedPixelErrorCount} but was {result.PixelErrorCount}\nExpected {expectFullPath}\ndiffers to actual {actualFullPath}\n {base64fyedActualImage}\n \n Diff is {newDiffImage} \n {base64fyedImageDiff}\nReplace {actualFullPath} with the new value or store the diff as {allowedDiffImage}.");
            }
            catch (Codeuctivity.ImageSharpCompareException)
            {
                Assert.True(false, $"Actual Dimension differs from expected \nExpected {expectFullPath}\ndiffers to actual {actualFullPath}\n {base64fyedActualImage}\n \nReplace {expectFullPath} with the new value.");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                throw new Exception($"Failed to parse actual image \n {base64fyedActualImage}", exception);
            }
        }
    }
}