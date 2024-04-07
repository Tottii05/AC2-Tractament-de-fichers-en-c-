using Utils;
using Model;

namespace FileHandling
{
    public class Program
    {
        public static void Main()
        {
            const string csvPath = "../../../files/Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv"; 
            const string xmlPath = "../../../files/Consum_d_aigua_a_Catalunya_per_comarques_20240402.xml";
            const string Spacing = "----------------------------------------";
            const string FirstMenuText = "Has convertido el Csv a Xml?\ns/n";
            const string SecondMenuText = "Que quieres hacer?\n1.Filtrar por población mayor a 200000\n2.Mostrar el consumo promedio por comarca\n3.Mostrar el mayor consumo domestico per capita\n4.Mostrar el menor consumo domestico per capita\n5.Filtrar por nombre o codigo de comarca";
            const string PoblationFilteredComarca = "Any: {0}\nComarca: {1}\nPoblacio: {2}";
            const string AverageConsumComarca = "Comarca: {0}\nConsum domestic per comarca: {1}";
            const string SubMenuText = "Como quieres filtrar\n1.Nombre\n2.Codigo de comarca";
            const string NameFilteredText = "Introduce el nombre de la comarca";
            const string CodeFilteredText = "Introduce el código de la comarca";
            const string NoResultsText = "No se han encontrado resultados";
            const string FilteredComarcaText = "Any: {0}\nCodiComarca: {1}\nComarca: {2}\nPoblacio: {3}\nDomesticXarxa: {4}\nActivitatsEconomiquesIFontsPropies: {5}\nTotal: {6}\nConsumDomesticPerCapita: {7}";
            const string InvalidOptionText = "Opción no válida";

            const int MaxPoblation = 200000;
            string firstMenuChoice, comarcaName, comarcaCode;
            int secondMenuChoice, subMenuChoice;
            bool leaveMenu = false, leaveSubMenu = false;

            Console.WriteLine(FirstMenuText);
            firstMenuChoice = Console.ReadLine().ToLower();

            while (!leaveMenu)
            {
                switch (firstMenuChoice)
                {
                    case "s":
                        leaveMenu = true;
                        break;
                    case "n":
                        Helper.ConvertCsvToXml(csvPath, xmlPath);
                        leaveMenu = true;
                        break;
                    default:
                        Console.WriteLine(InvalidOptionText);
                        firstMenuChoice = Console.ReadLine().ToLower();
                        break;
                }
            }
            leaveMenu = false;
            Console.WriteLine(SecondMenuText);
            secondMenuChoice = Convert.ToInt32(Console.ReadLine());
            while (!leaveMenu)
            {
                switch (secondMenuChoice)
                {
                    case 1:
                        List<WaterConsume> waterConsumes = Helper.SelectComarcaByBiggerThanPoblation(MaxPoblation, xmlPath);
                        Console.WriteLine(Spacing);
                        foreach (var waterConsume in waterConsumes)
                        {
                            Console.WriteLine(PoblationFilteredComarca, waterConsume.Any, waterConsume.Comarca, waterConsume.Poblacio);
                            Console.WriteLine(Spacing);
                        }
                        leaveMenu = true;
                        break;
                    case 2:
                        var averageConsumPerComarca = Helper.SelectAverageConsumPerComarca(xmlPath);
                        Console.WriteLine(Spacing);
                        foreach (var kvp in averageConsumPerComarca)
                        {
                            var comarca = kvp.Key;
                            var consumoPromedio = kvp.Value;
                            Console.WriteLine(AverageConsumComarca, comarca, consumoPromedio);
                            Console.WriteLine(Spacing);
                        }
                        leaveMenu = true;
                        break;
                    case 3:
                        var BiggestwaterConsumeByConsumDomestic = Helper.SelectBiggestConsumDomesticPerCapita(xmlPath);
                        Console.WriteLine(Spacing);
                        foreach (var kvp in BiggestwaterConsumeByConsumDomestic)
                        {
                            var waterConsume = kvp.Value;
                            Console.WriteLine(FilteredComarcaText, waterConsume.Any, waterConsume.CodiComarca, waterConsume.Comarca, waterConsume.Poblacio, waterConsume.DomesticXarxa, waterConsume.ActivitatsEconomiquesIFontsPropies, waterConsume.Total, waterConsume.ConsumDomesticPerCapita);
                            Console.WriteLine(Spacing);
                        }
                        leaveMenu = true;
                        break;
                    case 4:
                        var LowestWaterConsumeByConsumeDomestic = Helper.SelectLowestConsumDomesticPerCapita(xmlPath);
                        Console.WriteLine(Spacing);
                        foreach (var kvp in LowestWaterConsumeByConsumeDomestic)
                        {
                            var waterConsume = kvp.Value;
                            Console.WriteLine(FilteredComarcaText, waterConsume.Any, waterConsume.CodiComarca, waterConsume.Comarca, waterConsume.Poblacio, waterConsume.DomesticXarxa, waterConsume.ActivitatsEconomiquesIFontsPropies, waterConsume.Total, waterConsume.ConsumDomesticPerCapita);
                            Console.WriteLine(Spacing);
                        }
                        leaveMenu = true;
                        break;
                    case 5:
                        while (!leaveSubMenu)
                        {
                            Console.WriteLine(SubMenuText);
                            subMenuChoice = Convert.ToInt32(Console.ReadLine());
                            switch (subMenuChoice)
                            {
                                case 1:
                                    Console.WriteLine(NameFilteredText);
                                    comarcaName = Console.ReadLine().ToUpper().Trim();
                                    List<WaterConsume> filteredWaterConsumeByName = Helper.SelectByName(comarcaName, xmlPath);
                                    if (filteredWaterConsumeByName.Count == 0)
                                    {
                                        Console.WriteLine(NoResultsText);
                                    }
                                    else
                                    {
                                        foreach (var waterConsume in filteredWaterConsumeByName)
                                        {
                                            Console.WriteLine(FilteredComarcaText, waterConsume.Any, waterConsume.CodiComarca, waterConsume.Comarca, waterConsume.Poblacio, waterConsume.DomesticXarxa, waterConsume.ActivitatsEconomiquesIFontsPropies, waterConsume.Total, waterConsume.ConsumDomesticPerCapita);
                                            Console.WriteLine(Spacing);
                                        }
                                    }
                                    leaveSubMenu = true;
                                    break;
                                case 2:
                                    Console.WriteLine(CodeFilteredText);
                                    comarcaCode = Console.ReadLine();
                                    List<WaterConsume> filteredWaterConsumeByCode = Helper.SelectByCodiComarca(comarcaCode, xmlPath);
                                    if (filteredWaterConsumeByCode.Count == 0)
                                    {
                                        Console.WriteLine(NoResultsText);
                                    }
                                    else
                                    {
                                        foreach (var waterConsume in filteredWaterConsumeByCode)
                                        {
                                            Console.WriteLine(FilteredComarcaText, waterConsume.Any, waterConsume.CodiComarca, waterConsume.Comarca, waterConsume.Poblacio, waterConsume.DomesticXarxa, waterConsume.ActivitatsEconomiquesIFontsPropies, waterConsume.Total, waterConsume.ConsumDomesticPerCapita);
                                            Console.WriteLine(Spacing);
                                        }
                                    }
                                    leaveSubMenu = true;
                                    break;
                                default:
                                    Console.WriteLine(InvalidOptionText);
                                    break;
                            }
                        }
                        leaveMenu = true;
                        break;
                }
            }
        }
    }
}
