# OpenXmlToHtml

Converts docx to html

[![Nuget](https://img.shields.io/nuget/v/OpenXmlToHtml.svg)](https://www.nuget.org/packages/OpenXmlToHtml/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/7ba69957e12f4348a25e14e7db124cd6)](https://www.codacy.com/gh/Codeuctivity/OpenXmlToHtml/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/OpenXmlToHtml&utm_campaign=Badge_Grade)
[![Build status](https://ci.appveyor.com/api/projects/status/q9ybcnv886vcrdf8/branch/main?svg=true)](https://ci.appveyor.com/project/stesee/openxmltohtml/branch/main) [![Donate](https://img.shields.io/static/v1?label=Paypal&message=Donate&color=informational)](https://www.paypal.com/donate?hosted_button_id=7M7UFMMRTS7UE) [![Build Status](https://codeuctivity.visualstudio.com/PdfAValidatorApi/_apis/build/status/Codeuctivity.OpenXmlToHtml?branchName=main)](https://codeuctivity.visualstudio.com/PdfAValidatorApi/_build/latest?definitionId=2&branchName=main)

- 100% .net implementation
- No external infrastructure needed
- Focused on Windows and Linux support
- Demo CLI Api [OpenXmlToHtmlCli.zip](https://github.com/Codeuctivity/OpenXmlToHtml/releases)
- [Demo Open Api](http://openxmlconverter.azurewebsites.net/index.html)


```c#
await OpenXmlToHtml.ConvertToHtmlAsync(inputPathDocx, outputPathHtml);
```

## Linux

```bash
sudo apt install libgdiplus -y 
```

### Fonts and Linux - WIP Section

Some default fonts used in many docx can be installed on linux using

```bash
sudo apt install ttf-mscorefonts-installer -y && sudo fc-cache -vr
```
