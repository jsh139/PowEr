using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PowEr
{
    public class shitlist
    {
        protected Dictionary<string, int> ShitList;

        public void Load()
        {
            ShitList = new Dictionary<string, int>();

            if (File.Exists("shitlist.xml"))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("shitlist.xml");

                ShitList.Clear();

                foreach (XmlNode node in xml.SelectNodes("/shitList/shit"))
                    ShitList.Add(GetValue(node, "host"), Convert.ToInt32(GetValue(node, "level")));
            }
        }

        public void Save()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml("<shitList/>");

            XmlNode node = xml.SelectSingleNode("shitList");

            foreach (KeyValuePair<string, int> kvp in ShitList)
            {
                XmlElement e = xml.CreateElement("shit");
                node.AppendChild(e);

                XmlElement e2 = xml.CreateElement("host");
                e2.InnerText = kvp.Key; ;
                e.AppendChild(e2);

                e2 = xml.CreateElement("level");
                e2.InnerText = kvp.Value.ToString();
                e.AppendChild(e2);
            }

            xml.Save("shitlist.xml");
        }

        public string Add(string UserHost, int Level, bool Glob)
        {
            if (Glob)
                UserHost = HostGlobber(UserHost);
            else
                UserHost = FixHost(UserHost);

            string key = HostMatch(UserHost);

            if (key != null)
                ShitList.Remove(key);

            ShitList.Add(UserHost, Level);
            Save();

            return UserHost;
        }

        public void Del(string UserHost)
        {
            string key = HostMatch(UserHost);

            if (key != null)
            {
                ShitList.Remove(key);
                Save();
            }
        }

        public void ShShit(string UserHost, ref int Level)
        {
            Level = 0;
            string key = HostMatch(UserHost);

            if (key != null)
                Level = ShitList[key];
        }

        public int GetShitLevel(string UserHost)
        {
            string key = HostMatch(UserHost);

            if (key != null)
                return ShitList[key];
            else
                return 0;
        }

        private string GetValue(XmlNode node, string tag)
        {
            if (node.SelectSingleNode(tag) != null)
                return node.SelectSingleNode(tag).InnerText;
            else
                return "";
        }

        private string HostGlobber(string UserHost)
        {
            /* example@sub.domain.com -> example@*.domain.com */
            UserHost = FixHost(UserHost);

            if (UserHost.IndexOf(".users.undernet.org") < 0)
            {
                int Start = UserHost.IndexOf('@');

                if (UserHost.Substring(Start).Split('.').Length >= 3)
                {
                    /* @sub.domain.com -> @*.domain.com */
                    string Repl = UserHost.Substring(Start).Split('.')[0] + ".";
                    UserHost = UserHost.Replace(Repl, "@*.");
                }
            }

            return UserHost;
        }

        private string FixHost(string UserHost)
        {
            UserHost = UserHost.ToLower();
            UserHost = UserHost.TrimStart('~');

            return UserHost;
        }

        private string HostMatch(string UserHost)
        {
            UserHost = FixHost(UserHost);

            foreach (KeyValuePair<string, int> kvp in ShitList)
            {
                if (kvp.Key.IndexOf('*') < 0)
                {
                    if (UserHost == kvp.Key)
                        return kvp.Key;
                }
                else
                {
                    /* Wildcard match */
                    if (oplist.IsWildcardMatch(kvp.Key, UserHost))
                        return kvp.Key;
                }
            }

            return null;
        }
    }
}
