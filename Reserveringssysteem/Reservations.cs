using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Reservations
    {
        public static List<VoordeelMenu> VoordeelMenus;
        public static void ReservateTitle() // Call deze method om de onderstaande header te krijgen
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Logo.Reserveren + "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Reservate()
        {
            string name = GetName();
            int amountPeople = GetAmountOfPeople();
            string date = GetDate();

            string[] times = new[] { "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00" };
            string time = times[new SelectionMenu(times, Logo.Reserveren, "\nHoe laat wilt u komen eten?\n").Show()];

            string[] choices = GetDiscountMenus(amountPeople);
            string comments = AskForComments();

            Reservation reservation = new Reservation { Name = name, Date = date, Time = time, Size = amountPeople, DiscountMenus = choices, Comments = comments};

            SendEmail(reservation.ReservationId, name, time, date);

            ReservateTitle();
            Console.WriteLine($"Je hebt een reservering gemaakt op: {date} om {time} uur!\nJe reserveringscode is: {reservation.ReservationId}");
            reservation.Save();

            Utils.EnterTerug();
        }


        private static string GetName()
        {
            ReservateTitle();
            Console.CursorVisible = true;
            Console.WriteLine("Wat is uw naam?");
            string name = Console.ReadLine();
            while (true)
            {

                if (name.Length == 0)
                {
                    Console.Clear();
                    ReservateTitle();
                    Console.WriteLine("Geen naam ingevuld. Probeer opnieuw : \n");
                    name = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    break;
                }

            }
            Console.CursorVisible = false;
            return name;
        }
        private static int GetAmountOfPeople()
        {
            int amountPeople = 0;
            ReservateTitle();
            Console.CursorVisible = true;
            Console.WriteLine("Voor hoeveel mensen wilt u een reservering maken?");
            while (amountPeople <= 0)
            {
                var input = Console.ReadLine();
                try
                {
                    amountPeople = int.Parse(input);
                }
                catch
                {
                    amountPeople = 0;
                }
                if (amountPeople <= 0)
                {
                    ReservateTitle();
                    Console.WriteLine($"Je moet voor minimaal één persoon reserveren.\n\nVoor hoeveel mensen wilt u een reservering maken?\n");
                }
            }
            Console.CursorVisible = false;
            return amountPeople;
        }

        private static string GetDate()
        {
            var datumVandaag = DateTime.Today.ToString("dd-MM-yyyy");
            ReservateTitle();
            Console.CursorVisible = true;
            Console.WriteLine("Voor wanneer wilt u reserveren?");
            Console.WriteLine("Gebruik alstublieft het format: dd-MM-jjjj");
            Console.WriteLine($"\nDe datum van vandaag is {datumVandaag}");
            var date = Console.ReadLine();
            while (true)
            {
                if (!DateTime.TryParse(date, out DateTime dDate) || DateTime.Parse(date) < DateTime.Parse(datumVandaag))
                {
                    ReservateTitle();
                    Console.WriteLine("Opgegeven datum is niet gelijk aan het format of in het verleden. Het format is : dd-MM-jjjj");
                    date = Console.ReadLine();
                }
                else
                {
                    Console.CursorVisible = false;
                    return string.Format("{0:dd/MM/yyyy}", dDate);
                }
            }
        }

        public static string[] GetDiscountMenus(int amountPeople)
        {
            VoordeelMenus = Deserialize<List<VoordeelMenu>>("voordeelmenu.json");
            string[] Menus = new string[VoordeelMenus.Count + 1];
            for (int i = 0; i < VoordeelMenus.Count; i++)
            {
                Menus[i] = VoordeelMenus[i].Name;
            }
            Menus[^1] = "Geen voordeelmenu";
            var voordeelMenuKeuze = new SelectionMenu(new string[3] { "Voordeelmenu bekijken", "Voordeelmenu kiezen", "Geen voordeelmenu" }, Logo.Reserveren, "\nKies of u een voordeel menu neemt of bekijk het voordeel menu\n");
            string[] choices;
            while (true)
            {
                switch (voordeelMenuKeuze.Show())
                {
                    case 0:
                        ReservateTitle();
                        Console.WriteLine(MenuShow.VoordeelMenuShow());
                        Utils.EnterTerug();
                        break;
                    case 1:
                        choices = new string[amountPeople];
                        for (int i = 0; i < amountPeople; i++)
                        {
                            var voordeelKeuzes = new SelectionMenu(Menus, Logo.Reserveren, $"\nKies voor persoon { i + 1 } het voordeelmenu\n");
                            var keuze = voordeelKeuzes.Show();
                            choices[i] = Menus[keuze];
                        }
                        bool emptyChoice = true;
                        foreach (var str in choices)
                        {
                            if (str != "Geen voordeelmenu")
                                emptyChoice = false;
                            break;
                        }
                        if (emptyChoice)
                            choices = null;
                        return choices;
                    case 2:
                        return null;
                }
            }
        }
        public static string AskForComments()
        {
            ReservateTitle();
            var OpmerkingenMenuKeuze = new SelectionMenu(new string[2] { "Ja", "Nee" }, Logo.Reserveren, "\nHeeft u nog opmerkingen voor het restaurant of over de reservering?: \n");
            while (true)
            {
                switch (OpmerkingenMenuKeuze.Show())
                {
                    case 0:
                        ReservateTitle();
                        Console.WriteLine("Welke opmerking(en) wilt u nog geven aan het restaurant: \n");
                        var comment = Console.ReadLine();
                        return comment;
                    case 1:
                        return comment = "";
                }
            }
        }

        public static void SendEmail(string reservationCode, string name, string time, string date) // Method om de mail te sturen.
        {
            ReservateTitle();
            Console.CursorVisible = true;
            Console.WriteLine("Om uw reservering te bevestigen hebben wij uw mail adres nodig.\nNaar welk mail adres mogen wij de reservering sturen? : \n");
            var emailAddress = Console.ReadLine();
            while (!Utils.IsValidEmail(emailAddress))
            {
                ReservateTitle();
                Console.WriteLine($"{emailAddress} is geen geldig mail adres.\nNaar welk mail adres mogen wij de reservering sturen?:\n");
                emailAddress = Console.ReadLine();
            }
            Console.CursorVisible = false;
            Console.WriteLine("De bevestigingsmail wordt nu verstuurd. Sluit dit menu nog niet af......");
            string mailMessage = @$"Beste {name},

Hartelijk dank voor uw reservering bij Restaurant de Houten Vork op {date} om {time}.
Uw reserveringscode is : {reservationCode} , bewaar deze code goed. 

Tot dan!
                                ";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("RestaurantProjectB@gmail.com", "NieuwWachtwoord1337"),
                EnableSsl = true,
            };

            smtpClient.Send("RestaurantProjectB@gmail.com", emailAddress, "Uw reservering is bevestigd!", mailMessage);
            Console.Clear();
            Console.WriteLine("Bevestiginsmail verstuurd. Vergeet niet uw spamfolder te bekijken als u geen bevestigingsmail heeft gehad.\n");
        }
    }
}
