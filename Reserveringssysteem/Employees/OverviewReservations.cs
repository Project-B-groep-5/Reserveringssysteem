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
            var TimeSlots = GetTimes();
            var ReservationsPerTimeslot = new List<List<int>>();
            var OccupationPerTimeslot = new int[TimeSlots.Count];            
            var string_today = DateTime.Today.ToString("dd-MM-yyyy");           
            string string_tomorrow = DateTime.Today.AddDays(1).ToString("dd-MM-yyyy");

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
            var optionMenu = new SelectionMenu(new string[4] { "Reserveringen voor vandaag bekijken", "Reserveringen voor morgen bekijken","Andere dagen van deze maand met reserveringen bekijken", "Terug naar dashboard" }, Logo.Reserveringen, "\nWat wilt u bekijken? \n");
            switch (optionMenu.Show())
            {
                case 0:
                    ShowDay(sorted, TimeSlots, ReservationsPerTimeslot, string_today, OccupationPerTimeslot);
                    return;
                case 1:
                    ShowDay(sorted, TimeSlots, ReservationsPerTimeslot, string_tomorrow, OccupationPerTimeslot);
                    return;
                case 2:
                    ChooseDate();
                    return;
                case 3:
                    EmployeeActions.MainMenu();
                    return;
            }
        }
        public static List<string> GetTimes()
        {
          var allTimes = new List<string>();
            for (int a = 0; a < 7; a++)
                if (Json.Restaurant.OpeningHours[a] != "Gesloten")
                {
                    for (int i = int.Parse(Json.Restaurant.OpeningHours[a].Split(":")[0]); i < int.Parse(Json.Restaurant.OpeningHours[a].Split("-")[1].Split(":")[0]) - 1; i++)
                        for (int j = 0; j <= 30; j += 30)
                        {
                            string temp = $"{i}:{j:00}";
                            if (!allTimes.Contains(temp))
                                allTimes.Add(temp);
                        }
                }
            return allTimes;
        }
 
        public static void ShowDay(List<string> sorted,List<string> TimeSlots,List<List<int>> ReservationsPerTimeslot,string day, int[] OccupationPerTimeslot)
        {                   
            Header();
            Console.CursorVisible = true;
            if (day == DateTime.Today.ToString("dd-MM-yyyy"))
                Console.WriteLine($"Dit zijn de reserveringen voor vandaag {day}:");
            else if (day == DateTime.Today.AddDays(1).ToString("dd-MM-yyyy"))
                Console.WriteLine($"Dit zijn de reserveringen voor morgen {day}:");
            Console.ForegroundColor = ConsoleColor.White;
            if (sorted.Contains(day))
            {
                for (int i = 0; i < ReservationList.Count; i++)
                {
                    if (day == ReservationList[i].Date)
                    {
                        ReservationsPerTimeslot[TimeSlots.IndexOf(ReservationList[i].TimeSlot[0])].Add(i);
                        OccupationPerTimeslot[TimeSlots.IndexOf(ReservationList[i].TimeSlot[0])] += 1;
                    }
                }
                ShowReservations(TimeSlots, ReservationsPerTimeslot, OccupationPerTimeslot);

            }
            else Console.WriteLine("Er zijn geen reserveringen. \nDruk op enter om terug te gaan");
            Console.Read();
            Overview();
        }

        public static void ChooseDate()
        {
            var DaysWithOccupation = new List<string>();
            var TimeSlots = GetTimes();
            var ReservationsPerTimeslot = new List<List<int>>();
            var OccupationPerTimeslot = new int[TimeSlots.Count];
            var string_today = DateTime.Today.ToString("dd-MM-yyyy");
            string string_tomorrow = DateTime.Today.AddDays(1).ToString("dd-MM-yyyy");

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
            Header();
            Console.CursorVisible = true;
            if (ReservationList.Count > 0)
            {
                Console.WriteLine("Op deze dagen is gereserveerd:");
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < sorted.Count; i++)
                {
                    var temp = DateTime.Parse(sorted[i]);
                    if (temp.Month == DateTime.Today.Month && temp.Year == DateTime.Today.Year)
                        Console.WriteLine(sorted[i]);
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nVul een datum in om per tijdsslot het aantal mensen te zien die komen.");
                Console.ForegroundColor = ConsoleColor.White;
                string date = Console.ReadLine();
                while (true)
                {
                    if (date == "")
                    {
                        Utils.NoInput(ChooseDate, Overview, Logo.Reserveringen);
                        return;
                    }
                    else if (!DateTime.TryParse(date, out DateTime dDate) || !sorted.Contains(date))
                    {
                        Console.WriteLine("\nOpgegeven datum staat niet in de lijst of is niet gelijk aan het format. Het format is: DD-MM-JJJJ \n");
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
                    if (DateTime.Parse(date) == DateTime.Parse(ReservationList[i].Date))
                    {
                        ReservationsPerTimeslot[TimeSlots.IndexOf(ReservationList[i].TimeSlot[0])].Add(i);
                        OccupationPerTimeslot[TimeSlots.IndexOf(ReservationList[i].TimeSlot[0])] += 1;
                    }
                }
                ShowReservations(TimeSlots, ReservationsPerTimeslot, OccupationPerTimeslot);
            }
            else Utils.Enter(EmployeeActions.MainMenu, "Om terug te gaan, er zijn geen reserveringen");
        }
        public static void ShowReservations(List<string> TimeSlots, List<List<int>> ReservationsPerTimeslot, int[] OccupationPerTimeslot)
        {
            Console.WriteLine("\nTijdsloten  |   Aantal reserveringen   |   Reserveringen");
            Console.WriteLine("------------|--------------------------|-----------------------------------------------------------------------");
            for (int i = 0; i < TimeSlots.Count; i++)
            {
                if (ReservationsPerTimeslot[i].Count > 0)
                    Console.WriteLine($"  {TimeSlots[i]}     |             {OccupationPerTimeslot[i]}            |   {ReservationList[ReservationsPerTimeslot[i][0]].Name} heeft gereserveerd voor {ReservationList[ReservationsPerTimeslot[i][0]].Size}. ID = {ReservationList[ReservationsPerTimeslot[i][0]].ReservationId}");
                else Console.WriteLine($"  {TimeSlots[i]}     |             {OccupationPerTimeslot[i]}            | ");
                for (int a = 1; a < OccupationPerTimeslot[i]; a++)
                {
                    Console.WriteLine($"            |                          |   {ReservationList[ReservationsPerTimeslot[i][a]].Name} heeft gereserveerd voor {ReservationList[ReservationsPerTimeslot[i][a]].Size}. ID = {ReservationList[ReservationsPerTimeslot[i][a]].ReservationId}");
                }
                Console.WriteLine("------------|--------------------------|-----------------------------------------------------------------------");
            }
            Utils.Enter(Overview);
        }
    }
}
