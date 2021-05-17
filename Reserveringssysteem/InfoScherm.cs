using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using static Reserveringssysteem.Json;
using System.Globalization;
using System.Linq;

namespace Reserveringssysteem
{
    public class InfoScherm
    {
        public static Location point2 = new Location(51.9116, 4.4713); // Locatie voor restaurant, hardcoded op Dijkzigt voor testen.. 
        public static DataTable GetCoordinates(string address)
        {
            string url = "https://maps.google.com/maps/api/geocode/xml?address=" + address + "&key=AIzaSyByK-OKdQMmLpuBwQwwp3ABA4dNnQGbG9A";
            WebRequest request = WebRequest.Create(url);

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    DataTable dtCoordinates = new DataTable();
                    dtCoordinates.Columns.AddRange(new DataColumn[4] { 
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Address", typeof(string)),
                    new DataColumn("Latitude",typeof(string)),
                    new DataColumn("Longitude",typeof(string)) });
                    foreach (DataRow row in dsResult.Tables["result"].Rows)
                    {
                        string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                        DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                        dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                        //Console.WriteLine("\nLatitude = " + location[0]); // Print check om te kijken of Latitude klopt
                        //Console.WriteLine("\nLongitude = " + location[1]); // Print check om te kijken of Longitude klopt
                        var inputLatitude = location[0] ;
                        double convertedLatitude = Convert.ToDouble(inputLatitude, System.Globalization.CultureInfo.InvariantCulture);
                        var inputLongitude = location[1] ;
                        double convertedLongitude = Convert.ToDouble(inputLongitude, System.Globalization.CultureInfo.InvariantCulture);
                        Location point1 = new Location(convertedLatitude, convertedLongitude);
                        double DistanceToRest = CalculateDistance(point1, point2);
                        var roundedNumber = String.Format("{0:0.00}", DistanceToRest);  //Afronding van de double op twee decimalen
                        Console.WriteLine("\nDe Afstand vanaf het door u opgegeven adres en het restaurant is: " + roundedNumber + " kilometer");
                    }
                    return dtCoordinates;
                }
            }
        }
        public static double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.latitude * (Math.PI / 180.0);
            var num1 = point1.longitude * (Math.PI / 180.0);
            var d2 = point2.latitude * (Math.PI / 180.0);
            var num2 = point2.longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)))) / 1000;
        }

        public class Location
        {
            public double latitude { get; set; }
            public double longitude { get; set; }

            public Location(double latitude , double longitude)
            {
                this.latitude = latitude;
                this.longitude = longitude;
            }
        }
        public static void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.OverOns);
            Console.ResetColor();
            Restaurant restaurant = Deserialize<Restaurant>("restaurant.json");
            Location address = restaurant.Address;
            string[] openingHours = restaurant.OpeningHours;
            Console.WriteLine($@"{restaurant.Name}

{restaurant.Description}

Adress: {address.StreetName} {address.HouseNumber}, {address.PostalCode} {address.City}
Telefoonnummer: {restaurant.ContactInformation[1]}
E-mail: {restaurant.ContactInformation[0]}

Openingstijden:
Maandag: {openingHours[0]}
Dinsdag: {openingHours[1]}
Woensdag: {openingHours[2]}
Donderdag: {openingHours[3]}
Vrijdag: {openingHours[4]}
Zaterdag: {openingHours[5]}
Zondag: {openingHours[6]}");
            Console.WriteLine("\n\nWilt u de afstand berekenen vanaf uw postcode? Typ 'ja': \n");
            string antwoord = Console.ReadLine();

            if (antwoord == "ja")
            {
                bool isIntString; 
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(Logo.OverOns);
                Console.ResetColor();
                Console.WriteLine("\nVul uw straatnaam in: \n");
                string straatnaam = Console.ReadLine();
                while (true)
                {
                    isIntString = straatnaam.Any(letter => char.IsDigit(letter));
                    if (isIntString == true)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(Logo.OverOns);
                        Console.ResetColor();
                        Console.WriteLine("\nGraag enkel de straatnaam, zonder integers");
                        Console.WriteLine("\nVul uw straatnaam in: \n");
                        straatnaam = Console.ReadLine();
                    }
                    else
                        break;
                }
                Console.WriteLine("\nVul uw plaatsnaam in: \n");
                isIntString = true;
                string plaatsnaam = Console.ReadLine();
                while (isIntString == true)
                {
                    isIntString = plaatsnaam.Any(letter => char.IsDigit(letter));
                    if (isIntString == true)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(Logo.OverOns);
                        Console.ResetColor();
                        Console.WriteLine("\nGraag enkel de plaatsnaam, zonder integers");
                        Console.WriteLine("\nVul uw plaatsnaam in: \n");
                        plaatsnaam = Console.ReadLine();
                    }
                    else
                        break;
                }
                string inputForSite = straatnaam + "%20" + plaatsnaam ;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(Logo.OverOns);
                Console.ResetColor();
                Console.WriteLine($"\nAfstand tot restaurant berekenen vanaf {straatnaam}, {plaatsnaam}.....\n");
                DataTable InputAdres = GetCoordinates(inputForSite);
                Console.WriteLine(InputAdres);
            }
            else
            {
                Console.WriteLine();
            }
            Utils.EnterTerug();
        }
    }
}