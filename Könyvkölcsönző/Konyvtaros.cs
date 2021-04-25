using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Könyvkölcsönző
{
    public class Konyvtaros : EMBER // könyvtárosok
    {

        public Konyvtaros(int id, string felh, bool konyvtarosvagyok, bool diakvagyok, bool tanarvagyok) :
            base(id, felh, konyvtarosvagyok, diakvagyok, tanarvagyok) // könyvtáros konstruktora
        {
            this.id = id;
            this.felh = felh;
            this.konyvtarosvagyok = konyvtarosvagyok;
            this.diakvagyok = diakvagyok;
            this.tanarvagyok = tanarvagyok;
        }
        
        public void KolcsonzottKonyvek() //összes könyvet kiírja ami nincs épp a könyvtárban (kölcsönözve van)
        {
            Console.WriteLine("ID - KONYVID - Mufaj - kiadasev -cim");
            using(StreamReader sr = new StreamReader("kolcsonzott.txt"))
            {
                while (!sr.EndOfStream)
                {
                    Console.WriteLine(sr.ReadLine());
                }
            }
        }

        public void Adminfelulet()
        {
            Console.Clear();//szépségért (ha visszalép akárhonnan, akkor üríti a konzolt
            string welcome = "Üdvözlöm a Könyvtáros felületén !";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
            string iras3 = "1-Kölcsönzött könyvek";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras3.Length / 2)) + "}", iras3));
            string iras4 = "2-Könyv felvétel";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras4.Length / 2)) + "}", iras4));
            string iras5 = "3-Kilépés";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras5.Length / 2)) + "}", iras5));
            switch (Console.ReadLine())
            {
                case "1":
                    KolcsonzottKonyvek();
                    Console.WriteLine("Vissza szeretne lépni?");
                    if (Console.ReadLine().ToUpper() == "Y")
                    {
                        Adminfelulet();
                    }
                    break;
                case "2":
                    AddKonyv();
                    System.Threading.Thread.Sleep(2000);
                    Adminfelulet();
                    break;
                case "3":
                    Program.Main(new string[] { });
                    break;
                default:
                    Adminfelulet();
                    break;
            }

        }
            public void AddKonyv() //könyvek hozzáadása a rendszerhez
            {
                string h = "";
                bool enough = true; //addig lehessen könyveket felvinni, amíg nem elég
                while (enough)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Milyen könyvet szeretne csinálni?");
                    Console.ResetColor();
                    Console.WriteLine("Termeszettudomany\nSzepirodalom\nNyelvtan\nMatematika");
                    switch (Console.ReadLine())
                    {
                        case "Termeszettudomany":
                            h = "Termeszettudomany";
                            break;
                        case "Szepirodalom":
                            h = "Szepirodalom";
                            break;
                        case "Nyelvtan":
                            h = "Nyelvtan";
                            break;
                        case "Matematika":
                            h = "Matematika";
                            break;
                    }
                    konyvlista.Add(new Konyv(KONYVID_Count(), h,
                        kiadasevbeker(), cimbeker(),irobeker()));
                    //id-helper fájljával azonosítót adni, felhasználótól
                    //kért műfajúra + tőle bekért kiadási évre + címűre beállítani a példányt

                    Console.WriteLine("Elég?! (Y|N)"); 
                    if (Console.ReadLine().ToUpper() == "Y")
                    {
                        enough = false; //ha elég, kilép a ciklusból
                    }

                }
                konyvtotxt(); //a fentebb példányosított könyveket beleírja fájlba
            }
            

            public void konyvtotxt()
            {
                using (StreamWriter sw = new StreamWriter("konyvek.txt")) //txt fájl létrehozása a létező objektumokból
                {
                    foreach (var item in konyvlista)
                    {
                        sw.WriteLine(item.Konyv_azon + ";" + item.Mufaj + ";" + item.Kiadaseve + ";" + item.Cim + ";" + item.Iro);
                    }
                }
            }
            public int KONYVID_Count() //adatbázis hiányában így oldottam meg az ID auto_increment tulajdonságát
            {
                int a = KONYVID_VALUE_READ(); //beolvassa az id aktuális értékét
                a++; //lépteti egyel
                KONYVID_VALUE_WRITE(a); //majd visszaírja fájlba
                return a;
            }

            public int KONYVID_VALUE_READ()
            {
                int a = 0;
                using (StreamReader sr = new StreamReader(Methods.KonyvIDHELPER))
                {
                    while (!sr.EndOfStream)
                    {
                        a = Convert.ToInt32(sr.ReadLine());
                    }
                }
                return Convert.ToInt32(a);
            }

            public void KONYVID_VALUE_WRITE(int a)
            {
                using (StreamWriter sv = new StreamWriter(File.Create(Methods.KonyvIDHELPER)))
                {
                    sv.Write(a);
                }
            }

            private int kiadasevbeker() //könyv feltöltéshez segítőmetódus, bekéri a kiadás évét objektum létrehozása előtt
            {

                Console.WriteLine("A kiadás éve:");
                int kiadasev = Convert.ToInt32(Console.ReadLine());
                return kiadasev;
            }
            private string cimbeker() //könyv feltöltéshez segítőmetódus, bekéri a címét objektum létrehozása előtt
            {

                Console.WriteLine("A kiadás címe:");
                string cim = Console.ReadLine();
                return cim;
            }

            private string irobeker()
            {
                Console.WriteLine("A könyv írója");
                string iro = Console.ReadLine();
                return iro;
            }
         // könyvtáros
    }
}

