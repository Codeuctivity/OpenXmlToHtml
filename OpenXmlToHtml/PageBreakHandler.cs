using OpenXmlPowerTools;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    ///
    /// </summary>
    public class PageBreakHandler : IBreakHandler
    {
        /// <summary>
        /// DefaultBreakHandler is used if TransformBreak is not applied to a page break
        /// </summary>
        public IBreakHandler DefaultBreakHandler { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="defaultBreakHandler"></param>
        public PageBreakHandler(IBreakHandler defaultBreakHandler)
        {
            DefaultBreakHandler = defaultBreakHandler;
        }

        /// <summary>
        /// Default handler that transforms breaks into some HTML specific equivalent
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public IEnumerable<XNode> TransformBreak(XElement element)
        {
            if (element.Attribute(W.type)?.Value == "page")
            {
                var pageBreakDiv = new XElement(Xhtml.div);
                pageBreakDiv.Add(new XAttribute(H.Style, "break-before: page;"));
                return new XNode[] { pageBreakDiv };
            }

            return DefaultBreakHandler.TransformBreak(element);
        }
    }
}