using System;
using System.IO;

namespace Reserveringssysteem
{
	public class LogInEmployee
    {

		public static void LogIn()
        {
			Console.CursorVisible = true;
			string password = "";
            try
			{
				password = File.ReadAllText("password.txt");
			} 
			catch (FileNotFoundException)
            {
				while (password == "")
				{
					Console.WriteLine("Er is nog geen wachtwoord aangemaakt.\nNieuw wachtwoord:");
					var newPassword = Console.ReadLine();
					Console.Clear();
					Console.WriteLine("Herhaal het nieuwe wachtwoord:");
					if (Console.ReadLine() == newPassword)
						password = newPassword;
					else
					{
						Console.Clear();
						Console.WriteLine("De ingevoerde wachtwoorden komen niet overeen.");
						Console.ReadLine();
					}
					Console.Clear();
				}
				File.WriteAllText("password.txt", password);

            }
			Console.WriteLine("Voer wachtwoord in:");
			string ingevoerdWachtwoord = Console.ReadLine();
			Console.Clear();
			if (ingevoerdWachtwoord == password)
            {
				Console.CursorVisible = false;
				Console.WriteLine("Naar medewerkers omgeving.");
				EmployeeActions.ChangeRestaurantInfo();
				Utils.EnterTerug();
            }
			else 
            {
				Console.WriteLine("Probeer opnieuw.");
				LogIn();
            }

        }
		

	}
	
}
	