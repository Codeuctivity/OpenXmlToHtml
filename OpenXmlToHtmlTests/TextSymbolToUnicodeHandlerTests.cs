using Codeuctivity.OpenXmlToHtml;
using System.Collections.Generic;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class TextSymbolToUnicodeHandlerTests
    {
        [Theory]
        [InlineData("1", "•1", "Symbol")]
        [InlineData("1", "1", "arial")]
        public void ShouldTranslateTextWithCustomGlyphToUnicode(string original, string expectedEquivalent, string fontFamily)
        {
            var currentStyle = new Dictionary<string, string> { { "font-family", fontFamily } };

            var WordprocessingTextSymbolToUnicodeHandler = new TextSymbolToUnicodeHandler();

            var actual = WordprocessingTextSymbolToUnicodeHandler.TransformText(original, currentStyle);

            Assert.Equal(expectedEquivalent, actual);
        }
    }
}