using Codeuctivity.OpenXmlToHtml;
using OpenXmlPowerTools;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class OpenXmlToHtmlTests
    {
        [Theory]
        [InlineData("WingdingsSymbols.docx")]
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
        public void ShouldTranslateTextWithCustomGlyphToUnicode(string original, string expectedEquivalent, string fontFamily)
        {
            var currentStyle = new Dictionary<string, string> { { "font-family", fontFamily } };

            var WordprocessingTextSymbolToUnicodeHandler = new WordprocessingTextSymbolToUnicodeHandler();

            var actual = WordprocessingTextSymbolToUnicodeHandler.TransformText(original, currentStyle);

            Assert.Equal(expectedEquivalent, actual);
        }

        [Fact]
        public void ShouldTranslateSymbolsToUnicode()
        {
            var fontFamily = new Dictionary<string, string>
            {
                { "font-family", "Symbol" }
            };

            var symbolHandler = new SymbolHandler();
            var element = new XElement("symbol", new XAttribute(W._char, "F0D7"));

            var actual = symbolHandler.TransformSymbol(element, fontFamily);

            Assert.Equal("<span xmlns=\"http://www.w3.org/1999/xhtml\">⋅</span>", actual.ToString());
        }
    }
}