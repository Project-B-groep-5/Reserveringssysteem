﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    public class InfoScherm
    {
        public static void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(Logo.OverOns);
            Console.ResetColor();
            Console.WriteLine("\nRestaurant informatie:\n" +
                "Welkom bij de Houten Vork. Wij bezorgen u een lach met de lekkerste gerechten.\n" +
                "Wij bereiden de lekkerste vlees en vegetarische gerechten die u terug kunt vinden " +
                "in het menukaart.\n\n" +
                "Restaurant gegevens:\n"+
                "Adress: ........\n" +
                "Telefoonnummer: ........\n" +
                "E-mail: ........ ");
            Utils.EnterTerug();
        }
    }
}