using OpenXmlPowerTools;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Codeuctivity.OpenXmlToHtml
{
    public class ExportImageHandler : IImageHandler
    {
        public IDictionary<string, byte[]> Images { get; }

        public ExportImageHandler(IDictionary<string, byte[]> images)
        {
            Images = images;
        }

        public XElement TransformImage(ImageInfo imageInfo)
        {
            var cid = Guid.NewGuid().ToString();
            using var memoryStream = new MemoryStream();
            imageInfo.Bitmap.Save(memoryStream, imageInfo.Bitmap.RawFormat);

            Images.Add(cid, memoryStream.ToArray());

            var cidReference = $"cid: {cid}";

            return new XElement(Xhtml.img, new XAttribute(NoNamespace.src, cidReference), imageInfo.ImgStyleAttribute, imageInfo?.AltText != null ? new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
        }
    }
}