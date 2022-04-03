using Noita_UnlockAllProgress;

//Looking for secrets
//string pattern = @"secret\S*";
//string pattern2 = @"\S*_secret\S*";
//string input;
//using (StreamReader r = new StreamReader(@"C:\Users\kenny\Desktop\tmp\new 1.txt"))
//    input = r.ReadToEnd();

//var tmplist = new List<string>(); 


//Match m = Regex.Match(input, pattern);
//while (m.Success)
//{
//    tmplist.Add(m.Value);
//    m = m.NextMatch();
//}


//m = Regex.Match(input, pattern2);
//while (m.Success)
//{
//    tmplist.Add(m.Value);
//    m = m.NextMatch();
//}

//Looking for card unlocks
//string pattern = @"card_unlocked_\S*";
//string input;
//using (StreamReader r = new StreamReader(@"C:\Users\kenny\Desktop\tmp\card_unlocked.txt"))
//    input = r.ReadToEnd();

//var tmplist = new List<string>();


//Match m = Regex.Match(input, pattern);
//while (m.Success)
//{
//    tmplist.Add(m.Value);
//    m = m.NextMatch();
//}

//Looking for "progress"
//string pattern = @"essence\S*";
//string input;
//using (StreamReader r = new StreamReader(@"C:\Users\kenny\Desktop\tmp\misc.txt"))
//    input = r.ReadToEnd();

//var tmplist = new List<string>();


//Match m = Regex.Match(input, pattern);
//while (m.Success)
//{
//    tmplist.Add(m.Value);
//    m = m.NextMatch();
//}

//tmplist = tmplist.Where(x => !x.Contains(".png") && x.Contains("_")).ToList();

//for (int x = 0; x < tmplist.Count; x++)
//{
//    tmplist[x] = tmplist[x].Trim();
//    tmplist[x] = tmplist[x].Replace(")", "");
//    tmplist[x] = tmplist[x].Replace("(", "");
//    tmplist[x] = tmplist[x].Replace(".lua", "");
//    tmplist[x] = tmplist[x].Replace(".xml", "");
//    tmplist[x] = tmplist[x].Replace(@"""", "");
//    tmplist[x] = tmplist[x].Replace(",","");
//}

//foreach (string tmp in tmplist.Distinct())
//{
//    //Console.WriteLine(tmp);
//}


//var dir = Directory.GetFiles(@"C:\Users\kenny\AppData\LocalLow\Nolla_Games_Noita\save00\persistent\flags").Where(x => x.Contains("perk_picked"));
//foreach (string tmp in dir)
//{
//    var fi = new FileInfo(tmp);
//    Console.WriteLine(fi.Name);
//}


//Unlock Logic
var spell_cards = Helper.GetSpellCards();
var spell_actions = Helper.GetSpellActions();
var secrets = Helper.GetSecrets();
var misc = Helper.GetProgress();
var perks = Helper.GetPerks();
Console.WriteLine("What do you wish to unlock?");
Console.WriteLine("(1) Everything");
Console.WriteLine("(2) Spell Cards (unlocks all spells for spawn pool)");
Console.WriteLine("(3) Perk Progress (used perks)");
Console.WriteLine("(4) Spell Progress (used spells)");
Console.WriteLine("(5) Enemy Progress (enemies Killed)");
Console.WriteLine("(6) Secrets (amulet, hat, etc)");
Console.WriteLine("(7) In-game progress (orbs, mini-bosses, essences, endings, nohitrun, etc)");
var opt = Console.ReadLine();
while (opt == null || Helper.IsNumeric(opt.ToString()) == false)
{ Console.WriteLine("Please enter the number of the option you wish to use"); opt = Console.ReadLine(); }

