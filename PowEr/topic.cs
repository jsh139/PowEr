using System;
using System.Collections.Generic;
using System.IO;

namespace PowEr
{
    public class topic
    {
        protected List<string> Topics = new List<string>();

        public void Load()
        {
            Topics.Clear();
            if (File.Exists("topic.txt"))
            {
                using (StreamReader sr = new StreamReader("topic.txt"))
                {
                    while (!sr.EndOfStream)
                        Topics.Add(sr.ReadLine());
                    sr.Close();
                }
            }
        }

        public void Add(string Topic)
        {
            Topics.Add(Topic);
            Save();
        }

        public string RTopic()
        {
            Random r = new Random();
            return Topics[r.Next(0, Topics.Count - 1)];
        }

        private void Save()
        {
            using (StreamWriter sw = new StreamWriter("topic.txt"))
            {
                foreach (string s in Topics)
                    sw.WriteLine(s);
                sw.Close();
            }
        }
    }
}
