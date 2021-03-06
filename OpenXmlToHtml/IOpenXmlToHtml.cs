﻿using System.IO;
using System.Threading.Tasks;

namespace Codeuctivity.OpenXmlToHtml
{
    /// <summary>
    ///  Converts DOCX to HTML
    /// </summary>
    public interface IOpenXmlToHtml
    {
        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXmlFilePath"></param>
        /// <param name="destinationHtmlFilePath"></param>
        /// <returns>selfContainedHtmlFilePath</returns>
        Task ConvertToHtmlAsync(string sourceOpenXmlFilePath, string destinationHtmlFilePath);

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXmlFilePath"></param>
        /// <param name="destinationHtmlFilePath"></param>
        /// <param name="useWebSafeFonts">Use 'true' to replace every non web safe font with some fallback. Default is false.</param>
        /// <returns>selfContainedHtmlFilePath</returns>
        Task ConvertToHtmlAsync(string sourceOpenXmlFilePath, string destinationHtmlFilePath, bool useWebSafeFonts);

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <returns>selfContainedHtml</returns>
        Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml);

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="useWebSafeFonts">Use 'true' to replace every non web safe font with some fallback. Default is false.</param>
        /// <returns>selfContainedHtml</returns>
        Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, bool useWebSafeFonts);

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <returns>selfContainedHtml</returns>
        Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle);

        /// <summary>
        /// Converts DOCX to HTML
        /// </summary>
        /// <param name="sourceOpenXml"></param>
        /// <param name="fallbackPageTitle"></param>
        /// <param name="useWebSafeFonts">Use 'true' to replace every non web safe font with some fallback. Default is false.</param>
        /// <returns>selfContainedHtml</returns>
        Task<Stream> ConvertToHtmlAsync(Stream sourceOpenXml, string fallbackPageTitle, bool useWebSafeFonts);
    }
}