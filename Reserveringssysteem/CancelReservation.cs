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
            Console.ForegroundColor = ConsoleColor.Blue; // Maakt de kleur van header blauw
            Console.WriteLine(Logo.Annuleren);
            Console.ResetColor();
        }

        public static List<Reservation> ReservationsList; 
        public static void cancelReservation()
        {
            Console.CursorVisible = true;
            bool foundItem = false; 
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
                    ReservationsList = Deserialize<List<Reservation>>("reservations.json");          
                    for (int i = 0; i < ReservationsList.Count; i++)                            //Loopt door de reservation items om te kijken of de reserverings code erin staat
                    {
                        if (ReservationsList[i].ReservationId.ToLower() == input.ToLower())     //Als een match gevonden wordt, item verwijderen
                        {
                            Console.Clear();
                            CancelTitle();
                            Console.WriteLine("De code staat in het systeem en de reservering wordt nu geannuleerd..");
                            foundItem = true ;
                            ReservationsList.Remove(ReservationsList[i]);                      // Verwijderd bijbehorende item uit JSON
                            Serialize(ReservationsList,"reservations.json");                   // Slaat de JSON opnieuw op na de aanpassing
                            break;
                        }
                    }
                    }
                if (foundItem == false)                                                // Als er geen match is vragen of je het opnieuw wilt proberen
                {
                    Console.Clear();
                    CancelTitle();
                    Console.WriteLine("\nReserveringscode niet herkend.. Probeer opnieuw of ga terug: ");
                    input = Console.ReadLine();
                }
                else if (foundItem)                                                     // Sluit af als reservering geannuleerd is
                {
                    Console.WriteLine("\nDe reservering is geannuleerd!");
                    Utils.Enter();
                    break;
                }
            }
        }

    }
}
