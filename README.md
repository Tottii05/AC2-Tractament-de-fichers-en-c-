# AC2 Tractament de fichers en c#
## Páginas visitas que han sido de ayuda
- https://www.csvreader.com/code/cs/csv_to_xml.php
- https://joshclose.github.io/CsvHelper/examples/
- https://learn.microsoft.com/es-es/troubleshoot/developer/visualstudio/csharp/language-compilers/read-xml-data-from-url

## Usos de IA y resultados útiles
- ChatGPT
- - - Resultado 1
```
@"^[\p{L}\p{Nl}_][\p{L}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*$"
```
- - Resultado 2
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
- - Resultado 3
```
if (reader.NodeType == XmlNodeType.Element)
{
    switch (reader.Name)
    {
        case "Any":
            waterConsume.Any = reader.ReadElementContentAsString();
            break;
        case "Codi_comarca":
            waterConsume.CodiComarca = reader.ReadElementContentAsString();
            break;
        case "Comarca":
            waterConsume.Comarca = reader.ReadElementContentAsString();
            break;
        case "Població":
            waterConsume.Poblacio = reader.ReadElementContentAsString();
            break;
        case "Domèstic_xarxa":
            waterConsume.DomesticXarxa = reader.ReadElementContentAsString();
            break;
        case "Activitats_econòmiques_i_fonts_pròpies":
            waterConsume.ActivitatsEconomiquesIFontsPropies = reader.ReadElementContentAsString();
            break;
        case "Total":
            waterConsume.Total = reader.ReadElementContentAsString();
            break;
        case "Consum_domèstic_per_càpita":
            waterConsume.ConsumDomesticPerCapita = reader.ReadElementContentAsString();
            break;
    }
}
```
