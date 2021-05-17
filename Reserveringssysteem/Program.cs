using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class Program
    {
        public static List<Reservation> ReservationList = Deserialize<List<Reservation>>("reservations.json");
        public static readonly Restaurant Restaurant = Deserialize<Restaurant>("restaurant.json");
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            static void cancelMenu() => SelectionMenu.Make(new string[2] { "Annuleer een reservering", "Terug" }, new Action[] { CancelReservation.cancelReservation, null }, Logo.Annuleren, "\nKies een optie\n"); 
            static void dishMenu() => SelectionMenu.Make(new string[3] { "Bekijk de menukaart", "Zoeken op termen", "Terug" }, new Action[] { ShowAllDishes.F, DishFilter.F, null}, Logo.MenuKaart, "\n\nKies een optie\n");
            SelectionMenu.Make(new []{ "Reservering maken", "Reservering annuleren", "Bekijk de menukaart", "Informatie over ons", "[Voor Medewerkers]" }, new Action[] { Reservations.Reservate, cancelMenu, dishMenu, InfoScherm.ShowInfo, LogInEmployee.LogIn}, Logo.Welkom, "\nKies een optie\n");

            Main(args);
        }
    }
}