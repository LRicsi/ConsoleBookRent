using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
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
        public const string FelhasznalokFile = "felhasznalokdict.txt"; // beolvas a txt-ből induláskor a felhasználók adatbázisaként szolgál
        public const string IdHelperFile = "IDHELP.txt"; //id számoláshoz segítség
        public Konyvadmin admin0;
        public Diak users;
        public Konyvtaros admin;
        public Tanar teacher;

        
        public void Bejelentkez()
        {
            bool found = false;
            while (!found)
            {
                string welcome = "Kérem, jelentkezzen be !";
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
                string felhasznalonev = "Felhasználónév: ";
                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (felhasznalonev.Length / 2)) + "}",
                    felhasznalonev));

                string eltarolFelh = Console.ReadLine();
                string jelszo = "Jelszó: ";
                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (jelszo.Length / 2)) + "}", jelszo));
                string eltarolpw = Hash(PwHandler());
                // 1; admin; 8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918;
                //2; diak1; 5b6714286f8d5fa27df5eef4ee1eff05b97e3ebbc0951b00020373c0094102fa; false; true; false; könyvtaros,diak,tanar
                using (StreamReader sr = new StreamReader(FelhasznalokFile))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] sor = sr.ReadLine().Split(';');
                        if  (sor[2] == eltarolpw)
                        {
                            if(Convert.ToBoolean(sor[3]) == true) //admin
                            {
                                admin0 = new Konyvadmin(sor);

                            } else if(Convert.ToBoolean(sor[4]) == true) //diak
                            {
                                users = new Diak(Convert.ToInt32(sor[0]), sor[1], sor[2], Convert.ToBoolean(sor[3]), Convert.ToBoolean(sor[4]), Convert.ToBoolean(sor[5]));
                            }
                            else if (Convert.ToBoolean(sor[5]) == true) //tanar
                            {
                                teacher = new Tanar(Convert.ToInt32(sor[0]), sor[1], sor[2], Convert.ToBoolean(sor[3]),Convert.ToBoolean(sor[4]), Convert.ToBoolean(sor[5]));
                            }
                            found = true;
                        }
                    }
                }
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
            Console.WriteLine(users.Pw);
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
            //adatbázisként szolgáló txt fájl,
            //illetve ez tölti fel az induláskor ?
            //a még üres szótárakat, arraylisteket ?
            if (!File.Exists(FelhasznalokFile))
            {
                using (StreamWriter sv = new StreamWriter(File.Create("felhasznalokdict.txt"))) ;
            }
            // könyveket tartalmazó fájl kell még 


            if (!File.Exists(IdHelperFile))
            {
                using (StreamWriter sv = new StreamWriter(File.Create("idhelper"))) ;
            }
            
        }

        public void fajlbair(string user, string uspw,int id)
        {
            string[] sorok = { id.ToString(), user, uspw, "false", "true", "false"};
            using (StreamWriter sw = new StreamWriter(FelhasznalokFile, true))
            {
                foreach(var item in sorok)
                {
                    sw.Write(item + ";");

                }
                users = new Diak(Convert.ToInt16(sorok[0]),sorok[1], sorok[2], false, true, false); //egyből megkapja az adatot, nem kell újra bejelentkezni
                sw.WriteLine();
            }

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

        



    }//class
}//namespace
