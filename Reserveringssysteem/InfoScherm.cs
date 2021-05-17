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
        public static void ShowInfo()
        {
            Logo.PrintLogo(Logo.OverOns);
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
            Utils.EnterTerug();
        }
    }
}