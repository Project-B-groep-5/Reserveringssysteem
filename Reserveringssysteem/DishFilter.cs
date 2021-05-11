using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class DishFilter
    {
        public static List<Dish> DishList;

        public void Search(string keyWord)
        {
            DishList = Deserialize<List<Dish>>("dishes.json");
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
                Console.WriteLine();
            }
            else
                Console.WriteLine("Niks gevonden.");
            Console.WriteLine(result);
        }
    }
}
