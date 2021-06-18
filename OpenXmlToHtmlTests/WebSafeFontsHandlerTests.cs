using Codeuctivity.OpenXmlToHtml;
using OpenXmlPowerTools;
using System.Xml.Linq;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class WebSafeFontsHandlerTests
    {
        [Fact]
        public void ShouldTranslateFontInRunSymbolWithFontHandler()
        {
            var fontHandler = new WebSafeFontsHandler();

            var element = new XElement("run", new XElement(W.sym, new XAttribute(W.font, "SomeBadSymbolFont")), new XAttribute(PtOpenXml.FontName, "SomeBadRunFont"));

            var actual = fontHandler.TranslateRunStyleFont(element);

            Assert.Equal("Arial", actual);
        }

        [Fact]
        public void ShouldTranslateFontInRunWithFontHandler()
        {
            var fontHandler = new WebSafeFontsHandler();

            var element = new XElement("run", new XAttribute(PtOpenXml.FontName, "SomeBadRunFont"));

            var actual = fontHandler.TranslateRunStyleFont(element);

            Assert.Equal("Arial", actual);
        }

        [Fact]
        public void ShouldTranslateFontInParagraphWithFontHandler()
        {
            var fontHandler = new WebSafeFontsHandler();

            var element = new XElement("run", new XAttribute(PtOpenXml.FontName, "SomeBadRunFont"));

            var actual = fontHandler.TranslateParagraphStyleFont(element);

            Assert.Equal("Arial", actual);
        }
    }
}