using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    public class InfoScherm
    {
        public static void ShowInfo()
        {
            var logo = @"
   ___                                   
  / _ \__   _____ _ __    ___  _ __  ___ 
 | | | \ \ / / _ \ '__|  / _ \| '_ \/ __|
 | |_| |\ V /  __/ |    | (_) | | | \__ \
  \___/  \_/ \___|_|     \___/|_| |_|___/
                                         ";
            Console.WriteLine(logo + "\nOver ons");
        }
    }
}