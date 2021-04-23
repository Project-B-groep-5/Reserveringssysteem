using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Reserveringssysteem
{
    public class CancelReservation
    {
        public static List<Reservation> ReservationsList; 
        public static void cancelReservation()
        {
            var logo = @"
    ____                                               
   / __ \___  ________  ______   _____  ________  ____ 
  / /_/ / _ \/ ___/ _ \/ ___/ | / / _ \/ ___/ _ \/ __ \
 / _, _/  __(__  )  __/ /   | |/ /  __/ /  /  __/ / / /
/_/ |_|\___/____/\___/_/    |___/\___/_/   \___/_/ /_/ 
______________________________________________________
                                                      ";
            Console.WriteLine(logo + "\nVul je reserveringscode in : ");
            string input = Console.ReadLine();
            // Zit die erin? Vraag om Extra gegevens om te controleren?
            // Zit die er niet in? Nog een keer reservingscode invullen?
            // 
            ReservationsList = JsonConvert.DeserializeObject<List<Reservation>>(File.ReadAllText("reservations.json"));
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
