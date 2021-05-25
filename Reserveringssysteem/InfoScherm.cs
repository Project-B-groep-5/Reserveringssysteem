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
            
            Utils.Enter(Program.Main);
        }
    }
}