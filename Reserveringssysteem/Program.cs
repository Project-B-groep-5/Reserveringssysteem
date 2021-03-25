using System;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonSerialize(new Tafel[] {new Tafel(1, 6, new Klant("Oscar"), new Rekening(0.00)), new Tafel(2, 4, new Klant("Arjen"), new Rekening(100.0)) }, "tafels.json");
            var introMenu = new SelectionMenu(new string[4] { "1] Plaats een reservering", "2] Bekijk het menukaart", "3] Informatie over ons", "[Voor Medewerkers]" });
            
            introMenu.Show();
        }

    }
}