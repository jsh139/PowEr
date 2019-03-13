using System.Collections.Generic;
using System.IO;

namespace PowEr
{
    public class album
    {
        protected Dictionary<string, string> Songs = new Dictionary<string, string>();

        public void Load()
        {
            Songs.Clear();
            if (File.Exists("album.txt"))
            {
                using (StreamReader sr = new StreamReader("album.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split('|');
                        Songs.Add(line[0], line[1]);
                    }
                    sr.Close();
                }
            }
        }

        public void Add(string band, string song)
        {
            if (!Songs.ContainsKey(band.ToLower()))
                Songs.Add(band.ToLower(), song);
            Save();
        }

        public string Sing(string band)
        {
            if (Songs.ContainsKey(band.ToLower()))
                return Songs[band.ToLower()];
            else
                return "";
        }

        public string Album()
        {
            string TheAlbum = "";

            foreach (KeyValuePair<string, string> kvp in Songs)
                TheAlbum += kvp.Key + " ";

            return TheAlbum.TrimEnd(' ');
        }

        private void Save()
        {
            using (StreamWriter sw = new StreamWriter("album.txt"))
            {
                foreach (KeyValuePair<string, string> kvp in Songs)
                    sw.WriteLine(kvp.Key + "|" + kvp.Value);
                sw.Close();
            }
        }
    }
}
