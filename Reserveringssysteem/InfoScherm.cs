using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    public class InfoScherm
    {
        public double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public static void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(Logo.OverOns);
            Console.ResetColor();
            Console.WriteLine("\nRestaurant informatie:\n" +
                "Welkom bij de Houten Vork. Wij bezorgen u een lach met de lekkerste gerechten.\n" +
                "Wij bereiden de lekkerste vlees en vegetarische gerechten die u terug kunt vinden " +
                "in het menukaart.\n\n" +
                "Restaurant gegevens:\n"+
                "Adress: ........\n" +
                "Telefoonnummer: ........\n" +
                "E-mail: ........ ");
            Console.WriteLine("\n\nWilt u de afstand berekenen vanaf uw postcode? Typ 'ja': \n");
            string antwoord = Console.ReadLine();

            if (antwoord == "ja")
            {
                CalculateDistance(Location point1, Location point2);
                //public GeoCoordinate (double latitude, double longitude);
                //var sCoord = new GeoCoordinate(-62.0, 22.0);
                //var eCoord = new GeoCoordinate(-62.0, 21.0);

                //return sCoord.GetDistanceTo(eCoord);


            }
            Utils.EnterTerug();
        }
    }
}