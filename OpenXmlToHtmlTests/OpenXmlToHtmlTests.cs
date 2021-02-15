using Codeuctivity.OpenXmlToHtml;
using Codeuctivity.PuppeteerSharp;
using PdfSharp.Pdf.IO;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class OpenXmlToHtmlTests
    {
        [Theory]
        [InlineData("EmptyDocument.docx", 0)]
        [InlineData("WingdingsSymbols.docx", 71000)]
        [InlineData("BasicTextFormated.docx", 50)]
        [InlineData("Images.docx", 5)]
        public async Task ShouldConvertDocumentIntegrativeWithKnownAberrancyTest(string testFileName, int allowedPixelErrorCount)
        {
            var sourceOpenXmlFilePath = $"../../../TestInput/{testFileName}";
            var actualHtmlFilePath = Path.Combine(Path.GetTempPath(), $"Actual{testFileName}.html");
            var expectedHtmlFilePath = $"../../../ExpectedTestOutcome/{testFileName}.png";

            if (File.Exists(actualHtmlFilePath))
            {
                File.Delete(actualHtmlFilePath);
            }

            await OpenXmlToHtml.ConvertToHtmlAsync(sourceOpenXmlFilePath, actualHtmlFilePath);
            await DocumentAsserter.AssertRenderedHtmlIsEqual(actualHtmlFilePath, expectedHtmlFilePath, allowedPixelErrorCount);
        }

        [Fact]
        public async Task ShouldConvertDocumentIntegrativeWithToExpectedPageQuantityTest()
        {
            var testFileName = "TwoPages.docx";
            var sourceOpenXmlFilePath = $"../../../TestInput/{testFileName}";
            var actualHtmlFilePath = Path.Combine(Path.GetTempPath(), $"Actual{testFileName}.html");

            if (File.Exists(actualHtmlFilePath))
            {
                File.Delete(actualHtmlFilePath);
            }

            await OpenXmlToHtml.ConvertToHtmlAsync(sourceOpenXmlFilePath, actualHtmlFilePath);

            await using var chromiumRenderer = await Renderer.CreateAsync();
            var pathPdfizedHtml = actualHtmlFilePath + ".pdf";
            await chromiumRenderer.ConvertHtmlToPdf(actualHtmlFilePath, pathPdfizedHtml);
            AssertPdfPageCount(pathPdfizedHtml, 2);
        }

        private static void AssertPdfPageCount(string pathPdfizedHtml, int expectePageQuantity)
        {
            var pdfReader = PdfReader.Open(pathPdfizedHtml, PdfDocumentOpenMode.ReadOnly);
            Assert.Equal(expectePageQuantity, pdfReader.PageCount);
        }
    }
}