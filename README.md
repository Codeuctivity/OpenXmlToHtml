# OpenXmlToHtml

Converts docx to html

[![Nuget](https://img.shields.io/nuget/v/Codeuctivity.OpenXmlToHtml.svg)](https://www.nuget.org/packages/Codeuctivity.OpenXmlToHtml/) [![Build](https://github.com/Codeuctivity/OpenXmlToHtml/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/OpenXmlToHtml/actions/workflows/dotnet.yml)

- .net implementation
- No external infrastructure needed (No Microsoft Office or Libre Office needed)
- Focused on Windows and Linux support
- Demo CLI Api [OpenXmlToHtmlCli.zip](https://github.com/Codeuctivity/OpenXmlToHtml/releases)

```c#
await OpenXmlToHtml.ConvertToHtmlAsync(inputPathDocx, outputPathHtml);
```
