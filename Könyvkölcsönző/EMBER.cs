using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Könyvkölcsönző
{
    public abstract class EMBER
    {
        public const string FelhasznalokFile = "felhasznalokdict.txt"; // beolvas a txt-ből induláskor a felhasználók adatbázisaként szolgál
        protected int id; //emberek azonosítására
        protected bool konyvtarosvagyok = false; // könyvtárosok megkülönböztetése
        protected bool diakvagyok = false; //diákok megkülönböztetése
        protected bool tanarvagyok = false; // tanárok megkülönböztetése
        // könyveket tároló "adatbázis"
        public List<Konyv> konyvlista = new List<Konyv>(); //hogy minden user elérje

        public string felh;
        public int Id
        {
            get { return id; }
        }
        public EMBER(int id, string felh, bool konyvtarosvagyok, bool diakvagyok, bool tanarvagyok) // alaposztály konstruktora
        {
            this.id = id;
            this.konyvtarosvagyok = konyvtarosvagyok;
            this.diakvagyok = diakvagyok;
            this.tanarvagyok = tanarvagyok;
            this.felh = felh;
        }
        public int LeadasKiir()
        {
            Console.Clear();//szépségért felel
            Console.WriteLine("Melyiket szeretne leadni?");
            Console.WriteLine("   ID");
            FelhasznalokFile.ToLower();
            using (StreamReader sr = new StreamReader("kolcsonzott.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string[] helper = sr.ReadLine().Split(';'); //aktuális sor beolvasása
                    if (Convert.ToInt32(helper[0]) == this.Id) // ha megegyezik az aktuális felhasználó id-vel, kiírja az ő tartozásait
                    {
                        foreach (var item in helper)
                        {
                            Console.Write(item + " ");
                        }
                        Console.WriteLine();
                    }
                }
            }
            return Convert.ToInt32(Console.ReadLine());
        }

        public void KonyvLead(int leadni) //paraméterként kapja a leadni kívánt könyvet // -> a kölcsönzöttek közül eltávolítani, az elérhető könyvekbe berakni
        {
            List<string> lista = new List<string>(); //segédlista a fájlok manipulálásához
            string azasor = ""; //azt a sort tárolja majd, ahol a leadni kívánt könyv van
            bool found = false; //létező id-t szeretne-e leadni
            using (StreamReader sr = new StreamReader("kolcsonzott.txt")) //kölcsönzöttből kivenni
            {
                while (!sr.EndOfStream)
                {
                    string[] tombvisszair = sr.ReadLine().Split(';');
                    if (Convert.ToInt32(tombvisszair[1]) == leadni) //megtalálta a beírt id-t
                    {
                        azasor = tombvisszair[1] + ";" + tombvisszair[2] + ";" + tombvisszair[3] + ";" + tombvisszair[4]; //elmenti azt a sort kivételre
                        found = true; //megtalálta
                        konyvlista.Add(new Konyv(Convert.ToInt32(tombvisszair[1]), tombvisszair[2], Convert.ToInt32(tombvisszair[3]), tombvisszair[4],tombvisszair[5])); //vissza is teszi az elérhető könyvek közé
                    }
                    else
                    {
                        lista.Add(tombvisszair[0] + ";"+ tombvisszair[1] + ";" + tombvisszair[2] + ";" + tombvisszair[3] + ";" + tombvisszair[4]); //kolcsonzott txt-t írja vissza, a viszaadott nélkül
                    }
                }
            }
            if (found) //ha megtalálta a leadni kívánt könyvet
            {
                using (StreamWriter sw = new StreamWriter("kolcsonzott.txt"))
                {
                    foreach (var item in lista) //helyi listából visszaépíti a textfájlt, immáron a leadott könyv nélkül
                    {
                        sw.WriteLine(item);
                    }
                }

                using (StreamWriter sw = new StreamWriter("konyvek.txt",  true)) //illetve a könyveket tároló fájlba is rak egy sort, a visszaadott könyvet
                {
                    sw.WriteLine(azasor); //-> az a sor, amit eltároltunk az előbbiekben, és tartalmazza a visszaadni kívánt könyv sorát
                }
                
                Console.WriteLine("Sikeres leadas!");
                return;
            }

            Console.WriteLine("Ez a muvelet nem lehetseges!");
        }

        public void txttokonyv() //txt-ből beolvassa az "adatbázist" a listába
        {
            if (File.Exists("konyvek.txt") && new System.IO.FileInfo("konyvek.txt").Length > 1) //ha 0 bájtos a txt, akkor errort dobott, ennek kivédése
                using (StreamReader sr = new StreamReader("konyvek.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] sorok = sr.ReadLine().Split(';');
                        konyvlista.Add(new Konyv(Convert.ToInt16(sorok[0]), sorok[1], Convert.ToInt32(sorok[2]),
                            sorok[3],sorok[4])); //minden könyv 1 sor -> 1példány
                    }
                }
            else
                return;
        }
        public void konyvAdatbazis() //diáké - minden könyvet lát
        {
            Console.Clear();
            foreach (var item in konyvlista)
            {
                Console.WriteLine(item.Konyv_azon + " " + item.Mufaj + " " + item.Kiadaseve + " " + item.Cim + " " + item.Iro);
            }
        }

        public void konyvAdatbazis(string tantargy)//tanáré - 
        {
            Console.Clear();
            foreach (var item in konyvlista)
            {
                if(item.Mufaj == tantargy) //1 tanár csak a saját tantárgyaihoz kapcsolódó könyveket látja
                    Console.WriteLine(item.Konyv_azon + " " + item.Mufaj + " " + item.Kiadaseve + " " + item.Cim + " " + item.Iro);
            }
        }
        //kölcsönöz metódusok: könyvlistából eltávolítani (már nem elérhető a könyvtárban a kölcsönzött könyv) + 
        // + a megfelelő felhasználóhoz rendelni kikölcsönözöttként
        public string Kolcsonoz() //visszaad egy könyv id-t, diáké
        {
            Console.WriteLine("Melyik konyvet szeretne kikolcsonozni?(ID)");
            return Console.ReadLine();
        }

        public string Kolcsonoz(string tantargy) //tanáré
        {
            Console.WriteLine("Melyik konyvet szeretne kikolcsonozni?(ID)");
            string id = Console.ReadLine();
            foreach(var item in konyvlista)
            {
                if(tantargy == item.Mufaj && Convert.ToInt16(id) == item.Konyv_azon) //tanár csak a saját tantárgyához tud könyveket kölcsönözni, ennek ellenőrzése
                    return id;
            }
            return "undefined";
        }
        
        public string KolcsonozVegbemegy(string id) //paraméterül kapja a könyvet, amit szeretne kölcsönözni
        {
            string kolcsonzott = "";
            List<string> sorok = new List<string>(); //lokális segédlista, 
            using(StreamReader sr = new StreamReader("konyvek.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string sor = sr.ReadLine();
                    if (sor.Split(';')[0] != id) //fájlból úgy lehet törölni, ha újraírjuk az egészet -> mindent lement, kivéve a kölcsönzöttet
                        sorok.Add(sor);
                    else
                        kolcsonzott = sor;//törlésre mentjük
                }
            }
            int index = 0;
            foreach(var item in konyvlista)
            {
                if(Convert.ToString(item.Konyv_azon) == kolcsonzott.Split(';')[0]) //program futása közbeni "könyv adatbázisból" törölni az elérhető könyvekből azt a példányt
                {
                    konyvlista.RemoveAt(index);
                    break;
                }
                index++;
            }
            using (StreamWriter sw = new StreamWriter("konyvek.txt"))
            {//visszaírni az egész fájlt, a már hiányzó kölcsönzöttel
                foreach (var item in sorok)
                {
                    sw.WriteLine(item);
                }
            }
            return kolcsonzott; //Kolcsonzottkonyv metódusnak visszaadja
        }
        public void KolcsonzottKonyv(string sor, int kolcsonzoid)
            //értékül kapja a kölcsönzött könyvet + az id-t aki kölcsönözte
        {
            using(StreamWriter sr = new StreamWriter("kolcsonzott.txt", true)) //appendeli egy fájlba 
            {
                sr.WriteLine(kolcsonzoid + ";" + sor);
            }
        }
    }//ember
}
