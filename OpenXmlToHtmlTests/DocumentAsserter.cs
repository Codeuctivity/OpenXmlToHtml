using Codeuctivity;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    internal static class DocumentAsserter
    {
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

        internal static void AssertImageIsEqual(string actualImagePath, string expectImageFilePath)
        {
            var actualFullPath = Path.GetFullPath(actualImagePath);
            var expectFullPath = Path.GetFullPath(expectImageFilePath);

            Assert.True(File.Exists(actualFullPath), $"actualImagePath not found {actualFullPath}");
            Assert.True(File.Exists(expectFullPath), $"ExpectReferenceImagePath not found \n{expectFullPath}\n copy over \n{actualFullPath}\n if this is a new test case.");

            if (ImageSharpCompare.ImageAreEqual(actualImagePath, expectFullPath))
            {
                return;
            }

            var allowedDiffImage = $"{expectImageFilePath}.diff.png";

            if (File.Exists(allowedDiffImage))
            {
                var result = ImageSharpCompare.CalcDiff(actualImagePath, expectFullPath, allowedDiffImage);

                if (result.AbsoluteError == 0)
                {
                    return;
                }
            }

            var newDiffImage = $"{actualImagePath}.diff.png";
            using (var fileStreamDifferenceMask = File.Create(newDiffImage))
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(actualImagePath, expectFullPath))
            {
                SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }

            var base64fyedActualImage = Convert.ToBase64String(File.ReadAllBytes(actualFullPath));
            var base64fyedImageDiff = Convert.ToBase64String(File.ReadAllBytes(newDiffImage));
            Assert.True(ImageSharpCompare.ImageAreEqual(actualImagePath, expectFullPath), $"Expected {expectFullPath}\ndiffers to actual {actualFullPath}\n {base64fyedActualImage} \n Diff is {newDiffImage} \n {base64fyedImageDiff}");
        }
    }
}