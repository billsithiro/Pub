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