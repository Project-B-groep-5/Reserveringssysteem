using System;

namespace Reserveringssysteem
{
    public static class InfoScherm
    {
        public static void ShowInfo()
        {
            var menu = new SelectionMenu(new[] { "Afstand berekenen tot restaurant \n", "Terug" }, action: PrintInfo, title: "\nKies een optie:\n");
            switch(menu.Show())
            {
                case 0:
                    CalculateDistanceFromInput.Calculate();
                    break;
                case 1:
                    Program.Main();
                    break;
            }
        }
        private static void PrintInfo()
        {
            Logo.PrintLogo(Logo.OverOns);
            Restaurant restaurant = Json.Restaurant;
            Location address = restaurant.Address;
            Console.WriteLine($"{restaurant.Name}\n\n{restaurant.Description}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nAdress: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{address.StreetName} {address.HouseNumber}, {address.PostalCode} {address.City}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Telefoonnummer: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(restaurant.ContactInformation[1]);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("E-mail: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(restaurant.ContactInformation[0] + '\n');
            PrintDates();
        }

        public static void PrintDates()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Openingstijden:");
            var dagen = new[] { "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag" };
            for (int i = 0; i < dagen.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(dagen[i] + ": ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Json.Restaurant.OpeningHours[i]);
            }
        }
    }
}