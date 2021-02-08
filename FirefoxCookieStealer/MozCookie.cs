using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirefoxCookieStealer
{
    class MozCookie
    {
        public int ID { get; set; }
        public String OriginAttributes { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
        public String Host { get; set; }
        public String Path { get; set; }
        public int Expiry { get; set; }
        public int LastAccessed { get; set; }
        public int CreationTime { get; set; }
        public int IsSecure { get; set; }

        public int IsHttpOnly { get; set; }
        public int IsBrowserElement { get; set; }
        public int SameSite { get; set; }
        public int RawSameSite { get; set; }
        public int SchemeMap { get; set; }

    }
}
