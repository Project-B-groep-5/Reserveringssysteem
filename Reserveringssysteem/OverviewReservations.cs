using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;
using System.Linq;

namespace Reserveringssysteem
{
    public class OverviewReservations
    {
        private static void Header() => Logo.PrintLogo(Logo.Reserveringen);
        public static void Overview()
        {
            var DaysWithOccupation = new List<string>();
            var TimeSlots = new List<string> { "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00" };
            var ReservationsPerTimeslot = new List<List<int>>();
            var OccupationPerTimeslot = new int[TimeSlots.Count];
            var today = DateTime.Today;
            var string_today = today.ToString("dd-MM-yyyy");
            var tomorrow = today.AddDays(1);
            string string_tomorrow = tomorrow.ToString("dd-MM-yyyy");
            for (int i = 0; i < ReservationList.Count; i++)
            {
                if (!DaysWithOccupation.Contains(ReservationList[i].Date))
                {
                    DaysWithOccupation.Add(ReservationList[i].Date);
                }

            }
            List<string> sorted = DaysWithOccupation.OrderBy(x =>
            {
                DateTime.TryParse(x, out DateTime dt);
                return dt;
            }).ToList();
            for (int i = 0; i < TimeSlots.Count; i++)
            {
                ReservationsPerTimeslot.Add(new List<int>());
            }
            var optionMenu = new SelectionMenu(new string[4] { "Reserveringen voor vandaag bekijken", "Reserveringen voor morgen bekijken","Reserveringen andere datum bekijken", "Terug naar dashboard" }, Logo.Reserveringen, "\nWat wilt u doen?\n");
            switch (optionMenu.Show())
            {
                case 0:
                    Today(sorted, TimeSlots, ReservationsPerTimeslot, string_today, OccupationPerTimeslot);
                    return;
                case 1:
                    Tomorrow(sorted, TimeSlots, ReservationsPerTimeslot, string_tomorrow, OccupationPerTimeslot);
                    return;
                case 2:
                    ChooseDate(sorted, TimeSlots, ReservationsPerTimeslot, OccupationPerTimeslot);
                    return;
                case 3:
                    EmployeeActions.Menu();
                    return;
            }
        }
        public static void Tomorrow(List<string> sorted,List<string> TimeSlots,List<List<int>> ReservationsPerTimeslot,string string_tomorrow, int[] OccupationPerTimeslot)
        {                   
            Header();
            Console.CursorVisible = true;
            
            Console.WriteLine($"Dit zijn de reserveringen voor morgen {string_tomorrow}:");
            
            if (sorted.Contains(string_tomorrow))
            {
                for (int i = 0; i < ReservationList.Count; i++)
                {
                    if (string_tomorrow == ReservationList[i].Date)
                    {
                        ReservationsPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)].Add(i);
                        OccupationPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)] += 1;
                    }
                }
                ShowReservations(TimeSlots, ReservationsPerTimeslot, OccupationPerTimeslot);

            }
            else Console.WriteLine("Er zijn geen reserveringen morgen.\nDruk op enter om terug te gaan");
            Console.Read();
            Overview();
        }   
        public static void Today(List<string> sorted, List<string> TimeSlots, List<List<int>> ReservationsPerTimeslot, string today, int[] OccupationPerTimeslot)
        {   
            Header();
            Console.CursorVisible = true;
            for (int i = 0; i < TimeSlots.Count; i++)
            {
                ReservationsPerTimeslot.Add(new List<int>());
            }
            Console.WriteLine($"Dit zijn de reserveringen voor vandaag {today}:");
            if (sorted.Contains(today))
            {
                for (int i = 0; i < ReservationList.Count; i++)
                {
                    if (today == ReservationList[i].Date)
                    {
                        ReservationsPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)].Add(i);
                        OccupationPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)] += 1;
                    }
                }
                ShowReservations(TimeSlots, ReservationsPerTimeslot, OccupationPerTimeslot);

            }
            else Console.WriteLine("Er zijn geen reserveringen vandaag.\nDruk op enter om terug te gaan");
            Console.Read();
            Overview();
        }
        public static void ChooseDate(List<string> sorted, List<string> TimeSlots, List<List<int>> ReservationsPerTimeslot, int[] OccupationPerTimeslot)
        {                      
            Header();
            Console.CursorVisible = true;
                      
            Console.WriteLine("Op deze dagen is gereserveerd:");

            for (int i = 0; i < sorted.Count; i++)
            {
                Console.WriteLine(sorted[i]);
            }
            Console.WriteLine("Vul een datum in om per tijdsslot het aantal mensen te zien die komen.");
            string date = Console.ReadLine();
            while (true)
            {
                if (!DateTime.TryParse(date, out DateTime dDate) || !sorted.Contains(date))
                {
                    Console.WriteLine("\nOpgegeven datum staat niet in de lijst of is niet gelijk aan het format. Het format is: DD-MM-JJJJ\n");
                    date = Console.ReadLine();
                }
                else
                {
                    string.Format("{0:dd/MM/yyyy}", dDate);
                    break;
                }
            }
            for (int i = 0; i < ReservationList.Count; i++)
            {
                if (date == ReservationList[i].Date)
                {
                    ReservationsPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)].Add(i);
                    OccupationPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)] += 1;
                }
            }
            ShowReservations(TimeSlots, ReservationsPerTimeslot, OccupationPerTimeslot);
        }
        public static void ShowReservations(List<string> TimeSlots, List<List<int>> ReservationsPerTimeslot, int[] OccupationPerTimeslot)
        {
            Console.WriteLine("Tijdsloten      Aantal reserveringen      Reserveringen");
            for (int i = 0; i < TimeSlots.Count; i++)
            {
                if (ReservationsPerTimeslot[i].Count > 0)
                    Console.WriteLine($"  {TimeSlots[i]}                  {OccupationPerTimeslot[i]}                 {ReservationList[ReservationsPerTimeslot[i][0]].Name} heeft gereserveerd voor {ReservationList[ReservationsPerTimeslot[i][0]].Size}. ID = {ReservationList[ReservationsPerTimeslot[i][0]].ReservationId}");
                else Console.WriteLine($"  {TimeSlots[i]}                  {OccupationPerTimeslot[i]} ");
                for (int a = 1; a < OccupationPerTimeslot[i]; a++)
                {
                    Console.WriteLine($"                                           {ReservationList[ReservationsPerTimeslot[i][a]].Name} heeft gereserveerd voor {ReservationList[ReservationsPerTimeslot[i][a]].Size}. ID = {ReservationList[ReservationsPerTimeslot[i][a]].ReservationId}");
                }

            }
            Console.WriteLine("Druk op enter om terug te gaan");
            Console.Read();
            Overview();
        }
    }
}
