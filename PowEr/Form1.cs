using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PowEr
{
    public partial class Form1 : Form
    {
        public bool SafeMode = false;
        public string InitialServer = "us.undernet.org";
        public string InitialChannel = "#chat";
        //public string InitialServer = "irchost.lexisnexis.com";
        //public string InitialChannel = "#CourtLink";
        public int InitialPort = 6667;
        public string InitialNick = "PowEr";

        public static IrcClient irc = new IrcClient();
        public pubcomm pub;
        public List<string> PrevCommands = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string[] args)
        {
            InitializeComponent();

            foreach (string s in args)
            {
                if (s.ToUpper().StartsWith("/SERVER=") || s.ToUpper().StartsWith("-SERVER="))
                    InitialServer = s.Substring(s.IndexOf("=") + 1).Trim();
                else if (s.ToUpper().StartsWith("/PORT=") || s.ToUpper().StartsWith("-PORT="))
                    InitialPort = Convert.ToInt32(s.Substring(s.IndexOf("=") + 1).Trim());
                else if (s.ToUpper().StartsWith("/NICK=") || s.ToUpper().StartsWith("-NICK="))
                    InitialNick = s.Substring(s.IndexOf("=") + 1).Trim();
                else if (s.ToUpper().StartsWith("/CHANNEL=") || s.ToUpper().StartsWith("-CHANNEL="))
                    InitialChannel = s.Substring(s.IndexOf("=") + 1).Trim();
                else if (s.ToUpper().StartsWith("/SAFE"))
                    SafeMode = true;
            }

        }

        delegate void IrcListener();

        private void Form1_Load(object sender, EventArgs e)
        {
            // Wait time between messages, we can set this lower on own irc servers
            irc.SendDelay = 200;
            
            // We use channel sync, means we can use irc.GetChannel() and so on
            irc.ActiveChannelSyncing = true;
            irc.AutoRejoinOnKick = true;
            //irc.AutoRejoin = true;
            //irc.AutoReconnect = true;
            //irc.AutoRelogin = true;
            //irc.AutoRetry = true;
            irc.CtcpVersion = "Running EliTe PowErCodE by Haha";
            
            irc.OnChannelMessage += new IrcEventHandler(irc_OnChannelMessage);
            irc.OnChannelAction += new ActionEventHandler(irc_OnChannelAction);
            irc.OnQueryMessage += new IrcEventHandler(irc_OnQueryMessage);
            irc.OnRawMessage += new IrcEventHandler(irc_OnRawMessage);
            irc.OnError += new Meebey.SmartIrc4net.ErrorEventHandler(irc_OnError);
            irc.OnJoin += new JoinEventHandler(irc_OnJoin);
            irc.OnKick += new KickEventHandler(irc_OnKick);
            irc.OnOp += new OpEventHandler(irc_OnOp);
            irc.OnDeop += new DeopEventHandler(irc_OnDeop);
            irc.OnBan += new BanEventHandler(irc_OnBan);
            irc.OnCtcpRequest += new CtcpEventHandler(irc_OnCtcpRequest);
            irc.OnPart += new PartEventHandler(irc_OnPart);
            irc.OnQuit += new QuitEventHandler(irc_OnQuit);

            try
            {
                string[] ServerList = { InitialServer,
                                        "mesa.az.us.undernet.org",
                                        "newyork.ny.us.undernet.org",
                                        "us.undernet.org",
                                        "montreal.qc.ca.undernet.org",
                                        "santaana.ca.us.undernet.org",
                                        "tampa.fl.us.undernet.org",
                                        "dallas.tx.us.undernet.org",
                                        "vancouver.bc.ca.undernet.org" };

                irc.Connect(ServerList, InitialPort);
            }
            catch (Exception ex)
            {
                txtMain.AppendText(ex.Message);
                return;
            }

            try
            {
                pub = new pubcomm(SafeMode);

                // here we logon and register our nickname and so on 
                irc.Login(InitialNick, "PowEr", 4, "jsh139");
                // join the channel
                irc.RfcJoin(InitialChannel);
                
                new IrcListener(irc.Listen).BeginInvoke(null, null);

                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                // This should not happen but just in case, we handle it nicely
                Log(ex.Message, true, true);
            }
        }

        void irc_OnCtcpRequest(object sender, CtcpEventArgs e)
        {
            string Nick = e.Data.Nick;

            switch (e.CtcpCommand.ToUpper())
            {
                case "PING":
                case "TIME":
                case "ERRMSG":
                    irc.RfcNotice(Nick, "fuck off!");
                    break;
                case "FINGER":
                    irc.RfcPrivmsg(Nick, "Hey!! Do i kNoW yOu?? Oh Geez!! Remember tO AsK iF YoU rEAlLy wAnT tO kNoW!   " +
                        "d00d!!  WhY?? aRe Ya aShAme of YasElF??  oH yEaH!!  I reMeMbeR yA... Ya aRe jUsT a fUckiNg LamER!!" +
                        "aCk!! GeT yA dIrtY hAndS oFf mE,  SucKeR!!  :P    I sAiD fUck OfF d00d!!  DaMn YoU!!!!!!!!!!!!!!!!");
                    break;
                case "VER":
                case "VERSION":
                    irc.RfcNotice(Nick, "7th Authorized Edition (Enhanced) 2009 by TooL");
                    break;
            }
        }

        void irc_OnOp(object sender, OpEventArgs e)
        {
            pub.CheckOp(e);
        }

        void irc_OnBan(object sender, BanEventArgs e)
        {
            pub.CheckBan(e);
        }

        void irc_OnDeop(object sender, DeopEventArgs e)
        {
            pub.CheckDeop(e);
        }

        void irc_OnChannelAction(object sender, ActionEventArgs e)
        {
            pub.CheckChanAct(e);
        }

        void irc_OnKick(object sender, KickEventArgs e)
        {
            pub.CheckKick(e);
        }

        void irc_OnJoin(object sender, JoinEventArgs e)
        {
            pub.CheckJoin(e);
        }

        void irc_OnQueryMessage(object sender, IrcEventArgs e)
        {
            pub.CheckPrivMsg(e);
        }

        void irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            pub.CheckChanMsg(e);
        }

        void irc_OnQuit(object sender, QuitEventArgs e)
        {
            pub.CheckQuit(e);
        }

        void irc_OnPart(object sender, PartEventArgs e)
        {
            pub.CheckPart(e);
        }

        // this method handles when we receive "ERROR" from the IRC server
        void irc_OnError(object sender, Meebey.SmartIrc4net.ErrorEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Meebey.SmartIrc4net.ErrorEventHandler(irc_OnError), new object[] { sender, e });
            else
                Log(e.ErrorMessage, true, true);
        }

        void irc_OnRawMessage(object sender, IrcEventArgs e)
        {
            // InvokeRequired is true when we access it from another thread than the one
            // which created it. It is important since accessing a control from another
            // thread will raise an exception. We could also have used
            // _outputBox.InvokeRequired. Now we need to check this because irc.Listen()
            // fires the event that calls this function, and Listen() runs on another
            // thread.
            if (InvokeRequired)
            {
                BeginInvoke(new IrcEventHandler(irc_OnRawMessage), new object[] { sender, e });
            }
            else
            {
                string RawMessage = e.Data.RawMessage;
                if (RawMessage.IndexOf("PRIVMSG") > 0 && RawMessage.IndexOf("!") > 0)
                {
                    string Nick = RawMessage.Split(':')[1];
                    string Msg = RawMessage.Split(':')[2];
                    Msg = RawMessage.Substring(RawMessage.IndexOf(Msg));

                    if (RawMessage.IndexOf("ACTION ") > 0)
                    {
                        Nick = "* " + Nick.Substring(0, Nick.IndexOf("!"));
                        Msg = Msg.Replace("ACTION ", "").Replace("", "");
                    }
                    else if (RawMessage.IndexOf("PRIVMSG " + irc.Nickname) > 0)
                        Nick = "*" + Nick.Substring(0, Nick.IndexOf("!")) + "*";
                    else
                        Nick = "<" + Nick.Substring(0, Nick.IndexOf("!")) + ">";

                    RawMessage = String.Format("{0} {1}", Nick, Msg);
                }
                else if (RawMessage.IndexOf("power :No such nick") > 0)
                    irc.RfcNick("PowEr");
                else if (RawMessage.IndexOf(String.Format("301 {0} power", irc.Nickname)) > 0 ||
                    RawMessage.IndexOf(String.Format("311 {0} power", irc.Nickname)) > 0 ||
                    RawMessage.IndexOf(String.Format("312 {0} power", irc.Nickname)) > 0 ||
                    RawMessage.IndexOf(String.Format("318 {0} power", irc.Nickname)) > 0 ||
                    RawMessage.IndexOf(String.Format("319 {0} power", irc.Nickname)) > 0 ||
                    RawMessage.IndexOf(String.Format("330 {0} power", irc.Nickname)) > 0)
                    return;

                Log(RawMessage, true, false);
            }
        }
        
        private void txtCommand_KeyUp(object sender, KeyEventArgs e)
        {
            /* Simplify some commands */
            if (e.KeyCode == Keys.Enter)
            {
                if (txtCommand.Text.ToLower().StartsWith("/quote"))
                    irc.WriteLine(txtCommand.Text.Substring(7));
                else if (txtCommand.Text.ToLower().StartsWith("/me"))
                {
                    irc.RfcPrivmsg(irc.JoinedChannels[0], "ACTION" + txtCommand.Text.Substring(3) + "");
                    Log(String.Format("* {0} {1}", irc.Nickname, txtCommand.Text.Substring(4)), true, false);
                }
                else if (txtCommand.Text.ToLower().StartsWith("/msg"))
                {
                    string[] details = txtCommand.Text.Split(' ');

                    if (details.Length > 2)
                    {
                        string msg = txtCommand.Text.Substring(txtCommand.Text.IndexOf(details[2]));
                        irc.RfcPrivmsg(details[1], msg);
                        Log(String.Format("-> *{0}* {1}", details[1], msg), true, false);
                    }
                }
                else if (txtCommand.Text.ToLower().StartsWith("/quit"))
                    Exit();
                else if (txtCommand.Text.ToLower() == "/auth")
                {
                    irc.RfcPrivmsg("x@channels.undernet.org", "login jsh139 shiknays");
                    irc.RfcMode(irc.Nickname, "+x");
                }
                else if (txtCommand.Text.ToLower().StartsWith("/"))
                {
                    /* Unknown command */
                    return;
                }
                else
                {
                    irc.RfcPrivmsg(irc.JoinedChannels[0], txtCommand.Text);
                    Log(String.Format("<{0}> {1}", irc.Nickname, txtCommand.Text), true, false);
                }

                PrevCommands.Add(txtCommand.Text);
                txtCommand.Clear();
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (PrevCommands.Count > 0)
                {
                    int idx = PrevCommands.IndexOf(txtCommand.Text);

                    if (idx <= 0)
                        txtCommand.Text = PrevCommands[PrevCommands.Count - 1];
                    else
                        txtCommand.Text = PrevCommands[idx - 1];

                    txtCommand.SelectionStart = txtCommand.Text.Length;
                    txtCommand.ScrollToCaret();
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (PrevCommands.Count > 0)
                {
                    int idx = PrevCommands.IndexOf(txtCommand.Text);

                    if (idx >= PrevCommands.Count - 1)
                        txtCommand.Text = PrevCommands[0];
                    else
                        txtCommand.Text = PrevCommands[idx + 1];

                    txtCommand.SelectionStart = txtCommand.Text.Length;
                    txtCommand.ScrollToCaret();
                }
            }
        }

        private void Log(string Message, bool ToFile, bool IsError)
        {
            if (IsError)
            {
                txtMain.SelectionColor = Color.Cyan;
				txtMain.SelectionFont = new Font(txtMain.SelectionFont.FontFamily.Name, txtMain.SelectionFont.Size, FontStyle.Bold);
            }

            txtMain.AppendText(Message + "\r\n");
            txtMain.SelectionStart = txtMain.TextLength;
            txtMain.SelectionLength = 0;
            txtMain.ScrollToCaret();

            if (ToFile)
                File.AppendAllText(Application.StartupPath.TrimEnd('\\') + "\\botlog.txt", Message + "\r\n");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean quit when the form gets closed. Send a "die" message to quit less "cleanly".
            timer1.Enabled = false;
            if (irc.IsConnected)
                irc.RfcQuit("Bye!");
            Thread.Sleep(1000);
        }
        
        public void Exit()
        {
            // We are done, let's exit...
            timer1.Enabled = false;
            Close();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Application.DoEvents();

            this.Visible = false;
            notifyIcon1.Visible = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                notifyIcon1.Visible = true;
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (irc.IsConnected)
            {
                /* Nick prot */
                if (pub.NProt && irc.Nickname.ToLower() != "power")
                    irc.RfcWhois("power");

                /* Make sure we're joined to a channel */
                if (irc.JoinedChannels.Count == 0)
                    irc.RfcJoin(InitialChannel);
            }
            else
            {
                /* If we were disconnected, then reconnect */
                irc = new IrcClient();
                Form1_Load(sender, e);
            }
        }
    }
}
