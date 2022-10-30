// add common namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Pub
{
    class Program
    {
        static void Main(string[] args)
        {
            var price = new double[] { 550000, 565000, 610000, 595000, 760000, 810000 };

            var coefs = MLR.Calculate(new double[,] {
                    { 2600, 3, 20 },
                    { 3000, 4, 15 },
                    { 3200, 4, 18 },
                    { 3600, 3, 30 },
                    { 4000, 5, 8 },
                    { 4100, 6, 8 }
            }, price);            

            var result = MLR.Predict(new double[,] { { 3000, 3, 40 } }, coefs);
            Console.WriteLine(result[0]);
        }
    }
}
