using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class LargeNumber
    {
        public static string Add(string num1, string num2)
        {
            // Convert the strings to lists of digits and reverse them
            List<int> a = num1.Select(c => c - '0').Reverse().ToList();
            List<int> b = num2.Select(c => c - '0').Reverse().ToList();

            // Determine the length of the result
            int maxLength = Math.Max(a.Count, b.Count);
            List<int> result = new List<int>(maxLength + 1);

            // Perform the addition
            int carry = 0;
            for (int i = 0; i < maxLength; i++)
            {
                int sum = carry;
                if (i < a.Count) sum += a[i];
                if (i < b.Count) sum += b[i];

                result.Add(sum % 10);
                carry = sum / 10;
            }

            // Add the final carry, if any
            if (carry > 0) result.Add(carry);

            // Reverse the result list and convert it to a string
            result.Reverse();
            return new string(result.Select(x => (char)(x + '0')).ToArray());
        }

        public static string Subtract(string num1, string num2)
        {
            // Determine if the result is negative
            bool isNegative = false;
            if (Compare(num1, num2) < 0)
            {
                isNegative = true;
                string temp = num1;
                num1 = num2;
                num2 = temp;
            }

            // Convert the strings to lists of digits and reverse them
            List<int> a = num1.Select(c => c - '0').Reverse().ToList();
            List<int> b = num2.Select(c => c - '0').Reverse().ToList();

            // Determine the length of the result
            int maxLength = Math.Max(a.Count, b.Count);
            List<int> result = new List<int>(maxLength);

            // Perform the subtraction
            int carry = 0;
            for (int i = 0; i < maxLength; i++)
            {
                int diff = carry;
                if (i < a.Count) diff += a[i];
                if (i < b.Count) diff -= b[i];

                if (diff < 0)
                {
                    diff += 10;
                    carry = -1;
                }
                else
                {
                    carry = 0;
                }

                result.Add(diff);
            }

            // Add 10 to the result if the final carry is -1
            if (carry == -1) result.Add(10);

            // Reverse the result list and convert it to a string
            result.Reverse();
            string resultStr = new string(result.Select(x => (char)(x + '0')).ToArray());

            // Prepend a minus sign if the result is negative
            if (isNegative) resultStr = "-" + resultStr;

            return resultStr;
        }

        public static string Multiply(string num1, string num2)
        {
            // Convert the strings to lists of digits and reverse them
            List<int> a = num1.Select(c => c - '0').Reverse().ToList();
            List<int> b = num2.Select(c => c - '0').Reverse().ToList();

            // Initialize the result list and carry variable
            List<int> result = new List<int>();
            int carry = 0;

            // Perform the multiplication
            for (int i = 0; i < a.Count; i++)
            {
                int digit = a[i];
                for (int j = 0; j < b.Count; j++)
                {
                    int product = digit * b[j] + carry;
                    int index = i + j;
                    if (index >= result.Count)
                    {
                        result.Add(product % 10);
                    }
                    else
                    {
                        result[index] += product % 10;
                    }
                    carry = product / 10;
                }

                // Add the carry to the result
                if (carry > 0)
                {
                    result.Add(carry);
                    carry = 0;
                }
            }

            // Handle carrying over digits
            for (int i = 0; i < result.Count - 1; i++)
            {
                if (result[i] >= 10)
                {
                    result[i + 1] += result[i] / 10;
                    result[i] %= 10;
                }
            }

            // Reverse the result list and convert it to a string
            result.Reverse();
            return new string(result.Select(x => (char)(x + '0')).ToArray());
        }

        public static string Subtract2(string a, string b)
        {
            string result = "";
            int borrow = 0;
            for (int i = 0; i < a.Length; i++)
            {
                int digitA = a[a.Length - 1 - i] - '0';
                int digitB = i < b.Length ? b[b.Length - 1 - i] - '0' : 0;
                int diff = digitA - digitB - borrow;
                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result = diff + result;
            }
            return result.TrimStart('0');
        }

        public static string Divide(string dividend, string divisor)
        {            
            // Check for edge cases
            if (dividend == "0") return "0";
            if (divisor == "0") throw new DivideByZeroException();

            // Initialize variables
            string result = "";
            string remainder = "";
            int divisorLength = divisor.Length;

            // Iterate through each digit of the dividend
            for (int i = 0; i < dividend.Length; i++)
            {
                // Append the current digit to the remainder
                remainder += dividend[i];

                // Calculate the number of times the divisor goes into the current remainder
                int quotient = 0;
                while (Compare(remainder, divisor) >= 0)
                {
                    remainder = Subtract2(remainder, divisor);
                    quotient++;
                }

                // Append the quotient to the result
                result += quotient;

                // If this is the last digit of the dividend and the remainder is not zero,
                // append the remainder to the result
                if (i == dividend.Length - 1 && remainder != "0")
                {
                    result += "." + remainder;
                }

                // If the remainder is zero and we have reached the desired precision,
                // break out of the loop
                if (remainder == "0" && i >= dividend.Length - divisorLength)
                {
                    break;
                }
            }

            return result;
        }

        //public static string Divide(string num1, string num2)
        //{
        //    // Convert the strings to lists of digits and reverse them
        //    List<int> a = num1.Select(c => c - '0').Reverse().ToList();
        //    List<int> b = num2.Select(c => c - '0').Reverse().ToList();

        //    // Initialize the result list and the dividend list
        //    List<int> result = new List<int>();
        //    List<int> dividend = new List<int>();

        //    // Divide the numbers digit by digit
        //    for (int i = 0; i < a.Count; i++)
        //    {
        //        dividend.Add(a[i]);

        //        // Determine the quotient for this digit
        //        int quotient = 0;
        //        while (CompareLists(dividend, b) >= 0)
        //        {
        //            dividend = SubtractLists(dividend, b);
        //            quotient++;
        //        }

        //        result.Add(quotient);

        //        // If the dividend is non-empty and has leading zeros, remove them
        //        while (dividend.Count > 1 && dividend[dividend.Count - 1] == 0)
        //        {
        //            dividend.RemoveAt(dividend.Count - 1);
        //        }
        //    }

        //    // If the dividend is not empty, divide the remaining digits
        //    while (dividend.Count > 0)
        //    {
        //        // Shift the dividend left by one digit
        //        dividend.Insert(0, 0);

        //        // Determine the quotient for this digit
        //        int quotient = 0;
        //        while (CompareLists(dividend, b) >= 0)
        //        {
        //            dividend = SubtractLists(dividend, b);
        //            quotient++;
        //        }

        //        result.Add(quotient);

        //        // If the dividend is non-empty and has leading zeros, remove them
        //        while (dividend.Count > 1 && dividend[dividend.Count - 1] == 0)
        //        {
        //            dividend.RemoveAt(dividend.Count - 1);
        //        }

        //        // If the dividend is smaller than the divisor, break out of the loop
        //        if (CompareLists(dividend, b) < 0)
        //        {
        //            break;
        //        }
        //    }

        //    // Reverse the result list and convert it to a string
        //    result.Reverse();
        //    return string.Join("", result);
        //}

        private static List<int> SubtractLists(List<int> a, List<int> b)
        {
            // Initialize the result list
            List<int> result = new List<int>(a);

            // Subtract the lists element by element
            for (int i = 0; i < b.Count; i++)
            {
                result[i] -= b[i];
            }

            // Handle carrying over digits
            for (int i = 0; i < result.Count - 1; i++)
            {
                if (result[i] < 0)
                {
                    result[i] += 10;
                    result[i + 1]--;
                }
            }

            // If the result has leading zeros, remove them
            while (result.Count > 1 && result[result.Count - 1] == 0)
            {
                result.RemoveAt(result.Count - 1);
            }

            return result;
        }

        private static int CompareLists(List<int> a, List<int> b)
        {
            // Compare the lists element by element
            for (int i = 0; i < a.Count; i++)
            {
                if (i >= b.Count)
                {
                    return 1;
                }
                if (a[i] > b[i])
                {
                    return 1;
                }
                if (a[i] < b[i])
                {
                    return -1;
                }
            }

            // If the lists are equal in length, return 0
            if (a.Count == b.Count)
            {
                return 0;
            }

            // Otherwise, return -1
            return -1;
        }

        private static int Compare(string num1, string num2)
        {
            // Compare the lengths of the numbers
            if (num1.Length > num2.Length) return 1;
            if (num1.Length < num2.Length) return -1;

            // Compare the digits of the numbers
            for (int i = 0; i < num1.Length; i++)
            {
                if (num1[i] > num2[i]) return 1;
                if (num1[i] < num2[i]) return -1;
            }

            // The numbers are equal
            return 0;
        }

    }
}
