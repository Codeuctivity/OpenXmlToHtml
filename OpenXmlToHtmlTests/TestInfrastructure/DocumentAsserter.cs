using Codeuctivity.HtmlRenderer;
using Codeuctivity.ImageSharpCompare;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    internal static class DocumentAsserter
    {
        public static string TestsOutputDirectory => Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Generated");

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

            // Uncomment following line to update or create an expectaion file
            //File.Copy(actualImagePath, expectImageFilePath, true);

            var filePathInTestResultFolderOfExpectation = SaveToTestresults(expectImageFilePath, "Expected" + Path.GetFileName(expectImageFilePath));
            var filePathInTestResultFolderOfActual = SaveToTestresults(actualImagePath, Path.GetFileName(actualFullPath));

            Assert.True(File.Exists(actualFullPath), $"actualImagePath not found {actualFullPath}");
            Assert.True(File.Exists(expectFullPath), $"ExpectReferenceImagePath not found \n{expectFullPath}\n copy over \n{actualFullPath}\n if this is a new test case.");

            if (ImageSharpCompare.ImagesAreEqual(actualFullPath, expectFullPath))
            {
                DroptFilesFromTestResultFolder(filePathInTestResultFolderOfExpectation, filePathInTestResultFolderOfActual);
                return;
            }

            var osSpecificDiffFileSuffix = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" : "win";

            var allowedDiffImage = $"{expectFullPath}.diff.{osSpecificDiffFileSuffix}.png";
            var newDiffImage = $"{actualFullPath}.diff.png";

            if (!ImageSharpCompare.ImagesHaveEqualSize(actualFullPath, expectFullPath))
            {
                Assert.True(false, $"Actual Dimension differs from expected \nExpected {expectFullPath}\ndiffers to actual {actualFullPath} \nReplace {expectFullPath} with the new value.");
            }

            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(actualFullPath, expectFullPath))
            {
                await maskImage.SaveAsync(newDiffImage);
            }

            // Uncomment following line to update or create an allowed diff file
            //File.Copy(newDiffImage, allowedDiffImage, true);

            if (File.Exists(allowedDiffImage))
            {
                if (!ImageSharpCompare.ImagesHaveEqualSize(actualFullPath, allowedDiffImage))
                {
                    Assert.True(false, $"AllowedDiffImage Dimension differs from allowed \nReplace {allowedDiffImage} with {actualFullPath}.");
                }

                var resultWithAllowedDiff = ImageSharpCompare.CalcDiff(actualFullPath, expectFullPath, allowedDiffImage);

                Assert.True(resultWithAllowedDiff.PixelErrorCount <= allowedPixelErrorCount, $"Expected PixelErrorCount beyond {allowedPixelErrorCount} but was {resultWithAllowedDiff.PixelErrorCount}\nExpected {expectFullPath}\ndiffers to actual {actualFullPath}\n Diff is {newDiffImage}\n");

                DroptFilesFromTestResultFolder(filePathInTestResultFolderOfExpectation, filePathInTestResultFolderOfActual);
                return;
            }

            var result = ImageSharpCompare.CalcDiff(actualFullPath, expectFullPath);

            Assert.True(result.PixelErrorCount <= allowedPixelErrorCount, $"Expected PixelErrorCount beyond {allowedPixelErrorCount} but was {result.PixelErrorCount}\nExpected {expectFullPath}\ndiffers to actual {actualFullPath}\n Diff is {newDiffImage} \nReplace {actualFullPath} with the new value or store the diff as {allowedDiffImage}.");

            DroptFilesFromTestResultFolder(filePathInTestResultFolderOfExpectation, filePathInTestResultFolderOfActual);
        }

        private static void DroptFilesFromTestResultFolder(string filePathInTestResultFolderOfExpectation, string filePathInTestResultFolderOfActual)
        {
            File.Delete(filePathInTestResultFolderOfExpectation);
            File.Delete(filePathInTestResultFolderOfActual);
        }

        private static string SaveToTestresults(string filePath, string filename)
        {
            var netEnvironment = $"NetRuntime{Environment.Version}";

            var testAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
            var testResultDirectory = Path.Combine(testAssemblyDirectory, "../../../../TestResult");
            if (!Directory.Exists(testResultDirectory))
            {
                Directory.CreateDirectory(testResultDirectory);
            }
            var destinationPathInTestResultDirectory = Path.Combine(testResultDirectory, netEnvironment + filename);

            if (File.Exists(destinationPathInTestResultDirectory))
            {
                File.Delete(destinationPathInTestResultDirectory);
            }

            File.Copy(filePath, destinationPathInTestResultDirectory);

            return destinationPathInTestResultDirectory;
        }
    }
}