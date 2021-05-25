using System;

namespace Reserveringssysteem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Json.LoadJson();
            static void dishMenu() => SelectionMenu.Make(new string[6] { "Voorgerechten", "Hoofdgerechten", "Nagerechten", "Dranken", "Zoeken op ingrediënten of allergieën \n", "Terug" },  new Action[] { DishFilter.voorgerechten, DishFilter.hoofdgerechten, DishFilter.nagerechten, DishFilter.dranken, DishFilter.F, null}, Logo.MenuKaart);
            SelectionMenu.Make(new []{ "Reservering maken", "Reservering annuleren", "Bekijk de menukaart", "Informatie over ons \n", "Medewerkers Dashboard \n", "Afsluiten" }, new Action[] { Reservations.Reservate, Reservations.CancelReservation, dishMenu, InfoScherm.ShowInfo, LogInEmployee.LogIn, Close}, Logo.Welkom + $"\nBij {Json.Restaurant.Name}");
            
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