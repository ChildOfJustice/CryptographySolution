using System;

namespace SecondTask_2
{
    public class Program
    {
        private static byte[,] A =
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
        private static byte[,] revA =
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
                Console.Write(GetSboxElement((byte)i).ToString("X") + " ");
            }

            Console.WriteLine("\n\n");
            Console.WriteLine("inversed S box:");
            for (int i = 0; i < 256; i++)
            {
                if(i % 16 == 0)
                    Console.WriteLine();
                Console.Write(GetInversedSboxElement((byte)i).ToString("X") + " ");
            }
            
        }
        public static byte GetSboxElement(byte i)
        {
            
            // byte s = 0;
            // s = (byte)(SecondTask_1.Program.GaloisMultiplication(SecondTask_1.Program.GaloisReverse(i), 31) ^ 99);
            // return s;
            
            var currVector = new byte[8];
            
            byte currI = SecondTask_1.Program.GaloisReverse(i);
            for (int k = 0; k < 8; k++)
            {
                currVector[k] = (byte)((currI >> k) & 1);
            }
            
            var resVector = GaloisVectorMatrixMultiplication(A, currVector);
            
            byte res = 0;
            for (int k = 0; k < 8; k++)
            {
                res += (byte)(resVector[k] << k);
            }
            
            res ^= 0x63;

            return res;
        }
        public static uint GetInversedSboxElement(byte i)
        {
            
            
            // var s = Convert.ToByte(SecondTask_1.Program.GaloisMultiplication(i, 74) ^ 5);
            // s = (byte)SecondTask_1.Program.GaloisReverse(s);
            // return s;
            
            
            var currVector = new byte[8];
            
            byte currI = i;
             for (int k = 0; k < 8; k++)
             {
                 currVector[k] = (byte)((currI >> k) & 1);
             }
            
             var resVector = GaloisVectorMatrixMultiplication(revA, currVector);
            
             byte res = 0;
             for (int k = 0; k < 8; k++)
             {
                 res += (byte)(resVector[k] << k);
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
        
        
        static byte[] GaloisVectorMatrixMultiplication(byte[,] inputMartix, byte[] vector)
        {
            byte[] resultMatrix;
            
            resultMatrix = new byte[8];
            
            for (var i = 0; i < 8; i++)
            {
                resultMatrix[i] = 0;
            
                for (var k = 0; k < 8; k++)
                {
                    resultMatrix[i] ^= (byte)(inputMartix[i, k] & vector[k]);
                }
            }
            return resultMatrix;
        }
    }
}