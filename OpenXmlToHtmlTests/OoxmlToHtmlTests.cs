using Codeuctivity.OpenXmlToHtml;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlTests
{
    public class OoxmlToHtmlTests
    {
        [Theory]
        [InlineData("EmptyDocument.docx")]
        [InlineData("BasicTextFormated.docx")]
        public async Task ShouldConvertDocument(string testFileName)
        {
            var actualHtmlFilePath = Path.Combine(Path.GetTempPath(), $"Actual{testFileName}.html");
            var expectedHtmlFilePath = $"../../../ExpectedTestOutcome/{testFileName}.png";

            // TODO fix diff between windows and linux
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && testFileName == "BasicTextFormated.docx")
            {
                expectedHtmlFilePath = $"../../../ExpectedTestOutcome/{testFileName}.Linux.png";
            }

            if (File.Exists(actualHtmlFilePath))
            {
                File.Delete(actualHtmlFilePath);
            }

            await OpenXmlToHtml.ConvertToHtmlAsync($"../../../TestInput/{testFileName}", actualHtmlFilePath);

            await DocumentAsserter.AssertRenderedHtmlIsEqual(actualHtmlFilePath, expectedHtmlFilePath);
        }
    }
}