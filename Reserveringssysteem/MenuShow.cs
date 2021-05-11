using System;
using System.Collections.Generic;
using System.Text;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class MenuShow
    {
        public static List<Dish> DishList;

        public static string Show(int i)
        {
            string result = "";
            result += "\nNaam: " + DishList[i].Name + "\n" + "Prijs: " + DishList[i].Price.ToString("0.00") + " euro\n";
            int check = 0;
            if (DishList[i].Ingredients.Length != 0)
            {
                result += "Ingredienten: ";
                foreach (string ingredient in DishList[i].Ingredients)
                {
                    if (check == DishList[i].Ingredients.Length - 1)
                        result += ingredient;
                    else
                        result += ingredient + ", ";
                    check++;
                }
                result += "\n";
            }
            result += "Type: " + DishList[i].Type + "\n";
            result += "______________________________________\n";
            return result;
        }

        public static string CompleteMenuShow()
        {
            DishList = Deserialize<List<Dish>>("dishes.json");
            string result = "";
            for (int i = 0; i < DishList.Count; i++)
            {
                result += Show(i);
            }
            return result;
        }
    }
}
