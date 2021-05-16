using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class ShowAllDishes
    {
        public static List<Dish> DishList;

        public void Search()
        {
            string[] cat = new string[] { "Voorgerecht", "Hoofdgerecht", "Nagerecht", "Koude dranken", "Warme dranken", "Alcoholische dranken" };
            DishList = Deserialize<List<Dish>>("dishes.json");
            string result = "";
            for (int y = 0; y < 6; y++)                          // y = 0 : Voorgerecht && y = 5 : Warme dranken
            {
                for (int i = 0; i < DishList.Count; i++)         // Checkt elk item in de dishes.json
                {
                    if (DishList[i].Type.Contains(cat[y]))      // Als item[i] het type van cat[y] heeft : 
                    {
                        result += MenuShow.Show(i);
                    }
                }
                if (result.Length > 0)
                {
                    Console.WriteLine($"\n__________________________________________________________\n{cat[y]}\n__________________________________________________________\n {result}\n\n"); // Categorie eerst, daarna resultaat
                    result = "";
                }
                else
                {
                    Console.WriteLine($"{cat[y]} Heeft geen resultaten. --> Niks gevonden. De JSON heeft geen items.");
                }
            }
                Utils.EnterTerug();
                }

        public static void F()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.MenuKaart);
            Console.ResetColor();
            Console.CursorVisible = true;
            var ShowAllDishes = new ShowAllDishes();
            ShowAllDishes.Search();
        }
    }
}
