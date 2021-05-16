using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml;

namespace ThirdTask_2
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Console.WriteLine(GeneratePrimeNumber(512, 10));


            //Console.WriteLine(PrimeTests.RabinMillerTestisPrime(GeneratePrimeNumber(500, 10), 10));
            Console.WriteLine(PrimeTests.RabinMillerTest(FindPrime(512, 10), 10));
            //     Console.WriteLine(PrimeTests.FermaTest(991, 15));
            //     
            //     //return;
            //         
            //         
            //     BigInteger prime = 0;
            //
            //     var first2048BitNumber = BigInteger.Pow(2, 500);
            //     var last2048BitNumber = BigInteger.Pow(2, 1025) - 1;
            //     
            //     var maxToAdd = BigInteger.Pow(2, 1024);
            //     
            //     while (true)
            //     {
            //         // выберем случайное целое число a в отрезке [2, n − 2]
            //         RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            //     
            //         byte[] _a = new byte[first2048BitNumber.getBytes().LongLength];
            //     
            //         BigInteger a;
            //     
            //         do
            //         {
            //             rng.GetBytes(_a);
            //             a = new BigInteger(_a);
            //         }
            //         while (a < 2 || a >= maxToAdd);
            //     
            //         var ourCandidate = first2048BitNumber + a;
            //
            //         Console.WriteLine("Candidate is " + ourCandidate);
            //         if (PrimeTests.SolovayStrassenTest(ourCandidate, 10))
            //         {
            //             if (PrimeTests.RabinMillerTest(ourCandidate, 10))
            //             {
            //                
            //                 prime = ourCandidate;
            //                 break;
            //             }
            //         }
            //
            //         Console.WriteLine("It is not a prime number !-!");
            //         
            //     }
            //     
            //     Console.WriteLine("Prime number with hight probability: " + prime.ToString());
            // }
            //
        }

        public static BigInteger GeneratePrimeNumber(int sizeBits, int confidence)
        {
            BigInteger prime = 0;
        
            var firstNumber = BigInteger.Pow(2, sizeBits);
            var lastNumber = BigInteger.Pow(2, sizeBits+1) - 1;
            
            var maxToAdd = BigInteger.Pow(2, sizeBits);
            
            while (true)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            
                byte[] _a = new byte[firstNumber.ToByteArray().LongLength];
            
                BigInteger a;
            
                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= maxToAdd);
            
                var ourCandidate = firstNumber + a;
        
                //Console.WriteLine("Candidate is " + ourCandidate);
                
                if (PrimeTests.RabinMillerTest(ourCandidate, confidence))
                {
                   
                    prime = a;
                    break;
                }
                
        
                //Console.WriteLine("It is not a prime number !-!");
                
            }
            
            //Console.WriteLine(prime.ToString());
            return prime;
        }
        
        
        
        public static BigInteger FindPrime(int bitlength, int confidence)
        {
            //Generating a random number of bit length.

            //Filling bytes with pseudorandom.
            byte[] randomBytes = new byte[(bitlength / 8)+1];
            Random rand = new Random(Environment.TickCount);
            rand.NextBytes(randomBytes);
            //Making the extra byte 0x0 so the BigInts are unsigned (little endian).
            randomBytes[randomBytes.Length - 1] = 0x0;

            //Setting the bottom bit and top two bits of the number.
            //This ensures the number is odd, and ensures the high bit of N is set when generating keys.
            SetBitInByte(0, ref randomBytes[0]);
            SetBitInByte(7, ref randomBytes[randomBytes.Length - 2]);
            SetBitInByte(6, ref randomBytes[randomBytes.Length - 2]);

            while (true)
            {
                //Performing a Rabin-Miller primality test.
                bool isPrime = PrimeTests.RabinMillerTest(new BigInteger(randomBytes), confidence);
                if (isPrime)
                {
                    break;
                } else
                {
                    IncrementByteArrayLE(ref randomBytes, 2);
                    var upper_limit = new byte[randomBytes.Length];

                    //Clearing upper bit for unsigned, creating upper and lower bounds.
                    upper_limit[randomBytes.Length - 1] = 0x0;
                    BigInteger upper_limit_bi = new BigInteger(upper_limit);
                    BigInteger lower_limit = upper_limit_bi - 20;
                    BigInteger current = new BigInteger(randomBytes);

                    if (lower_limit<current && current<upper_limit_bi)
                    {
                        //Failed to find a prime, returning -1.
                        //Reached limit with no solutions.
                        return new BigInteger(-1);
                    }
                }
            }

            //Returning working BigInt.
            return new BigInteger(randomBytes);
        }
        
        public static void SetBitInByte(int bitNumFromRight, ref byte toSet)
        {
            byte mask = (byte)(1 << bitNumFromRight);
            toSet |= mask;
        }
        public static void IncrementByteArrayLE(ref byte[] randomBytes, int amt)
        {
            BigInteger n = new BigInteger(randomBytes);
            n += amt;
            randomBytes = n.ToByteArray();
        }
        
        
        
        // public static void PreviosMain(string[] args)
        // {
        //
        //     // Console.WriteLine(SoloveyShtrassenTest(7, 10));
        //     // Console.WriteLine(MillerRabinTest(7,10));
        //     // Console.WriteLine(FermaTest(7,10));
        //     // return;
        //     
        //     
        //     BigInteger prime = 0;
        //
        //     var first2048BitNumber = BigInteger.Pow(2, 1024);
        //     var last2048BitNumber = BigInteger.Pow(2, 2049) - 1;
        //     
        //     var maxToAdd = BigInteger.Pow(2, 2048);
        //     
        //     while (true)
        //     {
        //         // выберем случайное целое число a в отрезке [2, n − 2]
        //         RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //     
        //         byte[] _a = new byte[first2048BitNumber.ToByteArray().LongLength];
        //     
        //         BigInteger a;
        //     
        //         do
        //         {
        //             rng.GetBytes(_a);
        //             a = new BigInteger(_a);
        //         }
        //         while (a < 2 || a >= maxToAdd);
        //     
        //         var ourCandidate = first2048BitNumber + a;
        //
        //         Console.WriteLine("Candidate is " + ourCandidate);
        //         if (PreviousVersion.MillerRabinTest(ourCandidate, 10))
        //         {
        //             // if (SoloveyShtrassenTest(ourCandidate, 10))
        //             // {
        //                 // if (FermaTest(ourCandidate, 10))
        //                 // {
        //                     prime = a;
        //                     break;
        //                 //}
        //
        //                 //Console.WriteLine("Ferma does not think so");
        //             // }
        //         }
        //
        //         Console.WriteLine("It is not a prime number !-!");
        //         
        //     }
        //     
        //     Console.WriteLine(prime.ToString());
        //
        //
        //     // Console.WriteLine("Checking with other tests: ");
        //     // Console.WriteLine(SoloveyShtrassenTest(prime, 10));
        //     
        //     return;
        //     
        //     
        //     //working:
        //     for (BigInteger i = BigInteger.Pow(2, 2048); i < BigInteger.Pow(2, 2049)-1; i++)
        //     {
        //         if (PreviousVersion.MillerRabinTest(i, 10))
        //         {
        //             prime = i;
        //             break;
        //         }
        //             
        //     }
        //     
        //     Console.WriteLine(prime.ToString());
        //     
        //     Console.WriteLine();
        //     Console.WriteLine();
        //     
        //     
        //     //^
        //     
        //     
        //     
        //     
        //     
        //     
        //     
        //     
        //     
        //     //
        //     //
        //     // for (BigInteger i = BigInteger.Pow(2, 2048); i < BigInteger.Pow(2, 2049)-1; i++)
        //     // {
        //     //     if (SoloveyShtrassenTest(i, 10))
        //     //     {
        //     //         prime = i;
        //     //         break;
        //     //     }
        //     //         
        //     // }
        //     //
        //     // Console.WriteLine(prime.ToString());
        //     //
        //     // Console.WriteLine();
        //     // Console.WriteLine();
        //     //
        //     // prime = 0;
        //     // for (BigInteger i = BigInteger.Pow(2, 500); i < BigInteger.Pow(2, 2048); i++)
        //     // {
        //     //     if (FermaTest(i, BigInteger.Pow(2, 500)))
        //     //     {
        //     //         prime = i;
        //     //         break;
        //     //     }
        //     //         
        //     // }
        //     //
        //     // Console.WriteLine(prime.ToString());
        //     //
        //     // Console.WriteLine(FermaTest(9746347772161, 1));
        // }
        //
        //
    }
}