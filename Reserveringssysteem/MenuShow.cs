using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class MenuShow
    {
        public static string Show(int i)
        {
            string result = "";
            result += "\nNaam: " + DishList[i].Name + "\n" + "Prijs: " + DishList[i].Price.ToString("0.00") + " euro\n";
            int check = 0;
            if (DishList[i].Ingredients.Count != 0)
            {
                result += "Ingredienten: ";
                foreach (string ingredient in DishList[i].Ingredients)
                {
                    if (check == DishList[i].Ingredients.Count - 1)
                        result += ingredient;
                    else
                        result += ingredient + ", ";
                    check++;
                }
                result += "\n";
            }
            result += "Type: " + DishList[i].Type + "\n";
            result += "__________________________________________________________\n";
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
            string result = "";
            for (int i = 0; i < VoordeelMenus.Count; i++)
            {
                result += VoordeelMenus[i].Name + ":\n\nVoorgerecht: " + VoordeelMenus[i].VoorGerecht.Name + "\nHoofdgerecht: " + VoordeelMenus[i].HoofdGerecht.Name + "\nNagerecht: " + VoordeelMenus[i].NaGerecht.Name + "\nPrijs: " + VoordeelMenus[i].Prijs.ToString("0.00") + " euro\n";
                result += "__________________________________________________________\n";
            }
            return result;
        }
    }
}
