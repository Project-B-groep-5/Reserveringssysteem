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

        private static string _name;
        private static int _people;
        private static string _date;
        private static List<List<Table>> _tables = new List<List<Table>>();
        private static Table _table;
        private static string[] _times;
        private static List<string> _timeSlot;
        private static string[] _menus;
        private static string _comment;
        public static void Reservate()
        {
            GetName();
            
            if (_menus != null) FakePayment();
            AskForComments();
            
            Reservation reservation = new Reservation { Name = _name, Date = _date, TimeSlot = _timeSlot.ToArray(), Size = _people, DiscountMenus = _menus, Comments = _comment, AssignedTable = _table};
            
            SendEmail(reservation.ReservationId, _name, _timeSlot[0], _date);

            Console.Write($"U heeft een reservering gemaakt op: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(_date);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" om ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(_timeSlot[0]);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" uur!");
            Console.Write("Uw reserveringscode is: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(reservation.ReservationId);
            reservation.Save();

            Utils.Enter(Program.Main);
        }

        private static void GetTime()
        {
            var timesTitles = _times.ToList();
            timesTitles[^1] += " \n";
            timesTitles.Add("Terug");
            int time = new SelectionMenu(timesTitles.ToArray(), Logo.Reserveren, "\nHoe laat wilt u komen eten?\n").Show();
            if (time == timesTitles.Count - 1)
            {
                GetDate();
                return;
            }
            _table = GetTable(time);
            _timeSlot = new List<string>();
            for (int i = time; i <= time + 3 && i < _times.Length; i++)
                _timeSlot.Add(_times[i]);
            GetDiscountMenus();
        }

        private static Table GetTable(int time)
        {
            var tables = _tables[time].OrderBy(table => table.Size).ToList();
            return tables[0];
        }

        private static void FakePayment()
        {
            double price = 0.0;
            foreach (string menu in _menus)
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
                    var doorgaan = new SelectionMenu(check2, Logo.Reserveren, $"U betaald €{price:0.00} via {betaalMethodes[betaalMethode]}.\n");
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
        private static void GetName()
        {
            ReservateTitle();
            string name = Utils.Input("Wat is uw naam?");


            if (name.Length == 0)
            {
                Utils.NoInput(GetName, Program.Main, Logo.Reserveren);

            }
            Console.CursorVisible = false;
            _name = name;
            GetAmountOfPeople();
        }
        private static void GetAmountOfPeople()
        {
            int amountPeople = 0;
            ReservateTitle();
            while (amountPeople <= 0)
            {
                var input = Utils.Input("Voor hoeveel mensen wilt u een reservering maken?");
                if (input.Length == 0)
                {
                    Utils.NoInput(GetAmountOfPeople, GetName, Logo.Reserveren);
                    return;
                }
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
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"U moet voor minimaal één persoon reserveren door het invullen van een getal.\n\nVoor hoeveel mensen wilt u een reservering maken?\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            var checkArr = new string[2] { "Ja", "Nee" };
            var check = new SelectionMenu(checkArr, Logo.Reserveren, $"U heeft gekozen voor {amountPeople} personen.\nKlopt dit?\n");
            int i = check.Show();
            if (checkArr[i] == "Nee") GetAmountOfPeople();

            Console.CursorVisible = false;
            _people = amountPeople;
            GetDate();
        }
        private static void GetDate()
        {
            var datumVandaag = DateTime.Today.ToString("dd-MM-yyyy");
            ReservateTitle();
            Console.WriteLine("Voor wanneer wilt u reserveren?");
            Console.WriteLine("Gebruik alstublieft het format: dd-MM-jjjj");
            _date = Utils.Input($"\nDe datum van vandaag is {datumVandaag}");
            while (true)
            {
                if (_date == "")
                {
                    Utils.NoInput(GetDate, GetAmountOfPeople, Logo.Reserveren);
                    return;
                }
                else if (!DateTime.TryParse(_date, out DateTime dDate) || DateTime.Parse(_date) < DateTime.Parse(datumVandaag))
                {
                    ReservateTitle();
                    _date = Utils.Input("Opgegeven datum is niet gelijk aan het format of in het verleden.Het format is : dd - MM - jjjj");
                }
                else
                {
                    Console.CursorVisible = false;
                    _date = string.Format("{0:dd-MM-yyyy}", dDate);
                    break;
                }
            }

            _times = GetTimes();
            if (_times == null)
            {
                var optionMenu = new SelectionMenu(new string[2] { "Opnieuw proberen", "Stoppen" }, Logo.Reserveren, "\nHet restaurant is gesloten op de door uw gekozen datum, wat wilt u doen?\n");
                switch (optionMenu.Show())
                {
                    case 0:
                        GetDate();
                        return;
                    case 1:
                        Program.Main();
                        return;

                }
            }
            else if (_times.Length == 0)
            {
                var optionMenu = new SelectionMenu(new string[2] { "Opnieuw proberen", "Stoppen" }, Logo.Reserveren, "\nHet restaurant zit vol op de door uw gekozen datum, wat wilt u doen?\n");
                switch (optionMenu.Show())
                {
                    case 0:
                        GetDate();
                        return;
                    case 1:
                        Program.Main();
                        return;
                }
            }
            GetTime();
        }
        private static bool Check(string date, string time, int size)
        {
            var availableTables = Tables.ToList();
            foreach (var table in Tables)
            {
                if (table.Size < size)
                {
                    availableTables.Remove(table);
                    continue;
                }
                foreach (var reservation in ReservationList)
                    if (reservation.Date == date && reservation.TimeSlot.Contains(time) && reservation.AssignedTable.TableId == table.TableId)
                    {
                        availableTables.Remove(table);
                        break;
                    }
            }
            if (availableTables.Count > 0)
            {
                _tables.Add(availableTables);
                return true;
            }
            return false;
        }
        private static string[] GetTimes()
        {
            DateTime.TryParse(_date, out DateTime dDate);
            var day = (int)dDate.DayOfWeek -1;
            var times = new List<string>();
            var allTimes = new List<string>();
            try
            {
                for (int i = int.Parse(Json.Restaurant.OpeningHours[day].Split(":")[0]); i < int.Parse(Json.Restaurant.OpeningHours[day].Split("-")[1].Split(":")[0]) - 1; i++)
                    for (int j = 0; j <= 30; j += 30)
                        allTimes.Add($"{i}:{j:00}");
                for (int i = 0; i < allTimes.Count; i++)
                {
                    bool add = true;
                    for (int j = 0; j < 3; j++)
                        if (i + j < allTimes.Count)
                            if (!Check(_date, allTimes[i+j], _people))
                            {
                                add = false;
                                break;
                            }
                    if(add)
                        times.Add(allTimes[i]);
                }
                return times.ToArray();
            }
            catch { return null; }
        }
        private static void GetDiscountMenus()
        {
            string[] Menus = new string[VoordeelMenus.Count + 1];
            for (int i = 0; i < VoordeelMenus.Count; i++)
            {
                Menus[i] = VoordeelMenus[i].Name;
            }
            Menus[^1] = "Geen voordeelmenu";
            var voordeelMenuKeuze = new SelectionMenu(new[] { "Voordeelmenu kiezen", "À la carte \n", "Terug" }, Logo.Reserveren, "Het is voordeliger als u van tevoren kiest voor een voordeelmenu.\nKies of u een voordeelmenu neemt of à la carte komt eten\n");
            while (true)
            {
                switch (voordeelMenuKeuze.Show())
                {
                    case 0:
                        _menus = new string[_people];
                        for (int i = 0; i < _people; i++)
                        {
                            var keuze = SelectionMenu.Show(i+1);
                            _menus[i] = Menus[keuze];
                        }
                        ReservateTitle();
                        string gekozen = "U heeft gekozen voor:\n";
                        foreach (var menu in Menus)
                        {
                            int count = 0;
                            for (int i = 0; i < _menus.Length; i++)
                            {
                                if (menu == _menus[i])
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
                        foreach (var str in _menus)
                        {
                            if (str != "Geen voordeelmenu")
                            {
                                emptyChoice = false;
                                break;                               
                            }
                        }
                        if (emptyChoice) _menus = null;
                        return;
                    case 1:
                        string[] keuzeCheckArr2 = new string[2] { "Ja", "Nee" };
                        var voordeelCheck2 = new SelectionMenu(keuzeCheckArr2, Logo.Reserveren, "\nWeet u zeker dat u à la carte wilt eten?\n");
                        var keuzeCheck2 = voordeelCheck2.Show();
                        if (keuzeCheckArr2[keuzeCheck2] == "Nee") continue;
                        return;
                    case 2:
                        GetTime();
                        return;
                }
            }
        }
        private static void AskForComments()
        {
            ReservateTitle();
            var OpmerkingenMenuKeuze = new SelectionMenu(new[] { "Ja", "Nee" }, Logo.Reserveren, "\nHeeft u nog opmerkingen voor het restaurant of over de reservering?: \n");
            var choice = OpmerkingenMenuKeuze.Show();
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            if (choice == 0)
            {
                ReservateTitle();
                _comment = Utils.Input("Welke opmerking(en) wilt u nog geven aan het restaurant: \n");
            }
        }
        private static void SendEmail(string reservationCode, string name, string time, string date) // Method om de mail te sturen.
        {
            ReservateTitle();
            var emailAddress = Utils.Input("Om uw reservering te bevestigen hebben wij uw mail adres nodig.\nNaar welk mail adres mogen wij de reservering sturen?: ");
            while (!Utils.IsValidEmail(emailAddress))
            {
                ReservateTitle();
                Console.WriteLine();
                emailAddress = Utils.Input($"{emailAddress} is geen geldig mail adres.\nNaar welk mail adres mogen wij de reservering sturen?:\n");
            }
            var checkArr = new string[2] { "Ja", "Nee" };
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            var check = new SelectionMenu(new string[2] { "Ja", "Nee" }, Logo.Reserveren, $"\nHet ingevoerde emailadres is: {emailAddress}\nKlopt dit?\n");
            int i = check.Show();
            if (checkArr[i] == "Nee")
            {
                Console.Clear();
                SendEmail(reservationCode, name, time, date);                
            }
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("De bevestigingsmail wordt nu verstuurd. Sluit dit menu nog niet af......");
            string mailMessage = @$"Beste {name},

Hartelijk dank voor uw reservering bij Restaurant {Json.Restaurant.Name} op {date} om {time}.
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
                CancelTitle();
                Console.WriteLine("\nGeen reserveringen gevonden in het systeem.");
                Utils.Enter(Program.Main);
                return;
            }
            CancelTitle();
            string input = Utils.Input("Voer uw reserveringscode in.\nUw reserveringscode bestaat uit vier symbolen en kan teruggevonden worden in de reserveringsmail:\n");
            while (true)
            {
                if (input.ToLower().Length == 4)
                {

                    for (int i = 0; i < ReservationList.Count; i++)                            //Loopt door de reservation items om te kijken of de reserverings code erin staat
                    {
                        if (ReservationList[i].ReservationId.ToLower() == input.ToLower())     //Als een match gevonden wordt...
                        {
                            CancelTitle();
                            if (Utils.Confirm(Logo.Annuleren, "Weet u zeker dat u de reservering wilt annuleren?\n"))
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
                    Utils.NoInput(CancelReservation, Program.Main, Logo.Annuleren);
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
            CancelTitle();
            Serialize(ReservationList, "Assets/reservations.json");                   // Slaat de JSON opnieuw op na de aanpassing
            Console.WriteLine("De reservering is verwijderd.");
            Utils.Enter(Program.Main);
        }
        private static void Nee()
        {
            CancelTitle();
            Console.WriteLine("De reservering is niet verwijderd.");
            Utils.Enter(Program.Main);
        }
        private static string InputAgain()
        {
            CancelTitle();
            return Utils.Input("Reserveringscode onjuist. De reserveringscode bestaat uit vier karakters.\nVoer nogmaals uw reserveringscode in:\n");
        }
        private static void CancelTitle() => Logo.PrintLogo(Logo.Annuleren);
    }
}
