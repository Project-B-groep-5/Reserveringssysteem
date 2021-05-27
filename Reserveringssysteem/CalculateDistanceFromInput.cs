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
                Console.WriteLine("\nVul uw postcode in: \n");
                string postcode = Console.ReadLine();
                while (true)
                {
                    isIntString = postcode.Any(letter => char.IsDigit(letter));
                    if (!isIntString)
                    {
                        CallTitle();
                        Console.WriteLine("\nGraag de postcode.. Inclusief de letters op het eind.");
                        Console.WriteLine("\nVul uw postcode in: \n");
                        postcode = Console.ReadLine();
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
                    Console.WriteLine($"\nAfstand tot restaurant {Json.Restaurant.Name} berekenen vanaf {postcode}.....\n");
                    Console.WriteLine($"\nDe Afstand vanaf {postcode} en restaurant {Json.Restaurant.Name} is: " + CalculateDistance(location, Json.Restaurant.Address) + " kilometer");
                    Utils.Enter();
                }
                else
                {
                    Console.WriteLine("\nPostcode wordt niet gevonden of afstand is groter dan 300 kilometer. \nProbeer het opnieuw door het invullen van de straatnaam + woonplaats\n");
                    Utils.Enter();
                }

                }
                if (keuzeCheckArr2[keuzeCheck2] == "Terug")
                    return;
                if (keuzeCheckArr2[keuzeCheck2] == "Afstand berekenen met straatnaam, huisnummer en woonplaats \n")
                {
                    CallTitle();
                    Console.WriteLine("\nVul uw straatnaam en huisnummer in: \n");
                    string adres = Console.ReadLine();
                    while (true)
                    {
                        isIntString = adres.Any(letter => char.IsDigit(letter));
                        if (!isIntString)
                        {
                            CallTitle();
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
                        CallTitle();
                        Console.WriteLine("\nGraag enkel de plaatsnaam, zonder integers");
                        Console.WriteLine("\nVul uw plaatsnaam in: \n");
                        plaatsnaam = Console.ReadLine();
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
                    Console.WriteLine($"\nAfstand tot restaurant {Json.Restaurant.Name} berekenen vanaf {adres}, {plaatsnaam}.....\n");
                    Console.WriteLine($"\nDe Afstand vanaf {adres}, {plaatsnaam} en restaurant {Json.Restaurant.Name} is: " + CalculateDistance(location, Json.Restaurant.Address) + " kilometer");
                    Utils.Enter();
                }
                else
                {
                    Console.WriteLine("\nAdres wordt niet gevonden of afstand is groter dan 300 kilometer. \nVerander het adres of probeer het met een postcode");
                    Utils.Enter();
                }
                }
            }

        }
    }
