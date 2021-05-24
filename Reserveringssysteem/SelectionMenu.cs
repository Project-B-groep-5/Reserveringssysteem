using System;

namespace Reserveringssysteem
{
    class SelectionMenu
    {
        private readonly string[] _menuArray;
        private readonly string _menuLogo;
        private readonly string _menuTitle;

        public SelectionMenu(string[] array, string logo, string title = "\nKies een optie\n")
        {
            _menuArray = array;
            _menuLogo = logo;
            _menuTitle = title;
        }

        public int Show()
        {
            var optionsCount = _menuArray.Length;
            var optionSelected = 0;
            var done = false;
            Console.CursorVisible = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(_menuLogo);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(_menuTitle);
            Console.ResetColor();
            while (!done)
            {
                for (int i = 0; i < optionsCount; i++)
                {
                    if (optionSelected == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.WriteLine(_menuArray[i] + ' ');
                    Console.ResetColor();
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        optionSelected = Math.Max(0, optionSelected - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        optionSelected = Math.Min(optionsCount - 1, optionSelected + 1);
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                }
                Console.CursorTop = _menuLogo.Split('\n').Length + _menuTitle.Split('\n').Length;
            }
            Console.Clear();
            return optionSelected;
        }

        public static void Make(string[] titles, Action[] actions, string logo, string title = "\nKies een optie\n")
        {
            var menu = new SelectionMenu(titles, logo, title);
            var action = menu.Show();
            if (actions[action] != null)
            {
                actions[action]();
            }
        }
    }
}
