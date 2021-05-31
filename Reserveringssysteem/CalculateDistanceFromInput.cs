using System;
using System.Linq;

namespace Reserveringssysteem
{

    public class CalculateDistanceFromInput
    {
        public static void CallTitle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.Afstand);
            Console.ResetColor();
        }
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
            CallTitle();

            string[] keuzeCheckArr2 = new string[3] { "Afstand berekenen met postcode", "Afstand berekenen met straatnaam, huisnummer en woonplaats \n", "Terug" };
            var Check2 = new SelectionMenu(keuzeCheckArr2, Logo.Afstand, "\nWelke optie kiest u?\n");
            var keuzeCheck2 = Check2.Show();
            if (keuzeCheckArr2[keuzeCheck2] == "Afstand berekenen met postcode")
            {
                CallTitle();
                string postcode = Utils.Input("\nVul uw postcode in: \n");
                while (true)
                {
                    isIntString = postcode.Any(letter => char.IsDigit(letter));
                    if (!isIntString)
                    {
                        CallTitle();
                        postcode = Utils.Input("\nGraag de postcode.. Inclusief de letters op het eind.\n\nVul uw postcode in:\n");
                    }
                    else
                        break;
                }
                string[] postcodeZonderLetters = postcode.Split(' ');
                string postcodeLetters = postcodeZonderLetters[^1];
                Location location = new Location(postcode, postcodeLetters, null, "Nederland");
                CallTitle();
                var restaurant = Json.Restaurant;
                string afstand = CalculateDistance(location, Json.Restaurant.Address);
                double afstandDouble = Double.Parse(afstand); 
                if (afstandDouble < 300)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"\nAfstand tot restaurant ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Json.Restaurant.Name);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" berekenen vanaf ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(postcode);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("...");
                    Console.Write($"\nDe Afstand tussen ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(postcode);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" en restaurant ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Json.Restaurant.Name);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" is: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(CalculateDistance(location, Json.Restaurant.Address));
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" kilometer");
                    Utils.Enter();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\nPostcode wordt niet gevonden of afstand is groter dan");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("300");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" kilometer. \nProbeer het opnieuw door het invullen van de straatnaam + woonplaats\n");
                    Utils.Enter();
                }

                }
                if (keuzeCheckArr2[keuzeCheck2] == "Terug")
                    return;
                if (keuzeCheckArr2[keuzeCheck2] == "Afstand berekenen met straatnaam, huisnummer en woonplaats \n")
                {
                    CallTitle();
                    string adres = Utils.Input("\nVul uw straatnaam en huisnummer in: \n");
                    while (true)
                    {
                        isIntString = adres.Any(letter => char.IsDigit(letter));
                        if (!isIntString)
                        {
                            CallTitle();
                            adres = Utils.Input("\nGraag de straatnaam met huisnummer\n\nVul uw straatnaam en huisnummer in:\n");
                        }
                        else
                            break;
                    }
                string[] temp = adres.Split(' ');
                string huisnummer = temp[^1];
                string straatnaam = string.Join(' ', temp).Replace($" {huisnummer}", "");
                isIntString = true;
                string plaatsnaam = Utils.Input("\nVul uw plaatsnaam in: \n");
                while (isIntString == true)
                {
                    isIntString = plaatsnaam.Any(letter => char.IsDigit(letter));
                    if (isIntString == true)
                    {
                        CallTitle();
                        plaatsnaam = Utils.Input("\nGraag enkel de plaatsnaam, zonder integers\n\nVul uw plaatsnaam in: \n");
                    }
                    else
                        break;
                }
                Location location = new Location(straatnaam, huisnummer, null, plaatsnaam);
                CallTitle();
                var restaurant = Json.Restaurant;
                string afstand = CalculateDistance(location, Json.Restaurant.Address);
                double afstandDouble = Double.Parse(afstand);
                if (afstandDouble < 300)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"\nAfstand tot restaurant ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Json.Restaurant.Name);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" berekenen vanaf ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{adres}, {plaatsnaam}");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("...");
                    Console.Write($"\nDe Afstand tussen ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{adres}, {plaatsnaam}");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" en restaurant ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Json.Restaurant.Name);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" is: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(CalculateDistance(location, Json.Restaurant.Address));
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" kilometer");
                    Utils.Enter();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\nPostcode wordt niet gevonden of afstand is groter dan ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("300");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" kilometer. \nProbeer het opnieuw door het invullen van de straatnaam + woonplaats\n");
                    Utils.Enter();
                }
                }
            }

        }
    }
