using System;

namespace Reserveringssysteem
{
    public static class InfoScherm
    {
        public static void ShowInfo()
        {
            Logo.PrintLogo(Logo.OverOns);
            Restaurant restaurant = Json.Restaurant;
            Location address = restaurant.Address;
            string[] openingHours = restaurant.OpeningHours;
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
            Console.WriteLine(restaurant.ContactInformation[0]);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nOpeningstijden:");
            var dagen = new[] { "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag" };
            for (int i = 0; i < dagen.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(dagen[i] + ": ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(openingHours[i]);
            }            
            Utils.Enter(Program.Main);
        }
    }
}