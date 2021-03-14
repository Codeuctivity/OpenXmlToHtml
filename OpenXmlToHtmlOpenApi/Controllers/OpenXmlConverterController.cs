using Codeuctivity.OpenXmlToHtml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}