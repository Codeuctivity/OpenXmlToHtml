# OpenXmlToHtml

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/6026a6e87a234d1193639fd77285a0b3)](https://app.codacy.com/gh/Codeuctivity/OpenXmlToHtml?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/OpenXmlToHtml&utm_campaign=Badge_Grade_Settings)

Converts docx to html

- pure .net implementation
- No external infrastructure needed 
- Focused on Windows and Linux support

``` c#
await OpenXmlToHtml.ConvertToHtmlAsync(inputPathDocx, outputPathHtml);
```