using System;
using static Reserveringssysteem.Json;

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
        public static int Show(int person)
        {
            var optionSelected = 0;
            Logo.PrintLogo(Logo.Reserveren);
            for (int i = 0; i < VoordeelMenus.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(VoordeelMenus[i].Name + ":\n");
                Console.Write("Voorgerecht: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(VoordeelMenus[i].VoorGerecht.Name);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Hoofdgerecht: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(VoordeelMenus[i].HoofdGerecht.Name);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Nagerecht: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(VoordeelMenus[i].NaGerecht.Name);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Prijs: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"€{VoordeelMenus[i].Prijs:0.00}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("__________________________________________________________\n");
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nKies een voordeelmenu voor persoon {person}:\n");
            string[] Menus = new string[VoordeelMenus.Count + 1];
            for (int i = 0; i < VoordeelMenus.Count; i++)
            {
                Menus[i] = VoordeelMenus[i].Name;
            }
            Menus[^1] = "Geen voordeelmenu";
            while (true)
            {
                for (int i = 0; i < Menus.Length; i++)
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
                    Console.WriteLine(Menus[i] + ' ');
                    Console.ResetColor();
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        optionSelected = Math.Max(0, optionSelected - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        optionSelected = Math.Min(Menus.Length - 1, optionSelected + 1);
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        while (Console.KeyAvailable)
                            Console.ReadKey(true);
                        return optionSelected;
                }
                Console.CursorTop = Console.CursorTop - Menus.Length;
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
