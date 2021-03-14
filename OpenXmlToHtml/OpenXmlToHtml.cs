using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    ///  Converts DOCX to HTML
    /// </summary>
    public class OpenXmlToHtml : IOpenXmlToHtml
    {
        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXmlFilePath"></param>
        /// <param name="destinationHtmlFilePath"></param>
        /// <returns>selfContainedHtmlFilePath</returns>
        public async Task ConvertToHtmlAsync(string sourceOpenXmlFilePath, string destinationHtmlFilePath)
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
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <returns>selfContainedHtml</returns>
        public Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml)
        {
            return ConvertToHtmlAsync(sourceOpenXml, string.Empty);
        }

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <returns>selfContainedHtml</returns>
        public Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle)
        {
            if (sourceOpenXml == null)
            {
                throw new ArgumentNullException(nameof(sourceOpenXml));
            }

            return ConvertToHtmlInternalAsync(sourceOpenXml, fallbackPageTitle, new ImageHandler());
        }

        /// <summary>
        /// Converts docx to html
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <param name="images"></param>
        /// <returns>selfContainedHtml</returns>
        public static Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle, IDictionary<string, byte[]> images)
        {
            if (sourceOpenXml == null)
            {
                throw new ArgumentNullException(nameof(sourceOpenXml));
            }

            return ConvertToHtmlInternalAsync(sourceOpenXml, fallbackPageTitle, new ExportImageHandler(images));
        }

        private static async Task<Stream> ConvertToHtmlInternalAsync(Stream sourceOpenXml, string fallbackPageTitle, IImageHandler imageHandler)
        {
            using var memoryStream = new MemoryStream();
            await sourceOpenXml.CopyToAsync(memoryStream).ConfigureAwait(false);
            sourceOpenXml = memoryStream;

            using var wordProcessingDocument = WordprocessingDocument.Open(sourceOpenXml, true);
            var coreFilePropertiesPart = wordProcessingDocument.CoreFilePropertiesPart;
            var computedPageTitle = coreFilePropertiesPart?.GetXDocument().Descendants(DC.title).FirstOrDefault();
            var pageTitle = string.IsNullOrEmpty(computedPageTitle?.Value) ? fallbackPageTitle : computedPageTitle!.Value;

            var htmlElement = WmlToHtmlConverter.ConvertToHtml(wordProcessingDocument, CreateHtmlConverterSettings(pageTitle, imageHandler));

            var memoryStreamHtml = new MemoryStream();
            htmlElement.Save(memoryStreamHtml);
            memoryStreamHtml.Position = 0;
            return memoryStreamHtml;
        }

        private static WmlToHtmlConverterSettings CreateHtmlConverterSettings(string pageTitle, IImageHandler imageHandler)
        {
            var settings = new WmlToHtmlConverterSettings(pageTitle, imageHandler, new TextSymbolToUnicodeHandler(), new SymbolHandler(), new PageBreakHandler(new BreakHandler()), true, string.Empty, "@page { size: A4 } body { margin: 1cm auto; max-width: 20cm; padding: 0; }", "Codeuctivity-");

            return settings;
        }
    }
}