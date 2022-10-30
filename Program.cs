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
            var area = new double[] { 2600, 3000, 3200, 3600, 4000 };            
            var price = new double[] { 550000, 565000, 610000, 680000, 725000 };
            var coefs = SLR.Calculate(area, price);
            Console.WriteLine(SLR.Predict(3300, coefs));
            Console.WriteLine(string.Join(" ", SLR.Predict(new double[] { 3300, 2700, 5000 }, coefs)));
            Console.WriteLine(SSLR.Predict(3300, coefs));
            Console.WriteLine(string.Join(" ", SSLR.Predict(new double[] { 3300, 2700, 5000 }, coefs)));

            var prices = new double[] { 550000, 565000, 610000, 595000, 760000, 810000 };
            var coefs2 = MLR.Calculate(new double[,] {
                    { 2600, 3, 20 },
                    { 3000, 4, 15 },
                    { 3200, 4, 18 },
                    { 3600, 3, 30 },
                    { 4000, 5, 8  },
                    { 4100, 6, 8  }
            }, prices);            

            var result = MLR.Predict(new double[,] { { 3000, 3, 40 }, { 2500, 4, 5 } }, coefs2); // 498408.25158031, 578876.03748933            
            Console.WriteLine(string.Join(" ", result));
            
            var coefs3 = MLR.Calculate(new double[,] {
                    { 0,   8,   9,   50000 },
                    { 0,   8,   6,   45000 },
                    { 5,   6,   7,   60000 },
                    { 2,   10,  10,  65000 },
                    { 7,   9,   6,   70000 },
                    { 3,   7,   10,  62000 },
                    { 10,  7,   7,   72000 },
                    { 11,  7,   8,   80000 }
            }, 3);            
            Console.WriteLine(string.Join(" ", MLR.Predict(new double[,] { { 2, 9, 6 }, { 12, 10, 10 } }, coefs3)));
            Console.WriteLine(MLR.Predict(new double[] { 12, 10, 10 }, coefs3));
            // Answer 53713.86 and 93747.79

        }
    }
}
