using System;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;

namespace PowEr
{
    public class greet
    {
        public static string[] Greets = {
            "me winks at {0} ...^o^0",
            "say Hey {0}. Nice to SeE ya again!!",
            "me ~w~a~v~e~s~ to {0}!",
            "say Look!! It's {0}",
            "me **bows** to {0} ...  8)",
            "me *tickles* {0} ...",
            "say Wassup {0}??",
            "say It's party time, {0}!  ~",
            "say YaY!!!  {0} is in the hOusE.",
            "say How have ya been, {0}?",
            "say OH my GAWD! It's {0}!!" };

        public static void DoGreet(IrcClient irc, string Channel, string Nick)
        {
            string Greet = "";
            Random rand = new Random();

            Greet = String.Format(Greets[rand.Next(0, Greets.Length - 1)], Nick);

            if (Greet.Split(' ')[0] == "say")
                Greet = Greet.Substring(4);
            else
                Greet = "ACTION " + Greet.Substring(3) + "";

            irc.RfcPrivmsg(Channel, Greet);
        }
        
    }
}
