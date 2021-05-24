using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace Reserveringssysteem
{
    public class Location
    {
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location (string streetName, string houseNumber, string postalCode, string city)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            City = city;
            var location = GetLatLong($"{streetName}+{houseNumber}+{city}");
            Latitude = location.Item1;
            Longitude = location.Item2;
            
        }

        public static Tuple<double, double> GetLatLong(string address)
        {
            double latitude = 0.0;
            double longitude = 0.0;

            string url = "https://maps.google.com/maps/api/geocode/xml?address=" + address + "&key=" + Secrets.APIKey;
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
                        var inputLatitude = location[0];
                        latitude = Convert.ToDouble(inputLatitude, System.Globalization.CultureInfo.InvariantCulture);
                        var inputLongitude = location[1];
                        longitude = Convert.ToDouble(inputLongitude, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
            return Tuple.Create(latitude, longitude);
        }
    }
}
