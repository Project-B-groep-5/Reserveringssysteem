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
        public static void RemoveFromJSON(int i)
        {
            Console.Clear();
            CancelTitle();
            Console.WriteLine("De code staat in het systeem en de reservering wordt nu geannuleerd..");
            ReservationsList.Remove(ReservationsList[i]);                      // Verwijderd bijbehorende item uit JSON
            Serialize(ReservationsList, "reservations.json");                   // Slaat de JSON opnieuw op na de aanpassing
        }
        static void areUSure(int i) => SelectionMenu.Make(new string[2] { "Ja", "Nee" }, actions: new Action[] {cancelReservation , null }, Logo.Annuleren, "Weet u zeker dat u de reservering wilt annuleren?\n");

        public static List<Reservation> ReservationsList; 
        public static void cancelReservation()
        {
            Console.CursorVisible = true;
            bool foundItem = false;
            bool startover = true; 
            Console.Clear();
            CancelTitle();
            while (startover)
            {
                Console.WriteLine("\nVul uw reserveringscode in: \n");
                string input = Console.ReadLine();
                while (true)
                {
                    if (input.ToLower().Length != 4)
                    {
                        Console.Clear();
                        CancelTitle();
                        Console.WriteLine("\nReserveringscode moet uit 4 symbolen bestaan\n\nProbeer opnieuw of ga terug naar het hoofdmenu door op 'enter' te drukken: \n");
                        input = Console.ReadLine();
                        if (input == "")
                            break;
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
                                areUSure(i);
                                RemoveFromJSON(i);
                                startover = false;
                                break;
                            }
                        }
                    }
                    if (foundItem == false)                                                // Als er geen match is vragen of je het opnieuw wilt proberen
                    {
                        Console.Clear();
                        CancelTitle();
                        Console.WriteLine("\nReserveringscode niet herkend.. Probeer opnieuw of ga terug naar het hoofdmenu door op 'enter' te drukken: ");
                        input = Console.ReadLine();
                        if (input == "")
                            break;
                    }
                    else if (foundItem)                                                     // Sluit af als reservering geannuleerd is
                    {
                        Console.Clear();
                        CancelTitle();
                        Utils.Enter();
                        break;
                    }
                }
            }
        }
            

    }
}
