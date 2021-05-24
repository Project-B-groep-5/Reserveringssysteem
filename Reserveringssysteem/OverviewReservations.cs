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
            var TimeSlots = new List<string>();
            var ReservationsPerTimeslot = new List<List<int>>();
            var today = DateTime.Today.ToString();
            for (int i = 0; i < ReservationList.Count; i++)
            {
                if (!DaysWithOccupation.Contains(ReservationList[i].Date))
                {
                    DaysWithOccupation.Add(ReservationList[i].Date);
                }
                if (!TimeSlots.Contains(ReservationList[i].Time))
                {
                    TimeSlots.Add(ReservationList[i].Time);
                }
            }
            List<string> sorted = DaysWithOccupation.OrderBy(x =>
            {
                DateTime.TryParse(x, out DateTime dt);
                return dt;
            }).ToList();
            TimeSlots.Sort();
            var OccupationPerTimeslot = new int[TimeSlots.Count];

            Header();
            Console.CursorVisible = true;
            for (int i = 0; i < TimeSlots.Count; i++)
            {
                ReservationsPerTimeslot.Add(new List<int>());
            }
            Console.WriteLine("Dit zijn de reserveringen voor vandaag:");
            if (DaysWithOccupation.Contains(today))
            {
                for (int i = 0; i < ReservationList.Count; i++)
                {
                    if (today == ReservationList[i].Date)
                    {
                        ReservationsPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)].Add(i);
                        OccupationPerTimeslot[TimeSlots.IndexOf(ReservationList[i].Time)] += 1;
                    }
                }
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

            }
            else Console.WriteLine("Er zijn geen reserveringen vandaag.");
            Console.Read();

            var optionMenu = new SelectionMenu(new string[2] { "Andere datum bekijken", "Terug naar dashboard" }, Logo.Reserveringen, "\nWat wilt u doen?\n");
            switch (optionMenu.Show())
            {
                case 0:
                    OverviewReservations.ChooseDate();
                    return;
                case 1:
                    EmployeeActions.Menu();
                    return;                
            }


        }
        public static void ChooseDate()
        {
            var DaysWithOccupation = new List<string>();
            var TimeSlots = new List<string>();
            var ReservationsPerTimeslot = new List<List<int>>();
            var today = DateTime.Today.ToString("dd-MM-yyyy");
            for (int i = 0; i < ReservationList.Count; i++)
            {
                if (!DaysWithOccupation.Contains(ReservationList[i].Date) && DateTime.Parse(ReservationList[i].Date) > DateTime.Parse(today))
                {
                    DaysWithOccupation.Add(ReservationList[i].Date);
                }
                if (!TimeSlots.Contains(ReservationList[i].Time))
                {
                    TimeSlots.Add(ReservationList[i].Time);
                }
            }
            List<string> sorted = DaysWithOccupation.OrderBy(x =>
            {
                DateTime.TryParse(x, out DateTime dt);
                return dt;
            }).ToList();
            TimeSlots.Sort();
            var OccupationPerTimeslot = new int[TimeSlots.Count];
            Console.Clear();
            Header();
            Console.CursorVisible = true;
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
                if (!DateTime.TryParse(date, out DateTime dDate) || !DaysWithOccupation.Contains(date))
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
            Console.Read();
            var optionMenu = new SelectionMenu(new string[2] { "Andere datum bekijken", "Terug naar dashboard" }, Logo.Reserveringen, "\nWat wilt u doen?\n");
            switch (optionMenu.Show())
            {
                case 0:
                    OverviewReservations.ChooseDate();
                    return;
                case 1:
                    EmployeeActions.Menu();
                    return;
            }
        }
    }
}
