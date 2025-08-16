using Codeuctivity.OpenXmlPowerTools;
using Codeuctivity.OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    /// Exports every image of an Open XML
    /// </summary>
    public class ExportImageHandler : IImageHandler
    {
        /// <summary>
        /// Images of Open XML
        /// </summary>
        public IDictionary<string, byte[]> Images { get; }

        /// <summary>
        /// Transforms OpenXml Images to HTML embeddable images
        /// </summary>
        /// <param name="images"></param>
        public ExportImageHandler(IDictionary<string, byte[]> images)
        {
            Images = images;
        }

        /// <summary>
        /// Transforms images to Content-ID based embedded value
        /// </summary>
        /// <param name="imageInfo"></param>
        /// <returns></returns>
        public XElement TransformImage(ImageInfo imageInfo)
        {
            if (imageInfo?.Image == null)
            {
                throw new ArgumentException("ImageInfo or Image cannot be null", nameof(imageInfo));
            }

            var cid = Guid.NewGuid().ToString();
            using var memoryStream = new MemoryStream();
            imageInfo.Image.CopyTo(memoryStream);

            Images.Add(cid, memoryStream.ToArray());

            var cidReference = $"cid: {cid}";

            return new XElement(Xhtml.img, new XAttribute(NoNamespace.src, cidReference), imageInfo.ImgStyleAttribute, !string.IsNullOrEmpty(imageInfo?.AltText) ? new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
        }
    }
}