using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    class DishMenu
    {
        private string[] menuArray;

        public DishMenu(string[] array)
        {
            menuArray = array;
        }

        public int Show()
        {
            var optionsCount = menuArray.Length;
            var optionSelected = 0;
            var done = false;
            var state = "Reservating";
            var menuArrow = "-> ";
            var logo = @"
  __  __                  _                    _   
 |  \/  | ___ _ __  _   _| | ____ _  __ _ _ __| |_ 
 | |\/| |/ _ \ '_ \| | | | |/ / _` |/ _` | '__| __|
 | |  | |  __/ | | | |_| |   < (_| | (_| | |  | |_ 
 |_|  |_|\___|_| |_|\__,_|_|\_\__,_|\__,_|_|   \__|";

            if (state == "Reservating")
                Console.WriteLine(logo + "\n\nKies een optie\n");

            while (!done)
            {
                for (int i = 0; i < optionsCount; i++)
                {
                    if (optionSelected == i)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(menuArrow);
                    }
                    else
                    {
                        Console.Write("   "); //length of the menuArrow in spaces
                    }
                    Console.WriteLine(menuArray[i]);
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
