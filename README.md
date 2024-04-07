# AC2 Tractament de fichers en c#
## Páginas visitas que han sido de ayuda
- https://www.csvreader.com/code/cs/csv_to_xml.php
- https://joshclose.github.io/CsvHelper/examples/
- https://learn.microsoft.com/es-es/troubleshoot/developer/visualstudio/csharp/language-compilers/read-xml-data-from-url
- https://stackoverflow.com/questions/46808153/c-sharp-dictionary-store-action-method
- 

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
                    var properties = new Dictionary<string, Action<string>>
            {
                { "Any", value => waterConsume.Any = value },
                { "Codi_comarca", value => waterConsume.CodiComarca = value },
                { "Comarca", value => waterConsume.Comarca = value },
                { "Població", value => waterConsume.Poblacio = value },
                { "Domèstic_xarxa", value => waterConsume.DomesticXarxa = value },
                { "Activitats_econòmiques_i_fonts_pròpies", value => waterConsume.ActivitatsEconomiquesIFontsPropies = value },
                { "Total", value => waterConsume.Total = value },
                { "Consum_domèstic_per_càpita", value => waterConsume.ConsumDomesticPerCapita = value }
            };
```
- - Resultado 4
La razón para utilizar Action<string> en este caso es para definir dinámicamente qué hacer con el valor de cada elemento del XML. En lugar de tener un código largo y repetitivo para cada propiedad de WaterConsume, utilizas un diccionario que mapea el nombre de la propiedad con una acción que actualiza esa propiedad cuando se lee del XML.

Por ejemplo, cuando encuentras el elemento XML "Consum_domèstic_per_càpita", la acción asociada calcula la suma total y el recuento de los valores de consumo doméstico per cápita por comarca. Esto permite que el código sea más modular, flexible y fácilmente mantenible. Además, te evita repetir el mismo código para cada propiedad de WaterConsume.
