using Noita_UnlockAllProgress;
using System.Security.Cryptography;
using System.Text;


var test = File.ReadAllText(@"C:\Users\kenny\AppData\LocalLow\Nolla_Games_Noita\save00\stats\_stats.salakieli");
var test2 = File.ReadAllBytes(@"C:\Users\kenny\AppData\LocalLow\Nolla_Games_Noita\save00\stats\_stats.salakieli");

var key = Encoding.ASCII.GetBytes("536563726574734f66546865416c6c53");
var nonce = Encoding.ASCII.GetBytes("54687265654579657341726557617463");
var dataToEncrypt = Encoding.ASCII.GetBytes(@"<E key=""zombie_weak"" value=""880""> \n </E>");

ulong counter = 0;
using var counterMode = new AesCounterMode(nonce, counter);
using var encryptor = counterMode.CreateEncryptor(key, null);
using var decryptor = counterMode.CreateDecryptor(key, null);

var encryptedData = new byte[dataToEncrypt.Length];
var bytesWritten = encryptor.TransformBlock(dataToEncrypt, 0, dataToEncrypt.Length, encryptedData, 0);

var decrypted = new byte[dataToEncrypt.Length];
decryptor.TransformBlock(encryptedData, 0, bytesWritten, decrypted, 0);

//decrypted.Should().BeEquivalentTo(dataToEncrypt);
Console.WriteLine(test);
Console.WriteLine(Encoding.Default.GetString(dataToEncrypt));
Console.WriteLine(Encoding.Default.GetString(decrypted));
Console.WriteLine(Encoding.Default.GetString(encryptedData));


//Unlock Logic
var spell_cards = Helper.GetSpellCards();
var spell_actions = Helper.GetSpellActions();
var secrets = Helper.GetSecrets();
var misc = Helper.GetProgress();
var perks = Helper.GetPerks();
var enemies = Helper.GetEnemies();
var enemyxmltemplate = "<Stats deaths=\"0\" kills=\"0\" player_kills=\"0\" player_projectile_count=\"0\" >\r\r\n  <death_map>\r\r\n  </death_map>\r\r\n  <kill_map>\r\r\n  </kill_map>\r\r\n</Stats>\r\r\n";

Console.WriteLine("What do you wish to unlock?");
Console.WriteLine("(1) Everything");
Console.WriteLine("(2) Spell Cards (unlocks all spells for spawn pool)");
Console.WriteLine("(3) Picked Perks (used perks progress)");
Console.WriteLine("(4) Spell Actions (used spells progres)");
Console.WriteLine("(5) Enemies Killed (Killed enemies progress) (only available in the in-game mod atm)");
Console.WriteLine("(6) Secrets (amulet, hat, etc)");
Console.WriteLine("(7) In-game progress (orbs, mini-bosses, essences, endings, nohitrun, etc)");
var opt = Console.ReadLine();
while (opt == null || Helper.IsNumeric(opt.ToString()) == false)
{ Console.WriteLine("Please enter the number of the option you wish to use"); opt = Console.ReadLine(); }

var userpath = Environment.GetEnvironmentVariable("USERPROFILE");
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
var enemypath = path + @"\stats";
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
        if(Directory.Exists(enemypath))
        {
            count = 0;
            foreach(var enemy in enemies)
            {
                var fname = enemypath + @"\stats_" + enemy + ".xml";
                if (!File.Exists(fname))
                { File.WriteAllText(fname, enemyxmltemplate); count++; Console.WriteLine("Enemy " + enemy + " unlocked."); }
            }

            Console.WriteLine("--- " + count.ToString() + " Enemy Kill Progress Unlocked ---");
        }
        else
            Console.WriteLine("Could not find " + enemypath);          
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
{ Console.WriteLine("Invalid Directory"); Console.ReadLine();}
