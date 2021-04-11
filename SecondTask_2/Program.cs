using System;
using System.Collections.Generic;

namespace SecondTask_2
{
    internal class Program
    {
        private static uint[,] A =
        {
            { 1, 0, 0, 0, 1, 1, 1, 1 },
            { 1, 1, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 1, 1 },
            { 1, 1, 1, 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 1 }
        };
        private static uint[,] revA =
        {
            { 0, 0, 1, 0, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 1, 0 },
            { 0, 1, 0, 0, 1, 0, 0, 1 },
            { 1, 0, 1, 0, 0, 1, 0, 0 },
            { 0, 1, 0, 1, 0, 0, 1, 0 },
            { 0, 0, 1, 0, 1, 0, 0, 1 },
            { 1, 0, 0, 1, 0, 1, 0, 0 },
            { 0, 1, 0, 0, 1, 0, 1, 0 }
        };
        
        
        public static void Main(string[] args)
        {

            for (int i = 0; i < 256; i++)
            {
                if(i % 16 == 0)
                    Console.WriteLine();
                Console.Write(GetInversedSboxElement((uint)i).ToString("X") + " ");
            }
            


            // foreach (var number in MakeReducedDeductionSystem(42))
            // {
            //     Console.Write(number + ", ");
            // }
        }
        public static uint GetSboxElement(uint i)
        {
            // uint m = 0xf8;
            //     
            // var r = 0u;
            // var b = 0u;
            // if (i == 0)
            //     b = 0;
            // else b = SecondTask_1.Program.GaloisReverse(i);
            //     
            // for (var j = 0; j < 8; j++)
            // {
            //     r = (r << 1) | Xorbits(b & m);
            //     m = (m >> 1) | ((m & 1) << 7);
            // }
            //     
            // r ^= 0x63;


            var currVector = new uint[8];
            
            uint currI = SecondTask_1.Program.GaloisReverse(i);
            for (int k = 0; k < 8; k++)
            {
                currVector[k] = (currI >> k) & 1;
            }
            
            var resVector = GaloisVectorMatrixMultiplication(A, currVector);
            // foreach (var VARIABLE in resVector)
            // {
            //     Console.Write(VARIABLE + " ");
            // }
            // Console.Write("|||");
            uint res = 0;
            for (int k = 0; k < 8; k++)
            {
                res += (resVector[k] << k);
            }
            
            res ^= 0x63;

            return res;
        }
        public static uint GetInversedSboxElement(uint i)
        {
            for (int j = 0; j < 256; j++)
            {
                if (GetSboxElement((uint)j) == i)
                    return (uint)j;
            }

            return 257;
            //1
            // var currVector = new uint[8];
            //
            // uint currI = SecondTask_1.Program.GaloisReverse(i);//SecondTask_1.Program.GaloisReverse
            // for (int k = 0; k < 8; k++)
            // {
            //     currVector[k] = (currI >> k) & 1;
            // }
            //
            // var resVector = GaloisVectorMatrixMultiplication(revA, currVector);
            // // foreach (var VARIABLE in resVector)
            // // {
            // //     Console.Write(VARIABLE + " ");
            // // }
            // // Console.Write("|||");
            // uint res = 0;
            // for (int k = 0; k < 8; k++)
            // {
            //     res += (resVector[k] << k);
            // }
            //
            // res ^= 0x5;
            //
            // // Console.Write(res.ToString("X") + " ");
            // return res;
        }

        public static uint Xorbits(uint a)
        {
            uint res = 0;
            while (a != 0)
            {
                res ^= (a & 1);
                a >>= 1;
            }

            return res;
        }
        
        
        static uint[] GaloisVectorMatrixMultiplication(uint[,] inputMartix, uint[] vector)
        {
            uint[] resultMatrix;
            
  
            resultMatrix = new uint[8];
            
            for (var i = 0; i < 8; i++)
            {
                resultMatrix[i] = 0;
            
                for (var k = 0; k < 8; k++)
                {
                    resultMatrix[i] ^= inputMartix[i, k] & vector[k];
                }
            }
           

            return resultMatrix;
        }
        
        
        static List<uint> MakeReducedDeductionSystem(uint m)
        {
            var result = new List<uint>();
            result.Add(1);
            for (int i = 2; i < m; i++)
            {
                if (EuclidAlgorithm(m, (uint)i) == 1)
                {
                    result.Add((uint)i);
                }
            }

            return result;
        }
        
        
        
        static uint EuclidAlgorithm(uint m, uint n)
        {
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
            return n;
        }
    }
}

//http://www.allmath.ru/highermath/algebra/theorychisel-ugu/17.htm
//https://code-enjoy.ru/algoritm_evklida_na_c_sharp/

