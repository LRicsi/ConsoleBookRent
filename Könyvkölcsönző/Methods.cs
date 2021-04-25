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
    public class Methods
    {
        public const string FelhasznalokFile = "felhasznalokdict.txt"; // beolvas a txt-ből induláskor a felhasználók adatbázisaként szolgál
        public const string IdHelperFile = "IDHELP.txt"; //id számoláshoz segítség
        public const string KonyvIDHELPER = "IDHELP2.txt"; //könyv id számoláshoz segítség

        public string[] Bejelentkez()
        {
           // string [] datak = new string[10000];
            while (true) //addig megy, amíg be nem jelentkezik rendesen
            {
                string welcome = "Kérem, jelentkezzen be !";
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
                string felhasznalonev = "Felhasználónév: ";
                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (felhasznalonev.Length / 2)) + "}",
                    felhasznalonev));

                string eltarolFelh = Console.ReadLine(); //beírt felh.nevet tárolja
                string jelszo = "Jelszó: ";
                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (jelszo.Length / 2)) + "}", jelszo));
                string eltarolpw = Hash(PwHandler()); //beírt pw-t hasheli futásidőben + tárolja azt
                using (StreamReader sr = new StreamReader(FelhasznalokFile))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] sor = sr.ReadLine().Split(';'); //beolvassa a felhasználókat
                        if  (sor[1] == eltarolFelh && sor[2] == eltarolpw) // és nézi hogy a user + pw páros létezik-e + egyezik-e
                            return sor; //azt a sort, ahol van a felhasználó, visszaadja
                    }
                }
            }
        }
        public void Elhelyezkedes(string[] sor) //Bejelentkezéstől paraméterként kapja a bejelentkezett felhasználó adatait
        { // ez vizsgálja, hogy az aktuális felhasználó milyen szerepkörű
            //és azonnal példányosítja a felhasználót
            if (Convert.ToBoolean(sor[3]) == true) //konyvtaros
            {
                Konyvtaros konyvtaros = new Konyvtaros(Convert.ToInt16(sor[0]), sor[2], Convert.ToBoolean(sor[3]), Convert.ToBoolean(sor[4]), Convert.ToBoolean(sor[5]));
                konyvtaros.txttokonyv(); //plusz beolvassa a lokális listába a txt-ben tárolt könyvtárat
                konyvtaros.Adminfelulet();//és meghívja a felületüket
            }
            else if (Convert.ToBoolean(sor[4]) == true) //diak
            {
                Diak diak = new Diak(Convert.ToInt16(sor[0]), sor[2], Convert.ToBoolean(sor[3]), Convert.ToBoolean(sor[4]), Convert.ToBoolean(sor[5]));
                diak.txttokonyv();
                diak.Diakfelulet();
            }
            else if (Convert.ToBoolean(sor[5]) == true) //tanar
            {
                Tanar tanar = new Tanar(Convert.ToInt32(sor[0]), sor[2], Convert.ToBoolean(sor[3]), Convert.ToBoolean(sor[4]), Convert.ToBoolean(sor[5]), sor[6]);
                tanar.txttokonyv();
                tanar.Tanarfelulet();
            }
        }

        public void Regisztral()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string welcome = "Kérem Regisztráljon !";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
            Console.ResetColor();

            string mikent = "Válassza ki miként szeretne regisztrálni ( Tanár-1, Diák-2)!";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (mikent.Length / 2)) + "}", mikent));
            int valasz = Convert.ToInt32(Console.ReadLine()); //választás felhasználói fiókok között

            char tantargy = ' ';
            if(valasz == 1)
            {
                Console.WriteLine("Mit tanít maga?" + "Termeszettudomany(1), Szepirodalom(2), Nyelvtan (3), Matematika (4)");
                tantargy = Console.ReadKey().KeyChar; // tanár tantárgyának tárolása
            }
            string felhasznalonev = "Felhasználónév: ";
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (felhasznalonev.Length / 2)) + "}",
                felhasznalonev));

            string eltarolFelh = Console.ReadLine(); //felhasználónév eltárolása

            string jelszo = "Jelszó: ";
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (jelszo.Length / 2)) + "}", jelszo));

            string pw = PwHandler(); //jelszó kicsillagozása
            pw = Hash(pw); //jelszó titkostítása
            fajlbair(eltarolFelh,pw,ID_Count(),valasz, tantargy); //felhasználók fájlbaírása
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Kész...");
            System.Threading.Thread.Sleep(3000);
            Console.ResetColor();
            Program.Main(new string[0]); //main hívása, visszaugrik a főmenüre
        }
        public string Hash(string raw) //paraméterül kapja a "nyers" felhasználó által választott jelszót
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
                return builder.ToString(); //visszaadja a titkosított jelszavat
            }
        }
        public string PwHandler()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)//amíg nem nyomunk entert
            {
                if (info.Key != ConsoleKey.Backspace) //és nem törlünk
                {
                    Console.Write("*"); //csillagokat ír
                    password += info.KeyChar; //ide menti az aktuálisan beírt karaktereket
                }
                else if (info.Key == ConsoleKey.Backspace) //ha töröl
                {
                    if (!string.IsNullOrEmpty(password)) //és nem üres a sor (van mit kitörölni)
                    {
                        //1 karaktert eltávolít a jelszó karaktereiből
                        password = password.Substring(0, password.Length - 1);
                        //megnézi hol van a kurzor
                        int pos = Console.CursorLeft;
                        //kurzor balratolása egyel
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        //a hely space-vel való kitöltése
                        Console.Write(" ");
                        //kurzor balratolása újból
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password; //visszaadja a valódi beírt jelszavat
        }
        public void Fomenu() // a program főmenüje
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
                        Elhelyezkedes(Bejelentkez());
                        break;
                }
            } while (Console.ReadLine() != "3");
        }

       

        public void fajlbair(string user, string uspw, int id, int valasztott, char tantargy) //felhasználók regisztrációnál való fájlbaírása
        {
            bool tanar = false;
            bool diak = false;

            string targy = string.Empty;

            switch (tantargy)
            {
                case '1':
                    targy = "Termeszettudomany";
                    break;
                case '2':
                    targy = "Szepirodalom";
                    break;
                case '3':
                    targy = "Nyelvtan";
                    break;
                case '4':
                    targy = "Matematika";
                    break;
            }
            //diák - tanár megkülönböztetése
            if (valasztott == 1)
                tanar = true;
            else
                diak = true;

            string[] sorok = { id.ToString(), user, uspw, "false", diak.ToString().ToLower(), tanar.ToString().ToLower()}; //sorok tömb tárolja a regisztrált felhasználót
            try
            {
                using (StreamWriter sw = new StreamWriter(FelhasznalokFile, true)) //felhasználó hozzáadása a fájlhoz
                {
                    foreach (var item in sorok)
                    {
                        sw.Write(item + ";");

                    }
                    if(targy != string.Empty) //tanároknál a tárgy kezelése, hozzáadása a sor végére
                    {
                        sw.Write(targy + ";");
                    }
                    sw.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

        }
        public int ID_Count() //adatbázis hiányában így oldottam meg az ID auto_increment tulajdonságát
        {
            int a = ID_VALUE_READ(); //id aktuális értékének olvasása
            a++; //léptetése
            ID_VALUE_WRITE(a); //visszaírása
            return a;
        }

        public int ID_VALUE_READ()
        {
            int a = 0;
            try
            {
                using (StreamReader sr = new StreamReader(IdHelperFile))
                {
                    while (!sr.EndOfStream)
                    {
                        a = Convert.ToInt32(sr.ReadLine());
                    }
                }
                return Convert.ToInt32(a);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Convert.ToInt32(a);
        }

        public void ID_VALUE_WRITE(int a)
        {
            try
            {
                using (StreamWriter sv = new StreamWriter(File.Create(IdHelperFile)))
                {
                    sv.Write(a);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        





    }//class
}//namespace
