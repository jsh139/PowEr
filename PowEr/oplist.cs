using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace PowEr
{
    public class oplist
    {
        public class User
        {
            public int OpLevel;
            public int ProtLevel;
            public string UserHost;

            public User(string UserHost, int OpLevel, int ProtLevel)
            {
                this.OpLevel = OpLevel;
                this.ProtLevel = ProtLevel;
                this.UserHost = UserHost;
            }

            public User(string UserHost, string OpLevel, string ProtLevel)
            {
                this.OpLevel = Convert.ToInt32(OpLevel);
                this.ProtLevel = Convert.ToInt32(ProtLevel);
                this.UserHost = UserHost;
            }
        }

        protected Dictionary<string, User> UserList;

        public void Load()
        {
            UserList = new Dictionary<string, User>();

            if (File.Exists("oplist.xml"))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("oplist.xml");

                UserList.Clear();

                foreach (XmlNode node in xml.SelectNodes("/opList/op"))
                {
                    User u = new User(GetValue(node, "host"), GetValue(node, "level"), GetValue(node, "prot"));
                    UserList.Add(u.UserHost, u);
                }
            }
        }

        public void Save()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml("<opList/>");

            XmlNode node = xml.SelectSingleNode("opList");

            foreach (KeyValuePair<string, User> kvp in UserList)
            {
                XmlElement e = xml.CreateElement("op");
                node.AppendChild(e);

                XmlElement e2 = xml.CreateElement("host");
                e2.InnerText = kvp.Value.UserHost;
                e.AppendChild(e2);

                e2 = xml.CreateElement("level");
                e2.InnerText = kvp.Value.OpLevel.ToString();
                e.AppendChild(e2);

                e2 = xml.CreateElement("prot");
                e2.InnerText = kvp.Value.ProtLevel.ToString();
                e.AppendChild(e2);
            }

            xml.Save("oplist.xml");
        }

        public string Add(string UserHost, int OpLevel, int ProtLevel, bool Glob)
        {
            if (Glob)
                UserHost = HostGlobber(UserHost);
            else
                UserHost = FixHost(UserHost);

            string key = HostMatch(UserHost);
            
            if (key != null)
                UserList.Remove(key);

            UserList.Add(UserHost, new User(UserHost, OpLevel, ProtLevel));
            Save();

            return UserHost;
        }

        public void Del(string UserHost)
        {
            string key = HostMatch(UserHost);
            
            if (key != null)
            {
                UserList.Remove(key);
                Save();
            }
        }

        public void Shuser(string UserHost, ref int OpLevel, ref int ProtLevel)
        {
            OpLevel = ProtLevel = 0;
            string key = HostMatch(UserHost);
            
            if (key != null)
            {
                OpLevel = UserList[key].OpLevel;
                ProtLevel = UserList[key].ProtLevel;
            }
        }

        public int GetOpLevel(string UserHost)
        {
            string key = HostMatch(UserHost);
            
            if (key != null)
                return UserList[key].OpLevel;
            else
                return 0;
        }

        public int GetProtLevel(string UserHost)
        {
            string key = HostMatch(UserHost);

            if (key != null)
                return UserList[key].ProtLevel;
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

            foreach (KeyValuePair<string, User> kvp in UserList)
            {
                if (kvp.Key.IndexOf('*') < 0)
                {
                    if (UserHost == kvp.Key)
                        return kvp.Key;
                }
                else
                {
                    /* Wildcard match */
                    if (IsWildcardMatch(kvp.Key, UserHost))
                        return kvp.Key;
                }
            }
            
            return null;
        }

        public static bool IsWildcardMatch(String wildcard, String text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("^");

            foreach (char c in wildcard)
            {
                if (c == '*')
                    sb.Append(".*");
                else
                    sb.Append(Regex.Escape(c.ToString()));
            }

            sb.Append("$");

            Regex regex = new Regex(sb.ToString(), RegexOptions.IgnoreCase);

            return regex.IsMatch(text);
        }
    }
}
