using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public Task ConvertToHtmlAsync(string sourceOpenXmlFilePath, string destinationHtmlFilePath)
        {
            return ConvertToHtmlAsync(sourceOpenXmlFilePath, destinationHtmlFilePath, false);
        }

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXmlFilePath"></param>
        /// <param name="destinationHtmlFilePath"></param>
        /// <param name="useWebSafeFonts">Use 'true' to replace every non web safe font with some fallback. Default is false.</param>
        /// <returns>selfContainedHtmlFilePath</returns>
        public async Task ConvertToHtmlAsync(string sourceOpenXmlFilePath, string destinationHtmlFilePath, bool useWebSafeFonts)
        {
            if (!File.Exists(sourceOpenXmlFilePath))
            {
                throw new FileNotFoundException(sourceOpenXmlFilePath);
            }

            using var sourceIpenXml = new FileStream(sourceOpenXmlFilePath, FileMode.Open, FileAccess.Read);
            using var html = await ConvertToHtmlAsync(sourceIpenXml, sourceOpenXmlFilePath, useWebSafeFonts).ConfigureAwait(false);
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
            return ConvertToHtmlAsync(sourceOpenXml, false);
        }

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="useWebSafeFonts">Use 'true' to replace every non web safe font with some fallback. Default is false.</param>
        /// <returns>selfContainedHtml</returns>
        public Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, bool useWebSafeFonts)
        {
            return ConvertToHtmlAsync(sourceOpenXml, string.Empty, useWebSafeFonts);
        }

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <returns>selfContainedHtml</returns>
        public Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle)
        {
            return ConvertToHtmlAsync(sourceOpenXml, fallbackPageTitle, false);
        }

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <param name="useWebSafeFonts">Use 'true' to replace every non web safe font with some fallback. Default is false.</param>
        /// <returns>selfContainedHtml</returns>
        public Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle, bool useWebSafeFonts)
        {
            if (sourceOpenXml == null)
            {
                throw new ArgumentNullException(nameof(sourceOpenXml));
            }

            return ConvertToHtmlInternalAsync(sourceOpenXml, fallbackPageTitle, new ImageHandler(), useWebSafeFonts);
        }

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <param name="images"></param>
        /// <param name="useWebSafeFonts">Use 'true' to replace every non web safe font with some fallback. Default is false.</param>
        /// <returns>selfContainedHtml</returns>
        public static Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle, IDictionary<string, byte[]> images, bool useWebSafeFonts)
        {
            if (sourceOpenXml == null)
            {
                throw new ArgumentNullException(nameof(sourceOpenXml));
            }

            return ConvertToHtmlInternalAsync(sourceOpenXml, fallbackPageTitle, new ExportImageHandler(images), useWebSafeFonts);
        }

        private static async Task<Stream> ConvertToHtmlInternalAsync(Stream sourceOpenXml, string fallbackPageTitle, IImageHandler imageHandler, bool useWebSafeFonts)
        {
            using var memoryStream = new MemoryStream();
            await sourceOpenXml.CopyToAsync(memoryStream).ConfigureAwait(false);
            sourceOpenXml = memoryStream;

            using var wordProcessingDocument = WordprocessingDocument.Open(sourceOpenXml, true);
            var coreFilePropertiesPart = wordProcessingDocument.CoreFilePropertiesPart;
            var computedPageTitle = coreFilePropertiesPart?.GetXDocument().Descendants(DC.title).FirstOrDefault();
            var pageTitle = string.IsNullOrEmpty(computedPageTitle?.Value) ? fallbackPageTitle : computedPageTitle!.Value;

            var htmlElement = WmlToHtmlConverter.ConvertToHtml(wordProcessingDocument, CreateHtmlConverterSettings(pageTitle, imageHandler, useWebSafeFonts ? new WebSafeFontsHandler() : new FontHandler()));
            var html = new XDocument(new XDocumentType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null), htmlElement);
            var memoryStreamHtml = new MemoryStream();
            html.Save(memoryStreamHtml, SaveOptions.DisableFormatting);
            memoryStreamHtml.Position = 0;
            return memoryStreamHtml;
        }

        private static WmlToHtmlConverterSettings CreateHtmlConverterSettings(string pageTitle, IImageHandler imageHandler, IFontHandler fontHandler)
        {
            var settings = new WmlToHtmlConverterSettings(pageTitle, imageHandler, new TextSymbolToUnicodeHandler(), new SymbolHandler(), new PageBreakHandler(new BreakHandler()), fontHandler, true, string.Empty, "@page { size: A4 } body { margin: 1cm auto; max-width: 20cm; padding: 0; }", "Codeuctivity-");

            return settings;
        }
    }
}