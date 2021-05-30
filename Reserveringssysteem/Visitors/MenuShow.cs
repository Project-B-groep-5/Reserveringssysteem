using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class MenuShow
    {
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
