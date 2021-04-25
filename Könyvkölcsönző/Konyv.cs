using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Könyvkölcsönző
{
    public class Konyv
    {
        private List<Konyv> konyvlista = new List<Konyv>(); //könyveket tárolja ("adatbázisa")

        protected int konyv_azon;
        public int Konyv_azon
        {
            get { return konyv_azon; }
            set { konyv_azon = value; }
        }
        protected string mufaj;
        public string Mufaj
        {
            get { return mufaj; }
            set { mufaj = value; }
        }
        protected int kiadaseve;
        public int Kiadaseve
        {
            get { return kiadaseve; }
            set { kiadaseve = value; }
        }
        protected string cim;
        public string Cim
        {
            get { return cim; }
            set { cim = value; }
        }

        protected string iro;

        public string Iro
        {
            get { return iro; }
            set { iro = value; }
        }
        public Konyv(int konyv_azon, string mufaj, int kiadaseve, string cim, string iro)
        {
            this.konyv_azon = konyv_azon;
            this.mufaj = mufaj;
            this.kiadaseve = kiadaseve;
            this.cim = cim;
            this.iro = iro;

        }
    }//Könyv absztrakt osztálya
}
