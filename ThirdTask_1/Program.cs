using System;
using System.Numerics;

namespace ThirdTask_1
{
    internal class Program
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
        static BigInteger ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
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

        public static BigInteger EulerFunction(BigInteger n)
        {
            int res = 0;

            for (BigInteger i = 2; i < n; i++)
                if (EuclideanAlgorithm(i, n) == 1)
                    res++;
            
            return res + 1;
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
    }
}