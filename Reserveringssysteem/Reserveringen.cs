using System;
using System.Linq;

namespace Reserveringssysteem
{
    public class Reserveringen
    {
        public static Func<string> genereerReserveringscode = () => new string(Enumerable.Repeat("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789", 4).Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}
