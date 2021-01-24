using System.Collections.Generic;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    /// Replaces any char of wingdings with the Unicode equivalent
    /// </summary>
    public class WordprocessingTextSymbolToUnicodeHandler : IWordprocessingTextHandler
    {
        public static readonly Dictionary<char, char> WingdingsToUnicode = new Dictionary<char, char>
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
                foreach (var item in WingdingsToUnicode)
                {
                    text = text.Replace(item.Key, item.Value);
                }
            }
            return text;
        }
    }
}