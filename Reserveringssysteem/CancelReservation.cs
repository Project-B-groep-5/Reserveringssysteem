using System;
using System.Collections.Generic;
using System.IO;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class CancelReservation
    {
        public static List<Reservation> ReservationsList;
        private static int i = 0;
        private static bool keepTrue = true; 
        public static void cancelReservation()
        {
            Console.CursorVisible = true;
            Console.Clear();
            CancelTitle();
            keepTrue = true;
            Console.WriteLine("\nVoer uw reserveringscode in of druk 'enter' om terug te gaan naar het vorige scherm\nUw reserveringscode bestaat uit vier symbolen en kan teruggevonden worden in de reserveringsmail: \n");
            string input = Console.ReadLine();
            while (keepTrue)
            {
                if (input.ToLower().Length == 4)
                {
                    ReservationsList = Deserialize<List<Reservation>>("reservations.json");
                    if (ReservationsList.Count > 0)
                    {
                        for (i = 0; i < ReservationsList.Count; i++)                            //Loopt door de reservation items om te kijken of de reserverings code erin staat
                        {
                            if (ReservationsList[i].ReservationId.ToLower() == input.ToLower())     //Als een match gevonden wordt...
                            {
                                Console.Clear();
                                CancelTitle();
                                AreUSure();                                                         //Laat het keuzemenu zien; ja voor verwijderen <--> nee voor terug naar vorige scherm
                                keepTrue = false;
                                break;
                            }
                            input = InputAgain();
                        }
                    }
                    else                                                                          //Als er geen reserveringen in het systeem staan kan de for loop niet werken, vandaar deze else  
                    {
                        Console.Clear();
                        CancelTitle();
                        Console.WriteLine("\nGeen reserveringen gevonden in het systeem. Druk op 'enter' om weer terug te gaan naar het hoofdmenu.");
                        Utils.Enter();
                        keepTrue = false;
                    }
                }
                else if (input == "")                                                             // Input = 'enter' 
                {
                    keepTrue = false;
                    Utils.Enter();
                }
                else                                                                              // Nieuwe input
                {
                    input = InputAgain();
                }
            }
        }
        static void AreUSure() => SelectionMenu.Make(new string[2] { "Ja", "Nee" }, actions: new Action[] { ja, nee }, Logo.Annuleren, "\nWeet u zeker dat u de reservering wilt annuleren?\n");
        public static void ja()
        {
            DeleteReservation();
        }
        private static void DeleteReservation()
        {
            ReservationsList = Deserialize<List<Reservation>>("reservations.json");
            ReservationsList.Remove(ReservationsList[i]);
            Console.Clear();
            CancelTitle();
            Serialize(ReservationsList, "reservations.json");                   // Slaat de JSON opnieuw op na de aanpassing
            Console.WriteLine("\nDe reservering is verwijderd.\nDruk 'enter' om terug te gaan naar het hoofdmenu.");
            keepTrue = false;
            Utils.Enter();
        }
        public static void nee()
        {
            Console.Clear();
            CancelTitle();
            Console.WriteLine("\nDe reservering is niet verwijderd.");
            keepTrue = false;
            Utils.Enter();
        }
        public static string InputAgain()
        {
            Console.Clear();
            CancelTitle();
            Console.WriteLine("\nReserveringscode onjuist. De reserveringscode bestaat uit vier karakters. \nVoer uw reserveringscode nogmaals in of ga terug met 'enter': \n");
            string input = Console.ReadLine();
            if (input == "")
            {
                keepTrue = false;
                return input;
            }
            return input;
        }
        public static void CancelTitle() // Call deze method om de onderstaande header te krijgen
        {
            Console.ForegroundColor = ConsoleColor.Blue; // Maakt de kleur van header blauw
            Console.WriteLine(Logo.Annuleren);
            Console.ResetColor();
        }
    }
}
