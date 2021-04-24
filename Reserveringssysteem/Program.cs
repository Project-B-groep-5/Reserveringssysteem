using System;
using System.IO;
using static Reserveringssysteem.Json;
using System.Text.Json;
using System.Collections.Generic;

namespace Reserveringssysteem
{
    class Program
    {
        public static string state;
        public static List<Reservation> ReservationList;
        public static List<Dish> dishList;
        static void Main(string[] args)
        {
            ReservationList = Deserialize<List<Reservation>>("reservations.json");
            if (state == null)
            {
            var introMenu = new SelectionMenu(new string[4] { "1] Reservering", "2] Bekijk de menukaart", "3] Informatie over ons", "[Voor Medewerkers]" });
            Console.Clear();

                switch (introMenu.Show())
                {
                    case 0:
                        state = "Reservating";
                        break;
                    case 1:
                        state = "Menu";
                        break;
                    case 2:
                        InfoScherm.ShowInfo();
                        Console.ReadLine();
                        break;
                    case 3:
                        LogInEmployee.LogIn();
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Deze functie is nog niet geimplementeerd.");
                        break;
                }
            }
            if (state == "Reservating")
            {
                var reservationMenu = new ReservationMenu(new string[3] { "1] Maak een reservering", "2] Annuleer een reservering", "3] Terug" });
                Console.Clear();
                switch (reservationMenu.Show())
                {
                    case 0:
                        Reservations.Reservate();
                        Console.ReadLine();
                        break;
                    case 1:
                        CancelReservation.cancelReservation();
                        Console.ReadLine();
                        break;
                    case 2:
                        state = null ;
                        break; 

                }
            }

            else if (state == "Menu")
            { 
                var dishMenu = new DishMenu(new string[3] { "1] Bekijk de menukaart", "2] Zoeken op termen", "3] Terug" });
                switch (dishMenu.Show())
                {
                    case 0:
                        break;
                    case 1:
                        Console.WriteLine("Voer een term in:");
                        string keyWord = Console.ReadLine();
                        var dishFilter = new DishFilter();
                        dishFilter.Search(keyWord);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 2:
                        state = null;
                        break;
                }
            }
            Main(args);
        }
    }
}

