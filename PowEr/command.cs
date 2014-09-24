using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Meebey.SmartIrc4net;

namespace PowEr
{
    public partial class pubcomm
    {
        protected string[] AdminCommands = { "cease", "rehash", "server", "j", "p", "hop" };
        protected string[] MasterCommands = { "add", "del", "be", "say", "act", "msg", "load", "pub", "greet", "nprot", "shitadd", "shitdel", "sleep", "wakeup" };
        protected string[] OpCommands = { "op", "deop", "ban", "uban", "rtopic", "k", "whois", "lamer", "power", "topicadd",
                                       "t", "charge", "shuser"};
        protected string[] UserCommands = { "help", "album", "sing", "songadd", "beer", "bonk" };

        private bool ParseCommand(IrcClient irc, string Channel, string Nick, string UserHost, string Message)
        {
            int Level = ops.GetOpLevel(UserHost);
            string Command = Message.ToLower().Split(' ')[0];
            string CommandStr = "";

            if (Message.IndexOf(' ') >= 0)
                CommandStr = Message.Substring(Message.IndexOf(' ') + 1);

            foreach (string s in AdminCommands)
                if (s == Command && Level >= (int)OpLevel.Admin)
                    return DoCommand(irc, Channel, Nick, Command, CommandStr);

            foreach (string s in MasterCommands)
                if (s == Command && Level >= (int)OpLevel.Master)
                    return DoCommand(irc, Channel, Nick, Command, CommandStr);

            foreach (string s in OpCommands)
                if (s == Command && Level >= (int)OpLevel.Op)
                    return DoCommand(irc, Channel, Nick, Command, CommandStr);

            foreach (string s in UserCommands)
                if (s == Command && Level >= (int)OpLevel.User)
                    return DoCommand(irc, Channel, Nick, Command, CommandStr);

            return false;
        }

