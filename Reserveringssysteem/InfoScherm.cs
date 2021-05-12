using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace Reserveringssysteem
{
    public class InfoScherm
    {
        public static DataTable GetCoordinates(string address)
        {
            string url = "http://maps.google.com/maps/api/geocode/xml?address=" + address + "&sensor=false";
            WebRequest request = WebRequest.Create(url);

            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    DataTable dtCoordinates = new DataTable();
                    dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                    new DataColumn("Address", typeof(string)),
                    new DataColumn("Latitude",typeof(string)),
                    new DataColumn("Longitude",typeof(string)) });
                    foreach (DataRow row in dsResult.Tables["result"].Rows)
                    {
                        string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                        DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                        dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
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
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
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

        public static Location point1 = new Location(40.7128, 74.0060);
        public static Location point2 = new Location(36.7783, 119.4179);
        public static double DistanceToRest = CalculateDistance(point1, point2);


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
                DataTable InputAdres = GetCoordinates("Leeuwerikstraat 120"); 
                Console.WriteLine("\nUw afstand tot het restaurant is: " + DistanceToRest + " meter.");
            }
            else
            {
                Console.WriteLine("Jammer man");
            }
            Utils.EnterTerug();
        }
    }
}