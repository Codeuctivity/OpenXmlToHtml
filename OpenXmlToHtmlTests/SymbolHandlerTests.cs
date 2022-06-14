using Codeuctivity.OpenXmlPowerTools;
using Codeuctivity.OpenXmlToHtml;

using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class SymbolHandlerTests
    {
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