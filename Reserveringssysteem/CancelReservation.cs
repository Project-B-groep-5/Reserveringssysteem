using System;
using System.Collections.Generic;
using System.IO;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class CancelReservation
    {   
        public static void CancelTitle() // Call deze method om de onderstaande header te krijgen
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen; // Maakt de kleur van header groen
            Console.WriteLine(Logo.Annuleren);
            Console.ResetColor();
        }

        public static List<Reservation> ReservationsList; 
        public static void cancelReservation()
        {
            Console.Clear();
            CancelTitle();
            Console.WriteLine("\nVul je reserveringscode in: \n");
            string input = Console.ReadLine();
            while (true)
            {
                if (input.ToLower().Length != 4)
                {
                    Console.Clear();
                    CancelTitle();
                    Console.WriteLine("\nReserveringscode moet uit 4 symbolen bestaan\n\nVul je reserveringscode in: \n");
                    input = Console.ReadLine();
                }
                if (input.ToLower().Length == 4)
                {
                    Console.Clear();
                    break;
                }
            }
            ReservationsList = Deserialize<List<Reservation>>("reservations.json");
                for (int i = 0; i < ReservationsList.Count; i++)
                {
                    if (ReservationsList[i].ReservationId.ToLower() == input.ToLower())
                    {
                        Console.Clear();
                        Console.WriteLine("Ga naar annuleerfunctie");
                    }
                    else
                    {
                        Console.Clear();
                        CancelTitle();
                        Console.WriteLine("\nReserveringscode niet herkend.. Probeer opnieuw of ga terug: ");
                        input = Console.ReadLine();
                    }
               
            }
            Utils.EnterTerug();
        }

    }
}
