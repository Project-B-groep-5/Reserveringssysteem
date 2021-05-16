using System;
using System.IO;

namespace Reserveringssysteem
{
	public class LogInEmployee
    {
		private static void Header() => Logo.PrintLogo(Logo.Inloggen);

		public static void LogIn()
        {
			Console.CursorVisible = true;
			string password;
            try
			{
				password = File.ReadAllText("password.txt");
			} 
			catch (FileNotFoundException)
            {
				while (true)
				{
					Header();
					Console.CursorVisible = true;
					Console.WriteLine("Er is nog geen wachtwoord aangemaakt.\nNieuw wachtwoord:");
					var newPassword = Console.ReadLine();
					Header();
					Console.WriteLine("Herhaal het nieuwe wachtwoord:");
					if (Console.ReadLine() == newPassword)
					{
						password = newPassword;
						break;
					}
					else
					{
						Header();
						Console.WriteLine("De ingevoerde wachtwoorden komen niet overeen.");
						Utils.EnterTerug();
					}
				}
				File.WriteAllText("password.txt", password);

            }

			Header();
			Console.WriteLine("Voer wachtwoord in:");
			string ingevoerdWachtwoord = Console.ReadLine();

			if (ingevoerdWachtwoord == password)
				EmployeeActions.Menu();
			else 
            {
				Header();
				Console.WriteLine("Probeer opnieuw.");
				Utils.EnterTerug();
				LogIn();
            }

        }
		

	}
	
}
	