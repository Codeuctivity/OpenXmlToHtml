using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        /// <returns></returns>
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
        /// <returns></returns>
        public static Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml)
        {
            return ConvertToHtmlAsync(sourceOpenXml, string.Empty);
        }

        /// <summary>
        /// Converts docx to html
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <returns></returns>
        public static async Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle)
        {
            if (sourceOpenXml == null)
            {
                throw new ArgumentNullException(nameof(sourceOpenXml));
            }

            using var memoryStream = new MemoryStream();
            await sourceOpenXml.CopyToAsync(memoryStream).ConfigureAwait(false);
            sourceOpenXml = memoryStream;

            using var wordProcessingDocument = WordprocessingDocument.Open(sourceOpenXml, true);
            var coreFilePropertiesPart = wordProcessingDocument.CoreFilePropertiesPart;
            var pageTitle = (string)coreFilePropertiesPart?.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? fallbackPageTitle;

            // TODO: Determine max-width from size of content area.
            var htmlElement = WmlToHtmlConverter.ConvertToHtml(wordProcessingDocument, CreateHtmlConverterSettings(pageTitle));

            var html = new XDocument(new XDocumentType("html", null, null, null), htmlElement);

            var memoryStreamHtml = new MemoryStream();
            html.Save(memoryStreamHtml);
            memoryStreamHtml.Position = 0;
            return memoryStreamHtml;
        }

        private static WmlToHtmlConverterSettings CreateHtmlConverterSettings(string pageTitle)
        {
            return new WmlToHtmlConverterSettings()
            {
                AdditionalCss = "@page { size: A4 } body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                PageTitle = pageTitle,
                FabricateCssClasses = true,
                CssClassPrefix = "Codeuctivity-",
                RestrictToSupportedLanguages = false,
                RestrictToSupportedNumberingFormats = false,
                ImageHandler = imageInfo =>
                {
                    using var memoryStream = new MemoryStream();
                    imageInfo.Bitmap.Save(memoryStream, imageInfo.Bitmap.RawFormat);
                    var base64 = Convert.ToBase64String(memoryStream.ToArray());
                    var format = imageInfo.Bitmap.RawFormat;
                    var codec = ImageCodecInfo.GetImageDecoders().First(imageCodecInfo => imageCodecInfo.FormatID == format.Guid);
                    var mimeType = codec.MimeType;

                    var imageSource = $"data:{mimeType};base64,{base64}";

                    return new XElement(Xhtml.img, new XAttribute(NoNamespace.src, imageSource), imageInfo.ImgStyleAttribute, imageInfo.AltText != null ? new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                }
            };
        }
    }
}