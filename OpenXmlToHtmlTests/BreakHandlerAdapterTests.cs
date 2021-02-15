using Codeuctivity.OpenXmlToHtml;
using Moq;
using OpenXmlPowerTools;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class BreakHandlerAdapterTests
    {
        [Fact]
        public void ShouldTranslatePageBreaks()
        {
            var breakHandler = new Mock<IBreakHandler>();
            var breakHandlerAdapter = new PageBreakHandler(breakHandler.Object);

            var element = new XElement("br", new XAttribute(W.type, "page"));

            var actual = breakHandlerAdapter.TransformBreak(element);

            Assert.Equal("<div Style=\"break-before: page;\" xmlns=\"http://www.w3.org/1999/xhtml\" />", actual.Single().ToString());
            breakHandler.Verify(m => m.TransformBreak(It.IsAny<XElement>()), Times.Never());
        }

        [Fact]
        public void ShouldTranslatePage()
        {
            var breakHandler = new Mock<IBreakHandler>();
            var breakHandlerAdapter = new PageBreakHandler(breakHandler.Object);

            var element = new XElement("br");

            breakHandlerAdapter.TransformBreak(element);

            breakHandler.Verify(m => m.TransformBreak(element), Times.Once);
        }
    }
}