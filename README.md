https://www.csvreader.com/code/cs/csv_to_xml.php
https://joshclose.github.io/CsvHelper/examples/
https://learn.microsoft.com/es-es/troubleshoot/developer/visualstudio/csharp/language-compilers/read-xml-data-from-url





- ChatGPT
- - Resultado 1
```
var root = new XElement("data");

foreach (var record in records)
{
    var recordElement = new XElement("record");

    foreach (var kvp in record)
    {
        var elementName = kvp.Key;
        if (!IsValidXmlName(elementName))
        {
            elementName = Regex.Replace(elementName, @"[^\w\.-]", "_");
        }

        recordElement.Add(new XElement(elementName, kvp.Value));
    }

    root.Add(recordElement);
}

var xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
xmlDocument.Save(xmlPath);
Console.WriteLine(ConvertSuccess);
```
