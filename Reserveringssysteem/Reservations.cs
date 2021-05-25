using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public static class Reservations
    {
        private static void ReservateTitle() => Logo.PrintLogo(Logo.Reserveren);
        public static void Reservate()
        {
            string name = GetName();
            int amountPeople = GetAmountOfPeople();
            string date = GetDate();

            string[] times = GetTimes(date, amountPeople);
            if (times == null)
            {
                var optionMenu = new SelectionMenu(new string[2] { "Opnieuw proberen", "Stoppen" }, Logo.Reserveren, "\nHet restaurant is gesloten op de door uw gekozen datum, wat wilt u doen?\n");
                switch (optionMenu.Show())
                {
                    case 0:
                        Reservate();
                        return;
                    case 1:
                        return;

                }
            }
            else if (times.Length == 0)
            {
                var optionMenu = new SelectionMenu(new string[2] { "Opnieuw proberen", "Stoppen" }, Logo.Reserveren, "\nHet restaurant zit vol op de door uw gekozen datum, wat wilt u doen?\n");
                switch (optionMenu.Show())
                {
                    case 0:
                        Reservate();
                        return;
                    case 1:
                        return;

                }
            }
            int time = new SelectionMenu(times, Logo.Reserveren, "\nHoe laat wilt u komen eten?\n").Show();
            List<string> timeSlot = new List<string>();
            for (int i = time; i <= time + 3 && i < times.Length; i++)
                timeSlot.Add(times[i]);

            string[] choices = GetDiscountMenus(amountPeople);
            if (choices != null) FakePayment(choices);
            string comments = AskForComments();
            
            Reservation reservation = new Reservation { Name = name, Date = date, TimeSlot = timeSlot.ToArray(), Size = amountPeople, DiscountMenus = choices, Comments = comments};
            
            SendEmail(reservation.ReservationId, name, times[time], date);
            
            Console.WriteLine($"U heeft een reservering gemaakt op: {date} om {time} uur!\nUw reserveringscode is: {reservation.ReservationId}");
            reservation.Save();

            Utils.Enter(Program.Main);
        }
        private static void FakePayment(string[] choices)
        {
            double price = 0.0;
            foreach (string menu in choices)
            {
                for (int i = 0; i < VoordeelMenus.Count; i++)
                {
                    if (menu == VoordeelMenus[i].Name)
                    {
                        price += VoordeelMenus[i].Prijs;
                        break;
                    }
                }
            }
            string[] betaalMethodes = new string[5] { "iDEAL", "MasterCard", "Bancontact", "Paypal", "Afterpay" };
            string[] check = new string[2] { "Ja", "Nee" };
            string[] check2 = new string[2] { "Doorgaan", "Terug" };
            while (true)
            {
                var betaalKeuze = new SelectionMenu(betaalMethodes, Logo.Reserveren, "\nKies uw gewenste betaalmethode\n");
                int betaalMethode = betaalKeuze.Show();
                Console.WriteLine($"U heeft gekozen voor {betaalMethodes[betaalMethode]}.");

                var correcteBetaalMethode = new SelectionMenu(check, Logo.Reserveren, $"\nU heeft gekozen voor {betaalMethodes[betaalMethode]}.\nKlopt dit?\n");
                int betaalCheck = correcteBetaalMethode.Show();
                if (check[betaalCheck] == "Nee") continue;
                ReservateTitle();
                if (betaalMethodes[betaalMethode] == "Afterpay") Console.WriteLine("U kunt achteraf betalen");
                else
                {
                    var doorgaan = new SelectionMenu(check2, Logo.Reserveren, $"U betaald {price.ToString("0.00")} euro via {betaalMethodes[betaalMethode]}.\n");
                    int index = doorgaan.Show();
                    if (check2[index] == "Terug") continue;
                    ReservateTitle();
                    Console.WriteLine("U heeft succesvol betaald!");
                }
                Console.WriteLine("\nKlik  op 'enter' om door te gaan.");
                Console.Read();
                break;
            }
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
                    Console.WriteLine($"U moet voor minimaal één persoon reserveren door het invullen van een getal.\n\nVoor hoeveel mensen wilt u een reservering maken?\n");
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
        private static bool Check(string date, string time, int size)
        {
            int capacity = Json.Restaurant.Capacity;
            int occupied = 0;
            occupied += size;
            for (int i = 0; i < ReservationList.Count; i++)
            {
                if (ReservationList[i].Date == date && ReservationList[i].TimeSlot.Any(time.Contains))
                    occupied += ReservationList[i].Size;
            }
            if (occupied > capacity) return false;
            return true;
        }
        private static string[] GetTimes(string date, int size)
        {
            DateTime.TryParse(date, out DateTime dDate);
            var day = (int)dDate.DayOfWeek -1;
            var times = new List<string>();
            try
            {
                for (int i = int.Parse(Json.Restaurant.OpeningHours[day].Split(":")[0]); i < int.Parse(Json.Restaurant.OpeningHours[day].Split("-")[1].Split(":")[0]) - 1; i++)
                    for (int j = 0; j <= 30; j += 30)
                        if(Check(date, $"{i}:{j:00}", size))
                            times.Add($"{i}:{j:00}");
                return times.ToArray();
            }
            catch { return null; }
        }
        private static string[] GetDiscountMenus(int amountPeople)
        {
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
                        Utils.Enter();
                        break;
                    case 1:
                        choices = new string[amountPeople];
                        for (int i = 0; i < amountPeople; i++)
                        {
                            var voordeelKeuzes = new SelectionMenu(Menus, Logo.Reserveren, $"\nKies voor persoon { i + 1 } het voordeelmenu\n");
                            var keuze = voordeelKeuzes.Show();
                            choices[i] = Menus[keuze];
                        }
                        ReservateTitle();
                        string gekozen = "U heeft gekozen voor:\n";
                        foreach (var menu in Menus)
                        {
                            int count = 0;
                            for (int i = 0; i < choices.Length; i++)
                            {
                                if (menu == choices[i])
                                    count++;
                            }
                            if (count > 0)
                                gekozen += $"\n{menu} {count}x";
                        }

                        string[] keuzeCheckArr = new string[2] { "Ja", "Nee" };
                        var voordeelCheck = new SelectionMenu(keuzeCheckArr, Logo.Reserveren, $"\n{gekozen}\n\nKlopt dit?\n");
                        var keuzeCheck = voordeelCheck.Show();

                        if (keuzeCheckArr[keuzeCheck] == "Nee") continue;

                        bool emptyChoice = true;
                        foreach (var str in choices)
                        {
                            if (str != "Geen voordeelmenu")
                            {
                                emptyChoice = false;
                                break;                               
                            }
                        }
                        if (emptyChoice) choices = null;
                        return choices;
                    case 2:
                        string[] keuzeCheckArr2 = new string[2] { "Ja", "Nee" };
                        var voordeelCheck2 = new SelectionMenu(keuzeCheckArr2, Logo.Reserveren, "\nWeet u zeker dat u geen voordeelmenu wilt kiezen?\n");
                        var keuzeCheck2 = voordeelCheck2.Show();
                        if (keuzeCheckArr2[keuzeCheck2] == "Nee") continue;
                        return null;
                }
            }
        }
        private static string AskForComments()
        {
            ReservateTitle();
            var OpmerkingenMenuKeuze = new SelectionMenu(new string[2] { "Ja", "Nee" }, Logo.Reserveren, "\nHeeft u nog opmerkingen voor het restaurant of over de reservering?: \n");
            if (OpmerkingenMenuKeuze.Show() == 0)
            {
                ReservateTitle();
                Console.WriteLine("Welke opmerking(en) wilt u nog geven aan het restaurant: \n");
                var comment = Console.ReadLine();
                return comment;
            }
            return null;
        }
        private static void SendEmail(string reservationCode, string name, string time, string date) // Method om de mail te sturen.
        {
            ReservateTitle();
            Console.CursorVisible = true;
            Console.WriteLine("Om uw reservering te bevestigen hebben wij uw mail adres nodig.\nNaar welk mail adres mogen wij de reservering sturen?: ");
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
                Credentials = new NetworkCredential("RestaurantProjectB@gmail.com", Secrets.Password),
                EnableSsl = true,
            };

            smtpClient.Send("RestaurantProjectB@gmail.com", emailAddress, "Uw reservering is bevestigd!", mailMessage);
            
            ReservateTitle();
            Console.WriteLine("Bevestigingsmail verstuurd. Vergeet niet uw spamfolder te bekijken als u geen bevestigingsmail heeft gehad.\n");
        }
        public static void CancelReservation()
        {
            if (ReservationList.Count <= 0)
            {
                Console.Clear();
                CancelTitle();
                Console.WriteLine("\nGeen reserveringen gevonden in het systeem.");
                Utils.Enter();
                return;
            }
            Console.CursorVisible = true;
            Console.Clear();
            CancelTitle();
            Console.WriteLine("\nVoer uw reserveringscode in of druk 'enter' om terug te gaan naar het vorige scherm\nUw reserveringscode bestaat uit vier symbolen en kan teruggevonden worden in de reserveringsmail:\n");
            string input = Console.ReadLine();
            while (true)
            {
                if (input.ToLower().Length == 4)
                {

                    for (int i = 0; i < ReservationList.Count; i++)                            //Loopt door de reservation items om te kijken of de reserverings code erin staat
                    {
                        if (ReservationList[i].ReservationId.ToLower() == input.ToLower())     //Als een match gevonden wordt...
                        {
                            Console.Clear();
                            CancelTitle();
                            if (Utils.Confirm(Logo.Annuleren, "\nWeet u zeker dat u de reservering wilt annuleren?\n") == 0)
                                DeleteReservation(ReservationList[i]);
                            else
                                Nee();
                            return;
                        }
                    }
                    input = InputAgain();
                }
                else if (input == "")                                                             // Input = 'enter' 
                {
                    return;
                }
                else                                                                              // Nieuwe input
                {
                    input = InputAgain();
                }

            }
        }
        private static void DeleteReservation(Reservation reservation)
        {
            ReservationList.Remove(reservation);
            Console.Clear();
            CancelTitle();
            Serialize(ReservationList, "reservations.json");                   // Slaat de JSON opnieuw op na de aanpassing
            Console.WriteLine("\nDe reservering is verwijderd.");
            Utils.Enter(Program.Main);
        }
        private static void Nee()
        {
            Console.Clear();
            CancelTitle();
            Console.WriteLine("\nDe reservering is niet verwijderd.");
            Utils.Enter(Program.Main);
        }
        private static string InputAgain()
        {
            Console.Clear();
            CancelTitle();
            Console.WriteLine("\nReserveringscode onjuist. De reserveringscode bestaat uit vier karakters. \nVoer uw reserveringscode nogmaals in of ga terug met 'enter': \n");
            string input = Console.ReadLine();
            return input;
        }
        private static void CancelTitle() => Logo.PrintLogo(Logo.Annuleren);
    }
}
