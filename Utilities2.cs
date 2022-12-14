using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Pub
{   
    public static class Utilities2    
    {               
        // create a functin to perfom a scaling
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

    }
}