using System;
using System.Linq;

namespace Reserveringssysteem
{
    public class CalculateDistanceFromInput
    {
        public static string CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            var distanceToRest = (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)))) / 1000;
            return String.Format("{0:0.00}", distanceToRest);
        }
        public static void Calculate()
        {
            bool isIntString;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.Afstand);
            Console.ResetColor();

            string[] keuzeCheckArr2 = new string[3] { "Afstand berekenen met postcode", "Afstand berekenen met straatnaam, huisnummer en woonplaats", "\n  Terug" };
            var Check2 = new SelectionMenu(keuzeCheckArr2, Logo.Afstand, "\nWelke optie kiest u?\n");
            var keuzeCheck2 = Check2.Show();
            if (keuzeCheckArr2[keuzeCheck2] == "Afstand berekenen met postcode")
            {
                Console.WriteLine("Postcode");
            }
            if (keuzeCheckArr2[keuzeCheck2] == "\n  Terug")
                return; 
            if (keuzeCheckArr2[keuzeCheck2] == "Afstand berekenen met straatnaam, huisnummer en woonplaats")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(Logo.Afstand);
                Console.ResetColor();
                Console.WriteLine("\nVul uw straatnaam en huisnummer in: \n");
                string adres = Console.ReadLine();
                while (true)
                {
                    isIntString = adres.Any(letter => char.IsDigit(letter));
                    if (!isIntString)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(Logo.Afstand);
                        Console.ResetColor();
                        Console.WriteLine("\nGraag de straatnaam met huisnummer");
                        Console.WriteLine("\nVul uw straatnaam en huisnummer in: \n");
                        adres = Console.ReadLine();
                    }
                    else
                        break;
                }
                string[] temp = adres.Split(' ');
                string huisnummer = temp[^1];
                string straatnaam = string.Join(' ', temp).Replace($" {huisnummer}", "");
                Console.WriteLine("\nVul uw plaatsnaam in: \n");
                isIntString = true;
                string plaatsnaam = Console.ReadLine();
                while (isIntString == true)
                {
                    isIntString = plaatsnaam.Any(letter => char.IsDigit(letter));
                    if (isIntString == true)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(Logo.Afstand);
                        Console.ResetColor();
                        Console.WriteLine("\nGraag enkel de plaatsnaam, zonder integers");
                        Console.WriteLine("\nVul uw plaatsnaam in: \n");
                        plaatsnaam = Console.ReadLine();
                    }
                    else
                        break;
                }
                Location location = new Location(straatnaam, huisnummer, null, plaatsnaam);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(Logo.Afstand);
                Console.ResetColor();
                var restaurant = Json.Restaurant;
                Console.WriteLine($"\nAfstand tot restaurant {Json.Restaurant.Name} berekenen vanaf {adres}, {plaatsnaam}.....\n");
                Console.WriteLine($"\nDe Afstand vanaf {adres}, {plaatsnaam} en restaurant {Json.Restaurant.Name} is: " + CalculateDistance(location, Json.Restaurant.Address) + " kilometer");
                Utils.Enter();
            }
        }

    }
}