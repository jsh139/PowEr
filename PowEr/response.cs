using Meebey.SmartIrc4net;
using System;

namespace PowEr
{
    public class response
    {
        #region Responses
        public static string[] Default = {
            "me 0verl00ks {0}...",
            "say St0p buGgiNg mE {0}!!",
            "say Heya {0}!",
            "me *i*G*n*0*r*E*s* {0}.",
            "me ~W~a~v~e~s~ to {0}..",
            "say Yo' {0}!",
            "say aCk!! lEaVe mE aLonE d00d, {0}!!!",
            "say huh?? {0}!!",
            "say dAmNiT!! jUsT lEaVe mE aLonE, {0}!!",
            "say sOrRy {0}. I'm busy rIghT nOw.",
            "say pLeaSe tRy yA caLl lAtEr, {0}..",
            "say hEy {0}! cAn't yA lEavE mE aLoNe fOr a sEc??",
            "say wAit!! iS thAt yA, {0}??",
            "say YikEs!! it'S {0} aGaiN!",
            "say wtf d0 Ya wAnt fr0m mE, {0}??",
            "me rEfusEs to aCknoWlEgDe {0}'s eXistEnCe",
            "say 0h ShadDup, {0}!!!",
            "say g0 gEt youRsElf a lIfE, {0}!!!",
            "say do i know ya, {0}??",
            "say Ya Got anYthInG fOr mE, {0}??",
            "me shUts {0}'s mOuth wIth iTs fAvOritE UnDies, Heh hEh!!",
            "say YeAhrItE.. OkAy noW ShadDup, {0}!!",
            "say hEy {0}!!  wHy yA keEp bUgGinG mE??  dOn't yA haVe a lIfE??",
            "say oh mAn!! nOt yA aGaIn, {0}!!!",
            "say whAt! {0}!! Ya aRe iN lOvE wiTh mE??",
            "say oH bOy!! mY lifE iS tOugH enUfF wIthOut Ya {0}!",
            "me iS bUsy wIth a inVisibLe & toPlesS bAbE. {0}!!",
            "me Is iN nO m0od to cHat, {0}.",
            "say mAn!! I aM PuLLinG uP mY uNdiEs, {0}!!" };

        public static string[] Beer = {
            "me throws a cold beer to {0} and yells \"catch it buddy!!\"",
            "me passes {0} a SUPERSIZE freezy beer",
            "me slides an ice-cold brewski down the bar to {0}, and says \"Care for a menu, Pard?\"",
            "say Take it easy {0}. I'm afraid that ya can't drive home.",
            "me tosses {0} a nice amber microbrew!!",
            "me slides the beer down the bar, and says \"Yo {0} - Haven't seen ya in here in a while! How's it hangin?\"",
            "me stares at {0} and says \"You don't look 21 to me!\"",
            "say Show me ya ID, {0}!!!!",
            "me tosses {0} a nice cold beer and says \"Take it easy, Pal!!\"",
            "say Hey {0}! Ya look like the guy on AMERICA's MOST WANTED!",
            "say Ya sure, {0}??",
            "say How about buy some beers for the ladies in this room?? {0}",
            "me tosses {0} a nice cold beer",
            "say Hey {0}! Why are ya keep drinking tonite?? Ya tiny little heart is broken??",
            "say Beer?? Oh! Am I the only bartender here??",
            "say Hey {0}, here is a pepsi for ya.... little kiddo.",
            "say Grab this Dr. Pepper and go back home {0}!!",
            "say zInG To ThE RIM .... here is a Zima for ya, {0}!!",
            "say Hey {0}!!  Ya gotta STOP or Ya will have a real bad hangover tonite..",
            "me tosses {0} a nice cold beer and asks \"How are the women in the house tonite? *GRIN*\"",
            "say Hey, make this a double and go get ya babe, {0}!!",
            "say Oh oooo.. this is the last beer that i have, let's make a bid, {0}!",
            "me tosses {0} a Zima and mumbles \"Why do they always ask me for beer..\"",
            "me pretends noone is calling him and mumbles \"why do these folks always ask me for beer..and give me no tip..#!%~\"",
            "say Hey {0}!! Stop bugging me!!! can't ya see i'm busy with this woman?" };

        public static string[] BeerMe = {
            "say {0}!!  Naaah.. I don't drink d00d..", 
            "say Mmmm.. {0}, ain't ya scared when i'm drunk?", 
            "say I'm afraid i may do a masskick when i'm drunk {0}..",
            "say will ya give me a ride home when i'm drunk, {0}??",
            "say I need babes when i drink, {0}..  **grin**",
            "say may i drink with ya mate, {0}??? hehehhe",
            "say ahhhh!! that reminds me about the babe that i slept with.. **gRin**",
            "say thanx {0} .." };

        public static string[] BeerDelivery = {
            "say Hey {0}! ya sweet little friend, {1} buy ya a beer here..  :)",
            "me passes a nice cold beer to {0} for {1}..    :)",
            "me asks {1} \"Are ya sure {0} is above 20??\"",
            "me says \"save it for yaself {1}.. \"",
            "say {0}?? where?? i don't see {0} around here {1}...",
            "say Woo.. {1} is up to something now... hehehehe",
            "say Hi {0}! {1} buys ya a beer and would like to ask ya out for the nite..",
            "me tosses {0} a Zima for {1} ..   :)",
            "say Sorry {1}! I think {0} is drunk already..  :)",
            "me tosses a special brew to {0} .. \"From : {1}\"",
            "say Hey {1}! Ya sure {0} is okay with this??" };

