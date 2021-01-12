using Codeuctivity.OpenXmlToHtml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class OpenXmlToHtmlTests
    {
        [Theory]
        // [InlineData("WingdingsSymbols.docx")]
        [InlineData("EmptyDocument.docx")]
        [InlineData("BasicTextFormated.docx")]
        [InlineData("Images.docx")]
        public async Task ShouldConvertDocument(string testFileName)
        {
            var sourceOpenXmlFilePath = $"../../../TestInput/{testFileName}";
            var actualHtmlFilePath = Path.Combine(Path.GetTempPath(), $"Actual{testFileName}.html");
            var expectedHtmlFilePath = $"../../../ExpectedTestOutcome/{testFileName}.png";

            if (File.Exists(actualHtmlFilePath))
            {
                File.Delete(actualHtmlFilePath);
            }

            await OpenXmlToHtml.ConvertToHtmlAsync(sourceOpenXmlFilePath, actualHtmlFilePath);

            await DocumentAsserter.AssertRenderedHtmlIsEqual(actualHtmlFilePath, expectedHtmlFilePath);
        }

        [Theory]
        [InlineData("1", "•1", "Symbol")]
        [InlineData("1", "1", "arial")]
        public void ShouldTranslateToUnicode(string original, string expectedEquivalent, string fontFamily)
        {
            var currentStyle = new Dictionary<string, string> { { "font-family", fontFamily } };

            var WordprocessingTextSymbolToUnicodeHandler = new WordprocessingTextSymbolToUnicodeHandler();

            var actual = WordprocessingTextSymbolToUnicodeHandler.TransformText(original, currentStyle);

            Assert.Equal(expectedEquivalent, actual);
        }
    }
}