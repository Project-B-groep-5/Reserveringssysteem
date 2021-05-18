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
            static void secondMenuScreen() => SelectionMenu.Make(new string[5] { "Voorgerechten", "Hoofdgerechten", "Nagerechten", "Dranken", "Terug"}, new Action[] { DishFilter.voorgerechten, DishFilter.hoofdgerechten, DishFilter.nagerechten, DishFilter.dranken, null}, Logo.MenuKaart);
            static void dishMenu() => SelectionMenu.Make(new string[3] { "Bekijk de menukaart", "Zoeken op ingrediënten of allergieën", "Terug" }, new Action[] { secondMenuScreen, DishFilter.F, null}, Logo.MenuKaart);
            SelectionMenu.Make(new []{ "Reservering maken", "Reservering annuleren", "Bekijk de menukaart", "Informatie over ons", "[Voor Medewerkers]", "Afsluiten" }, new Action[] { Reservations.Reservate, CancelReservation.cancelReservation, dishMenu, InfoScherm.ShowInfo, LogInEmployee.LogIn, Close}, Logo.Welkom);
            
          Main(args);
        }

        static void Close()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@" __   __   ___     ___     ___  __      __ ___     _     
 \ \ / /  /   \   /   \   | _ \ \ \    / /| __|   | |    
  \ V /   | - |   | - |   |   /  \ \/\/ / | _|    | |__  
  _\_/_   |_|_|   |_|_|   |_|_\   \_/\_/  |___|   |____| 
_| """"""""|_|""""""""""|_|""""""""""|_|""""""""""|_|""""""""""|_|""""""""""|_|""""""""""| 
""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'
_______________________________________________________________________________________________________");
            Console.ResetColor();
            Environment.Exit(0);
        }
    }
}