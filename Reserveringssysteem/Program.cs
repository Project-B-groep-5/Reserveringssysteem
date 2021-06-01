using System;

namespace Reserveringssysteem
{
    class Program
    {
        public static void DishMenu() => SelectionMenu.Make(new[] { "Bekijk de menukaart", "Zoeken op ingrediënten of allergieën \n", "Terug" }, new Action[] { DishFilter.SecondMenuScreen, DishFilter.ThirdMenuScreen, Main }, Logo.MenuKaart);
        
        public static void Main()
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Json.LoadJson();
            SelectionMenu.Make(new []{ "Reservering maken", "Reservering annuleren", "Bekijk de menukaart", "Informatie over ons", "Afstand tot restaurant berekenen \n", "Medewerkers Dashboard \n", "Afsluiten" }, new Action[] { Reservations.Reservate, Reservations.CancelReservation, DishMenu, InfoScherm.ShowInfo, CalculateDistanceFromInput.Calculate,LogInEmployee.LogIn, Close}, Logo.Welkom);
          Main();
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