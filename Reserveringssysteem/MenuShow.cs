using System;
using System.Collections.Generic;
using System.Text;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class MenuShow
    {
        private static List<Dish> DishList = Deserialize<List<Dish>>("dishes.json");

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
            string result = "";
            for (int i = 0; i < DishList.Count; i++)
            {
                result += Show(i);
            }
            return result;
        }

        public static string VoordeelMenuShow()
        {
            var VoordeelMenu = Deserialize<List<VoordeelMenu>>("voordeelmenu.json");
            string result = "";
            for (int i = 0; i < VoordeelMenu.Count; i++)
            {
                result += VoordeelMenu[i].Name + ":\n\nVoorgerecht: " + VoordeelMenu[i].VoorGerecht.Name + "\nHoofdgerecht: " + VoordeelMenu[i].HoofdGerecht.Name + "\nNagerecht: " + VoordeelMenu[i].NaGerecht.Name + "\nPrijs: " + VoordeelMenu[i].Prijs.ToString("0.00") + " euro\n";
                result += "______________________________________________________\n";
            }
            return result;
        }
    }
}