        public static string[] Hit = {
            "me *superspinkicks {0}'s nasty ass...",
            "me kicks the hell out of {0}  :P",
            "me blocks and slaps {0} in the face",
            "me beats {0} all around the channel  ===D",
            "me pulls out its .35mm and FIRES at {0}!  ** BANG! BANG!! **",
            "me spits at {0}.   GotChA!!!",
            "me defends and **sidekicks** {0}'s head!  *PoK!!*",
            "me grabs {0}'s throat and squeezes it!!   Arrrgh..  now ya are dead :P",
            "me hits {0} over the head with a baseball bat!  *PoK!!*" };

        public static string[] Thanks = {
            "say No problemo {0}!",
            "say {0}, Ya've Welcome.",
            "say Don't mention it {0}" };

        public static string[] Shutup = {
            "say {0}, who do ya think ya are??  ===D",
            "say Well.. ya better shut ya mouth {0}!!",
            "say Ooops! What can i do with my nature, {0}??" };

        public static string[] Nice = {
            "me thanx {0} for being so nice.",
            "say Ya are such a nice person {0}!  :)",
            "me ~bows~ to {0} ..",
            "me claps for having a nice friend here, {0}  :)",
            "say Thanks {0}!!",
            "say I liKe tHaT {0}!!  Thank YoU!!  *smoooch*",
            "me thanx {0} fRoM tHe b0tToM of tHe b0TheArT!!",
            "me fliEs hiGh iNto thE skY, {0}!!!!",
            "me kisSeS {0} !!! heh hEh.." };
        #endregion

        public static void DoResponse(IrcClient irc, string Channel, string Nick, string Message)
        {
            Random rand = new Random();
            string Response = "";
            Message = Message.ToLower();

            if (Message.IndexOf("beer") >= 0)
            {
                if (Message.IndexOf("beer for ") >= 0)
                {
                    string TargetNick = Message.Substring(Message.IndexOf("beer for ") + 9);
                    if (TargetNick.IndexOf(' ') >= 0)
                        TargetNick = TargetNick.Substring(0, TargetNick.IndexOf(' '));

                    if (TargetNick.ToLower() == "yaself" || TargetNick.ToLower() == "yourself" || TargetNick.ToLower() == irc.Nickname.ToLower())
                        Response = String.Format(BeerMe[rand.Next(0, BeerMe.Length - 1)], Nick);
                    else if (TargetNick.ToLower() == "me" || TargetNick.ToLower() == "myself" || TargetNick.ToLower() == Nick.ToLower())
                        Response = String.Format(Beer[rand.Next(0, Beer.Length - 1)], Nick);
                    else
                    {
                        IrcUser usr = irc.GetIrcUser(TargetNick);
                        if (usr != null)
                            Response = String.Format(BeerDelivery[rand.Next(0, BeerDelivery.Length - 1)], usr.Nick, Nick);
                        else
                            Response = String.Format("say Such person {0} does not exist, {1}!!", TargetNick, Nick);
                    }
                }
                else
                    Response = String.Format(Beer[rand.Next(0, Beer.Length - 1)], Nick);
            }
            else if (Message.IndexOf("stupid") >= 0 || Message.IndexOf("lame") >= 0 || Message.IndexOf("poke") >= 0 ||
                Message.IndexOf("bonk") >= 0 || Message.IndexOf("hit") >= 0 || Message.IndexOf("kick") >= 0 ||
                Message.IndexOf("dumb") >= 0 || Message.IndexOf("slap") >= 0)

                Response = String.Format(Hit[rand.Next(0, Hit.Length - 1)], Nick);
            else if (Message.IndexOf("kiss") >= 0 || Message.IndexOf("sweet") >= 0 || Message.IndexOf("awesome") >= 0 ||
                Message.IndexOf("miss") >= 0 || Message.IndexOf("hug") >= 0 || Message.IndexOf("cool") >= 0 ||
                Message.IndexOf("love") >= 0 || Message.IndexOf("bow") >= 0 || Message.IndexOf("kewl") >= 0 ||
                Message.IndexOf("god") >= 0 || Message.IndexOf("best") >= 0 || Message.IndexOf("nice") >= 0)

                Response = String.Format(Nice[rand.Next(0, Nice.Length - 1)], Nick);
            else if (Message.IndexOf("thanx") >= 0 || Message.IndexOf("danke") >= 0 || Message.IndexOf("thank") >= 0)
                Response = String.Format(Thanks[rand.Next(0, Thanks.Length - 1)], Nick);
            else if (Message.IndexOf("shut") >= 0 && Message.IndexOf("up") >= 0)
                Response = String.Format(Shutup[rand.Next(0, Shutup.Length - 1)], Nick);
            else if (Message.IndexOf("sorry") >= 0)
                Response = String.Format("say takE iT eaSy, {0}!", Nick);
            else if (Message.IndexOf("fuck") >= 0 || Message.IndexOf("sux") >= 0 || Message.IndexOf("suck") >= 0)
            {
                IrcUser usr = irc.GetIrcUser(Nick);
                if (usr != null)
                {
                    oplist ops = new oplist();
                    ops.Load();
                    if (ops.GetOpLevel(usr.Ident + "@" + usr.Host) <= (int)pubcomm.OpLevel.Op)
                    {
                        irc.RfcKick(Channel, Nick, "nEvEr tRy iT aGaiN!!");
                        return;
                    }
                }
                
                Response = String.Format("say Plez dOn't Say thaT to mE, {0}!!", Nick);
            }
            else
                Response = String.Format(Default[rand.Next(0, Default.Length - 1)], Nick);

            if (Response.Split(' ')[0] == "say")
                Response = Response.Substring(4);
            else
                Response = "ACTION " + Response.Substring(3) + "";

            irc.RfcPrivmsg(Channel, Response);
        }
    }
}
