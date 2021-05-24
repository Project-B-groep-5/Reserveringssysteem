using System;

namespace Reserveringssysteem
{
    class Program
    {
        public static void Main()
        {
            Console.CursorVisible = false;
            Json.LoadJson();
            static void secondMenuScreen() => SelectionMenu.Make(new string[5] { "Voorgerechten", "Hoofdgerechten", "Nagerechten", "Dranken \n", "Terug"}, new Action[] { DishFilter.voorgerechten, DishFilter.hoofdgerechten, DishFilter.nagerechten, DishFilter.dranken, dishMenu}, Logo.MenuKaart);
            static void dishMenu() => SelectionMenu.Make(new string[3] { "Bekijk de menukaart", "Zoeken op ingrediënten of allergieën \n", "Terug" }, new Action[] { secondMenuScreen, DishFilter.F, Main}, Logo.MenuKaart);
            SelectionMenu.Make(new []{ "Reservering maken", "Reservering annuleren", "Bekijk de menukaart", "Informatie over ons \n", "Medewerkers Dashboard \n", "Afsluiten" }, new Action[] { Reservations.Reservate, Reservations.CancelReservation, dishMenu, InfoScherm.ShowInfo, LogInEmployee.LogIn, Close}, Logo.Welkom);
        }

        private static void Close()
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