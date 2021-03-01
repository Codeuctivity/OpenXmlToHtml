using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    ///  Converts docx to html
    /// </summary>
    public static class OpenXmlToHtml
    {
        /// <summary>
        /// Converts docx to html
        /// </summary>
        /// <param name="sourceOpenXmlFilePath"></param>
        /// <param name="destinationHtmlFilePath"></param>
        /// <returns>selfContainedHtmlFilePath</returns>
        public static async Task ConvertToHtmlAsync(string sourceOpenXmlFilePath, string destinationHtmlFilePath)
        {
            if (!File.Exists(sourceOpenXmlFilePath))
            {
                throw new FileNotFoundException(sourceOpenXmlFilePath);
            }

            using var sourceIpenXml = new FileStream(sourceOpenXmlFilePath, FileMode.Open, FileAccess.Read);
            using var html = await ConvertToHtmlAsync(sourceIpenXml, sourceOpenXmlFilePath).ConfigureAwait(false);
            using var destinationHtmlFile = new FileStream(destinationHtmlFilePath, FileMode.CreateNew, FileAccess.Write);
            await html.CopyToAsync(destinationHtmlFile).ConfigureAwait(false);
        }

        /// <summary>
        /// Converts docx to html
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <returns>selfContainedHtml</returns>
        public static Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml)
        {
            return ConvertToHtmlAsync(sourceOpenXml, string.Empty);
        }

        /// <summary>
        /// Converts docx to html
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <returns>selfContainedHtml</returns>
        public static Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle)
        {
            if (sourceOpenXml == null)
            {
                throw new ArgumentNullException(nameof(sourceOpenXml));
            }

            return ConvertToHtmlInternalAsync(sourceOpenXml, fallbackPageTitle);
        }

        private static async Task<Stream> ConvertToHtmlInternalAsync(Stream sourceOpenXml, string fallbackPageTitle)
        {
            using var memoryStream = new MemoryStream();
            await sourceOpenXml.CopyToAsync(memoryStream).ConfigureAwait(false);
            sourceOpenXml = memoryStream;

            using var wordProcessingDocument = WordprocessingDocument.Open(sourceOpenXml, true);
            var coreFilePropertiesPart = wordProcessingDocument.CoreFilePropertiesPart;
            var computedPageTitle = coreFilePropertiesPart?.GetXDocument().Descendants(DC.title).FirstOrDefault();
            var pageTitle = string.IsNullOrEmpty(computedPageTitle?.Value) ? fallbackPageTitle : computedPageTitle!.Value;

            var htmlElement = WmlToHtmlConverter.ConvertToHtml(wordProcessingDocument, CreateHtmlConverterSettings(pageTitle));

            var memoryStreamHtml = new MemoryStream();
            htmlElement.Save(memoryStreamHtml);
            memoryStreamHtml.Position = 0;
            return memoryStreamHtml;
        }

        private static WmlToHtmlConverterSettings CreateHtmlConverterSettings(string pageTitle)
        {
            var settings = new WmlToHtmlConverterSettings(pageTitle, new ImageHandler(), new TextSymbolToUnicodeHandler(), new SymbolHandler(), new PageBreakHandler(new BreakHandler()), true, string.Empty, "@page { size: A4 } body { margin: 1cm auto; max-width: 20cm; padding: 0; }", "Codeuctivity-");

            return settings;
        }
    }
}