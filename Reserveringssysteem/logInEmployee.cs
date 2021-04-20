using System;
using System.IO;

namespace Reserveringssysteem
{
	public class LogInEmployee
    {

		public static void LogIn()
        {
			string password = File.ReadAllText("password.txt");
			Console.WriteLine("Voer wachtwoord in:");
			string ingevoerdWachtwoord = Console.ReadLine();
			Console.Clear();
			if (ingevoerdWachtwoord == password)
            {
				Console.WriteLine("Naar medewerkers omgeving.");
            }
			else 
            {
				Console.WriteLine("Probeer opnieuw.");
				LogIn();
            }

        }
		

	}
	
}
	