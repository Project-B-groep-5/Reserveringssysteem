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

            static void dishMenu() => SelectionMenu.Make(new string[3] { "Bekijk de menukaart", "Zoeken op termen", "Terug" }, new Action[] { ShowAllDishes.F, DishFilter.F, null}, Logo.MenuKaart);
            static void cancelMenu() => SelectionMenu.Make(new string[2] { "Annuleer een reservering", "Terug" }, new Action[] { CancelReservation.cancelReservation, null }, Logo.Annuleren, "\nKies een optie\n"); 
            SelectionMenu.Make(new []{ "Reservering maken", "Reservering annuleren", "Bekijk de menukaart", "Informatie over ons", "[Voor Medewerkers]", "Afsluiten" }, new Action[] { Reservations.Reservate, cancelMenu, dishMenu, InfoScherm.ShowInfo, LogInEmployee.LogIn, Close}, Logo.Welkom);
            
          Main(args);
        }

        static void Close()
        {
            Console.WriteLine(@" __   __   ___     ___     ___  __      __ ___     _     
 \ \ / /  /   \   /   \   | _ \ \ \    / /| __|   | |    
  \ V /   | - |   | - |   |   /  \ \/\/ / | _|    | |__  
  _\_/_   |_|_|   |_|_|   |_|_\   \_/\_/  |___|   |____| 
_| """"""""|_|""""""""""|_|""""""""""|_|""""""""""|_|""""""""""|_|""""""""""|_|""""""""""| 
""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'");
            Environment.Exit(0);
        }
    }
}