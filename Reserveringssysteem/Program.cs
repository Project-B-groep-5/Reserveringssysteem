using System;

namespace Reserveringssysteem
{
    class Program
    {
        static void Main(string[] args)
        {
            int optionsCount = 4;
            int optionSelected = 0;
            bool done = false;
            string[] menuArray = new string[4]{"1] Plaats een reservering","2] Bekijk het menukaart","3] Informatie over ons","[Voor Medewerkers]"}; 
            string menuArrow = "-> ";
            string logo = @"
 _____           _           _     ____  
|  __ \         (_)         | |   |  _ \ 
| |__) | __ ___  _  ___  ___| |_  | |_) |
|  ___/ '__/ _ \| |/ _ \/ __| __| |  _ < 
| |   | | | (_) | |  __/ (__| |_  | |_) |
|_|   |_|  \___/| |\___|\___|\__| |____/ 
               _/ |                      
              |__/   Console v0.1";

            Console.WriteLine(logo + "\n");
            while (!done)
            {
                for (int i = 0; i < optionsCount; i++)
                {
                    if (optionSelected == i)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(menuArrow);
                    } else
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
                if (!done)
                {
                    Console.CursorTop = Console.CursorTop - optionsCount;
                }
                //switch
            }
         }
    }
}