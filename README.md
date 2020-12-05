# OpenXmlToHtml

Converts docx to html

[![Nuget](https://img.shields.io/nuget/v/OpenXmlToHtml.svg)](https://www.nuget.org/packages/OpenXmlToHtml/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/7ba69957e12f4348a25e14e7db124cd6)](https://www.codacy.com/gh/Codeuctivity/OpenXmlToHtml/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Codeuctivity/OpenXmlToHtml&amp;utm_campaign=Badge_Grade)
[![Build status](https://ci.appveyor.com/api/projects/status/q9ybcnv886vcrdf8/branch/main?svg=true)](https://ci.appveyor.com/project/stesee/openxmltohtml/branch/main)

- pure .net implementation
- No external infrastructure needed 
- Focused on Windows and Linux support

``` c#
await OpenXmlToHtml.ConvertToHtmlAsync(inputPathDocx, outputPathHtml);
```