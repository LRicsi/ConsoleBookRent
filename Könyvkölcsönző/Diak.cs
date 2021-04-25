using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Könyvkölcsönző
{
    public class Diak : EMBER //diákok
    {
        public Diak(int id, string felh, bool konyvtarosvagyok, bool diakvagyok, bool tanarvagyok) : base(id, felh, konyvtarosvagyok, diakvagyok, tanarvagyok) //diákok konstruktora
        {
            this.id = id;
            this.felh = felh;
          
        }

        public void Diakfelulet()
        {
            Console.Clear(); //szépségért (ha visszalép akárhonnan, akkor üríti a konzolt
            string welcome = "Üdvözlöm a Diák felületén !";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
            string iras1 = "1.Kölcsönzés ";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras1.Length / 2)) + "}", iras1));
            string iras2 = "2.Könyv leadás ";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras2.Length / 2)) + "}", iras2));
            string iras3 = "3.Kilépés ";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras3.Length / 2)) + "}", iras3));

            DiakTask(Console.ReadKey().KeyChar); //menürendszert irányítja
        }

        public void DiakTask(char key) //menürendszert irányítja
        {
            switch (key)
            {
                case '1':
                    konyvAdatbazis(); //összes könyv kiíratása
                    string kolcson = Kolcsonoz(); //mit szeretne kölcsönözni
                    if (kolcson != "undefinded") //létezzen olyan id, amit akar
                    { 
                        KolcsonzottKonyv(KolcsonozVegbemegy(kolcson), this.id); //metódushívás a fájlok kezelésére + a kölcsönzés végrehajtására - paraméterek (kívánt könyv id + bejelentkezett "objektum" id-je)
                        Console.WriteLine("Sikeres kolcsonzes!");
                        System.Threading.Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("Ez a muvelet nem lehetseges!");
                        System.Threading.Thread.Sleep(2000);
                    }
                    Diakfelulet();
                    break;
                case '2':
                    KonyvLead(LeadasKiir()); //mit szeretne leadni, paraméterként kapja egyből a lead metódusa
                    Diakfelulet();
                    break;
                case '3':
                    Program.Main(new string[]  {  });
                    break;
                default:
                    Diakfelulet();
                    break;
            }
        }
    }
}
