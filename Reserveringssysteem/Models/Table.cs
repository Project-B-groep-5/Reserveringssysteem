using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Table
    {
        public string TableId { get; set; }
        public int Size { get; set; }

        public Table(string tableId, int size)
        {
            TableId = tableId;
            Size = size;
        }

        public static void TableManager()
        {
            string[] namen = new string[Tables.Count + 2];
            namen[0] = "Toevoegen \n";
            for (int i = 0; i < Tables.Count - 1; i++)
                namen[i + 1] = $"Tafel {Tables[i].TableId}, {Tables[i].Size} zitplaatsen.";
            if (Tables != null)
                namen[^2] = $"Tafel {Tables[^1].TableId}, {Tables[^1].Size} zitplaatsen. \n";
            namen[^1] = "Terug";
            var menu = new SelectionMenu(namen, Logo.Tafels, "\nKies een tafel om deze te wijzigen / verwijderen.\n");
            var choice = menu.Show();
            if (choice == 0)
                AddTable();
            else if (choice == Tables.Count + 1)
                EmployeeActions.ChangeMenu();
            else
            {
                menu = new SelectionMenu(new[] { "Wijzigen", "Verwijderen", "Terug" }, Logo.Tafels);
                switch (menu.Show())
                {
                    case 0:
                        ChangeTable(Tables[choice-1], Tables);
                        return;
                    case 1:
                        RemoveTable(Tables[choice-1], Tables);
                        return;
                    case 2:
                        TableManager();
                        break;
                }
            }

        }

        private static void RemoveTable(Table table, List<Table> Tables)
        {
            if (Utils.Confirm(Logo.Tafels, $"\n\nWeet u zeker dat u tafel {table.TableId} wilt verwijderen?"))
            {
                if (Tables.Remove(table))
                    Serialize(Tables, "Assets/tables.json");
                Logo.PrintLogo(Logo.Tafels);
                Console.WriteLine("Tafel verwijderd.");
            }
            Utils.Enter(TableManager);
        }

        private static void ChangeTable(Table table, List<Table> Tables)
        {
            if (Utils.Confirm(Logo.Tafels, $"\n\nWeet u zeker dat u tafel {table.TableId} wilt wijzigen?"))
            {
                Console.CursorVisible = true;
                int size = 0;
                while (size < 1)
                {
                    while (true)
                    {
                        Logo.PrintLogo(Logo.Tafels);
                        Console.WriteLine("Hoeveel mensen moeten er aan deze tafel kunnen zitten?");
                        try
                        {
                            size = int.Parse(Console.ReadLine());
                            break;
                        }
                        catch
                        {
                            Logo.PrintLogo(Logo.Tafels);
                            Console.WriteLine("Dit is geen nummer.");
                            Utils.Enter("om opnieuw te proberen");
                        }
                    }
                    if (size < 1)
                    {
                        Logo.PrintLogo(Logo.Tafels);
                        Console.WriteLine("Tafels kunnen niet leeg gelaten worden\nVul een getal boven de nul in.");
                        Utils.Enter("om opnieuw te proberen");
                    }
                }
                table.Size = size;
                Logo.PrintLogo(Logo.Tafels);
                Console.WriteLine("Tafel gewijzigd");
                Serialize(Tables, "Assets/tables.json");
                Utils.Enter(TableManager);
            }
        }

        private static void AddTable()
        {
            Console.CursorVisible = true;
            string id = "";
            bool unique = false;
            while (!unique)
            {
                Logo.PrintLogo(Logo.Tafels);
                Console.WriteLine("Wat is het nieuwe nummer van de tafel?");
                id = Utils.Input();
                if (id == "")
                {
                    Logo.PrintLogo(Logo.Tafels);
                    Console.WriteLine("Het nummer kan niet leeggelaten worden.");
                    Utils.Enter();
                    continue;
                }
                unique = true;
                foreach (var table in Tables)
                {
                    if (id.Equals(table.TableId, StringComparison.CurrentCultureIgnoreCase))
                    {
                        unique = false;
                        break;
                    }
                }
                if (!unique)
                {
                    Logo.PrintLogo(Logo.Tafels);
                    Console.WriteLine("Er bestaat al een tafel met dit nummer.");
                    Utils.Enter();
                }
            }
            int size = 0;
            while(size < 1)
            {
                while (true)
                {
                    Logo.PrintLogo(Logo.Tafels);
                    Console.WriteLine("Hoeveel mensen moeten er aan deze tafel kunnen zitten?");
                    try
                    {
                        size = int.Parse(Utils.Input());
                        break;
                    }
                    catch
                    {
                        Logo.PrintLogo(Logo.Tafels);
                        Console.WriteLine("Dit is geen nummer.");
                        Utils.Enter("om opnieuw te proberen");
                    }
                }
                if (size < 1)
                {
                    Logo.PrintLogo(Logo.Tafels);
                    Console.WriteLine("Tafels kunnen niet leeg gelaten worden\nVul een getal boven de nul in.");
                    Utils.Enter("om opnieuw te proberen");
                }
            }
            Tables.Add(new Table(id, size));
            Logo.PrintLogo(Logo.Tafels);
            Console.WriteLine("Tafel toegevoegd.");
            Serialize(Tables, "Assets/tables.json");
            Utils.Enter(TableManager);
        }
    }
}