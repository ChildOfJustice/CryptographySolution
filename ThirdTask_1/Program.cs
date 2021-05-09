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
        public static BigInteger FastPowModeBigInt(BigInteger number, BigInteger pow, BigInteger mode)
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
            // BigInteger a = 7, b = 3, x, y, gcd;
            // gcd = ExtendedEuclideanAlgorithm(a, b, out x, out y);
            // Console.WriteLine($"{x} * {a} + {y} * {b} = {gcd}"); // -2 * 7 + 1 * 3 = 1
            // gcd = ExtendedEuclideanAlgorithm(b, a, out y, out x);
            // Console.WriteLine($"{x} * {a} + {y} * {b} = {gcd}"); // 1 * 7 + -2 * 3 = 1
            //
            // Console.WriteLine(EulerFunction(7));


            // Console.WriteLine(CalcSymbolL(3,11));
            // Console.WriteLine(CalcSymbolL(6,7));
            // Console.WriteLine(CalcSymbolL(68,113));
            Console.WriteLine(L(3,11));
            Console.WriteLine(L(6,7));
            Console.WriteLine(L(68,113));

            Console.WriteLine();
            
            Console.WriteLine(J(2,15));
            Console.WriteLine(J(506,1103));
            Console.WriteLine(J(-286,4272943));
        }
        public static int L(int a, int p)
        {
            if (ExtendedEuclideanAlgorithm(a, p, out BigInteger x1, out BigInteger y1) != 1)
                return 0;
            
            if (a == 1)
            {
                return 1;
            }
            if (a % 2 == 0)
            {
                return ((int)(L(a / 2, p) * Math.Pow(-1, (p * p - 1) / 8)));
            }
            if ((a % 2 != 0) && (a != 1))
            {
                return ((int)(L(p % a, a) * Math.Pow(-1, (a - 1) * (p - 1) / 4)));
            }
            return 0;
        }

        public static int J(int a, int m)
        {
            if (ExtendedEuclideanAlgorithm(a, m, out BigInteger x1, out BigInteger y1) != 1)
                return 0;
            int x = a; //Заводим переменные x, y, s под a, m
                int y = m; // и знак вычисляемого символа (a/m).
                int s = 1;
                if (a < 0) //Если a - отрицательное число
                {
                    x = -a; //выносим (по свойству 4)
                    s = (int)Math.Pow((-1), ((m - 1) / 2)); //символ (-1/m) по формуле (свойство 2).???
                }

                while (true)
                {
                    int t = 0; //Количество двоек, вынесенных из "числителя" символа.
                    int c = x % y; //Заменяем a остатком по модулю m
                    x = c; //в соответствии со свойством 1.
                    if (x == 0)
                    {
                        return 1; //Если m делится на a, (a/m)=1
                    }
                    else
                    {
                        while (x % 2 == 0) //Пока a четно, выносим (свойство 4)
                        {
                            x = x / 2; //двойки, считаем их количество
                            t++;
                        }
                        if (t % 2 == 1) s = (int)(s * Math.Pow( (-1) ,((y * y - 1) / 8))); //Четное количество двоек влияет на знак,для нечетного - по формуле

                        //Имеем (a/m), где a, m - нечетные.
                        //Используем формулу (свойство 5) и перевернем символ Якоби,если мы не можем вычислить его по свойству 2, т.е. a! = 1.
                        if(x > 1)
                        {
                            s = (int)(s * Math.Pow((-1), (((x - 1) / 2) * ((y - 1) / 2))));
                            c = x;
                            x = y;
                            y = c;
                        }
                        else //Иначе если a = 1, (a/m)=1.
                        {
                            break; //Вычисление окончено.
                        }
                    }
                }
                return s;
        }

        public static int Jacobi(BigInteger a, BigInteger b)
        {
            int r = 1;
            while (a != 0)
            {
                int t = 0;
                while ((a & 1) == 0)
                {
                    t++;
                    a >>= 1;
                }

                if ((t & 1) != 0)
                {
                    BigInteger temp = b % 8;
                    if (temp == 3 || temp == 5)
                    {
                        r = -r;
                    }
                }

                BigInteger a4 = a % 4, b4 = b % 4;
                if (a4 == 3 && b4 == 3)
                {
                    r = -r;
                }

                BigInteger c = a;
                a = b % c;
                b = c;
            }

            return r;
        }

        public static BigInteger BinaryGCD(BigInteger A, BigInteger B)
        {
            BigInteger k = 1;
            while ((A != 0) && (B != 0))
            {
                while (((A & 1) == 0) && ((B & 1) == 0))
                {
                    A >>= 1;
                    B >>= 1;
                    k <<= 1;
                }
                while ((A & 1) == 0) A >>= 1;
                while ((B & 1) == 0) B >>= 1;
                if (A >= B) A -= B; else B -= A;
            }
            return B * k;
        }









        //Do not work
        public static BigInteger CalcSymbolL(BigInteger a, BigInteger p)
        {
            //критерий Эйлера:
            return FastPowModeBigInt(a, (p - 1) / 2, p);
        }
        public static BigInteger CalcSymbolJ(BigInteger a, BigInteger p)
        {
            //закон взаимности
            BigInteger powerForOne = ((p - 1) / 2) * ((a - 1) / 2);
            int sign = 1;
            if (!powerForOne.IsEven)
                sign = -1;

            return sign*CalcSymbolL(a,p);
        }
    }
}