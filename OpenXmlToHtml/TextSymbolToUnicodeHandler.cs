using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System.Collections.Generic;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    /// Replaces any char of wingdings with the Unicode equivalent
    /// </summary>
    public class TextSymbolToUnicodeHandler : ITextHandler
    {
        /// <summary>
        /// Dictionary used to translate symbol chars to Unicode
        /// </summary>
        private static readonly Dictionary<char, char> SymbolToUnicode = new Dictionary<char, char>
        {
            { '','•' }
        };

        /// <summary>
        /// Replaces any char of wingdings with the Unicode equivalent
        /// </summary>
        public string TransformText(string text, Dictionary<string, string> fontFamily)
        {
            if (fontFamily.TryGetValue("font-family", out var currentFontFamily) && currentFontFamily == "Symbol")
            {
                foreach (var item in SymbolToUnicode)
                {
                    text = text.Replace(item.Key, item.Value);
                }
            }

            return text;
        }
    }
}