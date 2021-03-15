using Codeuctivity.OpenXmlToHtml;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenXmlToHtmlCli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: OpenXmlToHtmlCli <sourceOpenXmlFilePath> <destinationHtmlFilePath>");
                return 1;
            }

            var inputPathDocx = args[0];
            var outputPathHtml = args[1];

            if (!File.Exists(inputPathDocx))
            {
                Console.WriteLine($"Could not find source {inputPathDocx}.");
                return 1;
            }

            if (File.Exists(outputPathHtml))
            {
                Console.WriteLine($"Destination {outputPathHtml} already exists.");
                return 1;
            }

            Console.WriteLine($"Converting {inputPathDocx} to {outputPathHtml}");
            await new OpenXmlToHtml().ConvertToHtmlAsync(inputPathDocx, outputPathHtml);
            return 0;
        }
    }
}