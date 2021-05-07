using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ThirdTask_1
{
    public class Program
    {
        // вычисление символа Лежандра;
        // вычисление символа Якоби
        
        
        static BigInteger EuclideanAlgorithm(BigInteger m, BigInteger n){
            BigInteger nod;
            
            while(m != n)
            {
                if(m > n)
                {
                    m = m - n;
                }
                else
                {
                    n = n - m;
                }
            }

            nod = n;
            return nod;
        }
        public static BigInteger ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            
    
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
 
            BigInteger gcd = ExtendedEuclideanAlgorithm(b % a, a, out x, out y);
    
            BigInteger newY = x;
            BigInteger newX = y - (b / a) * x;
    
            x = newX;
            y = newY;
            return gcd;
        }
        public static BigInteger FastPowModeBigInt(BigInteger number, uint pow, BigInteger mode)
        {
            BigInteger result = 1;      
            while (pow != 0) {
                if (pow % 2 == 1)  result = (result * number) % mode;
                pow >>= 1;
                result = (result * result) % mode;
            }
            return result;
        }

        // public static BigInteger EulerFunction(BigInteger n)
        // {
        //     int res = 0;
        //
        //     for (BigInteger i = 2; i < n; i++)
        //         if (EuclideanAlgorithm(i, n) == 1)
        //             res++;
        //     
        //     return res + 1;
        // }
        public static BigInteger EulerFunction(BigInteger m)
        {
            if (m < 2)
                throw new ArgumentException("m must be >= 2.");

            FactorOut(m, out List<BigInteger> primes, out List<int> degrees);
            BigInteger result = 1;
            for (int i = 0; i < primes.Count; i++)
            {
                BigInteger tm = BigInteger.Pow(primes[i], degrees[i] - 1);
                result *= primes[i] * tm - tm;
            }
            return result;
        }

        // https://e-maxx.ru/algo/euler_function вычисление функции эйлера через факторизацию числа
        public static void FactorOut(BigInteger value, out List<BigInteger> primes, out List<int> degrees)
        {
            if (value < 1)
                throw new ArgumentException("value must be >= 1.");
            else if (value < 3)
            {
                primes = new List<BigInteger> { value };
                degrees = new List<int> { 1 };
                return;
            }
            
            primes = GetPrimeNumbers(Sqrt(value) + 1);
            degrees = new List<int>(primes.Count);
            for (int i = 0; i < primes.Count; ++i) degrees.Add(0);

            for (int i = 0; i < primes.Count; ++i)
            {
                while (value % primes[i] == 0)
                {
                    value /= primes[i];
                    degrees[i]++;
                }
                if (value == 1)
                    break;
            }
            if (value > 1)
            {
                primes.Add(value);
                degrees.Add(1);
            }

            for (int i = primes.Count - 1; i > -1; --i)
            {
                if (degrees[i] == 0)
                {
                    primes.RemoveAt(i);
                    degrees.RemoveAt(i);
                }
            }
        }
        public static List<BigInteger> GetPrimeNumbers(BigInteger m)
        {
            if (m <= 2)
                throw new ArgumentException("m must be > 2.");
            
            List<BigInteger> result = new List<BigInteger>();
            for (BigInteger i = 2; i < m; ++i)
                if (CheckPrimeNumber(i))
                    result.Add(i);
            return result;
        }

        public static bool CheckPrimeNumber(BigInteger number)
        {
            for (BigInteger i = 2; i < Sqrt(number); i++)
            {
                if (number / i != 0)
                    return false;
            }

            return true;
        }
        
        public static BigInteger Sqrt(BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static Boolean isSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root*root;
            BigInteger upperBound = (root + 1)*(root + 1);

            return (n >= lowerBound && n < upperBound);
        }
        
        
        
        
        
        
        
        


        public static void Main(string[] args)
        {
            // int x;
            // int y;
            //
            // Console.WriteLine(Gcd(98, 178, out x, out y));
            // Console.WriteLine(x);
            // Console.WriteLine(y);
            BigInteger a = 7, b = 3, x, y, gcd;
            gcd = ExtendedEuclideanAlgorithm(a, b, out x, out y);
            Console.WriteLine($"{x} * {a} + {y} * {b} = {gcd}"); // -2 * 7 + 1 * 3 = 1
            gcd = ExtendedEuclideanAlgorithm(b, a, out y, out x);
            Console.WriteLine($"{x} * {a} + {y} * {b} = {gcd}"); // 1 * 7 + -2 * 3 = 1

            Console.WriteLine(EulerFunction(7));
        }




        // public static int CalcSymbolL(BigInteger a, BigInteger p)
        // {
        //     bool negative = false;
        //     if (a.Sign == -1)
        //     {
        //         a *= -1;
        //         negative = true;
        //     }
        //
        //     a = a % p;
        //     FactorOut(a, out List<BigInteger> primes, out List<int> degrees);
        // }
        
    }
}