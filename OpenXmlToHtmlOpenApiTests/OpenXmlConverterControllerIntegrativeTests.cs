using AngleSharp;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace OpenXmlToHtmlOpenApiTests
{
    public class OpenXmlConverterControllerIntegrativeTests : IClassFixture<WebApplicationFactory<OpenXmlToHtmlOpenApi.Startup>>
    {
        private readonly WebApplicationFactory<OpenXmlToHtmlOpenApi.Startup> _factory;

        public OpenXmlConverterControllerIntegrativeTests(WebApplicationFactory<OpenXmlToHtmlOpenApi.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/", "text/html; charset=utf-8")]
        [InlineData("/swagger/v1/swagger.json", "application/json; charset=utf-8")]
        public async Task ShouldAccessEndpointSuccessfull(string route, string contentType)
        {
            // Arrange
            var client = _factory.CreateClient();
            var expectedUrl = new Uri($"https://localhost{route}");

            // Act
            var response = await client.GetAsync(expectedUrl).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task ShouldConvertOpenXmlToHtml()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost/OpenXmlConverter");
            using var file = new ByteArrayContent(File.ReadAllBytes("../../../TestInput/BasicTextFormated.docx"));
            file.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            var multipartContent = new MultipartFormDataContent
            {
                { file, "openXmlFile", Path.GetFileName("BasicTextFormated.docx") }
            };
            request.Content = multipartContent;

            // Act
            var response = await client.SendAsync(request).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html", response.Content.Headers.ContentType.MediaType);
            await AssertHtmlContentAsync(response.Content, "Lorem Ipsum");
        }

        [Fact]
        public async Task ShouldConvertOpenXmlToPdf()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost/OpenXmlConverter/ConvertToPdf");
            using var file = new ByteArrayContent(File.ReadAllBytes("../../../TestInput/BasicTextFormated.docx"));
            file.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            var multipartContent = new MultipartFormDataContent
            {
                { file, "openXmlFile", Path.GetFileName("BasicTextFormated.docx") }
            };
            request.Content = multipartContent;

            // Act
            var response = await client.SendAsync(request).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", response.Content.Headers.ContentType.MediaType);
        }

        private static async Task AssertHtmlContentAsync(HttpContent content, string expectedText)
        {
            var context = BrowsingContext.New(Configuration.Default);

            var convertedMarkup = await content.ReadAsStringAsync();
            var document = await context.OpenAsync(req => req.Content(convertedMarkup));

            var actualText = document.QuerySelector(".Codeuctivity-000000")?.TextContent;
            Assert.Equal(expectedText, actualText);
        }
    }
}