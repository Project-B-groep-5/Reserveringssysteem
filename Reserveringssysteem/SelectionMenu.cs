using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    class SelectionMenu
    {
        private string[] menuArray;
        private string menuLogo;
        private string menuTitle;

        public SelectionMenu(string[] array, string logo, string title)
        {
            menuArray = array;
            menuLogo = logo;
            menuTitle = title;
        }

        public int Show()
        {
            var optionsCount = menuArray.Length;
            var optionSelected = 0;
            var done = false;
            var menuArrow = "-> ";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(menuLogo);
            Console.ResetColor();
            Console.WriteLine(menuTitle);
            while (!done)
            {
                for (int i = 0; i < optionsCount; i++)
                {
                    if (optionSelected == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.WriteLine(menuArray[i] + ' ');
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
                Console.CursorTop = Console.CursorTop - optionsCount;
            }
            Console.Clear();
            return optionSelected;
        }

    }
}