var userpath = System.Environment.GetEnvironmentVariable("USERPROFILE");
var path = userpath + @"\AppData\LocalLow\Nolla_Games_Noita\save00";
Console.WriteLine("");
Console.WriteLine("Is this the correct location for your save? " + path);
Console.WriteLine("y(es)/n(no):");
var re = Console.ReadLine();
while (re == null || (re != "y" && re!= "n" && re != "yes" && re!= "no"))
{ Console.WriteLine("Please enter the Y(es) or N(o)"); re = Console.ReadLine(); }
if (re == "n" || re == "no")
{ Console.WriteLine(@"Enter the location of your save folder (for example C:\Users\Johny\AppData\LocalLow\Nolla_Games_Noita\save00):"); path = Console.ReadLine(); }
Console.WriteLine("");
if (Directory.Exists(path) && !Directory.Exists(path + @"\persistent\flags"))
    Console.WriteLine("You need to have started at least 1 game before you can unlock everything");

path += @"\persistent\flags";
if (Directory.Exists(path))
{
    var count = 0;

    //Spell Cards
    if (opt == "1" || opt == "2")
    {
        foreach (var spell in spell_cards)
        {
            if (!File.Exists(path + @"\" + spell))
                using (StreamWriter sw = File.CreateText(path + @"\" + spell))
                { sw.WriteLine("why are you looking here"); count++; Console.WriteLine("Spell Card " + spell + " unlocked."); }
            else
                Console.WriteLine("Spell Card " + spell + " was already unlocked.");
        }

        Console.WriteLine("--- " + count.ToString() + " Spell Cards Unlocked ---");
    }

    //Perks Used
    if (opt == "1" || opt == "3")
    {
        count = 0;
        foreach (var perk in perks)
        {
            if (!File.Exists(path + @"\" + perk))
                using (StreamWriter sw = File.CreateText(path + @"\" + perk))
                { sw.WriteLine("why are you looking here"); count++; Console.WriteLine("Perk " + perk + " unlocked."); }
            else
                Console.WriteLine("Perk " + perk + " was already unlocked.");
        }

        Console.WriteLine("--- " + count.ToString() + " Picked Perks Unlocked ---");
    }

    //Spell Actions
    if (opt == "1" || opt == "4")
    {
        count = 0;
        foreach (var spell in spell_actions)
        {
            if (!File.Exists(path + @"\" + spell))
                using (StreamWriter sw = File.CreateText(path + @"\" + spell))
                { sw.WriteLine("why are you looking here"); count++; Console.WriteLine("Spell Action " + spell + " unlocked."); }
            else
                Console.WriteLine("Spell Action " + spell + " was already unlocked.");
        }

        Console.WriteLine("--- " + count.ToString() + " Spell Actions Unlocked ---");
    }

    //Enemies
    if (opt == "1" || opt == "5")
    {
        count = 0;
    }

    //Secrets
    if (opt == "1" || opt == "6")
    {
        count = 0;
        foreach (var secret in secrets)
        {
            if (!File.Exists(path + @"\" + secret))
                using (StreamWriter sw = File.CreateText(path + @"\" + secret))
                { sw.WriteLine("why are you looking here"); count++; Console.WriteLine("Secret " + secret + " unlocked."); }
            else
                Console.WriteLine("Secret " + secret + " was already unlocked.");
        }

        Console.WriteLine("--- " + count.ToString() + " Secrets Unlocked ---");
    }

    //Misc Progress
    if (opt == "1" || opt == "7")
    {
        count = 0;
        foreach (var progress in misc)
        {
            if (!File.Exists(path + @"\" + progress))
                using (StreamWriter sw = File.CreateText(path + @"\" + progress))
                { sw.WriteLine("why are you looking here"); count++; Console.WriteLine(progress + " unlocked."); }
            else
                Console.WriteLine(progress + " was already unlocked.");
        }

        Console.WriteLine("--- " + count.ToString() + " Misc Progress Unlocked ---");
    }

    Console.WriteLine("");
    Console.WriteLine("Press any key to exit");
    Console.ReadLine();
}
else
    Console.WriteLine("Invalid Directory"); Console.ReadLine();
