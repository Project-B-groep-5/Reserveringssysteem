using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Reservations
    {
        public static void reservateTitle() // Call deze method om de onderstaande header te krijgen
        {
            var header = @"
    ____                                               
   / __ \___  ________  ______   _____  ________  ____ 
  / /_/ / _ \/ ___/ _ \/ ___/ | / / _ \/ ___/ _ \/ __ \
 / _, _/  __(__  )  __/ /   | |/ /  __/ /  /  __/ / / /
/_/ |_|\___/____/\___/_/    |___/\___/_/   \___/_/ /_/ 
                                                      ";
            Console.WriteLine(header); 
        }
        public static void Reservate()
        {
            string name;
            int size = 0;
            string date;
            string time = "" ;
            Reservation reservation;
            reservateTitle();
            Console.WriteLine("Wat is uw naam?");
            name = Console.ReadLine();
            while (true)
            {

                if (name.Length == 0)
                {
                    Console.Clear();
                    reservateTitle(); 
                    Console.WriteLine("Geen naam ingevuld. Probeer opnieuw : \n");
                    name = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    break;
                }

            }
            

            
            reservateTitle();
            Console.WriteLine("Voor hoeveel mensen wilt u een reservering maken?");
            while (size == 0)
            {
                var input = Console.ReadLine();
                try
                {
                    size = int.Parse(input);
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine($"{input} is geen correcte waarde.\nVul alstublieft een getal in.");
                }
            }
            Console.Clear();
            reservateTitle();
            Console.WriteLine("Voor wanneer wilt u reserveren?");
            Console.WriteLine("Gebruik alstublieft het format: DD-MM-JJJJ");
            date = Console.ReadLine();
            Console.Clear();
            var timeMenu = new ReservationMenu(new string[7] { "1] 17:00", "2] 17:30", "3] 18:00", "4] 18:30", "5] 19:00", "6] 19:30", "7] 20:00" });
            switch (timeMenu.Show())
            {
                case 0:
                    time = "17:00";
                    break; 
                case 1:
                    time = "17:30";
                    break;
                case 2:
                    time = "18:00";
                    break;
                case 3:
                    time = "18:30";
                    break;
                case 4:
                    time = "19:00";
                    break;
                case 5:
                    time = "19:30";
                    break;
                case 6:
                    time = "20:00";
                    break;

            }
            Console.Clear();
            reservation = new Reservation { Name = name, Date = date, Time = time, Size = size };
            Console.WriteLine($"Je hebt een reservering gemaakt op : {date} om : {time} uur!\nJe reserveringscode is : {reservation.ReservationId}");
        }
    }
}