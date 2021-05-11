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
        static void Main(string[] args)
        {
            ReservationList = Deserialize<List<Reservation>>("reservations.json");
            if (state == null)
            {
            var introMenu = new SelectionMenu(new string[5] { "Reservering maken","Reservering annuleren", "Bekijk de menukaart", "Informatie over ons", "[Voor Medewerkers]" }, Logo.Welkom, "\n\nWelkom bij [Restaurant]!\n");
            Console.Clear();

                switch (introMenu.Show())
                {
                    case 0:
                        state = "Reservating";
                        break;
                    case 1:
                        state = "Reservering annuleren";
                        break;
                    case 2:
                        state = "Menu";
                        break;
                    case 3:
                        InfoScherm.ShowInfo();
                        break;
                    case 4:
                        LogInEmployee.LogIn();
                        break;
                    default:
                        Console.WriteLine("Deze functie is nog niet geimplementeerd.");
                        break;
                }
            }
            if (state == "Reservating")
            {
                Reservations.Reservate();
            }
            else if (state ==  "Reservering annuleren")
            {
                CancelReservation.cancelReservation();
                
            }

            else if (state == "Menu")
            { 
                var dishMenu = new SelectionMenu(new string[3] { "Bekijk de menukaart", "Zoeken op termen", "Terug" }, Logo.MenuKaart, "\n\nKies een optie\n");
                switch (dishMenu.Show())
                {
                    case 0:
                        break;
                    case 1:
                        Console.WriteLine("Voer een term in:");
                        string keyWord = Console.ReadLine();
                        var dishFilter = new DishFilter();
                        dishFilter.Search(keyWord);
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

