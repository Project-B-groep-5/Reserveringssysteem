using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    static class Utils
    {
        public static void Enter(string msg = "om terug te gaan")
        {
            Console.CursorVisible = false;
            string message = $"\n\nDruk op 'enter' {msg}.";
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

        public static int Confirm(string logo, string message)
        {
            var menu = new SelectionMenu(new[] { "Ja", "Nee" }, logo, message);
            return menu.Show();
        }
    }
}
