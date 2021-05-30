using System;

namespace Reserveringssysteem
{
    class SelectionMenu
    {
        private readonly string[] _menuArray;
        private readonly string _menuLogo;
        private readonly string _menuTitle;

        public SelectionMenu(string[] array, string logo, string title = "Kies een optie:\n")
        {
            _menuArray = array;
            _menuLogo = logo;
            _menuTitle = title;
        }

        public int Show()
        {
            var optionSelected = 0;
            Logo.PrintLogo(_menuLogo);
            Console.WriteLine(_menuTitle);
            Console.ResetColor();
            while (true)
            {
                for (int i = 0; i < _menuArray.Length; i++)
                {
                    if (optionSelected == i)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
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
                        optionSelected = Math.Min(_menuArray.Length - 1, optionSelected + 1);
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        while (Console.KeyAvailable)
                            Console.ReadKey(true);
                        return optionSelected;
                }
                Console.CursorTop = _menuLogo.Split('\n').Length + _menuTitle.Split('\n').Length + 1;
            }

        }

        public static void Make(string[] titles, Action[] actions, string logo, string title = "Kies een optie:\n")
        {
            var menu = new SelectionMenu(titles, logo, title);
            var action = menu.Show();
            actions[action]?.Invoke();
        }
    }
}
