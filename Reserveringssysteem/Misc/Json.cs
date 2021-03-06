using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Reserveringssysteem
{
    public static class Json
    {
        public static List<Reservation> ReservationList { get; set; }
        public static Restaurant Restaurant { get; set; }
        public static List<Dish> DishList { get; set; }
        public static List<VoordeelMenu> VoordeelMenus { get; set; }
        public static List<Table> Tables { get; set; }

        public static void Serialize(object obj, string filename) => File.WriteAllText(filename, JsonConvert.SerializeObject(obj, Formatting.Indented));
        private static T Deserialize<T>(string filename) => JsonConvert.DeserializeObject<T>(File.ReadAllText(filename));
    
        public static void LoadJson()
        {
            ReservationList = Deserialize<List<Reservation>>("Assets/reservations.json");
            Restaurant = Deserialize<Restaurant>("Assets/restaurant.json");
            DishList = Deserialize<List<Dish>>("Assets/dishes.json");
            VoordeelMenus = Deserialize<List<VoordeelMenu>>("Assets/voordeelmenu.json");
            Tables = Deserialize<List<Table>>("Assets/tables.json");
        }
    }
}
