using System;
using System.Linq;

namespace Reserveringssysteem
{
    public class Reservations
    {
        public static Reservation Reservate()
        {
            Console.WriteLine("What is your name?");
            var name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("When do you want to reservate?");
            Console.WriteLine("Use the format: dd-mm-yyyy");
            var date = Console.ReadLine();
            Console.Clear();
            Console.WriteLine(@$"You made a new reservation for: {date}
Your reservation code is: {GenerateReservationCode()}");
            //return new Reservation(name, date, size, menu, id);
            return null;
        }
        public static Func<string> GenerateReservationCode = () => new string(Enumerable.Repeat("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789", 4).Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}
