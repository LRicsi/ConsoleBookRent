using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; //sha256-kódoláshoz kell
using System.IO;
using System.Runtime.CompilerServices; 

namespace Könyvkölcsönző
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            Methods user = new Methods();
            user.Fomenu();
            Console.ReadLine();
        }
    }
}
