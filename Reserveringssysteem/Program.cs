﻿using System;
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
                var introMenu = new SelectionMenu(new string[5] { "Reservering maken","Reservering annuleren", "Bekijk de menukaart", "Informatie over ons", "[Voor Medewerkers]" }, Logo.Welkom, "\nKies een optie\n");
                Console.Clear();

                switch (introMenu.Show())
                {
                    case 0:
                        Reservations.Reservate();
                        break;
                    case 1:
                        CancelReservation.cancelReservation();
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
            else if (state == "Menu")
            {
                var dishMenu = new SelectionMenu(new string[3] { "Bekijk de menukaart", "Zoeken op termen", "Terug" }, Logo.MenuKaart, "\n\nKies een optie\n");
                switch (dishMenu.Show())
                {
                    case 0:
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(Logo.MenuKaart);
                        Console.ResetColor();
                        Console.WriteLine("\nVoer een term in: \n");
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

