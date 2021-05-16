using System;
using System.Numerics;
using System.Security.Cryptography;

namespace ThirdTask_2
{
    public class PrimeTests
    {
        
        public static bool miillerTest(BigInteger d, BigInteger n)
        {
         
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] _a = new byte[n.ToByteArray().LongLength];

            BigInteger a;

            do
            {
                rng.GetBytes(_a);
                a = new BigInteger(_a);
            } while (a < 2 || a >= n - 2);

            //BigInteger a = 2 + (int)(r.Next() % (n - 4));
     
            // Compute a^d % n
            BigInteger x = BigInteger.ModPow(a, d, n);
     
            if (x == 1 || x == n - 1)
                return true;
     
            // Keep squaring x while one of the
            // following doesn't happen
            // (i) d does not reach n-1
            // (ii) (x^2) % n is not 1
            // (iii) (x^2) % n is not n-1
            while (d != n - 1)
            {
                x = (x * x) % n;
                d *= 2;
         
                if (x == 1)
                    return false;
                if (x == n - 1)
                    return true;
            }
     
            // Return composite
            return false;
        }
        public static bool RabinMillerTestisPrime(BigInteger n, int k)
        {
         
            // Corner cases
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;
     
            // Find r such that n = 2^d * r + 1
            // for some r >= 1
            BigInteger d = n - 1;
         
            while (d % 2 == 0)
                d /= 2;
     
            // Iterate given nber of 'k' times
            for (int i = 0; i < k; i++)
                if (!miillerTest(d, n))
                    return false;
     
            return true;
        }
        
        public static bool RabinMillerTest(BigInteger source, int certainty)
        {
            //Filter out basic primes.
            if (source == 2 || source == 3)
            {
                return true;
            }
            //Below 2, and % 0? Not prime.
            if (source < 2 || source % 2 == 0)
            {
                return false;
            }

            //Finding even integer below number.
            BigInteger d = source - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            //Getting a random BigInt using bytes.
            Random rng = new Random(Environment.TickCount);
            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            //Looping to check random factors.
            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    //Generating new random bytes to check as a factor.
                    rng.NextBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= source - 2);

                //Checking for x=1 or x=s-1.
                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                {
                    continue;
                }

