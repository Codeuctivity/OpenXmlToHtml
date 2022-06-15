using Codeuctivity.OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System.Linq;
using System.Xml.Linq;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    /// Replaces any font that is not part a white list of fonts with Arial
    /// </summary>
    public class WebSafeFontsHandler : IFontHandler
    {
        /// <summary>
        /// Web Safe Fonts for HTML and CSS https://www.w3schools.com/cssref/css_websafe_fonts.asp
        /// </summary>
        public string[] WebSafeFontNames { get; private set; }

        private FontHandler FontHandler { get; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public WebSafeFontsHandler()
        {
            WebSafeFontNames = new[] { "Arial", "Verdana", "Helvetica", "Tahoma", "Trebuchet MS", "Times New Roman", "Georgia", "Garamond", "Courier New", "Brush Script MT" };
            FontHandler = new FontHandler();
        }

        /// <summary>
        /// Use this ctor to use a custom list of WebSafeFontNames
        /// </summary>
        /// <param name="webSafeFontNames"></param>
        public WebSafeFontsHandler(string[] webSafeFontNames)
        {
            WebSafeFontNames = webSafeFontNames;
            FontHandler = new FontHandler();
        }

        /// <summary>
        /// Replaces any font not white listed
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public string TranslateParagraphStyleFont(XElement paragraph)
        {
            var unsafeFont = FontHandler.TranslateParagraphStyleFont(paragraph);

            return tranlateToWebSafeFont(unsafeFont);
        }

        /// <summary>
        /// Replaces any font not white listed
        /// </summary>
        /// <param name="run"></param>
        /// <returns></returns>
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