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
            var tu = new double[] { 15584,  15585,  15586, 15587, 15588, 15589, 15590, 15591, 15592, 15593, 15594,  15595, 15596, 15597, 15598, 15599,  15600,  15601,  15602, 15603, 15604, 15605, 15606, 15607, 15608, 15609, 15610, 15611, 15612,  15613 };
            //var tu = Enumerable.Range(1, 16).Select(a => (double)a).ToArray();
            //var days = new double[] { 5,      6,      7,     1,     2,     3,     4,     5,     6,     7,     1,      2,     3,     4,     5,     6,      7,      1,      2,     3,     4,     5,     6,     7,     1,     2,     3,     4,     5,      6     };
            var data = new double[] { 111.11, 102.59, 82.03, 83.57, 47.58, 26.18, 37.06, 51.23, 73.16, 74.38, 108.09, 52.08, 26.05, 43.47, 69.22, 109.31, 114.58, 101.30, 40.25, 12.23, 22.27, 26.08, 66.09, 49.32, 89.02, 51.54, 34.04, 29.45, 103.59, 75.03 };
            //var data = new double[] { 4.8, 4.1, 6.0, 6.5, 5.8, 5.2, 6.8, 7.4, 6.0, 5.6, 7.5, 7.8, 6.3, 5.9, 8.0, 8.4 };

            var ts = new TSLR(data);
            var forecast = ts.Forecast(10);

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

            var dataset = ReadCSV("c:\\pub\\test.csv");
            var coefs5 = MLR.Calculate(dataset, 4);
            var yhat = MLR.Predict(new double[] { 55, 50, 0, 2.1 }, coefs5);
        }

        // create a function to read a csv file and put it in a matrix
        public static double[,] ReadCSV(string path)
        {
            var lines = File.ReadAllLines(path);
            var matrix = new double[lines.Length, lines[0].Split(',').Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Split(',');
                for (int j = 0; j < line.Length; j++)
                {
                    matrix[i, j] = double.Parse(line[j]);
                }
            }
            return matrix;
        }

        
    }
}
