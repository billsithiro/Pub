using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public static class Utilities
    {
        public static void MakeTrainTest(double[][] allData, out double[][] trainData, out double[][] testData)
        {
            // split allData into 80% trainData and 20% testData
            Random rnd = new Random(0);
            int totRows = allData.Length;
            int numCols = allData[0].Length;

            int trainRows = (int)(totRows * 0.80); // hard-coded 80-20 split
            int testRows = totRows - trainRows;

            trainData = new double[trainRows][];
            testData = new double[testRows][];

            int[] sequence = new int[totRows]; // create a random sequence of indexes
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }

            int si = 0; // index into sequence[]
            int j = 0; // index into trainData or testData

            for (; si < trainRows; ++si) // first rows to train data
            {
                trainData[j] = new double[numCols];
                int idx = sequence[si];
                Array.Copy(allData[idx], trainData[j], numCols);
                ++j;
            }

            j = 0; // reset to start of test data
            for (; si < totRows; ++si) // remainder to test data
            {
                testData[j] = new double[numCols];
                int idx = sequence[si];
                Array.Copy(allData[idx], testData[j], numCols);
                ++j;
            }
        } // MakeTrainTest

        public static void Normalize(double[][] dataMatrix, int[] cols)
        {
            // normalize specified cols by computing (x - mean) / sd for each value
            foreach (int col in cols)
            {
                double sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += dataMatrix[i][col];
                double mean = sum / dataMatrix.Length;
                sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += (dataMatrix[i][col] - mean) * (dataMatrix[i][col] - mean);
                // thanks to Dr. W. Winfrey, Concord Univ., for catching bug in original code
                double sd = Math.Sqrt(sum / (dataMatrix.Length - 1));
                for (int i = 0; i < dataMatrix.Length; ++i)
                    dataMatrix[i][col] = (dataMatrix[i][col] - mean) / sd;
            }
        }

        public static void ShowVector(double[] vector, int valsPerRow, int decimals, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
            {
                if (i % valsPerRow == 0) Console.WriteLine("");
                Console.Write(vector[i].ToString("F" + decimals).PadLeft(decimals + 4) + " ");
            }
            if (newLine == true) Console.WriteLine("");
        }

        public static void ShowMatrix(double[][] matrix, int numRows, int decimals, bool newLine)
        {
            for (int i = 0; i < numRows; ++i)
            {
                Console.Write(i.ToString().PadLeft(3) + ": ");
                for (int j = 0; j < matrix[i].Length; ++j)
                {
                    if (matrix[i][j] >= 0.0) Console.Write(" "); else Console.Write("-");
                    Console.Write(Math.Abs(matrix[i][j]).ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine == true) Console.WriteLine("");
        }
        
        public static double[] MinMaxScale(double[] values, double min, double max)
        {
            var result = new double[values.Length];
            var minVal = values.Min();
            var maxVal = values.Max();
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = (values[i] - minVal) / (maxVal - minVal) * (max - min) + min;
            }
            return result;
        }

        // create a function to calculate var.s
        public static double VarS(double[] values)
        {
            var mean = values.Average();
            var sum = 0.0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += Math.Pow(values[i] - mean, 2);
            }
            return sum / (values.Length - 1);
        }

        // create a function to calculate f.dist.rt
        public static double FDistRT(double x, double df1, double df2)
        {
            var result = 0.0;
            var a = df1 * x / (df1 * x + df2);
            var b = df1 / 2;
            var c = df2 / 2;
            result = 1 - BetaDist(a, b, c);
            return result;
        }

        // create a function to calculate beta.dist
        public static double BetaDist(double x, double a, double b)
        {
            var result = 0.0;
            var c = Math.Pow(x, a - 1) * Math.Pow(1 - x, b - 1);
            var d = Math.Pow(x, a) * Math.Pow(1 - x, b);
            result = c / d;
            return result;
        }

        // create a function to perform a fast fourier transform
        public static double[] FFT(double[] values)
        {
            var result = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var sum = 0.0;
                for (int j = 0; j < values.Length; j++)
                {
                    var angle = 2 * Math.PI * i * j / values.Length;
                    sum += values[j] * Math.Cos(angle);
                }
                result[i] = sum;
            }
            return result;
        }

        // create a function to perform a fast inverse fourier transform
        public static double[] IFFT(double[] values)
        {
            var result = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var sum = 0.0;
                for (int j = 0; j < values.Length; j++)
                {
                    var angle = 2 * Math.PI * i * j / values.Length;
                    sum += values[j] * Math.Sin(angle);
                }
                result[i] = sum / values.Length;
            }
            return result;
        }

        // create a function to perform a discrete cosine transform
        public static double[] DCT(double[] values)
        {
            var result = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var sum = 0.0;
                for (int j = 0; j < values.Length; j++)
                {
                    var angle = Math.PI * i * (j + 0.5) / values.Length;
                    sum += values[j] * Math.Cos(angle);
                }
                result[i] = sum;
            }
            return result;
        }

        // create a function to perform a discrete sine transform
        public static double[] DST(double[] values)
        {
            var result = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var sum = 0.0;
                for (int j = 0; j < values.Length; j++)
                {
                    var angle = Math.PI * i * (j + 0.5) / values.Length;
                    sum += values[j] * Math.Sin(angle);
                }
                result[i] = sum;
            }
            return result;
        }

        public static string InfixToPostfix(string expression)
        {
            int Precedence(char c)
            {
                switch (c)
                {
                    case '+':
                    case '-':
                        return 1;
                    case '*':
                    case '/':
                        return 2;
                    case '^':
                        return 3;
                    default:
                        return -1;
                }
            }

            var stack = new Stack<char>();
            var output = new StringBuilder();
            foreach (var c in expression)
            {
                if (c == ' ')
                    continue;

                if (char.IsDigit(c) || c == '.')
                {
                    output.Append(c);
                }
                else if (c == '(')
                {
                    stack.Push(c);
                }
                else if (c == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        output.Append(' ');
                        output.Append(stack.Pop());
                    }
                    if (stack.Count > 0 && stack.Peek() != '(')
                    {
                        throw new ArgumentException("Invalid expression");
                    }
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && Precedence(c) <= Precedence(stack.Peek()))
                    {
                        output.Append(' ');
                        output.Append(stack.Pop());
                    }
                    output.Append(' ');
                    stack.Push(c);
                }
            }

            output.Append(' ');
            while (stack.Count > 0)
                output.Append(stack.Pop());

            return output.ToString();
        }

        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }
}
