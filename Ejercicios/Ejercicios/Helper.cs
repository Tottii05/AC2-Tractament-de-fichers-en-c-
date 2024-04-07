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

        public static List<WaterConsume> SelectAllByYearAndComarca(string path)
        {
            var waterConsumes = new List<WaterConsume>();

            using (var reader = XmlReader.Create(path))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();
                    while (reader.Read())
                    {
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
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "record")
                        {
                            break;
                        }
                    }

                    waterConsumes.Add(waterConsume);
                }
            }
            return waterConsumes;
        }

        public static List<WaterConsume> SelectComarcaByBiggerThanPoblation(int poblation, string xmlPath)
        {
            var waterConsumes = new List<WaterConsume>();

            using (var reader = XmlReader.Create(xmlPath))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();
                    while (reader.Read())
                    {
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
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "record")
                        {
                            break;
                        }
                    }

                    if (waterConsume.Poblacio != null && int.Parse(waterConsume.Poblacio) > poblation)
                    {
                        waterConsumes.Add(waterConsume);
                    }
                }
            }
            return waterConsumes;
        }

        public static Dictionary<string, WaterConsume> SelectBiggestConsumDomesticPerCapita(string path)
        {
            var waterConsumesByYear = new Dictionary<string, WaterConsume>();

            using (var reader = XmlReader.Create(path))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();
                    while (reader.Read())
                    {
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
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "record")
                        {
                            break;
                        }
                    }

                    if (!waterConsumesByYear.ContainsKey(waterConsume.Any))
                    {
                        waterConsumesByYear[waterConsume.Any] = waterConsume;
                    }
                    else
                    {
                        if (waterConsume.ConsumDomesticPerCapita != null &&
                            waterConsumesByYear[waterConsume.Any].ConsumDomesticPerCapita != null &&
                            float.Parse(waterConsumesByYear[waterConsume.Any].ConsumDomesticPerCapita) <
                            float.Parse(waterConsume.ConsumDomesticPerCapita))
                        {
                            waterConsumesByYear[waterConsume.Any] = waterConsume;
                        }
                    }
                }
            }
            return waterConsumesByYear;
        }

        public static Dictionary<string, WaterConsume> SelectLowestConsumDomesticPerCapita(string path)
        {
            var waterConsumesByYear = new Dictionary<string, WaterConsume>();

            using (var reader = XmlReader.Create(path))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();
                    while (reader.Read())
                    {
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
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "record")
                        {
                            break;
                        }
                    }

                    if (!waterConsumesByYear.ContainsKey(waterConsume.Any))
                    {
                        waterConsumesByYear[waterConsume.Any] = waterConsume;
                    }
                    else
                    {
                        if (waterConsume.ConsumDomesticPerCapita != null &&
                            waterConsumesByYear[waterConsume.Any].ConsumDomesticPerCapita != null &&
                            float.Parse(waterConsumesByYear[waterConsume.Any].ConsumDomesticPerCapita) >
                            float.Parse(waterConsume.ConsumDomesticPerCapita))
                        {
                            waterConsumesByYear[waterConsume.Any] = waterConsume;
                        }
                    }
                }
            }
            return waterConsumesByYear;
        }

        public static List<WaterConsume> SelectByName(string name, string xmlPath)
        {
            var waterConsumes = new List<WaterConsume>();

            using (var reader = XmlReader.Create(xmlPath))
            {
                while (reader.ReadToFollowing("record"))
                {
                    var waterConsume = new WaterConsume();

                    while (reader.Read())
                    {
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
                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "record")
                        {
                            break;
                        }
                    }

                    if (waterConsume.Comarca == name)
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

            try
            {
                using (var reader = XmlReader.Create(xmlPath))
                {
                    while (reader.ReadToFollowing("record"))
                    {
                        var waterConsume = new WaterConsume();

                        while (reader.Read())
                        {
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
                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "record")
                            {
                                break;
                            }
                        }

                        if (waterConsume.CodiComarca == codiComarca)
                        {
                            waterConsumes.Add(waterConsume);
                        }
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return waterConsumes;
        }
    }
}