        private bool DoCommand(IrcClient irc, string TheChannel, string Nick, string Command, string CommandStr)
        {
            string Channel = TheChannel;

            /* Assume first channel if command executed via privmsg */
            if (Channel == null)
                Channel = irc.JoinedChannels[0];

            switch (Command)
            {
                case "be":
                    if (CommandStr != "")
                        irc.RfcNick(CommandStr);
                    else
                        irc.RfcNotice(Nick, "Command format: be <nickname>");
                    break;
                case "cease":
                    if (CommandStr == "")
                    {
                        irc.RfcQuit("Oops!!");
                        Application.Exit();
                    }
                    break;
                case "rehash":
                    if (CommandStr == "")
                    {
                        string TheExecutable = Application.ExecutablePath;
                        string Args = "/NICK=" + irc.Nickname;
                        Args += " /CHANNEL=" + Channel;
                        Args += " /SERVER=" + irc.Address;
                        Args += " /PORT=" + irc.Port;

                        if (File.Exists(TheExecutable))
                        {
                            Process.Start(TheExecutable, Args);
                            irc.RfcQuit("Rehashing. BBL..");
                            Application.Exit();
                        }
                    }
                    break;
                case "j":
                    if (CommandStr != "")
                        irc.RfcJoin(CommandStr);
                    else
                        irc.RfcNotice(Nick, "Command format: j <#channel>");
                    break;
                case "p":
                    if (CommandStr != "")
                        irc.RfcPart(CommandStr);
                    else
                        irc.RfcNotice(Nick, "Command format: p <#channel>");
                    break;
                case "hop":
                    if (CommandStr != "")
                    {
                        if (irc.JoinedChannels.Contains(CommandStr))
                        {
                            irc.RfcPart(CommandStr);
                            irc.RfcJoin(CommandStr);
                        }
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: hop <#channel>");
                    break;
                case "help":
                    if (CommandStr == "")
                        GetHelp(irc, Nick);
                    else
                        GetHelp(irc, Nick, CommandStr);
                    break;
                case "uban":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        string Target = Details[0];

                        if (Target.IndexOf('!') >= 0)
                        {
                            /* Hostmask unban .. easy */
                            if (shits.GetShitLevel(Target) >= 100)
                                irc.RfcNotice(Nick, Target + " is shitlisted");
                            else
                                irc.Unban(Channel, Target);
                        }
                        else
                        {
                            /* Nickname unban. */ 
                            IrcUser usr = irc.GetIrcUser(Target);
                            if (usr != null)
                            {
                                string TargetHostMask = usr.Ident + "@" + usr.Host;

                                if (shits.GetShitLevel(TargetHostMask) >= 100)
                                    irc.RfcNotice(Nick, Target + " is shitlisted");
                                else
                                {
                                    /* Get list of channel bans and loop through to see which hostmask matches */
                                    Channel chl = irc.GetChannel(Channel);
                                    if (chl != null)
                                        foreach (string s in chl.Bans)
                                            if (oplist.IsWildcardMatch(s, Target + "!" + TargetHostMask))
                                                irc.Unban(Channel, s);
                                }
                            }
                            else
                                irc.RfcNotice(Nick, "Cannot find " + Target + " on channel");
                        }
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: uban <nickname/user@host>");
                    break;
                case "shitadd":
                    if (CommandStr != "")
                    {
                        int Level = 50;
                        string[] Details = CommandStr.Split(' ');

                        string Host = "";
                        string TargetNick = Details[0];
                        IrcUser usr = irc.GetIrcUser(TargetNick);

                        if (usr != null)
                            Host = usr.Ident + "@" + usr.Host;
                        else
                            Host = TargetNick;  // They must have passed in user@host

                        if (Details.Length > 1)
                            Level = Convert.ToInt32(Details[1]);

                        string NewHost = shits.Add(Host, Level, (usr != null));

                        irc.RfcNotice(Nick, String.Format("{0} shitlisted at Level: {1}", NewHost, Level));

                        if (usr != null && Level < 100)
                            irc.Deop(Channel, TargetNick);
                        else if (usr != null && Level > 50)
                            DoCommand(irc, Channel, TargetNick, "charge", TargetNick);
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: shitadd <nickname/user@host> [<shitlevel>]");

                    break;
                case "shitdel":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        string TargetNick = Details[0];

                        string Host = "";
                        IrcUser usr = irc.GetIrcUser(TargetNick);

                        if (usr != null)
                            Host = usr.Ident + "@" + usr.Host;
                        else
                            Host = TargetNick;  // They must have passed in user@host

                        if (shits.GetShitLevel(Host) > 0)
                        {
                            shits.Del(Host);
                            irc.RfcNotice(Nick, String.Format("{0} deleted from shitlist", Host));
                        }
                        else
                            irc.RfcNotice(Nick, String.Format("{0} not found in shitlist", Host));
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: shitdel <nickname/user@host>");

                    break;
                case "server":
                    irc.RfcNotice(Nick, "Not implemented yet");
                    break;
                case "op":
                    if (CommandStr != "")
                    {
                        foreach (string s in CommandStr.Split(' '))
                            irc.Op(Channel, s);
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: op <nickname> [<nickname2>] [<nickname3>]");

                    break;
                case "deop":
                    if (CommandStr != "")
                    {
                        foreach (string s in CommandStr.Split(' '))
                        {
                            if (s.ToLower() != irc.Nickname.ToLower())
                                irc.Deop(Channel, s);
                            else
                                irc.RfcKick(Channel, Nick, "UnderGroUnD BeTrAyaL SpoTTeD!!");
                        }
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: deop <nickname> [<nickname2>] [<nickname3>]");

                    break;
                case "ban":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        string Target = Details[0];

                        if (Target.IndexOf('!') >= 0)
                        {
                            /* Hostmask ban */
                            if (Target.StartsWith("*!"))
                            {
                                string UserHost = Target.Remove(0, 2);

                                IrcUser usr = irc.GetIrcUser(irc.Nickname);
                                if (usr != null)
                                {
                                    if (oplist.IsWildcardMatch(UserHost, usr.Ident + "@" + usr.Host))
                                    {
                                        irc.RfcKick(Channel, Nick, "UnderGroUnD BeTrAyaL SpoTTeD!!");
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                if (oplist.IsWildcardMatch(Target.Substring(0, Target.IndexOf('!')), irc.Nickname))
                                {
                                    irc.RfcKick(Channel, Nick, "UnderGroUnD BeTrAyaL SpoTTeD!!");
                                    return true;
                                }
                            }

                            /* Ban em */
                            irc.Ban(Channel, Target);
                        }
                        else
                        {
                            /* Nickname ban */
                            if (Target.ToLower() == irc.Nickname.ToLower())
                                irc.RfcKick(Channel, Nick, "UnderGroUnD BeTrAyaL SpoTTeD!!");
                            else
                            {
                                IrcUser usr = irc.GetIrcUser(Target);
                                if (usr != null)
                                {
                                    if (ops.GetProtLevel(usr.Ident + "@" + usr.Host) < 200)
                                    {
                                        irc.Deop(Channel, Target);
                                        irc.Ban(Channel, "*!" + usr.Ident + "@" + usr.Host);
                                    }
                                    else
                                        irc.RfcNotice(Nick, Target + " is deop/ban protected");
                                }
                                else
                                    irc.RfcNotice(Nick, "Cannot find " + Target + " on channel");
                            }
                        }
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: ban <nickname/user@host>");
                    
                    break;
                case "charge":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        string TargetNick = Details[0];

                        IrcUser usr = irc.GetIrcUser(TargetNick);
                        if (usr != null)
                        {
                            DoCommand(irc, Channel, Nick, "ban", TargetNick);
                            DoCommand(irc, Channel, Nick, "k", TargetNick);
                        }
                        else
                            irc.RfcNotice(Nick, "Cannot find " + TargetNick + " on channel");
                    }
                    break;
                case "album":
                    if (CommandStr == "")
                        irc.RfcNotice(Nick, "Current song list: " + songs.Album());
                    break;
                case "sing":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        string band = Details[0];

                        string song = songs.Sing(band);

                        if (song != "")
                            irc.RfcPrivmsg(Channel, song);
                        else
                            irc.RfcNotice(Nick, "That song is not in my album yet!");
                    }

                    break;
                case "songadd":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');

                        if (Details.Length > 1)
                        {
                            string band = Details[0];
                            string song = CommandStr.Substring(CommandStr.IndexOf(' ') + 1);

                            songs.Add(band, song);
                            irc.RfcNotice(Nick, "Ya song has been recorded..");
                        }
                        else
                            irc.RfcNotice(Nick, "Command format: songadd <band> <lyrics>");
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: songadd <band> <lyrics>");
                    break;
                case "add":
                    if (CommandStr != "")
                    {
                        int Level = 300;
                        int ProtLevel = 0;
                        string[] Details = CommandStr.Split(' ');

                        string Host = "";
                        string TargetNick = Details[0];
                        IrcUser usr = irc.GetIrcUser(TargetNick);

                        if (usr != null)
                            Host = usr.Ident + "@" + usr.Host;
                        else
                            Host = TargetNick;  // They must have passed in user@host
                            
                        if (Details.Length > 1)
                            Level = Convert.ToInt32(Details[1]);
                        if (Details.Length > 2)
                            ProtLevel = Convert.ToInt32(Details[2]);

                        string NewHost = ops.Add(Host, Level, ProtLevel, (usr != null));

                        irc.RfcNotice(Nick, String.Format("{0} added at OpLevel: {1}  ProtLevel: {2}",
                            NewHost, Level, ProtLevel));

                        if (usr != null)
                        {
                            irc.RfcNotice(TargetNick, String.Format("{0} has added ya to my powerlist at OpLevel: {1}  ProtLevel: {2}. For a " +
                                "list of commands /msg {3} help",
                                Nick, Level, ProtLevel, irc.Nickname));
                            irc.Op(Channel, TargetNick);
                        }
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: add <nickname/user@host> [<oplevel>] [<protlevel>]");

                    break;
                case "del":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        string TargetNick = Details[0];

                        string Host = "";
                        IrcUser usr = irc.GetIrcUser(TargetNick);

                        if (usr != null)
                            Host = usr.Ident + "@" + usr.Host;
                        else
                            Host = TargetNick;  // They must have passed in user@host

                        if (ops.GetOpLevel(Host) > 0)
                        {
                            ops.Del(Host);
                            irc.RfcNotice(Nick, String.Format("{0} deleted from powerlist", Host));
                        }
                        else
                            irc.RfcNotice(Nick, String.Format("{0} not found in powerlist", Host));
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: del <nickname/user@host>");

                    break;
                case "shuser":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        string TargetNick = Details[0];

                        IrcUser usr = irc.GetIrcUser(TargetNick);
                        if (usr != null)
                        {
                            int Level = 0;
                            int ProtLevel = 0;
                            int ShitLevel = 0;
                            string Host = usr.Ident + "@" + usr.Host;
                            
                            ops.Shuser(Host, ref Level, ref ProtLevel);
                            shits.ShShit(Host, ref ShitLevel);

                            if (Nick.ToLower() == TargetNick.ToLower())
                                irc.RfcNotice(Nick, String.Format("Ya levels are: OpLevel: {0}  ProtLevel: {1}",
                                    Level, ProtLevel));
                            else
                                irc.RfcNotice(Nick, String.Format("{0}'s levels are: OpLevel: {1}  ProtLevel: {2}  ShitLevel: {3}",
                                    TargetNick, Level, ProtLevel, ShitLevel));
                        }
                        else
                            irc.RfcNotice(Nick, "Cannot find " + TargetNick + " in current channel");
                    }

                    break;
                case "load":
                    if (CommandStr != "")
                    {
                        if (CommandStr.ToLower() == "powerlist")
                        {
                            ops.Load();
                            irc.RfcNotice(Nick, "Powerlist has been loaded into memory");
                        }
                        else if (CommandStr.ToLower() == "shitlist")
                        {
                            shits.Load();
                            irc.RfcNotice(Nick, "Shitlist has been loaded into memory");
                        }
                    }

                    break;
                case "topicadd":
                    if (CommandStr != "")
                    {
                        topics.Add(CommandStr);
                        irc.RfcNotice(Nick, "Ya topic has been recorded..");
                    }
                    else
                        irc.RfcNotice(Nick, "Command format: topicadd <topic>");

                    break;
                case "rtopic":
                    if (CommandStr == "")
                    {
                        string Topic = topics.RTopic();
                        if (Topic != "")
                            irc.RfcTopic(Channel, Topic);
                    }
                    break;
                case "t":
                    if (CommandStr != "")
                        irc.RfcTopic(Channel, CommandStr);
                    else
                        irc.RfcNotice(Nick, "Command format: t <topic>");
                    break;
                case "k":
                    if (CommandStr != "")
                    {
                        string TargetNick = CommandStr.Split(' ')[0];

                        if (TargetNick.ToLower() != irc.Nickname.ToLower())
                        {
                            IrcUser usr = irc.GetIrcUser(TargetNick);
                            if (usr != null)
                            {
                                if (ops.GetProtLevel(usr.Ident + "@" + usr.Host) < 200)
                                    irc.RfcKick(Channel, TargetNick, "PowEr");
                                else
                                    irc.RfcNotice(Nick, TargetNick + " is kick protected");
                            }
                            else
                                irc.RfcNotice(Nick, "Cannot find " + TargetNick + " in current channel");
                        }
                        else
                            irc.RfcKick(Channel, Nick, "UnderGroUnD BeTrAyaL SpoTTeD!!");
                    }
                    break;
                case "power":
                    if (CommandStr == "")
                        irc.Op(Channel, Nick);
                    else
                        return false;
                    break;
                case "whois":
                    if (CommandStr != "")
                    {
                        if (InsultOn)
                        {
                            string[] Details = CommandStr.Split(' ');
                            string TargetNick = Details[0];

                            IrcUser usr = irc.GetIrcUser(TargetNick);
                            if (usr != null)
                            {
                                TargetNick = usr.Nick;
                                if (TargetNick.ToLower() == irc.Nickname.ToLower())
                                    TargetNick = Nick;

                                irc.RfcPrivmsg(Channel, insults.Whois(TargetNick, SafeMode));
                            }
                        }
                        else
                            irc.RfcNotice(Nick, "Can't talk riGhT nOw");
                    }
                    break;
                case "lamer":
                    if (CommandStr != "")
                    {
                        if (InsultOn)
                        {
                            string[] Details = CommandStr.Split(' ');
                            string TargetNick = Details[0];

                            IrcUser usr = irc.GetIrcUser(TargetNick);
                            if (usr != null)
                            {
                                TargetNick = usr.Nick;
                                if (TargetNick.ToLower() == irc.Nickname.ToLower())
                                {
                                    irc.RfcPrivmsg(Channel, insults.Lamer(Nick, SafeMode));
                                    irc.RfcPrivmsg(Channel, "GotChA!!");
                                }
                                else
                                    irc.RfcPrivmsg(Channel, insults.Lamer(TargetNick, SafeMode));
                            }
                        }
                        else
                            irc.RfcNotice(Nick, "Can't talk riGhT nOw");
                    }
                    break;
                case "bonk":
                    if (CommandStr != "")
                    {
                        if (InsultOn)
                        {
                            string[] Details = CommandStr.Split(' ');
                            string TargetNick = Details[0];

                            IrcUser usr = irc.GetIrcUser(TargetNick);
                            if (usr != null)
                            {
                                TargetNick = usr.Nick;
                                if (TargetNick.ToLower() == irc.Nickname.ToLower())
                                    TargetNick = Nick;

                                irc.RfcPrivmsg(Channel, "ACTION " + insults.Bonk(TargetNick, SafeMode) + "");
                            }
                        }
                        else
                            irc.RfcNotice(Nick, "Can't talk riGhT nOw");
                    }
                    break;
                case "beer":
                    if (CommandStr != "")
                        response.DoResponse(irc, Channel, Nick, Command + " " + CommandStr);
                    break;
                case "say":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        if (Details.Length > 1)
                        {
                            string TargetChan = Details[0];
                            string Msg = CommandStr.Substring(CommandStr.IndexOf(' ') + 1);

                            irc.RfcPrivmsg(TargetChan, Msg);
                        }
                    }
                    break;
                case "act":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        if (Details.Length > 1)
                        {
                            string TargetChan = Details[0];
                            string Msg = CommandStr.Substring(CommandStr.IndexOf(' ') + 1);

                            irc.RfcPrivmsg(TargetChan, "ACTION " + Msg + "");
                        }
                    }
                    break;
                case "msg":
                    if (CommandStr != "")
                    {
                        string[] Details = CommandStr.Split(' ');
                        if (Details.Length > 1)
                        {
                            string TargetNick = Details[0];
                            string Msg = CommandStr.Substring(CommandStr.IndexOf(' ') + 1);

                            irc.RfcPrivmsg(TargetNick, Msg);
                        }
                    }
                    break;
                case "pub":
                    if (CommandStr != "")
                    {
                        if (CommandStr.ToLower() == "on")
                            PubOn = true;
                        else if (CommandStr.ToLower() == "off")
                            PubOn = false;
                    }

                    irc.RfcNotice(Nick, "PUB is " + (PubOn ? "ON" : "OFF"));
                    break;
                case "greet":
                    if (CommandStr != "")
                    {
                        if (CommandStr.ToLower() == "on")
                            GreetOn = true;
                        else if (CommandStr.ToLower() == "off")
                            GreetOn = false;
                    }
                    
                    irc.RfcNotice(Nick, "GREET is " + (GreetOn ? "ON" : "OFF"));
                    break;
                case "nprot":
                    if (CommandStr != "")
                    {
                        if (CommandStr.ToLower() == "on")
                            NProtOn = true;
                        else if (CommandStr.ToLower() == "off")
                            NProtOn = false;
                    }

                    irc.RfcNotice(Nick, "NPROT is " + (NProtOn ? "ON" : "OFF"));
                    break;
                case "sleep":
                    if (CommandStr == "" && InsultOn)
                    {
                        InsultOn = false;

                        if (TheChannel != null)
                            irc.RfcPrivmsg(Channel, "OK - I will go to sleep now");
                        else
                            irc.RfcNotice(Nick, "OK - I will go to sleep now");
                    }
                    break;
                case "wakeup":
                    if (CommandStr == "" && !InsultOn)
                    {
                        InsultOn = true;

                        if (TheChannel != null)
                            irc.RfcPrivmsg(Channel, "Well .. I am ready to R0cK!!");
                        else
                            irc.RfcNotice(Nick, "Well .. I am ready to R0cK!!");
                    }
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}
