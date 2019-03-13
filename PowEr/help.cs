using Meebey.SmartIrc4net;
using System.Threading;

namespace PowEr
{
    public partial class pubcomm
    {
        public void GetHelp(IrcClient irc, string Nick)
        {
            string HelpText = "  ";

            irc.RfcPrivmsg(Nick, "==============================HELP================================");
            irc.RfcPrivmsg(Nick, "Level 500+ Commands:");

            foreach (string s in AdminCommands)
                HelpText += " " + s;

            irc.RfcPrivmsg(Nick, HelpText);

            Thread.Sleep(500);

            irc.RfcPrivmsg(Nick, "Level 450+ Commands:");

            HelpText = "  ";
            foreach (string s in MasterCommands)
                HelpText += " " + s;

            irc.RfcPrivmsg(Nick, HelpText);

            Thread.Sleep(500);

            irc.RfcPrivmsg(Nick, "Level 300+ Commands:");

            HelpText = "  ";
            foreach (string s in OpCommands)
                HelpText += " " + s;

            irc.RfcPrivmsg(Nick, HelpText);

            Thread.Sleep(500);

            irc.RfcPrivmsg(Nick, "Level 0+ Commands:");

            HelpText = "  ";
            foreach (string s in UserCommands)
                HelpText += " " + s;

            irc.RfcPrivmsg(Nick, HelpText);
            irc.RfcPrivmsg(Nick, "==================================================================");
            irc.RfcPrivmsg(Nick, "For help with a specific command, /msg " + irc.Nickname + " <command>");
        }

