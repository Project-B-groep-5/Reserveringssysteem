using System;
using System.Collections.Generic;
using System.IO;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class CancelReservation
    {
        public static bool answerNo;
        public static void CancelTitle() // Call deze method om de onderstaande header te krijgen
        {
            Console.ForegroundColor = ConsoleColor.Blue; // Maakt de kleur van header blauw
            Console.WriteLine(Logo.Annuleren);
            Console.ResetColor();
        }
        public static void RemoveFromJSON()
        {
            Console.Clear();
            CancelTitle();                 
            Serialize(ReservationsList, "reservations.json");                   // Slaat de JSON opnieuw op na de aanpassing
        }

        public static void LeaveMenu()
        {
            answerNo = true;
        }
        static void areUSure() => SelectionMenu.Make(new string[2] { "Ja", "Nee" }, actions: new Action[] { RemoveFromJSON, LeaveMenu }, Logo.Annuleren, "Weet u zeker dat u de reservering wilt annuleren?\n");

        public static List<Reservation> ReservationsList; 
        public static void cancelReservation()
        {
            Console.CursorVisible = true;
            bool foundItem = false;
            bool startover = true;
            answerNo = false;
            Console.Clear();
            CancelTitle();
            while (startover == true && answerNo == false)
            {
                Console.WriteLine("\nVul uw reserveringscode in: \n");
                string input = Console.ReadLine();
                while (true && answerNo == false)
                {
                    if (input.ToLower().Length != 4)
                    {
                        Console.Clear();
                        CancelTitle();
                        Console.WriteLine("\nReserveringscode moet uit 4 symbolen bestaan\n\nProbeer opnieuw of ga terug naar het hoofdmenu door op 'enter' te drukken: \n");
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            startover = false;
                            foundItem = true;
                        }
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
                                areUSure();
                                if (foundItem || answerNo == false)
                                {
                                    ReservationsList.Remove(ReservationsList[i]);
                                    RemoveFromJSON();                                               // Verwijderd bijbehorende item uit JSON
                                    foundItem = true;
                                    startover = false;
                                    break;
                                }
                               else
                                {
                                    Console.Clear();
                                    CancelTitle();
                                    Console.WriteLine("Uw reservering is niet geannuleerd");
                                    Utils.Enter();
                                }
                            }
                        }
                    }
                    if (foundItem == false && answerNo == false)                                                // Als er geen match is vragen of je het opnieuw wilt proberen
                    {
                        Console.Clear();
                        CancelTitle();
                        Console.WriteLine("\nReserveringscode niet herkend.. Probeer opnieuw of ga terug naar het hoofdmenu door op 'enter' te drukken: ");
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            startover = false;
                            foundItem = true;
                        }
                    }
                    else if (input == "" && answerNo == false)
                    {
                        startover = false;
                        Utils.Enter();
                        break;
                    }
                    else if (foundItem == true && input != "" && answerNo == false)
                    {
                        Console.Clear();
                        CancelTitle();
                        Console.WriteLine("\nReservering geannuleerd! Indien u opnieuw wilt reserveren kunt u dit weer doen nadat u op 'enter' heeft gedrukt.");
                        Utils.Enter();
                        break;
                    }
                    else if (foundItem || answerNo == false)                                                     // Sluit af als reservering geannuleerd is
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