                //Iterating to check for prime.
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                    {
                        return false;
                    }
                    else if (x == source - 1)
                    {
                        break;
                    }
                }

                if (x != source - 1)
                {
                    return false;
                }
            }

            //All tests have failed to prove composite, so return prime.
            return true;
        }


        // public static bool RabinMillerTest(BigInteger thisVal, int confidence)
        // {
        //  
        //         if((thisVal.Sign) == -1)        // negative
        //                 thisVal = -thisVal;
        //
        //
        //         if (thisVal.IsEven)
        //          return false;
        //
        //
        //         // calculate values of s and t
        //         BigInteger p_sub1 = thisVal - (new BigInteger(1));
        //         int s = 0;
        //
        //         for(int index = 0; index < p_sub1.dataLength; index++)
        //         {
        //                 uint mask = 0x01;
        //
        //                 for(int i = 0; i < 32; i++)
        //                 {
        //                         if((p_sub1.data[index] & mask) != 0)
        //                         {
        //                                 index = p_sub1.dataLength;      // to break the outer loop
        //                                 break;
        //                         }
        //                         mask <<= 1;
        //                         s++;
        //                 }
        //         }
        //
        //         BigInteger t = p_sub1 >> s;
        //
        //  int bits = thisVal.bitCount();
        //  BigInteger a = new BigInteger();
        //  Random rand = new Random();
        //
        //  for(int round = 0; round < confidence; round++)
        //  {
        //   bool done = false;
        //
        //   while(!done)		// generate a < n
        //   {
        //    int testBits = 0;
        //
        //    // make sure "a" has at least 2 bits
        //    while(testBits < 2)
        //     testBits = (int)(rand.NextDouble() * bits);
        //
        //    a.genRandomBits(testBits, rand);
        //
        //    int byteLen = a.dataLength;
        //
        //                         // make sure "a" is not 0
        //    if(byteLen > 1 || (byteLen == 1 && a.data[0] != 1))
        //     done = true;
        //   }
        //
        //                 // check whether a factor exists (fix for version 1.03)
        //   BigInteger gcdTest = a.gcd(thisVal);
        //                 if(gcdTest.dataLength == 1 && gcdTest.data[0] != 1)
        //                         return false;
        //
        //                 BigInteger b = a.modPow(t, thisVal);
        //
        //                 /*
        //                 Console.WriteLine("a = " + a.ToString(10));
        //                 Console.WriteLine("b = " + b.ToString(10));
        //                 Console.WriteLine("t = " + t.ToString(10));
        //                 Console.WriteLine("s = " + s);
        //                 */
        //
        //                 bool result = false;
        //
        //                 if(b.dataLength == 1 && b.data[0] == 1)         // a^t mod p = 1
        //                         result = true;
        //
        //                 for(int j = 0; result == false && j < s; j++)
        //                 {
        //                         if(b == p_sub1)         // a^((2^j)*t) mod p = p-1 for some 0 <= j <= s-1
        //                         {
        //                                 result = true;
        //                                 break;
        //                         }
        //
        //                         b = (b * b) % thisVal;
        //                 }
        //
        //                 if(result == false)
        //                         return false;
        //         }
        //  return true;
        // }
        //
        // public static bool SolovayStrassenTest(BigInteger thisVal, int confidence)
        // {
        //  
        //         if((thisVal.data[BigInteger.maxLength-1] & 0x80000000) != 0)        // negative
        //                 thisVal = -thisVal;
        //      
        //
        //         if(thisVal.dataLength == 1)
        //         {
        //                 // test small numbers
        //                 if(thisVal.data[0] == 0 || thisVal.data[0] == 1)
        //                         return false;
        //                 else if(thisVal.data[0] == 2 || thisVal.data[0] == 3)
        //                         return true;
        //         }
        //
        //         if((thisVal.data[0] & 0x1) == 0)     // even numbers
        //                 return false;
        //
        //
        //  int bits = thisVal.bitCount();
        //  BigInteger a = new BigInteger();
        //  BigInteger p_sub1 = thisVal - 1;
        //  BigInteger p_sub1_shift = p_sub1 >> 1;
        //
        //  Random rand = new Random();
        //
        //  for(int round = 0; round < confidence; round++)
        //  {
        //   bool done = false;
        //
        //   while(!done)		// generate a < n
        //   {
        //    int testBits = 0;
        //
        //    // make sure "a" has at least 2 bits
        //    while(testBits < 2)
        //     testBits = (int)(rand.NextDouble() * bits);
        //
        //    a.genRandomBits(testBits, rand);
        //
        //    int byteLen = a.dataLength;
        //
        //                         // make sure "a" is not 0
        //    if(byteLen > 1 || (byteLen == 1 && a.data[0] != 1))
        //     done = true;
        //   }
        //
        //                 // check whether a factor exists (fix for version 1.03)
        //   BigInteger gcdTest = a.gcd(thisVal);
        //                 if(gcdTest.dataLength == 1 && gcdTest.data[0] != 1)
        //                         return false;
        //
        //   // calculate a^((p-1)/2) mod p
        //
        //   BigInteger expResult = a.modPow(p_sub1_shift, thisVal);
        //   if(expResult == p_sub1)
        //           expResult = -1;
        //
        //                 // calculate Jacobi symbol
        //                 BigInteger jacob = Jacobi(a, thisVal);
        //
        //                 //Console.WriteLine("a = " + a.ToString(10) + " b = " + thisVal.ToString(10));
        //                 //Console.WriteLine("expResult = " + expResult.ToString(10) + " Jacob = " + jacob.ToString(10));
        //
        //                 // if they are different then it is not prime
        //                 if(expResult != jacob)
        //    return false;
        //  }
        //
        //  return true;
        // }
        //
        // public static bool FermaTest(System.Numerics.BigInteger n, int k)
        //  {
        //      // если n == 2 или n == 3 - эти числа простые, возвращаем true
        //      if (n == 2 || n == 3)
        //          return true;
        //
        //      // если n < 2 или n четное - возвращаем false
        //      if (n < 2 || n % 2 == 0)
        //          return false;
        //
        //      // if (startPosCheckFrom < 1)
        //      //     throw new ArgumentException("Cannot check from this position");
        //
        //      // for(BigInteger a = startPosCheckFrom; a < n-1; a++)
        //      // {
        //      //     if (MHP(a, n-1, n) != 1) 
        //      //     {
        //      //         return false;
        //      //     }
        //      // }
        //      
        //      
        //      // повторить k раз
        //      for (int i = 0; i < k; i++)
        //      {
        //          // выберем случайное целое число a в отрезке [2, n − 2]
        //          RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //
        //          byte[] _a = new byte[n.ToByteArray().LongLength];
        //
        //          System.Numerics.BigInteger a;
        //
        //          do
        //          {
        //              rng.GetBytes(_a);
        //              a = new System.Numerics.BigInteger(_a);
        //           //Console.WriteLine("choosing..");
        //          }
        //          while (a < 2 || a >= n - 2);
        //
        //          if (ThirdTask_1.Program.FastPowMod(a, n - 1, n) != 1)
        //          {
        //           // Console.WriteLine("FOUND! " + a);
        //           // //Console.WriteLine(ThirdTask_1.Program.ExtendedEuclideanAlgorithm());
        //           // Console.WriteLine(ThirdTask_1.Program.FastPowMod(a,n-1,n));
        //           // Console.WriteLine(System.Numerics.BigInteger.ModPow(a, n - 1, n));
        //           return false;
        //          }
        //          // if (MHP(a, n-1, n) != 1) 
        //          // {
        //          //     return false;
        //          //     
        //          // }
        //      }
        //      
        //      
        //      return true;
        //  }
        // public static System.Numerics.BigInteger MHP(System.Numerics.BigInteger a, System.Numerics.BigInteger k, System.Numerics.BigInteger b)
        //  {
        //      //return ThirdTask_1.Program.FastPowModeBigInt(a, k % ThirdTask_1.Program.EulerFunction(b), b);
        //      return System.Numerics.BigInteger.ModPow(a, k % ThirdTask_1.Program.EulerFunction(b), b);
        //      //return Math.Pow(a, k % ThirdTask_1.Program.EulerFunction(b)) % b;
        //  }
        //
        //
        //
        // public static int Jacobi(BigInteger a, BigInteger b)
        // {
        //  // Jacobi defined only for odd integers
        //  if((b.data[0] & 0x1) == 0)
        //   throw (new ArgumentException("Jacobi defined only for odd integers."));
        //
        //  if(a >= b)      a %= b;
        //  if(a.dataLength == 1 && a.data[0] == 0)      return 0;  // a == 0
        //  if(a.dataLength == 1 && a.data[0] == 1)      return 1;  // a == 1
        //
        //  if(a < 0)
        //  {
        //   if( (((b-1).data[0]) & 0x2) == 0)       //if( (((b-1) >> 1).data[0] & 0x1) == 0)
        //    return Jacobi(-a, b);
        //   else
        //    return -Jacobi(-a, b);
        //  }
        //
        //  int e = 0;
        //  for(int index = 0; index < a.dataLength; index++)
        //  {
        //   uint mask = 0x01;
        //
        //   for(int i = 0; i < 32; i++)
        //   {
        //    if((a.data[index] & mask) != 0)
        //    {
        //     index = a.dataLength;      // to break the outer loop
        //     break;
        //    }
        //    mask <<= 1;
        //    e++;
        //   }
        //  }
        //
        //  BigInteger a1 = a >> e;
        //
        //  int s = 1;
        //  if((e & 0x1) != 0 && ((b.data[0] & 0x7) == 3 || (b.data[0] & 0x7) == 5))
        //   s = -1;
        //
        //  if((b.data[0] & 0x3) == 3 && (a1.data[0] & 0x3) == 3)
        //   s = -s;
        //
        //  if(a1.dataLength == 1 && a1.data[0] == 1)
        //   return s;
        //  else
        //   return (s * Jacobi(b % a1, a1));
        // }
        //
    }
}