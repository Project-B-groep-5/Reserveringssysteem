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
				EmployeeActions.ChangePassword();
				password = File.ReadAllText("password.txt");
			}

			Header();
			Console.WriteLine("Voer wachtwoord in:");
			string ingevoerdWachtwoord = Console.ReadLine();

			if (ingevoerdWachtwoord == password)
				EmployeeActions.Menu();
			else 
            {
				Header();
				Console.WriteLine("Incorrect wachtwoord.");
				Utils.Enter("om opnieuw te proberen");
				LogIn();
            }

        }
		

	}
	
}
	