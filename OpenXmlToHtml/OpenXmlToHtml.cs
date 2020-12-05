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
    public static class OpenXmlToHtml
    {
        public static async Task ConvertToHtmlAsync(string sourceOpenXmlFilePath, string destinationHtmlFilePath)
        {
            if (!File.Exists(sourceOpenXmlFilePath))
            {
                throw new FileNotFoundException(sourceOpenXmlFilePath);
            }

            using var sourceIpenXml = new FileStream(sourceOpenXmlFilePath, FileMode.Open);
            using var html = await ConvertToHtmlAsync(sourceIpenXml).ConfigureAwait(false);
            using var destinationHtmlFile = new FileStream(destinationHtmlFilePath, FileMode.CreateNew, FileAccess.Write);
            await html.CopyToAsync(destinationHtmlFile).ConfigureAwait(false);
        }

        public static async Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string sourceOpenXmlFilePath = "")
        {
            if (sourceOpenXml == null)
            {
                throw new ArgumentNullException(nameof(sourceOpenXml));
            }

            if (!sourceOpenXml.CanSeek)
            {
                var memoryStream = new MemoryStream();
                await sourceOpenXml.CopyToAsync(memoryStream).ConfigureAwait(false);
                sourceOpenXml = memoryStream;
            }

            using var wordProcessingDocument = WordprocessingDocument.Open(sourceOpenXml, true);
            var coreFilePropertiesPart = wordProcessingDocument.CoreFilePropertiesPart;
            var pageTitle = (string)coreFilePropertiesPart?.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? sourceOpenXmlFilePath;

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
                CssClassPrefix = "pt-",
                RestrictToSupportedLanguages = false,
                RestrictToSupportedNumberingFormats = false,
                ImageHandler = imageInfo =>
                {
                    using var memoryStream = new MemoryStream();
                    imageInfo.Bitmap.Save(memoryStream, null);
                    var base64 = Convert.ToBase64String(memoryStream.ToArray());
                    var format = imageInfo.Bitmap.RawFormat;
                    var codec = ImageCodecInfo.GetImageDecoders().First<ImageCodecInfo>(c => c.FormatID == format.Guid);
                    var mimeType = codec.MimeType;

                    var imageSource = $"data:{mimeType};base64,{base64}";

                    return new XElement(Xhtml.img, new XAttribute(NoNamespace.src, imageSource), imageInfo.ImgStyleAttribute, imageInfo.AltText != null ? new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                }
            };
        }
    }
}