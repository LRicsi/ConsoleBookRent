using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Könyvkölcsönző
{
    class Konyvadmin
    {
        Konyvtaros admin;

        public Konyvadmin(string [] sor)
        {
            this.admin = new Konyvtaros(Convert.ToInt32(sor[0]), sor[1], sor[2], Convert.ToBoolean(sor[3]), Convert.ToBoolean(sor[4]), Convert.ToBoolean(sor[5]));
            Adminfelulet();
        }
        // xu/
        public void Adminfelulet()
        {
            Console.Clear();
            string welcome = "Üdvözlöm a Könyvtáros felületén !";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
            string iras = "Tanarfelvetel";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras.Length / 2)) + "}", iras));
            string iras2 = "Elfogadások";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras2.Length / 2)) + "}", iras2));
            string iras3 = "Lejáratok";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras3.Length / 2)) + "}", iras3));
        }
    }
}
