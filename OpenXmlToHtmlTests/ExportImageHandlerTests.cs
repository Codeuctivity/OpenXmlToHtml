using Codeuctivity.OpenXmlToHtml;
using OpenXmlPowerTools.OpenXMLWordprocessingMLToHtmlConverter;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class ExportImageHandlerTests
    {
        [Fact]
        public async Task ShouldUseExportImageHandlerAsync()
        {
            var sourcePngFilePath = $"../../../TestInput/TestInput.png";
            var exportTarget = new Dictionary<string, byte[]>();
            using var fileStream = new FileStream(sourcePngFilePath, FileMode.Open, FileAccess.Read);
            using var memorystream = new MemoryStream();
            await fileStream.CopyToAsync(memorystream);
            var expectedImage = memorystream.ToArray();

            fileStream.Position = 0;

            var imageInfo = new ImageInfo
            {
                AltText = "AltText",
                Image = fileStream,
            };

            var exportImageHandler = new ExportImageHandler(exportTarget);

            var actual = exportImageHandler.TransformImage(imageInfo);

            Assert.True(exportTarget.Count == 1);
            var exportedImage = exportTarget.Single();

            Assert.Equal(exportedImage.Value, expectedImage);
            Assert.Equal($"<img src=\"cid: { exportedImage.Key}\" alt=\"AltText\" xmlns=\"http://www.w3.org/1999/xhtml\" />", actual.ToString());
        }
    }
}