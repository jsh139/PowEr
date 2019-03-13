using System;
using System.Collections.Generic;
using System.IO;

namespace PowEr
{
    public class insult
    {
        protected List<string> Whoises = new List<string>();
        protected List<string> Lamers = new List<string>();
        protected List<string> Bonks = new List<string>();

        public void Load()
        {
            Whoises.Clear();
            Lamers.Clear();
            
            if (File.Exists("whois.txt"))
            {
                using (StreamReader sr = new StreamReader("whois.txt"))
                {
                    while (!sr.EndOfStream)
                        Whoises.Add(sr.ReadLine());
                    sr.Close();
                }
            }

            if (File.Exists("lamer.txt"))
            {
                using (StreamReader sr = new StreamReader("lamer.txt"))
                {
                    while (!sr.EndOfStream)
                        Lamers.Add(sr.ReadLine());
                    sr.Close();
                }
            }

            if (File.Exists("bonk.txt"))
            {
                using (StreamReader sr = new StreamReader("bonk.txt"))
                {
                    while (!sr.EndOfStream)
                        Bonks.Add(sr.ReadLine());
                    sr.Close();
                }
            }
        }

        public string Whois(string TargetNick, bool BePolite)
        {
            string Insult = "";
            Random r = new Random();

            do
                Insult = String.Format("{0} is {1}", TargetNick, Whoises[r.Next(0, Whoises.Count - 1)]);
            while (BePolite && (Insult.ToLower().IndexOf("fuck") >= 0 || Insult.ToLower().IndexOf("shit") >= 0 ||
                Insult.ToLower().IndexOf("ass") >= 0 || Insult.ToLower().IndexOf("bitch") >= 0) || Insult.ToLower().IndexOf("dick") >= 0);

            return Insult;
        }

        public string Lamer(string TargetNick, bool BePolite)
        {
            string Insult = "";
            Random r = new Random();

            do
                Insult = String.Format(Lamers[r.Next(0, Lamers.Count - 1)], TargetNick);
            while (BePolite && (Insult.ToLower().IndexOf("fuck") >= 0 || Insult.ToLower().IndexOf("shit") >= 0 ||
                Insult.ToLower().IndexOf("ass") >= 0 || Insult.ToLower().IndexOf("bitch") >= 0) || Insult.ToLower().IndexOf("dick") >= 0);
            
            return Insult;
        }

        public string Bonk(string TargetNick, bool BePolite)
        {
            string Insult = "";
            Random r = new Random();

            do
                Insult = String.Format("bonks {0} with a {1}!", TargetNick, Bonks[r.Next(0, Bonks.Count - 1)]);
            while (BePolite && (Insult.ToLower().IndexOf("fuck") >= 0 || Insult.ToLower().IndexOf("shit") >= 0 ||
                Insult.ToLower().IndexOf("ass") >= 0 || Insult.ToLower().IndexOf("bitch") >= 0) || Insult.ToLower().IndexOf("dick") >= 0);

            return Insult;
        }
    }
}
