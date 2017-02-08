using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HeatExchanger;

namespace HeatExchanger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dla parametrów początkowych i geometrii oblicza temperatury końcowe i strumień ciepła.");
            Console.ReadKey();
            Console.Clear();
            
            eNTU model = new eNTU();
            Console.WriteLine("Temperatura końcowa wody = {0} [deg C]", Math.Round(model.t_ce, 2));
            Console.WriteLine("Temperatura końcowa powietrza = {0} [deg C]", Math.Round(model.t_he, 2));
            Console.WriteLine("Strumień ciepła = {0} [W]", Math.Round(model.q, 2));          

            Console.ReadKey();

        }
    }
}
