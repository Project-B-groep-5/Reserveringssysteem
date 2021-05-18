using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static Reserveringssysteem.Json;
using System.Linq;

namespace Reserveringssysteem
{
    public class OverviewReservations
    {
        public static List<Reservation> ReservationsList;
        public static int[] OccupationPerTimeslot;
        public static List<List<int>> ReservationsPerTimeslot;
        public static List<string> DaysWithOccupation;
        public static List<string> TimeSlots;
        public static DateTime dDate;
        public static void ReservationsTitle() 
        {
            Console.ForegroundColor = ConsoleColor.Blue; 
            Console.WriteLine(Logo.Reserveringen);
            Console.ResetColor();
        }
        public static void Overview()
        {
            
            DaysWithOccupation = new List<string>();
            TimeSlots = new List<string>();
            ReservationsPerTimeslot = new List<List<int>>();
            ReservationsList = Deserialize<List<Reservation>>("reservations.json");

            for (int i = 0; i < ReservationsList.Count; i++)
            {
               if (!DaysWithOccupation.Contains(ReservationsList[i].Date))
               {
                    DaysWithOccupation.Add(ReservationsList[i].Date);
               }
               if (!TimeSlots.Contains(ReservationsList[i].Time))
               {
                    TimeSlots.Add(ReservationsList[i].Time);
               }
            }
            List<string> sorted = DaysWithOccupation.OrderBy(x =>
            {
                DateTime dt;
                DateTime.TryParse(x, out dt);
                return dt;
            }).ToList<string>();
            TimeSlots.Sort();
            OccupationPerTimeslot = new int[TimeSlots.Count];
            Console.CursorVisible = true;
            Console.Clear();
            ReservationsTitle();
            for (int i = 0; i < TimeSlots.Count; i++)
            {
                ReservationsPerTimeslot.Add(new List<int>());
            }
            Console.WriteLine("Op deze dagen is gereserveerd:");

            for (int i = 0; i < sorted.Count; i++)
            {
                Console.WriteLine(sorted[i]);
            }
            Console.WriteLine("Vul een datum in om per tijdsslot het aantal mensen te zien die komen.");
            string date = Console.ReadLine();
            while (true)
            {
                if (!DateTime.TryParse(date, out dDate) || !DaysWithOccupation.Contains(date))
                {
                    Console.WriteLine("\nOpgegeven datum staat niet in de lijst of is niet gelijk aan het format. Het format is: DD-MM-JJJJ\n");
                    date = Console.ReadLine();
                }
                else
                {
                    String.Format("{0:dd/MM/yyyy}", dDate);
                    break;
                }
            }
            for (int i = 0; i < ReservationsList.Count; i++)
            {
                if (date == ReservationsList[i].Date)
                {
                    ReservationsPerTimeslot[TimeSlots.IndexOf(ReservationsList[i].Time)].Add(i);
                    OccupationPerTimeslot[TimeSlots.IndexOf(ReservationsList[i].Time)] += 1;
                }
            }
            Console.WriteLine(ReservationsPerTimeslot[0]);
                     Console.WriteLine("Tijdsloten      Aantal reserveringen      Reserveringen");
            for(int i = 0; i < TimeSlots.Count; i++)
            {
                if (ReservationsPerTimeslot[i].Count > 0)
                    Console.WriteLine($"  {TimeSlots[i]}                  {OccupationPerTimeslot[i]}                 {ReservationsList[ReservationsPerTimeslot[i][0]].Name} heeft gereserveerd voor {ReservationsList[ReservationsPerTimeslot[i][0]].Size}. ID = {ReservationsList[ReservationsPerTimeslot[i][0]].ReservationId}");
                else Console.WriteLine($"  {TimeSlots[i]}                  {OccupationPerTimeslot[i]} ");
                for (int a= 1; a < OccupationPerTimeslot[i]; a++)
                {
                    Console.WriteLine($"                                     {ReservationsList[ReservationsPerTimeslot[i][a]].Name} heeft gereserveerd voor {ReservationsList[ReservationsPerTimeslot[i][a]].Size}. ID = {ReservationsList[ReservationsPerTimeslot[i][a]].ReservationId}");
                }
            }
            Utils.Enter();
        }
    }
}
