using System;
using System.Collections.Generic;
using System.IO;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class CancelReservation
    {
        public static List<Reservation> ReservationsList; 
        public static void cancelReservation()
        {
            Console.Clear();
            Console.Write(Logo.Reserveren);
            Console.WriteLine("\nVul je reserveringscode in: ");
            string input = Console.ReadLine();
            while (true)
            {
                if (input.ToLower().Length != 4)
                {
                    Console.Clear();
                    Console.WriteLine(Logo.Reserveren + "\nReserveringscode moet uit 4 symbolen bestaan\nVul je reserveringscode in : ");
                    input = Console.ReadLine();
                }
                if (input.ToLower().Length == 4)
                {
                    Console.Clear();
                    break;
                }
            }
            // Zit die erin? Vraag om Extra gegevens om te controleren?
            // Zit die er niet in? Nog een keer reservingscode invullen?
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
                        Console.WriteLine(Logo.Reserveren + "\nReserveringscode niet herkend.. Probeer opnieuw of ga terug: ");
                        input = Console.ReadLine();
                        //break;
                    }
               
            }
            Utils.EnterTerug();
        }

    }
}
