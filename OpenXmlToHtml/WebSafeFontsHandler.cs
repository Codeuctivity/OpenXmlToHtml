using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System.Linq;
using System.Xml.Linq;

namespace Codeuctivity.OpenXmlToHtml
{
    public class WebSafeFontsHandler : IFontHandler
    {
        /// <summary>
        /// Best Web Safe Fonts for HTML and CSS https://www.w3schools.com/cssref/css_websafe_fonts.asp
        /// </summary>
        public string[] WebSafeFontNames { get; private set; }

        public FontHandler FontHandler { get; }

        public WebSafeFontsHandler()
        {
            WebSafeFontNames = new[] { "Arial", "Verdana", "Helvetica", "Tahoma", "Trebuchet MS", "Times New Roman", "Georgia", "Garamond", "Courier New", "Brush Script MT" };
            FontHandler = new FontHandler();
        }

        public WebSafeFontsHandler(string[] webSafeFontNames)
        {
            WebSafeFontNames = webSafeFontNames;
            FontHandler = new FontHandler();
        }

        public string TranslateParagraphStyleFont(XElement paragraph)
        {
            var unsafeFont = FontHandler.TranslateParagraphStyleFont(paragraph);

            return tranlateToWebSafeFont(unsafeFont);
        }

        public string TranslateRunStyleFont(XElement run)
        {
            var unsafeFont = FontHandler.TranslateRunStyleFont(run);

            return tranlateToWebSafeFont(unsafeFont);
        }

        private string tranlateToWebSafeFont(string unsafeFont)
        {
            if (WebSafeFontNames.Contains(unsafeFont))
            {
                return unsafeFont;
            }

            return "Arial";
        }
    }
}