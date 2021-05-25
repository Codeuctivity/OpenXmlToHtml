using Codeuctivity.OpenXmlToHtml;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class ExportImageHandlerTests
    {
        [Fact]
        public void ShouldUseExportImageHandler()
        {
            var sourcePngFilePath = $"../../../TestInput/TestInput.png";
            var exportTarget = new Dictionary<string, byte[]>();
            var someBitmap = new Bitmap(sourcePngFilePath);

            var imageInfo = new ImageInfo
            {
                AltText = "AltText",
                Bitmap = someBitmap,
            };

            var exportImageHandler = new ExportImageHandler(exportTarget);

            var actual = exportImageHandler.TransformImage(imageInfo);

            Assert.True(exportTarget.Count == 1);
            var exportedImage = exportTarget.Single();
            Assert.Equal(exportedImage.Value, ImageToByte(someBitmap));
            Assert.Equal($"<img src=\"cid: { exportedImage.Key}\" alt=\"AltText\" xmlns=\"http://www.w3.org/1999/xhtml\" />", actual.ToString());
        }

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}