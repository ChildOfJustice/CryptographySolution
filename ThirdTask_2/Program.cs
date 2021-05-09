using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml;

namespace ThirdTask_2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            //Console.WriteLine(PrimeTests.RabinMillerTest(7, 10));
            
            //return;
                
                
            BigInteger prime = 0;

            var first2048BitNumber = BigInteger.Pow(2, 500);
            var last2048BitNumber = BigInteger.Pow(2, 1025) - 1;
            
            var maxToAdd = BigInteger.Pow(2, 1024);
            
            while (true)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            
                byte[] _a = new byte[first2048BitNumber.getBytes().LongLength];
            
                BigInteger a;
            
                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= maxToAdd);
            
                var ourCandidate = first2048BitNumber + a;

                Console.WriteLine("Candidate is " + ourCandidate);
                if (PrimeTests.SolovayStrassenTest(ourCandidate, 10))
                {
                    if (PrimeTests.RabinMillerTest(ourCandidate, 10))
                    {
                       
                        prime = a;
                        break;
                    }
                }

                Console.WriteLine("It is not a prime number !-!");
                
            }
            
            Console.WriteLine(prime.ToString());
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