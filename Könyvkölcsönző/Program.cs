using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; //sha256-kódoláshoz kell
using System.IO;
using System.Runtime.CompilerServices; //fájlkezeléshez kell

namespace Könyvkölcsönző
{
    //Egy iskola könyvkölcsönzési rendszere
    //öröklés,polimorfizmus
    //absztrakt osztály, interface
    //delegáltak, generikusok
    //operátor kiterjesztés, kivételkezelés
    //fájlkezelés
    //verem,sor
    //generikusok
    //eseménykezelő - pl ha kikölcsönzött valaki egy könyvet, vagy regisztrált

    class Program
    {

        static void Main(string[] args)
        {
            Methods admin = new Methods();
            admin.Fomenu();
            admin.CheckStart();

            Console.ReadLine();
        }//main
    }//class Program
    public abstract class EMBER
    {
        public const string FelhasznalokFile = "felhasznalokdict.txt"; // beolvas a txt-ből induláskor a felhasználók adatbázisaként szolgál
        protected int id; //emberek azonosítására
        protected bool konyvtarosvagyok = false; // könyvtárosok megkülönböztetése
        protected bool diakvagyok = false; //diákok megkülönböztetése
        protected bool tanarvagyok = false; // tanárok megkülönböztetése
        
        private string pw;
        public string Pw { 
            get { return pw; } 
            set { pw = value;  } 
        }
        public string felh;
        public int Id
        {
            get { return id; }
        }
        //public bool Konyvtarosvagyok
        //{
        //    get { return konyvtarosvagyok; }
        //    set { konyvtarosvagyok = value; }
        //}
        //public bool Diakvagyok
        //{
        //    get { return diakvagyok; }
        //    set { diakvagyok = value; }
        //}
        //public bool Tanarvagyok
        //{
        //    get { return tanarvagyok; }
        //    set { tanarvagyok = value; }
        //}

        public EMBER(int id, string felh,string pw, bool konyvtarosvagyok,bool diakvagyok, bool tanarvagyok) // alaposztály konstruktora
        {
            this.id = id;
            this.konyvtarosvagyok = konyvtarosvagyok;
            this.diakvagyok = diakvagyok;
            this.tanarvagyok = tanarvagyok;
            this.felh = felh;
            this.pw = pw;
        }

        //public int idleptet() //id léptetéséért felelős, exceptionnal megcsinálni még
        //{
        //    id++;
        //    return id;
        //}
    }//ember
   public class Konyvtaros : EMBER // könyvtárosok
   {
       public Konyvtaros(int id, string felh, string pw, bool konyvtarosvagyok, bool diakvagyok, bool tanarvagyok) : base(id,felh,pw,konyvtarosvagyok,diakvagyok,tanarvagyok)// könyvtáros konstruktora
       {
       }
       
       public void TanarFelvesz(int id)
        {
            using(StreamReader sr = new StreamReader(FelhasznalokFile))
            {
                while (!sr.EndOfStream)
                {
                    string[] sor = sr.ReadLine().Split(';');
                    if (id == Convert.ToInt32(sor[0]))
                    {
                        Console.WriteLine("Megtaláltam a(z) {0}, kívánja tanárrá avatni (y/n)?",id);
                        if (Console.ReadLine().ToLower()== "y")
                        {
                            sor[4] = false.ToString();
                            sor[5] = true.ToString();
                        }
                    }
                }
            }
        }

       

   } // könyvtáros

   public class Diak : EMBER //diákok
   {
       public int evfolyam;
       public string osztaly;
       public Diak(int id, string felh, string pw, bool konyvtarosvagyok, bool diakvagyok, bool tanarvagyok) : base(id, felh, pw, konyvtarosvagyok, diakvagyok, tanarvagyok) //diákok konstruktora
       { 
           this.id = id;
           this.felh = felh;
           this.Pw = pw;
           /*this.evfolyam = evfolyam;
           this.osztaly = osztaly;  majd*/
       }
   }

   public class Tanar : EMBER //tanárok
   {
       public string tantargy;
       public Tanar(int id, string felh, string pw, bool konyvtarosvagyok, bool diakvagyok, bool tanarvagyok) : base(id, felh, pw, konyvtarosvagyok, diakvagyok, tanarvagyok) //diákok konstruktora
       {
           this.tantargy = tantargy;
       }
    }
    ////////////////////////////////////////
    ////////////////////////////////////////
    /// könyvek
    public abstract class Konyv
    {
        protected int konyv_azon;
        protected string mufaj;
        protected int kiadaseve;
        protected string cim;

        public Konyv(int konyv_azon, string mufaj, int kiadaseve, string cim)
        {
            this.konyv_azon = konyv_azon;
            this.mufaj = mufaj;
            this.kiadaseve = kiadaseve;
            this.cim = cim;
        }
    }//Könyv absztrakt osztálya

    public class Hangoskonyv : Konyv
    {
        public Hangoskonyv(int konyv_azon, string mufaj, int kiadaseve, string cim) : base (konyv_azon,mufaj,kiadaseve,cim)
        {

        }
    }

    public class Szepirodalom : Konyv
    {
        public Szepirodalom(int konyv_azon, string mufaj, int kiadaseve, string cim) : base(konyv_azon, mufaj, kiadaseve, cim)
        {

        }
    }

    public class Tankonyv : Konyv
    {
        protected string tantargy;
        protected int evfolyam;
        public Tankonyv(int konyv_azon, string mufaj, int kiadaseve, string cim,string tantargy,int evfolyam) : base(konyv_azon, mufaj, kiadaseve, cim)
        {
            this.tantargy = tantargy;
            this.evfolyam = evfolyam;
        }
    }

    public class Nyelvkonyv : Konyv
    {
        protected string nyelv;
        protected string szint;
        public Nyelvkonyv(int konyv_azon, string mufaj, int kiadaseve, string cim,string nyelv,string szint) : base(konyv_azon, mufaj, kiadaseve, cim)
        {
            this.nyelv = nyelv;
            this.szint = szint;
        }
    }




}//namespace
