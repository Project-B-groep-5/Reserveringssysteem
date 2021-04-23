using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Reserveringssysteem.Json;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace Reserveringssysteem
{
    public class DishFilter
    {
        public static List<Dish> DishList;

        private string Show(int i)
        {
            string result = "";
            result += "\nNaam: " + DishList[i].Name + "\n" + "Prijs: " + DishList[i].Price + "\n";
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

        public void Search(string keyWord)
        {
            DishList = JsonConvert.DeserializeObject<List<Dish>>(File.ReadAllText("dishes.json"));
            string result = "";
            bool check = true;

            for (int i = 0; i < DishList.Count; i++)
            {
                check = true;
                if (DishList[i].Name.ToLower().Contains(keyWord.ToLower()) || DishList[i].Type.ToLower().Contains(keyWord.ToLower()))
                {
                    result += Show(i);
                    check = false;
                }
                if (check)
                {
                    for (int j = 0; j < DishList[i].Tags.Length; j++)
                    {
                        if (DishList[i].Tags[j].ToLower().Contains(keyWord.ToLower()))
                        {
                            result += Show(i);
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
                            result += Show(i);
                            check = false;
                            break;
                        }
                    }
                }
            }

            if (result.Length > 0)
            {
                Console.WriteLine();
            }
            else
                Console.WriteLine("Niks gevonden.");
            Console.WriteLine(result);
        }
    }
}