        public void GetHelp(IrcClient irc, string Nick, string Command)
        {
            /****************************************************************************************************************
            { "cease", "rehash", "server", "j", "p", "hop" }
            { "add", "del", "be", "say", "act", "msg", "load", "pub", "greet", "shitadd", "shitdel" }
            { "op", "deop", "ban", "uban", "rtopic", "k", "whois", "lamer", "power", "topicadd", "t", "charge", "shuser"}
            { "help", "album", "sing", "songadd", "beer" }
            ****************************************************************************************************************/

            string HelpText = "";
            string HelpText2 = "";

            switch (Command)
            {
                case "cease":
                    HelpText = "Usage: cease";
                    HelpText2 = "Description: Instructs bot to quit IRC session";
                    break;
                case "rehash":
                    HelpText = "Usage: rehash";
                    HelpText2 = "Description: Instructs bot to quit IRC session and restart";
                    break;
                case "server":
                    HelpText = "Usage: server <servername> [<port>]";
                    HelpText2 = "Description: Instructs bot to switch servers to \"servername\" and optional \"port\"";
                    break;
                case "j":
                    HelpText = "Usage: j <#channel>";
                    HelpText2 = "Description: Instructs bot to join \"#channel\"";
                    break;
                case "p":
                    HelpText = "Usage: p <#channel>";
                    HelpText2 = "Description: Instructs bot to part \"#channel\"";
                    break;
                case "hop":
                    HelpText = "Usage: hop <#channel>";
                    HelpText2 = "Description: Instructs bot to part and rejoin \"#channel\"";
                    break;
                case "add":
                    HelpText = "Usage: add <nick/user@host> [<oplevel>] [<protlevel>]";
                    HelpText2 = "Description: Adds a user to the bot's Powerlist. Specifiy a nickname in the channel, or a user@host. Operator levels start " +
                        "at 300 and go to 500. Protection levels are 100 for deop protection, 200 for kick/ban protection, and 0 for none";
                    break;
                case "del":
                    HelpText = "Usage: del <nick/user@host>";
                    HelpText2 = "Description: Deletes a user from the bot's Powerlist. Specifiy a nickname in the channel, or a user@host";
                    break;
                case "be":
                    HelpText = "Usage: be <nickname>";
                    HelpText2 = "Description: Instructs bot to change nicknames to \"nickname\"";
                    break;
                case "say":
                    HelpText = "Usage: say <#channel> <message>";
                    HelpText2 = "Description: Instructs bot to say \"message\" to \"#channel\"";
                    break;
                case "act":
                    HelpText = "Usage: act <#channel> <message>";
                    HelpText2 = "Description: Instructs bot to perform /me \"message\" to \"#channel\"";
                    break;
                case "msg":
                    HelpText = "Usage: msg <nickname> <message>";
                    HelpText2 = "Description: Instructs bot to private message \"message\" to \"nickname\"";
                    break;
                case "load":
                    HelpText = "Usage: load <\"powerlist\"/\"shitlist\">";
                    HelpText2 = "Description: Reloads the Powerlist or Shitlist from disk";
                    break;
                case "pub":
                    HelpText = "Usage: pub <\"on\"/\"off\">";
                    HelpText2 = "Description: Turns public bot responses on or off";
                    break;
                case "greet":
                    HelpText = "Usage: greet <\"on\"/\"off\">";
                    HelpText2 = "Description: Turns user greetings on or off";
                    break;
                case "nprot":
                    HelpText = "Usage: nprot <\"on\"/\"off\">";
                    HelpText2 = "Description: Turns nick protection on or off";
                    break;
                case "shitadd":
                    HelpText = "Usage: shitadd <nick/user@host> <level>";
                    HelpText2 = "Description: Adds a user to the bot's Shitlist. Specifiy a nickname in the channel, or a user@host. " +
                        "Shit levels are 50 for no ops on channel, and 100 for kick/ban on join";
                    break;
                case "shitdel":
                    HelpText = "Usage: shitdel <nick/user@host>";
                    HelpText2 = "Description: Deletes a user from the bot's Shitlist. Specifiy a nickname in the channel, or a user@host";
                    break;
                case "op":
                    HelpText = "Usage: op <nick1> [<nick2>] [<nick3>] ...";
                    HelpText2 = "Description: Instructs bot to op the specified nicks in the current channel";
                    break;
                case "deop":
                    HelpText = "Usage: deop <nick1> [<nick2>] [<nick3>] ...";
                    HelpText2 = "Description: Instructs bot to deop the specified nicks in the current channel";
                    break;
                case "ban":
                    HelpText = "Usage: ban <nick/user@host>";
                    HelpText2 = "Description: Instructs bot to ban a user. Specifiy a nickname in the channel, or a user@host";
                    break;
                case "uban":
                    HelpText = "Usage: uban <nick/user@host>";
                    HelpText2 = "Description: Instructs bot to unban a user. Specifiy a nickname in the channel, or a user@host";
                    break;
                case "rtopic":
                    HelpText = "Usage: rtopic";
                    HelpText2 = "Description: Instructs bot to change the topic in the current channel to a random topic (see also: topicadd)";
                    break;
                case "k":
                    HelpText = "Usage: k <nick>";
                    HelpText2 = "Description: Instructs bot to kick a user off the current channel";
                    break;
                case "whois":
                    HelpText = "Usage: whois <nick>";
                    HelpText2 = "Description: Insults a user";
                    break;
                case "lamer":
                    HelpText = "Usage: lamer <nick>";
                    HelpText2 = "Description: Insults a user";
                    break;
                case "power":
                    HelpText = "Usage: power";
                    HelpText2 = "Description: Instructs bot to op you in the current channel";
                    break;
                case "topicadd":
                    HelpText = "Usage: topicadd <topic>";
                    HelpText2 = "Description: Adds a topic to the bot's random topic list (see also: rtopic)";
                    break;
                case "t":
                    HelpText = "Usage: t <topic>";
                    HelpText2 = "Description: Instructs bot to change the topic in the current channel to the specified phrase";
                    break;
                case "charge":
                    HelpText = "Usage: charge <nick>";
                    HelpText2 = "Description: Bans and then kicks a user off of the current channel";
                    break;
                case "shuser":
                    HelpText = "Usage: shuser <nick>";
                    HelpText2 = "Description: Shows the OpLevel, ProtLevel, and ShitLevel of the specified nickname";
                    break;
                case "album":
                    HelpText = "Usage: album";
                    HelpText2 = "Description: Shows the list of songs recorded in the bot's album (see also: sing, songadd)";
                    break;
                case "sing":
                    HelpText = "Usage: sing <band>";
                    HelpText2 = "Description: Sings a song from the band (see also: album, songadd)";
                    break;
                case "songadd":
                    HelpText = "Usage: songadd <band> <lyrics>";
                    HelpText2 = "Description: Adds a song to the bot's album (see also: sing, album)";
                    break;
                case "beer":
                    HelpText = "Usage: beer [for <nick>]";
                    HelpText2 = "Description: Tells the bot to give you (or an optional person) a beer";
                    break;
                case "sleep":
                    HelpText = "Usage: sleep";
                    HelpText2 = "Description: Turns public insulting off";
                    break;
                case "wakeup":
                    HelpText = "Usage: wakeup";
                    HelpText2 = "Description: Turns public insulting on";
                    break;
                default:
                    irc.RfcPrivmsg(Nick, "Invalid command: " + Command);
                    return;
            }

            irc.RfcPrivmsg(Nick, "Help for command: " + Command);
            irc.RfcPrivmsg(Nick, HelpText);
            irc.RfcPrivmsg(Nick, HelpText2);
        }
    }
}
