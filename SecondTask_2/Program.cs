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

            Console.WriteLine("S box:");
            for (int i = 0; i < 256; i++)
            {
                if(i % 16 == 0)
                    Console.WriteLine();
                Console.Write(GetSboxElement((uint)i).ToString("X") + " ");
            }

            Console.WriteLine("\n\n");
            Console.WriteLine("inversed S box:");
            for (int i = 0; i < 256; i++)
            {
                if(i % 16 == 0)
                    Console.WriteLine();
                Console.Write(GetInversedSboxElement((uint)i).ToString("X") + " ");
            }
            
        }
        public static uint GetSboxElement(uint i)
        {
            var currVector = new uint[8];
            
            uint currI = SecondTask_1.Program.GaloisReverse(i);
            for (int k = 0; k < 8; k++)
            {
                currVector[k] = (currI >> k) & 1;
            }
            
            var resVector = GaloisVectorMatrixMultiplication(A, currVector);
            
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
            var currVector = new uint[8];
            
             uint currI = i;
             for (int k = 0; k < 8; k++)
             {
                 currVector[k] = (currI >> k) & 1;
             }
            
             var resVector = GaloisVectorMatrixMultiplication(revA, currVector);
            
             uint res = 0;
             for (int k = 0; k < 8; k++)
             {
                 res += (resVector[k] << k);
             }
            
             res ^= 0x5;
             
             return SecondTask_1.Program.GaloisReverse(res);
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
    }
}