using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Reservations
    {
        public static void ReservateTitle() // Call deze method om de onderstaande header te krijgen
        {
            Console.WriteLine(Logo.Reserveren); 
        }
        public static void Reservate()
        {
            string name;
            int size = 0;
            string date;
            string time = "" ;
            Reservation reservation;
            ReservateTitle();
            Console.WriteLine("Wat is uw naam?");
            name = Console.ReadLine();
            while (true)
            {

                if (name.Length == 0)
                {
                    Console.Clear();
                    ReservateTitle(); 
                    Console.WriteLine("Geen naam ingevuld. Probeer opnieuw : \n");
                    name = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    break;
                }

            }
            

            
            ReservateTitle();
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
                    ReservateTitle();
                    Console.WriteLine($"{input} is geen correcte waarde.\nVul alstublieft een getal in.");
                }
            }
            Console.Clear();
            ReservateTitle();
            Console.WriteLine("Voor wanneer wilt u reserveren?");
            Console.WriteLine("Gebruik alstublieft het format: DD-MM-JJJJ");
            date = Console.ReadLine();
            while (true)
            {
                if (date.Length != 10)
                {
                    Console.Clear();
                    ReservateTitle();
                    Console.WriteLine("Houd je aan de format aub: DD-MM-JJJJ, Probeer opnieuw:");
                    date = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    break;
                }
            }
            
            var timeMenu = new SelectionMenu(new string[7] { "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00" }, Logo.Reserveren, "\nHoe laat wilt u komen eten?\n");
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
            
            if (ReservationCheck.Check(date, time, size))
            {
                reservation = new Reservation { Name = name, Date = date, Time = time, Size = size };
                Console.WriteLine($"Je hebt een reservering gemaakt op : {date} om : {time} uur!\nJe reserveringscode is : {reservation.ReservationId}");
            }
            else
            {
                var optionMenu = new SelectionMenu(new string[2] { "Opnieuw proberen", "Stoppen"}, Logo.Reserveren, "\nHet restaurant zit vol op de door uw gekozen datum en tijd, wat wilt u doen?\n");
                switch (optionMenu.Show())
                {
                    case 0:
                        Reservate();
                        break;
                    case 1:
                        Program.state = null;
                        break;
                    
                }
            }
        }
    }
}