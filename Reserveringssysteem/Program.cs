using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class Program
    {
        public static List<Reservation> ReservationList = Deserialize<List<Reservation>>("reservations.json");
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            static void dishMenu() => SelectionMenu.Make(new string[3] { "Bekijk de menukaart", "Zoeken op termen", "Terug" }, new Action[] { null, DishFilter.F, null}, Logo.MenuKaart, "\n\nKies een optie\n");
            SelectionMenu.Make(new []{ "Reservering maken", "Reservering annuleren", "Bekijk de menukaart", "Informatie over ons", "[Voor Medewerkers]" }, new Action[] { Reservations.Reservate, CancelReservation.cancelReservation, dishMenu, InfoScherm.ShowInfo, LogInEmployee.LogIn}, Logo.Welkom, "\nKies een optie\n");
            
          Main(args);
        }
    }
}