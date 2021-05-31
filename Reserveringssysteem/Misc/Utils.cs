using System;

namespace Reserveringssysteem
{
    static class Utils
    {
        public static void Enter(string msg = "om terug te gaan")
        {
            Enter(null, msg);
        }
        public static void Enter(Action action, string msg = "om terug te gaan")
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nDruk op 'enter' {msg}.");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();
            action?.Invoke();
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

        public static bool Confirm(string logo, string message)
        {
            var menu = new SelectionMenu(new[] { "Ja", "Nee" }, logo, message);
            return menu.Show() == 0;
        }

        public static string Input(string message)
        {
            if (message != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(message);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = true;
            return Console.ReadLine();
        }
        public static string Input()
        {
            return Input(null);
        }

        public static void NoInput(Action over, Action back, string logo)
        {
            SelectionMenu.Make(new[] { "Opnieuw proberen", "Terug" }, new Action[] { over, back }, logo, "Niks ingevuld.\n\nKies een optie:\n");
        }
    }
}
