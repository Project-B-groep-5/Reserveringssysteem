using System;
using System.Collections.Generic;

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

        private static List<Table> GetTables() => Json.Deserialize<List<Table>>("tables.json");

        public static void TableManager()
        {
            var tables = GetTables();
            string[] namen = new string[tables.Count + 2];
            for (int i = 0; i < tables.Count; i++)
                namen[i] = $"Tafel {tables[i].TableId}, {tables[i].Size} zitplaatsen.";
            namen[^2] = "Toevoegen";
            namen[^1] = "Terug";
            var menu = new SelectionMenu(namen, Logo.Tafels, "\n\nKies een tafel om deze te wijzigen / verwijderen.\n");
            var choice = menu.Show();
            if (choice == tables.Count)
                AddTable();
            else if (choice == tables.Count + 1)
                return;
            else
            {
                menu = new SelectionMenu(new[] { "Wijzigen", "Verwijderen", "Terug" }, Logo.Tafels);
                while (true)
                {
                    switch (menu.Show())
                    {
                        case 0:
                            ChangeTable(tables[choice], tables);
                            return;
                        case 1:
                            RemoveTable(tables[choice], tables);
                            return;
                        case 2:
                            continue;
                    }
                    break;
                }
            }

        }

        private static void RemoveTable(Table table, List<Table> tables)
        {
            if(Utils.Confirm(Logo.Tafels, $"\n\nWeet u zeker dat u tafel {table.TableId} wilt verwijderen?") == 0)
            {
                if (tables.Remove(table))
                    Json.Serialize(tables, "tables.json");
            }
            Logo.PrintLogo(Logo.Tafels);
            Console.WriteLine("Tafel verwijderd.");
            Utils.Enter();
        }

        private static void ChangeTable(Table table, List<Table> tables)
        {
            if (Utils.Confirm(Logo.Tafels, $"\n\nWeet u zeker dat u tafel {table.TableId} wilt wijzigen?") == 0)
            {
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
                Utils.Enter();
                Json.Serialize(tables, "tables.json");
            }
        }

        private static void AddTable()
        {
            var tables = GetTables();
            string id = "";
            bool unique = false;
            while (!unique)
            {
                Logo.PrintLogo(Logo.Tafels);
                Console.WriteLine("Wat is het nieuwe nummer van de tafel?");
                id = Console.ReadLine();
                if (id == "")
                {
                    Logo.PrintLogo(Logo.Tafels);
                    Console.WriteLine("Het nummer kan niet leeggelaten worden.");
                    Utils.Enter();
                    continue;
                }
                unique = true;
                foreach (var table in tables)
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
            tables.Add(new Table(id, size));
            Logo.PrintLogo(Logo.Tafels);
            Console.WriteLine("Tafel toegevoegd.");
            Utils.Enter();
            Json.Serialize(tables, "tables.json");
        }
    }
}