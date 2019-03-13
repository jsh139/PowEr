using Meebey.SmartIrc4net;
using System.Threading;

namespace PowEr
{
    /// <summary>
    /// Class to handle all public IRC events/communication
    /// </summary>
    public partial class pubcomm
    {
        public enum OpLevel
        {
            Admin = 500,
            Master = 450,
            Op = 300,
            User = 0
        }

        protected bool PubOn = true;
        protected bool GreetOn = true;
        protected bool NProtOn = true;
        protected bool InsultOn = true;
        protected bool SafeMode = false;

        protected oplist ops;
        protected shitlist shits;
        protected topic topics;
        protected album songs;
        protected insult insults;

        /// <summary>
        /// 
        /// </summary>
        public pubcomm(bool SafeMode)
        {
            this.SafeMode = SafeMode;

            ops = new oplist();
            ops.Load();
            shits = new shitlist();
            shits.Load();
            topics = new topic();
            topics.Load();
            songs = new album();
            songs.Load();
            insults = new insult();
            insults.Load();
        }

        public bool NProt { get { return NProtOn; } }

        /// <summary>
        /// Called when a private msg is received
        /// </summary>
        /// <param name="msg">IrcEventArgs</param>
        public void CheckPrivMsg(IrcEventArgs msg)
        {
            IrcClient irc = msg.Data.Irc;

            string channel = msg.Data.Channel;
            string nick = msg.Data.Nick;
            string userhost = msg.Data.Ident + "@" + msg.Data.Host;
            string message = msg.Data.Message;

            ParseCommand(irc, channel, nick, userhost, message);

            if (message.ToLower().IndexOf("fuck") >= 0)
            {
                irc.RfcPrivmsg(nick, "go FUcK yourself against the wall!!");
                irc.RfcPrivmsg(nick, "YoU MoThEr FuCkEr!!!!!");
                irc.RfcPrivmsg(nick, "SOn Of tHe bItCh!!!!");
                irc.RfcPrivmsg(nick, "go FuCk yourself AgAiNsT tHe wAlL!!!");
                irc.RfcPrivmsg(nick, "Hahahahaha!!!!!!!");
                
                Thread.Sleep(5000);
                irc.RfcPrivmsg(nick, "GoTcHa!!!!");
                Thread.Sleep(5000);
                irc.RfcPrivmsg(nick, "lOsEr!!!!    fUcK yOu!!!");
                Thread.Sleep(5000);
                irc.RfcPrivmsg(nick, "GoTcHa!!!  AaaaHhhhAaa!!!!!!! :P");
            }
        }

        /// <summary>
        /// Called when a msg is broadcast to the channel
        /// </summary>
        /// <param name="msg">IrcEventArgs</param>
        public void CheckChanMsg(IrcEventArgs msg)
        {
            IrcClient irc = msg.Data.Irc;

            string channel = msg.Data.Channel;
            string nick = msg.Data.Nick;
            string userhost = msg.Data.Ident + "@" + msg.Data.Host;
            string message = msg.Data.Message;

            if (!ParseCommand(irc, channel, nick, userhost, message))
                if (message.ToLower().IndexOf(irc.Nickname.ToLower()) >= 0 && nick != irc.Nickname && PubOn)
                    response.DoResponse(irc, channel, nick, message);
        }

        /// <summary>
        /// Called when a /me msg is broadcast to the channel
        /// </summary>
        /// <param name="msg">ActionEventArgs</param>
        public void CheckChanAct(ActionEventArgs msg)
        {
            IrcClient irc = msg.Data.Irc;

            string channel = msg.Data.Channel;
            string nick = msg.Data.Nick;
            string userhost = msg.Data.Ident + "@" + msg.Data.Host;
            string message = msg.ActionMessage;

            /* No commands through actions */
            if (message.ToLower().IndexOf(irc.Nickname.ToLower()) >= 0 && nick != irc.Nickname && PubOn)
                response.DoResponse(irc, channel, nick, message);
        }

        /// <summary>
        /// Called when a user joins the channel
        /// </summary>
        /// <param name="join">JoinEventArgs</param>
        public void CheckJoin(JoinEventArgs join)
        {
            IrcClient irc = join.Data.Irc;

            if (join.Who == irc.Nickname)
            {
                /* Give it 2 seconds after initial channel join */
                Thread.Sleep(2000);
                irc.RfcMode(join.Channel, "+tn");
            }
            else
            {
                /* Check for Ops */
                string UserHost = join.Data.Ident + "@" + join.Data.Host;

                if (ops.GetOpLevel(UserHost) >= 300)
                    irc.Op(join.Channel, join.Who);

                /* Check for greet */
                if (ops.GetOpLevel(UserHost) >= 420 && GreetOn)
                    greet.DoGreet(irc, join.Channel, join.Who);

                /* Check for shitlist */
                if (shits.GetShitLevel(UserHost) >= 100)
                {
                    irc.Ban(join.Channel, UserHost);
                    irc.RfcKick(join.Channel, join.Who, "dId I eVeR sAy yA cAn bE heRe??");
                    irc.RfcNotice(join.Who, "Get The FuCK OuT Of MY ChaNneL..");
                }
                else if (shits.GetShitLevel(UserHost) > 0)
                {
                    irc.Deop(join.Channel, join.Who);
                    irc.RfcPrivmsg(join.Channel, "No Ops for " + join.Who + "..");
                }
            }
        }

