using System;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class Program
    {
        static void Main(string[] args)
        {
            Serialize(new Table[] {new Table("1", 6, new Customer("Oscar"), new Bill(0.00)), new Table("2", 4, new Customer("Arjen"), new Bill(100.0)) }, "tables.json");
            var introMenu = new SelectionMenu(new string[4] { "1] Plaats een reservering", "2] Bekijk het menukaart", "3] Informatie over ons", "[Voor Medewerkers]" });
            introMenu.Show();
        }

    }
}