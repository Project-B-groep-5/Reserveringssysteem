using System;
using System.IO;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class EmployeeActions
    {
		public static void MainMenu()
        {
			SelectionMenu.Make(new[] { "Reserveringen weergeven", "Gegevens wijzigen", "Terug" }, new Action[] { OverviewReservations.Overview, ChangeMenu, Program.Main}, Logo.Dashboard);
        }
        private static void ChangeMenu()
        {
			SelectionMenu.Make(new[] { "Restaurant gegevens", "Tafels", "Gerechten / menus", "Wachtwoord \n", "Terug" }, new Action[] { ChangeRestaurantInfo, Table.TableManager, null, ChangePassword, MainMenu }, Logo.Dashboard, "\nKies een optie om te wijzigen\n");
        }

        private static void ChangeRestaurantInfo()
        {
            Serialize(new Restaurant("De houten vork", new Location("Wijhaven", "107", "3011 WN", "Rotterdam"), 100, new string[] { "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "Gesloten" }, new string[] { "oscar.vugt@gmail.com", "06-12932305"}), "restaurant.json");
        }

        public static void ChangePassword()
        {
			string password;
			while (true)
			{
				Logo.PrintLogo(Logo.Wachtwoord);
				Console.CursorVisible = true;
				Console.WriteLine("Nieuw wachtwoord:");
				var newPassword = Console.ReadLine();
				Logo.PrintLogo(Logo.Wachtwoord);
				Console.WriteLine("Herhaal het nieuwe wachtwoord:");
				if (Console.ReadLine() == newPassword)
				{
					password = newPassword;
					break;
				}
				else
				{
					Logo.PrintLogo(Logo.Wachtwoord);
					Console.WriteLine("De ingevoerde wachtwoorden komen niet overeen.");
					Utils.Enter("om opnieuw te proberen");
				}
			}
			File.WriteAllText("password.txt", password);
		}
    }
}
