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

                switch (introMenu.Show())
                {
                    case 0:
                        state = "Reservating";
                        break;
                    case 1:
                        state = "Menu";
                        break;
                    case 2:
                        state = "Information";
                        break;
                    case 3:
                        state = "Employee";
                        LogInEmployee.LogIn();
                        break;
                    default:
                        Console.WriteLine("Deze functie is nog niet geimplementeerd.");
                        break;
                }
            }
            if (state == "Reservating")
            {
                var reservationMenu = new ReservationMenu(new string[3] { "1] Maak een reservering", "2] Annuleer een reservering", "3] Terug" });
                switch (reservationMenu.Show())
                {
                    case 0:
                        state = "Reservate";
                        Serialize(new List<Reservation>() { new Reservation() { Name = "Oscar", Size = 6, Date = "26-04-2021", Time = "18:00" } }, "reservations.json");
                        Reservations.Reservate();
                        break;
                    case 1:
                        state = "Cancel";
                        CancelReservation.cancelReservation();
                        break;
                    case 2:
                        state = null ;
                        //goto case 0; //Case 2 moet terug naar homescreen gaan tho
                        break; 

                }
            }

            else if (state == "Menu")
            { 
                var dishMenu = new DishMenu(new string[3] { "1] Bekijk de menukaart", "2] Zoeken op termen", "3] Terug" });
                switch (dishMenu.Show())
                {
                    case 0:
                        break; //oscar zn shit
                    case 1:
                        state = "DishFilter";
                        break;
                    case 2:
                        state = "Home"; // werkt niet
                        break;

                }
            }
            else if (state == "Information")
            {
                InfoScherm.ShowInfo(); 
            }
            else if (state == "Employee")
            {
                Console.WriteLine("Medewerker?");
                Console.ReadLine();
            }
            if(state == "DishFilter")
            {
                Console.WriteLine("Voer een term in:");
                string keyWord = Console.ReadLine();
                var dishFilter = new DishFilter();
                dishFilter.Search(keyWord);
            }
        }
    }
}

