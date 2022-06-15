# OpenXmlToHtml

Converts docx to html

[![Nuget](https://img.shields.io/nuget/v/OpenXmlToHtml.svg)](https://www.nuget.org/packages/OpenXmlToHtml/) [![Build](https://github.com/Codeuctivity/OpenXmlToHtml/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/OpenXmlToHtml/actions/workflows/dotnet.yml)

- 100% .net implementation
- No external infrastructure needed
- Focused on Windows and Linux support
- Demo CLI Api [OpenXmlToHtmlCli.zip](https://github.com/Codeuctivity/OpenXmlToHtml/releases)
- [Demo Open Api](http://openxmlconverter.azurewebsites.net/index.html)

```c#
await OpenXmlToHtml.ConvertToHtmlAsync(inputPathDocx, outputPathHtml);
```

### Fonts and Linux - WIP Section

Some windows specific fonts used in many docx can be installed on linux using

```bash
sudo apt install ttf-mscorefonts-installer -y && sudo fc-cache -vr
```
