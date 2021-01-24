using System.Collections.Generic;
using System.Xml.Linq;

using System;
using OpenXmlPowerTools;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;

namespace Codeuctivity.OpenXmlToHtml
{

    /// <summary>
    /// Default handler that transforms every symbol into some html encoded font specific char
    /// </summary>
    public class SymbolHandler : IWordprocessingSymbolHandler
    {
                private DefaultSymbolHandler DefaultSymbolHandler { get; set; }
        private WordprocessingTextSymbolToUnicodeHandler WordprocessingTextSymbolToUnicodeHandler { get; set; }

        public SymbolHandler()
        {
            DefaultSymbolHandler = new DefaultSymbolHandler();
            WordprocessingTextSymbolToUnicodeHandler = new WordprocessingTextSymbolToUnicodeHandler();
        }
        /// <summary>
        /// Default handler that transforms every symbol into some html encoded font specific char
        /// </summary>
        /// <param name="element"></param>
        /// <param name="fontFamily"></param>
        /// <returns></returns>
        public XElement TransformSymbol(XElement element, Dictionary<string, string> fontFamily)
        {
            if (fontFamily.TryGetValue("font-family", out var currentFontFamily) && currentFontFamily == "Symbol")
            {
                // var cs = (string)element.Attribute(W._char);

                // foreach (var item in WordprocessingTextSymbolToUnicodeHandler.WingdingsToUnicode)
                // {
                var     cs = ('\u206F').ToString();
                // }
                    //  var c = Convert.ToInt32(cs, 16);
            return new XElement(Xhtml.span,  "🤩");
            }

            return DefaultSymbolHandler.TransformSymbol(element, fontFamily);
        }
    }
}