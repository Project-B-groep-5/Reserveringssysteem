using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    static class Utils
    {
        public static void EnterTerug()
        {
            Console.CursorVisible = false;
            string message = "\n\nDruk op 'enter' om terug te gaan";
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.ReadLine();
            Console.ResetColor();
            Console.Clear();
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
