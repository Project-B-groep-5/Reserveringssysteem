using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    class ReservationMenu
    {
        private string[] menuArray;

        public ReservationMenu(string[] array)
        {
            menuArray = array;
        }

        public int Show()
        {
            var optionsCount = menuArray.Length;
            var optionSelected = 0;
            var done = false;
            var state = "Reservating" ; 
            var menuArrow = "-> ";
            var logo = @"
    ____                                               
   / __ \___  ________  ______   _____  ________  ____ 
  / /_/ / _ \/ ___/ _ \/ ___/ | / / _ \/ ___/ _ \/ __ \
 / _, _/  __(__  )  __/ /   | |/ /  __/ /  /  __/ / / /
/_/ |_|\___/____/\___/_/    |___/\___/_/   \___/_/ /_/ 
                                                      ";

            if (state == "Reservating")
                Console.WriteLine(logo + "\nKies een optie\n");
            else if (state == "reserveren")
                Console.WriteLine(logo + "\nHoe laat wilt u komen eten?\n");

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