        /// <summary>
        /// Called when a user is kicked off the channel
        /// </summary>
        /// <param name="kick">KickEventArgs</param>
        public void CheckKick(KickEventArgs kick)
        {
            IrcClient irc = kick.Data.Irc;

            if (kick.Who != irc.Nickname)
            {
                /* Check prot */
                IrcUser usr = irc.GetIrcUser(kick.Whom);
                if (usr != null)
                    if (ops.GetProtLevel(usr.Ident + "@" + usr.Host) >= 200)
                        irc.RfcKick(kick.Channel, kick.Who, kick.Whom + " is kick protected d00d!!");
            }
        }

        /// <summary>
        /// Called when a user is opped in the channel
        /// </summary>
        /// <param name="op">OpEventArgs</param>
        public void CheckOp(OpEventArgs op)
        {
            IrcClient irc = op.Data.Irc;

            if (op.Who != irc.Nickname && op.Whom != irc.Nickname)
            {
                /* Check for shitlist */
                IrcUser usr = irc.GetIrcUser(op.Whom);
                if (usr != null)
                {
                    string UserHost = usr.Ident + "@" + usr.Host;

                    if (shits.GetShitLevel(UserHost) > 0)
                    {
                        irc.Deop(op.Channel, op.Whom);
                        irc.RfcPrivmsg(op.Channel, "No Ops for " + op.Whom + "..");
                    }
                }

                /* Check for authority */
                if (op.Who != null)
                {
                    usr = irc.GetIrcUser(op.Who);
                    if (usr != null)
                    {
                        string UserHost = usr.Ident + "@" + usr.Host;

                        if (ops.GetOpLevel(UserHost) < 300)
                        {
                            irc.Deop(op.Channel, op.Who);
                            irc.RfcNotice(op.Who, "Ya level is not high enuff to op ppl who is not in my list!!");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when a user is deopped in the channel
        /// </summary>
        /// <param name="deop">DeopEventArgs</param>
        public void CheckDeop(DeopEventArgs deop)
        {
            IrcClient irc = deop.Data.Irc;

            if (deop.Who != irc.Nickname)
            {
                if (deop.Whom == irc.Nickname)
                    irc.RfcNotice(deop.Who, "Op me back LaMeR!");
                else
                {
                    /* Check prot */
                    IrcUser usr = irc.GetIrcUser(deop.Whom);
                    if (usr != null)
                        if (ops.GetProtLevel(usr.Ident + "@" + usr.Host) >= 100)
                            irc.Op(deop.Channel, deop.Whom);
                }
            }
        }

        /// <summary>
        /// Called when a user is banned from the channel
        /// </summary>
        /// <param name="ban">BanEventArgs</param>
        public void CheckBan(BanEventArgs ban)
        {
            bool WeBanned = false;
            int ProtLevel = 0;
            string UserHost = ban.Hostmask;
            IrcClient irc = ban.Data.Irc;

            if (ban.Who != irc.Nickname)
            {
                if (UserHost.StartsWith("*!"))
                {
                    /* Hostmask ban */
                    UserHost = UserHost.Remove(0, 2);

                    IrcUser usr = irc.GetIrcUser(irc.Nickname);
                    if (usr != null)
                        WeBanned = oplist.IsWildcardMatch(UserHost, usr.Ident + "@" + usr.Host);

                    ProtLevel = ops.GetProtLevel(UserHost);
                }
                else
                {
                    /* Nickname ban */
                    string TargetNick = UserHost.Substring(0, UserHost.IndexOf('!'));
                    WeBanned = oplist.IsWildcardMatch(TargetNick, irc.Nickname);

                    IrcUser usr = irc.GetIrcUser(TargetNick);
                    if (usr != null)
                        ProtLevel = ops.GetProtLevel(usr.Ident + "@" + usr.Host);
                }

                if (WeBanned || ProtLevel >= 200)
                {
                    irc.Unban(ban.Channel, ban.Hostmask);
                    irc.RfcKick(ban.Channel, ban.Who, "wTf dO yA ThiNk Ya DoIn pUnK?!");
                }
            }
        }

        public void CheckQuit(QuitEventArgs quit)
        {
            IrcClient irc = quit.Data.Irc;

            /* Check to see if we are the only ones left on our channels and not opped */
            if (quit.Who != irc.Nickname)
            {
                foreach (string s in irc.JoinedChannels)
                {
                    Channel c = irc.GetChannel(s);
                    if (c.Users.Count == 1)
                    {
                        ChannelUser usr = irc.GetChannelUser(c.Name, irc.Nickname);
                        if (!usr.IsOp)
                        {
                            irc.RfcPart(c.Name);
                            irc.RfcJoin(c.Name);
                        }
                    }
                }
            }
        }

        public void CheckPart(PartEventArgs part)
        {
            /* Check to see if we are the only ones left on the channel and not opped */
            IrcClient irc = part.Data.Irc;

            if (part.Who != irc.Nickname)
            {
                Channel c = irc.GetChannel(part.Channel);
                if (c.Users.Count == 1)
                {
                    ChannelUser usr = irc.GetChannelUser(part.Channel, irc.Nickname);
                    if (!usr.IsOp)
                    {
                        irc.RfcPart(c.Name);
                        irc.RfcJoin(c.Name);
                    }
                }
            }
        }
    }
}
