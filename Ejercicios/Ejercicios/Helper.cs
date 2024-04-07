using System.Xml;
using CsvHelper;
using Model;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Utils
{
    public static class Helper
    {
        private static bool IsValidXmlName(string name)
        {
            return Regex.IsMatch(name, @"^[\p{L}\p{Nl}_][\p{L}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*$");
        }

        public static void ConvertCsvToXml(string csvPath, string xmlPath)
        {
            const string ConvertSuccess = "Csv convertido a Xml correctamente";

            var records = new List<Dictionary<string, string>>();

            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var record = new Dictionary<string, string>();

                    foreach (var header in csv.HeaderRecord)
                    {
                        var cleanedHeader = header.Replace(" ", "_");
                        record[cleanedHeader] = csv.GetField(header);
                    }

                    records.Add(record);
                }
            }

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
        }

        public static Dictionary<string, float> SelectAverageConsumPerComarca(string path)
        {
            var totalConsumptionByComarca = new Dictionary<string, float>();
            var countByComarca = new Dictionary<string, int>();

            using (var reader = XmlReader.Create(path))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();

                    var properties = new Dictionary<string, Action<string>>
            {
                { "Any", value => waterConsume.Any = value },
                { "Codi_comarca", value => waterConsume.CodiComarca = value },
                { "Comarca", value => waterConsume.Comarca = value },
                { "Població", value => waterConsume.Poblacio = value },
                { "Domèstic_xarxa", value => waterConsume.DomesticXarxa = value },
                { "Activitats_econòmiques_i_fonts_pròpies", value => waterConsume.ActivitatsEconomiquesIFontsPropies = value },
                { "Total", value => waterConsume.Total = value },
                { "Consum_domèstic_per_càpita", value =>
                    {
                        if (!totalConsumptionByComarca.ContainsKey(waterConsume.Comarca))
                        {
                            totalConsumptionByComarca[waterConsume.Comarca] = float.Parse(value);
                            countByComarca[waterConsume.Comarca] = 1;
                        }
                        else
                        {
                            totalConsumptionByComarca[waterConsume.Comarca] += float.Parse(value);
                            countByComarca[waterConsume.Comarca]++;
                        }
                    }
                }
            };

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element && properties.TryGetValue(reader.Name, out var action))
                        {
                            action(reader.ReadElementContentAsString());
                        }
                    }
                }
            }
            var averageConsumptionByComarca = new Dictionary<string, float>();
            foreach (var comarca in totalConsumptionByComarca.Keys)
            {
                averageConsumptionByComarca[comarca] = totalConsumptionByComarca[comarca] / countByComarca[comarca];
            }

            return averageConsumptionByComarca;
        }


        public static List<WaterConsume> SelectComarcaByBiggerThanPoblation(int poblation, string xmlPath)
        {
            var waterConsumes = new List<WaterConsume>();

            using (var reader = XmlReader.Create(xmlPath))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();

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

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element && properties.TryGetValue(reader.Name, out var action))
                        {
                            action(reader.ReadElementContentAsString());
                        }
                    }
                    if (waterConsume.Poblacio != null && int.TryParse(waterConsume.Poblacio, out int poblacion) && poblacion > poblation)
                    {
                        waterConsumes.Add(waterConsume);
                    }
                }
            }

            return waterConsumes;
        }


        public static Dictionary<string, WaterConsume> SelectBiggestConsumDomesticPerCapita(string path)
        {
            var comarcaByYear = new Dictionary<string, WaterConsume>();

            using (var reader = XmlReader.Create(path))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();

                    var properties = new Dictionary<string, Action<string>>
            {
                { "Any", value => waterConsume.Any = value },
                { "Codi_comarca", value => waterConsume.CodiComarca = value },
                { "Comarca", value => waterConsume.Comarca = value },
                { "Població", value => waterConsume.Poblacio = value },
                { "Domèstic_xarxa", value => waterConsume.DomesticXarxa = value },
                { "Activitats_econòmiques_i_fonts_pròpies", value => waterConsume.ActivitatsEconomiquesIFontsPropies = value },
                { "Total", value => waterConsume.Total = value },
                { "Consum_domèstic_per_càpita", value =>
                    {
                        waterConsume.ConsumDomesticPerCapita = value;
                        if (!comarcaByYear.ContainsKey(waterConsume.Any) || float.Parse(value) > float.Parse(comarcaByYear[waterConsume.Any].ConsumDomesticPerCapita))
                        {
                            comarcaByYear[waterConsume.Any] = waterConsume;
                        }
                    }
                }
            };

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element && properties.TryGetValue(reader.Name, out var action))
                        {
                            action(reader.ReadElementContentAsString());
                        }
                    }
                }
            }

            return comarcaByYear;
        }


        public static Dictionary<string, WaterConsume> SelectLowestConsumDomesticPerCapita(string path)
        {
            var comarcaByYear = new Dictionary<string, WaterConsume>();

            using (var reader = XmlReader.Create(path))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();

                    var properties = new Dictionary<string, Action<string>>
                    {
                { "Any", value => waterConsume.Any = value },
                { "Codi_comarca", value => waterConsume.CodiComarca = value },
                { "Comarca", value => waterConsume.Comarca = value },
                { "Població", value => waterConsume.Poblacio = value },
                { "Domèstic_xarxa", value => waterConsume.DomesticXarxa = value },
                { "Activitats_econòmiques_i_fonts_pròpies", value => waterConsume.ActivitatsEconomiquesIFontsPropies = value },
                { "Total", value => waterConsume.Total = value },
                { "Consum_domèstic_per_càpita", value =>
                {
                        waterConsume.ConsumDomesticPerCapita = value;
                        if (!comarcaByYear.ContainsKey(waterConsume.Any) || float.Parse(value) < float.Parse(comarcaByYear[waterConsume.Any].ConsumDomesticPerCapita))
                    {
                            comarcaByYear[waterConsume.Any] = waterConsume;
                        }
                    }
                }
            };

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element && properties.TryGetValue(reader.Name, out var action))
                        {
                            action(reader.ReadElementContentAsString());
                        }
                    }
                }
            }

            return comarcaByYear;
        }

        public static List<WaterConsume> SelectByName(string name, string xmlPath)
        {
            var waterConsumes = new List<WaterConsume>();

            using (var reader = XmlReader.Create(xmlPath))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();

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

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element && properties.TryGetValue(reader.Name, out var action))
                        {
                            action(reader.ReadElementContentAsString());
                        }
                    }
                    if (waterConsume.Comarca != null && waterConsume.Comarca.ToUpper().Contains(name))
                    {
                        waterConsumes.Add(waterConsume);
                    }
                }
            }

            return waterConsumes;
        }

        public static List<WaterConsume> SelectByCodiComarca(string codiComarca, string xmlPath)
        {
            var waterConsumes = new List<WaterConsume>();

            using (var reader = XmlReader.Create(xmlPath))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();

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

                    while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element && properties.TryGetValue(reader.Name, out var action))
                        {
                            action(reader.ReadElementContentAsString());
                        }
                    }
                    if (waterConsume.CodiComarca != null && waterConsume.CodiComarca.Contains(codiComarca))
                    {
                        waterConsumes.Add(waterConsume);
                    }
                }
            }

            return waterConsumes;
        }
    }
}