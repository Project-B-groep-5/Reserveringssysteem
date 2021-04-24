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
            Console.WriteLine(Logo.Reserveren + "\nVul je reserveringscode in : ");
            string input = Console.ReadLine();
            // Zit die erin? Vraag om Extra gegevens om te controleren?
            // Zit die er niet in? Nog een keer reservingscode invullen?
            ReservationsList = Deserialize<List<Reservation>>("reservations.json");
            for (int i = 0; i < ReservationsList.Count; i++)
            {
                if (ReservationsList[i].ReservationId.ToLower() == input.ToLower())
                {
                    Console.WriteLine("AYY HET LUKT");
                }
                else
                    Console.WriteLine("Het lukt niet!");
            }
        }

    }
}
