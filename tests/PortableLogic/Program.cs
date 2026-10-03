using System;
using TinkoffBanks.ViewModel;
class Program {
 static int count;
 static void Check(bool condition, string name) { if(!condition) throw new Exception(name); count++; }
 static void Main() {
 var item = new TwitItem { RawContent = "#ihatetowait #5inline #3avail #2closed #15mtowait #angryoldlady #nochairs #retardedteller #ussrstyle #robberyinprogress comment" };
 Check(item.Inline==5 && item.Avail==3 && item.Closed==2 && item.MToWait==15,"counts");
 Check(item.ClosedStr=="Закрыто касс: 2","closed label must use Closed");
 Check(item.AngryOldLady && item.NoChairs && item.RetardedTeller && item.UssrStyle && item.RobberyInProgress,"flags");
 Check(item.Content.Trim()=="comment","metadata removed");
 item.RawContent="fresh";
 Check(item.Inline==0 && item.Avail==0 && item.Closed==0 && item.MToWait==0,"counts reset");
 Check(!item.AngryOldLady && !item.NoChairs && !item.RetardedTeller && !item.UssrStyle && !item.RobberyInProgress,"flags reset");
 Check(item.Content=="fresh","new content");
 item.RawContent="#2htowait"; Check(item.MToWait==120,"hours converted");
 item.RawContent=""; Check(item.MToWait==0 && item.Content=="","empty content resets");
 item.RawContent="#999999999999999999inline"; Check(item.Inline==0,"out of range count does not throw");
 Console.WriteLine("PASS: " + count + " tweet parsing assertions");
 }
}
