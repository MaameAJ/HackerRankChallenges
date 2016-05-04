using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerRankChallenges
{
    static class Solution
    {
        //TODO test multiplication, addition and subtraction methods in a test library
        static void Main(String[] args)
        {
           /* int n = Convert.ToInt32(Console.ReadLine());*/

            //if (n <= 12)
            //{
            //    Console.WriteLine(CalculateFactorial(n));
            //}
            //else if(n <= 20)
            //{
            //    Console.WriteLine(CalculateLongFactorial(n));
            //}
            //else
            //{
            //    Console.WriteLine(CalculateBigFactorial(n));
            //} 
            CalculateBigFactorial(100);
            Console.ReadKey();

        }

        static int CalculateFactorial(int n)
        {
            int answer;
            if(n < 0)
            {
                return -1;
            }
            else if(n == 0 || n == 1)
            {
                answer = 1;//return 1;
            }
            else
            {
                answer = /*return*/ n * CalculateFactorial(n - 1);
            }

            Console.WriteLine("{0}!\t{1}", n, answer);
            return answer;
        }

        static long CalculateLongFactorial(int n)
        {
            
            if(n < 13)
            {
                return CalculateFactorial(n);
            }
            else
            {
                long answer = n * CalculateLongFactorial(n - 1);
                Console.WriteLine("{0}!\t{1}", n, answer);
                return answer;
            }
        }

        static BigNumber CalculateBigFactorial(int n)
        {
            if(n < 1 || n > 100)
            {
                throw new NotSupportedException("n must be between 1 and 100 inclusive.");
            }
            else if(n <= 12)
            {
                return new BigNumber(CalculateFactorial(n));
            }
            else if( n <= 20)
            {
                return new BigNumber(CalculateLongFactorial(n));
            }
            else
            {
                BigNumber answer = new BigNumber(n) * CalculateBigFactorial(n - 1);
                Console.WriteLine("{0}!\t{1}", n, answer);
                return answer;
            }

        }

        public static List<int> ToList(this int i)
        {
            List<int> number = new List<int>();
            if(i == 0)
            {
                number.Insert(0, 0);
                return number;
            }

            while(i > 0)
            {
                number.Insert(0, i % 10);
                i /= 10;
            }

            return number;
        }

        public static List<int> ToList(this long i)
        {
            List<int> number = new List<int>();
            if (i == 0)
            {
                number.Insert(0, 0);
                return number;
            }

            while (i > 0)
            {
                long remainder = i % 10;
                number.Insert(0, (int) remainder);
                i /= 10;
            }

            return number;
        }
        
        class BigNumber : IComparable<BigNumber>
        {
            private List<int> digits;
            private bool negative;

            public int this[int index]
            {
                get { return digits[index]; }
            }

            public bool IsNegative
            {
                get { return negative; }
            }

            private BigNumber(List<int> list, bool isNegative = false)
            {
                negative = isNegative;

                if (list == null || list.Count == 0)
                {
                    digits = 0.ToList();
                }
                else 
                {
                    digits = new List<int>(list);

                    for (int i = 1; i < list.Count - 1 && list[0] == 0; i++)
                    {
                        if(i != 0)
                        {
                            digits.RemoveRange(0, i);
                            break;
                        }
                    }
                }
            }

            public BigNumber() : this(null)
            {

            }

            public BigNumber(int n) : this (Math.Abs(n).ToList(), isNegative: n < 0)
            {}

            public BigNumber(long n) : this(Math.Abs(n).ToList(), isNegative: n < 0) { }

            public override string ToString()
            {
                string str = String.Empty;
                for (int i = 0; i < digits.Count; i++)
                {
                    str += digits[i].ToString();
                }
                return str;
            }

            public static BigNumber operator +(BigNumber a, BigNumber b)
            {
                if (a == null || b == null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    List<int> augend, addend, sum = new List<int>();
                    int carry = 0, i = 0;

                    if(a.IsNegative && !b.IsNegative)
                    {
                        BigNumber subtrahend = new BigNumber(a.digits);
                        return a - subtrahend;
                    }
                    else if(b.IsNegative && !a.IsNegative)
                    {
                        BigNumber subtrahend = new BigNumber(b.digits);
                        return b - subtrahend;
                    }
                    else if (a.digits.Count >= b.digits.Count)
                    {
                        augend = new List<int>(a.digits);
                        addend = new List<int>(b.digits);
                    }
                    else
                    {
                        augend = new List<int>(b.digits);
                        addend = new List<int>(a.digits);
                    }

                    augend.Reverse();
                    addend.Reverse();

                    for (; i < addend.Count; i++)
                    {
                        int total = augend[i] + addend[i] + carry;
                        carry = total / 10;
                        sum.Insert(0, total % 10);
                    }

                    for (; i < augend.Count; i++)
                    {
                        int total = augend[i] + carry;
                        carry = total / 10;
                        sum.Insert(0, total % 10);
                    }

                    if (carry != 0)
                    {
                        sum.Insert(0, carry);
                    }

                    return new BigNumber (sum, a.IsNegative);

                }
            }

            public static BigNumber operator -(BigNumber a, BigNumber b)
            {
                if (a == null || b == null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    bool isNegative = false;
                    List<int> minuend = new List<int>(a.digits);
                    List<int> subtrahend = new List<int>(b.digits);

                    if(a.IsNegative != b.IsNegative)
                    {
                        BigNumber sum = new BigNumber(minuend) + new BigNumber(subtrahend);
                        sum.negative = a.IsNegative;
                        return sum;
                    }
                    else if(a.IsNegative && b.IsNegative)
                    {
                        Swap(ref minuend, ref subtrahend);
                        isNegative = true;
                    }

                    List<int> difference = new List<int>();
                    int borrow = 0, i = 0;

                    minuend.Reverse();
                    subtrahend.Reverse();

                    for (; i < subtrahend.Count; i++)
                    {

                        int diff = minuend[i] - subtrahend[i] - borrow;

                        if (diff < 0)
                        {
                            borrow = 1;
                            diff += 10;
                        }
                        else
                        {
                            borrow = 0;
                        }

                        difference.Insert(index: 0, item: diff);
                    }

                    for (; i < minuend.Count; i++)
                    {
                        int diff = minuend[i] - borrow;
                        if (diff < 0)
                        {
                            borrow = 1;
                            diff += 10;
                        }
                        else
                        {
                            borrow = 0;
                        }

                        difference.Insert(0, diff);
                    }

                    return new BigNumber(difference, isNegative: borrow > 0 || isNegative);
                }
            }

            public static BigNumber operator *(BigNumber a, BigNumber b)
            {
                if (a == null || b == null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    List<int> multiplicand, multiplier, midprod = new List<int>();
                    BigNumber product = new BigNumber();
                    int carry = 0;

                    if (a.digits.Count >= b.digits.Count)
                    {
                        multiplicand = new List<int>(a.digits);
                        multiplier = new List<int>(b.digits);
                    }
                    else
                    {
                        multiplicand = new List<int>(b.digits);
                        multiplier = new List<int>(a.digits);
                    }

                    multiplicand.Reverse();
                    multiplier.Reverse();

                    int i = 0;

                    for (int j = 0; i < multiplier.Count; i++, j++)
                    {
                        midprod.Clear();

                        for (int k = 0; k < j; k++)
                        {
                            midprod.Insert(0, 0);
                        }

                        for (int k = 0; k < multiplicand.Count; k++)
                        {

                            int prod = (multiplicand[k] * multiplier[i]) + carry;
                            carry = prod / 10;
                            midprod.Insert(0, prod % 10);
                        }

                        if (carry != 0)
                        {
                            midprod.Insert(0, carry);
                        }

                        product += new BigNumber(midprod, a.IsNegative != b.IsNegative);

                    }

                    return product;
                }
            }

            private static void Swap(ref List<int> a, ref List<int> b)
            {
                List<int> holder = a;
                a = b;
                b = holder;
            }

            public override bool Equals(object obj)
            {
                if(obj is BigNumber)
                {
                    BigNumber other = obj as BigNumber;
                    if(negative = other.negative && digits.Count == other.digits.Count)
                    {
                        for(int i = 0; i < digits.Count; i++)
                        {
                            if(digits[i] != other.digits[i])
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }

                return false;
            }

            public override int GetHashCode()
            {
                return digits.GetHashCode() * negative.GetHashCode();
            }

            public int CompareTo(BigNumber other)
            {
                if(!Equals(other))
                {
                    if(negative == other.negative)
                    {
                        if(digits.Count == other.digits.Count)
                        {
                            for(int i = digits.Count - 1; i >= 0; i--)
                            {
                                if(this[i] > other[i])
                                {
                                    return 1;
                                }
                                else if(this[i] < other[i])
                                {
                                    return -1;
                                }
                            }
                        }
                        else
                        {
                            return digits.Count.CompareTo(other.digits.Count);
                        }
                    }
                    else if(negative && !other.IsNegative)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }

                return 0;
            }
        }
    }

}
