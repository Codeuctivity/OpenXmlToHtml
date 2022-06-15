# OpenXmlToHtml

Converts docx to html

[![Nuget](https://img.shields.io/nuget/v/OpenXmlToHtml.svg)](https://www.nuget.org/packages/OpenXmlToHtml/) [![Build](https://github.com/Codeuctivity/OpenXmlToHtml/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/OpenXmlToHtml/actions/workflows/dotnet.yml)

- .net implementation
- No external infrastructure needed (No Microsoft Office or Libre Office needed)
- Focused on Windows and Linux support
- Demo CLI Api [OpenXmlToHtmlCli.zip](https://github.com/Codeuctivity/OpenXmlToHtml/releases)
- Azure [Demo Open Api](http://openxmlconverter.azurewebsites.net/index.html) (running on limited budget... may be offline after usage quota exceeds)

```c#
await OpenXmlToHtml.ConvertToHtmlAsync(inputPathDocx, outputPathHtml);
```

## Pdf Output on azure linux plan

For the pdf conversion there are some dependencies missing on azure.

```bash
export DEBIAN_FRONTEND="noninteractive"
apt-get install -y libnss3 libnspr4 libatk1.0-0 libatk-bridge2.0-0 libcups2 libdrm2 libxkbcommon0 libxcomposite1 libxdamage1 libxfixes3 libxrandr2 libgbm1 libasound2 libpangox-1.0-0
```

## Fonts and Linux - WIP Section

Some windows specific fonts used in many docx can be installed on linux using

```bash
sudo apt install ttf-mscorefonts-installer -y && sudo fc-cache -vr
```
