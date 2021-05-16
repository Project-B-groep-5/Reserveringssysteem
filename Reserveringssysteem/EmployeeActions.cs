using System;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class EmployeeActions
    {
        public static void ChangeRestaurantInfo()
        {
            Serialize(new Restaurant("De houten vork", new Location("Wijhaven", "107", "3011 WN", "Rotterdam"), 100, new string[] { "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "Gesloten" }, new string[] { "oscar.vugt@gmail.com", "06-12932305"}), "restaurant.json");
        }
    }
}
