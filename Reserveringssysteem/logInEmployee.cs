using System;
using System.IO:
namespace Reserveringssysteem
{
	class logInEmployee
    {

		public static void Main()
        {
			string password = File.ReadAllText("password.txt");
			Console.WriteLine("Voer wachtwoord in:");
			string ingevoerdWachtwoord = Console.ReadLine();
			Console.Clear();
			if (ingevoerdWachtwoord == password)
            {
				Console.WriteLine("Naar medewerkers omgeving.");
            }
			else if (ingevoerdWachtwoord != password)
            {
				Console.WriteLine("Probeer opnieuw.");
				Main();
            }

        }
		

	}
	
}
	