using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Könyvkölcsönző
{
    public class Tanar : EMBER //tanárok
    {
        public string tantargy;
        public Tanar(int id, string felh, bool konyvtarosvagyok, bool diakvagyok,
            bool tanarvagyok, string tantargy) : base(id, felh,
            konyvtarosvagyok, diakvagyok, tanarvagyok) //diákok konstruktora
        {
            this.tantargy = tantargy;
        }
        public void Tanarfelulet()
        {
            Console.Clear();//szépségért (ha visszalép akárhonnan, akkor üríti a konzolt
            string welcome = "Üdvözlöm a Tanár felületén !";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcome.Length / 2)) + "}", welcome));
            string iras1 = "1.Kölcsönzés";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras1.Length / 2)) + "}", iras1));
            string iras2 = "2.Könyv leadás";

            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras2.Length / 2)) + "}", iras2));
            string iras3 = "3.Kilépés ";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (iras3.Length / 2)) + "}", iras3));


            TanarTask(Console.ReadKey().KeyChar); //menü irányító
        }

        public void TanarTask(char key) //menü irányító
        {
            switch (key)
            {
                case '1':
                    konyvAdatbazis(this.tantargy); //tanár kiíratását hívja, a saját objektumalapú tantárgyaként
                    string kolcson = Kolcsonoz(this.tantargy); //tanárhoz tartozó metúdus hívása (mit szeretne kölcsönözni)
                    if (kolcson != "undefinded") //ha létezik amit szeretne
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
                    Tanarfelulet();
                    break;
                case '2':
                    KonyvLead(LeadasKiir()); //mit szeretne leadni, paraméterként kapja egyből a lead metódusa
                    Tanarfelulet();
                    break;
                case '3':
                    Console.Clear();
                    Program.Main(new string[] { });
                    break;
                default:
                    Tanarfelulet();
                    break;
            }
        }
    }
}
