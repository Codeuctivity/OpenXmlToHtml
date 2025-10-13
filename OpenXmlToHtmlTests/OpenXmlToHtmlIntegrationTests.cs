using Codeuctivity.HtmlRenderer;
using Codeuctivity.OpenXmlToHtml;
using PdfSharp.Pdf.IO;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class OpenXmlToHtmlIntegrationTests
    {
        private const string xhtmlPrimer = "<html xmlns=\"http://www.w3.org/1999/xhtml\"";
        private readonly OpenXmlToHtml openXmlToHtml;

        public OpenXmlToHtmlIntegrationTests()
        {
            openXmlToHtml = new OpenXmlToHtml();
        }

        [Theory]
        [InlineData("EmptyDocument.docx", 0, false)]
        //[InlineData("WingdingsSymbols.docx", 71000)]
        [InlineData("Symbols.docx", 71000, false)]
        [InlineData("BasicTextFormated.docx", 15000, false)]
        [InlineData("Images.docx", 9000, true)]
        [InlineData("Font.docx", 4500, true)]
        public async Task ShouldConvertDocumentIntegrativeWithKnownAberrancyTest(string testFileName, int allowedPixelErrorCount, bool useWebSafeFonts)
        {
            var sourceOpenXmlFilePath = $"../../../TestInput/{testFileName}";
            var actualHtmlFilePath = Path.Combine(Path.GetTempPath(), $"Actual{testFileName}.html");
            var expectedHtmlFilePath = $"../../../ExpectedTestOutcome/{testFileName}.png";

            if (File.Exists(actualHtmlFilePath))
            {
                File.Delete(actualHtmlFilePath);
            }

            await openXmlToHtml.ConvertToHtmlAsync(sourceOpenXmlFilePath, actualHtmlFilePath, useWebSafeFonts);

            AssertXhtmlIsValid(actualHtmlFilePath);
            await DocumentAsserter.AssertRenderedHtmlIsEqual(actualHtmlFilePath, expectedHtmlFilePath, allowedPixelErrorCount);
        }

        [Theory]
        [InlineData("Images.docx")]
        public async Task ShouldConvertDocumentAndExportImagesIntegrativeTest(string testFileName)
        {
            var sourceOpenXmlFilePath = $"../../../TestInput/{testFileName}";

            using var sourceIpenXml = new FileStream(sourceOpenXmlFilePath, FileMode.Open, FileAccess.Read);
            var exportedImages = new Dictionary<string, byte[]>();

            var actuelHtml = await OpenXmlToHtml.ConvertToHtmlAsync(sourceIpenXml, "fallbackTitle", exportedImages, false);

            Assert.Equal(2, exportedImages.Count);

            Assert.True(IsValidBitmap(exportedImages.First().Value));
            Assert.True(IsValidBitmap(exportedImages.Last().Value));

            AssertXhtmlIsValid(actuelHtml);
        }

        private static bool IsValidBitmap(byte[] blob)
        {
            using var image = SKBitmap.Decode(blob);
            return image != null && image.Width > 1 && image.Height > 1;
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

            await openXmlToHtml.ConvertToHtmlAsync(sourceOpenXmlFilePath, actualHtmlFilePath);

            AssertXhtmlIsValid(actualHtmlFilePath);
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

        private static void AssertXhtmlIsValid(string actualHtmlFilePath)
        {
            var messages = new StringBuilder();
            var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema, DtdProcessing = DtdProcessing.Ignore };
            settings.ValidationEventHandler += (sender, args) => messages.AppendLine(args.Message);
            var reader = XmlReader.Create(actualHtmlFilePath, settings);
#pragma warning disable S108 // Nested blocks of code should not be left empty
            while (reader.Read()) { }
#pragma warning restore S108 // Nested blocks of code should not be left empty

            if (!File.ReadAllText(actualHtmlFilePath).Contains(xhtmlPrimer))
            {
                messages.AppendLine("Xhtml root element missing");
            }

            Assert.True(messages.Length == 0, messages.ToString());
        }

        private void AssertXhtmlIsValid(Stream actualHtml)
        {
            var messages = new StringBuilder();
            var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema, DtdProcessing = DtdProcessing.Ignore };
            settings.ValidationEventHandler += (sender, args) => messages.AppendLine(args.Message);
            var reader = XmlReader.Create(actualHtml, settings);
#pragma warning disable S108 // Nested blocks of code should not be left empty
            while (reader.Read()) { }
#pragma warning restore S108 // Nested blocks of code should not be left empty

            Assert.True(messages.Length == 0, messages.ToString());
        }
    }
}