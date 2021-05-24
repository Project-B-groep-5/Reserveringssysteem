using System;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class DishFilter
    {
        public void Search(string keyWord)
        {
            string result = "";
            for (int i = 0; i < DishList.Count; i++)
            {
                bool check = true;
                if (DishList[i].Name.ToLower().Contains(keyWord.ToLower()) || DishList[i].Type.ToLower().Contains(keyWord.ToLower()))
                {
                    result += MenuShow.Show(i);
                    check = false;
                }
                if (check)
                {
                    for (int j = 0; j < DishList[i].Tags.Length; j++)
                    {
                        if (DishList[i].Tags[j].ToLower().Contains(keyWord.ToLower()))
                        {
                            result += MenuShow.Show(i);
                            check = false;
                            break;
                        }
                    }
                }
                if (check)
                {
                    for (int j = 0; j < DishList[i].Ingredients.Length; j++)
                    {
                        if (DishList[i].Ingredients[j].ToLower().Contains(keyWord.ToLower()))
                        {
                            result += MenuShow.Show(i);
                            break;
                        }
                    }
                }
            }

            if (result.Length > 0)
            {
                Console.WriteLine(result);
            }
            else
                Console.WriteLine("Niks gevonden.");
            Utils.Enter(Program.Main);
        }

        public static void F()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.MenuKaart);
            Console.ResetColor();
            Console.CursorVisible = true;
            Console.WriteLine("\nVoer een term in: \n");
            string keyWord = Console.ReadLine();
            var dishFilter = new DishFilter();
            dishFilter.Search(keyWord);
        }
        public static void voorgerechten()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.MenuKaart);
            Console.ResetColor();
            Console.CursorVisible = true;
            var dishFilter = new DishFilter();
            dishFilter.Search("voorgerechten");
        }
        public static void hoofdgerechten()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.MenuKaart);
            Console.ResetColor();
            Console.CursorVisible = true;
            var dishFilter = new DishFilter();
            dishFilter.Search("hoofdgerechten");
        }
        public static void nagerechten()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.MenuKaart);
            Console.ResetColor();
            Console.CursorVisible = true;
            var dishFilter = new DishFilter();
            dishFilter.Search("nagerechten");
        }
        public static void dranken()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.MenuKaart);
            Console.ResetColor();
            Console.CursorVisible = true;
            var dishFilter = new DishFilter();
            dishFilter.Search("dranken");
        }
    }
}
