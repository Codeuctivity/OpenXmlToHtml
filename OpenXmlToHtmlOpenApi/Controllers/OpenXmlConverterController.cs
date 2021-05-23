using Codeuctivity.OpenXmlToHtml;
using Codeuctivity.PuppeteerSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenXmlToHtmlOpenApi.Controllers
{
    /// <summary>
    /// OpenXml converter
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OpenXmlConverterController : ControllerBase
    {
        private readonly IOpenXmlToHtml _openXmlToHtml;

        /// <summary>
        /// OpenXmlConverter ctor
        /// </summary>
        /// <param name="openXmlToHtml"></param>
        public OpenXmlConverterController(IOpenXmlToHtml openXmlToHtml)
        {
            _openXmlToHtml = openXmlToHtml;
        }

        /// <summary>
        /// Converts OpenXmlFile to HTML
        /// </summary>
        /// <param name="openXmlFile"></param>
        /// <returns>HTML</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConvertToHtml(IFormFile openXmlFile)
        {
            if (openXmlFile.Length > 0)
            {
                var htmlStream = await _openXmlToHtml.ConvertToHtmlAsync(openXmlFile.OpenReadStream());
                return File(htmlStream, "text/html");
            }
            return BadRequest("Request contains no document");
        }

        /// <summary>
        /// Converts OpenXmlFile to PDF
        /// </summary>
        /// <param name="openXmlFile"></param>
        /// <returns>PDF</returns>
        [HttpPost]
        [Route("ConvertToPdf")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConvertToPdf(IFormFile openXmlFile)
        {
            await using var chromiumRenderer = await Renderer.CreateAsync();
            if (openXmlFile.Length > 0)
            {
                var pathHtml = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.html");
                var pathPdf = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

                var htmlStream = await _openXmlToHtml.ConvertToHtmlAsync(openXmlFile.OpenReadStream());
                try
                {
                    using var fileStreamHtml = new FileStream(pathHtml, FileMode.CreateNew);
                    await htmlStream.CopyToAsync(fileStreamHtml);
                    await chromiumRenderer.ConvertHtmlToPdf(pathHtml, pathPdf);
                    var pdf = await System.IO.File.ReadAllBytesAsync(pathPdf);
                    return File(pdf, "application/pdf", $"{openXmlFile.FileName}.pdf");
                }
                finally
                {
                    System.IO.File.Delete(pathHtml);
                    System.IO.File.Delete(pathPdf);
                }
            }
            return BadRequest("Request contains no document");
        }
    }
}