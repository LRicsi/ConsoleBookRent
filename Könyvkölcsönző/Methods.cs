using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; //sha256-kódoláshoz kell
using System.IO; // fájlkezeléshez
using System.Runtime.Remoting.Messaging; 
namespace Könyvkölcsönző
{
    class Methods
    {
        public string session = ""; //az éppen bejelentkezett felhasználó tárolása
        public const string FelhasznalokFile = "felhasznalokdict.txt"; // beolvas a txt-ből induláskor a felhasználók adatbázisaként szolgál
        public const string IdHelperFile = "IDHELP.txt"; //id számoláshoz segítség
        public Dictionary<string, string> felhasznalokdict = new Dictionary<string, string>(); // felhasználók tárolója, későbbi fejlesztés pl. try catch exception ha ugyan az a felhasználó akar regisztrálni
        
        public static void CreateArrList()
        {
            var arlist = new ArrayList();
        }
        public void Bejelentkez()
        {
            string welcome = "Kérem, jelentkezzen be !";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
            string felhasznalonev = "Felhasználónév: ";
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (felhasznalonev.Length / 2)) + "}",
                felhasznalonev));

            string eltarolFelh = Console.ReadLine();
            string jelszo = "Jelszó: ";
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (jelszo.Length / 2)) + "}", jelszo));
            string eltarolpw = PwHandler();

            //megírni a user+pw ellenőrzést dictionary-vel
            if (felhasznalokdict.ContainsKey(eltarolFelh))
            {
                if (felhasznalokdict.ContainsValue(eltarolpw))
                {
                    //ide meghívni újra a menüt? bejelentkezve
                    //elmenti a felhasználót a session változóba
                    Console.WriteLine("Sikeres Bejelentkezés!");
                }
            }
            else
            {
                Console.WriteLine("A megadott felhasználónév helytelen!");
            }
        }

        public void Regisztral()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string welcome = "Kérem Regisztráljon !";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
            Console.ResetColor();
            string felhasznalonev = "Felhasználónév: ";
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (felhasznalonev.Length / 2)) + "}",
                felhasznalonev));

            string eltarolFelh = Console.ReadLine();

            string jelszo = "Jelszó: ";
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (jelszo.Length / 2)) + "}", jelszo));

            string pw = PwHandler();
            pw=Hash(pw);
            fajlbair(eltarolFelh,pw,ID_Count());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Kész...");
            Console.ResetColor();
        }
        public string Hash(string raw)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - byte tömböt ad vissza 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(raw));
                //a .NET Unicode (UTF-16) kódolású, így nem kell ellenőrizni a beírt jelszó megfelelő formátumát
                //mivel minden ami az UTF-16ban benne van, az UTF-8 is kezeli

                // a byte tömböt stringgé alakítja  
                StringBuilder builder = new StringBuilder(); //stringmanipulácó miatt kell,a sha titkosítás bufferjét használja
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); //felhasználó által beírt jelszó felülírása a titkosítással
                }

                return builder.ToString();

            }
        }
        public string PwHandler()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }
        public void Fomenu()
        {
            string udv = "Üdvözli a BookRent!";
            string kerem = "Kérem válassza ki, mit szeretne tenni...";
            string regisztral = "Regisztráció - 1";
            string bejelentkez = "Bejelentkezés - 2";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (udv.Length / 2)) + "}", udv));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (kerem.Length / 2)) + "}", kerem));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (regisztral.Length / 2)) + "}", regisztral));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (bejelentkez.Length / 2)) + "}", bejelentkez));
            do
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        Regisztral();
                        break;
                    case "2":
                        Bejelentkez();
                        break;
                }
            } while (Console.ReadLine() != "3");
            
            //string 
            //Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (textToEnter.Length / 2)) + "}", textToEnter));
        }
        public void CheckStart()
        {
            //mivel nincs szerver, ez a metódus ellenőrzi hogy létezik-e az 
            //adatbázisként szolgáló txt fájl, illetve ez tölti fel az induláskor
            //a még üres szótárakat, arraylisteket
            CreateArrList();
            if (!File.Exists(FelhasznalokFile))
            {
                using (StreamWriter sv = new StreamWriter(File.Create("felhasznalokdict.txt"))) ;
            }
            // könyveket tartalmazó fájl kell még 


            if (!File.Exists(IdHelperFile))
            {
                using (StreamWriter sv = new StreamWriter(File.Create("idhelper"))) ;
            }


            string user = "", pw = "", id = "";
            int szamlalo = 0;
            using (StreamReader sr = new StreamReader("1.txt"))
            {
                while (!sr.EndOfStream)
                {
                    if (szamlalo % 2 == 0)
                    {
                        user = sr.ReadLine();
                        szamlalo++;
                    }
                    else if(szamlalo%3==0)
                    {
                        id = sr.ReadLine();
                        szamlalo++;
                    }
                    else if(!(szamlalo%2==0))
                    {
                        pw = sr.ReadLine();
                        szamlalo++;
                    }
                    
                    felhasznalokdict.Add(user,pw);
                    user = "";
                    pw = "";
                }
            }
        }

        public void fajlbair(string user, string uspw,int id)
        {
            string[] sorok = {user, uspw,id.ToString()};
            File.AppendAllLines(Path.Combine(FelhasznalokFile),sorok);
            
        }

        public void BookManage()
        {
            //csak könyvtáros használhatja, hozzájuk kötni valahogy
            //kell ez egyáltalán?
        }

        public int ID_Count() 
        {
            int a = ID_VALUE_READ();
            a++;
            ID_VALUE_WRITE(a);
            return a;
        }

        public int ID_VALUE_READ()
        {
            int a = 0;
            using (StreamReader sr = new StreamReader(IdHelperFile))
            {
                while (!sr.EndOfStream)
                {
                   a = Convert.ToInt32(sr.ReadLine());
                }
            }
            return Convert.ToInt32(a);
        }

        public void ID_VALUE_WRITE(int a)
        {
            using (StreamWriter sv = new StreamWriter(File.Create(IdHelperFile)))
            {
                sv.Write(a);
            }
        }

        public void arraylistappend(string a,string b, string c)
        {

        }



    }//class
}//namespace
