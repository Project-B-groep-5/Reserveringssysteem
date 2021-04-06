using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Reservations
    {
        public static void Reservate()
        {
            string name;
            int size = 0;
            string date;
            string time;

            Console.WriteLine("Wat is uw naam?");
            name = Console.ReadLine();
            Console.Clear();
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
                    Console.WriteLine($"{input} is niet een correcte waarde.\nVul alstublieft een getal in.");                    
                }
            }
            Console.Clear();
            Console.WriteLine("Voor wanneer wilt u reserveren?");
            Console.WriteLine("Gebruik alstublieft het format: DD-MM-JJJJ");
            date = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Hoe laat wilt u komen eten?");
            time = Console.ReadLine();
            Console.Clear();
            var reservation = new Reservation { Name = name, Date = date, Time = time, Size = size };
            Console.WriteLine($"You made a new reservation for: {date}\nYour reservation code is: {reservation.ReservationId}");
        }
    }
}
