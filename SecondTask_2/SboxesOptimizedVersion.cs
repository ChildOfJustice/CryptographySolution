using System;

namespace SecondTask_2
{
    public class SboxesOptimizedVersion
    {
        public static byte InversePolynom(byte n)
        {
            return Pow(n, 254);
        }
        public static byte Pow(byte n, byte pow)
        {
            byte res = 1;
            while (pow != 0)
            {
                if ((pow & 1) != 0)
                    res = MultyplyPolynoms(res, n);
                n = MultyplyPolynoms(n, n);
                pow >>= 1;
            }
            return res;
        }

        public static byte MultyplyPolynoms(byte a, byte b)
        {
            byte p = 0;
            byte counter;
            byte hi_bit_set;
            for (counter = 0; counter < 8; counter++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }
                hi_bit_set = (byte)(a & 0x80);
                a <<= 1;
                if (hi_bit_set != 0)
                {
                    a ^= 0x1b;
                }
                b >>= 1;
            }
            return p;
        }
        public static byte[] GenerateSbox()
        {
            byte[] res = new byte[256];
            byte d = 0;
            for (int i = 0; i < 256; i++)
            {
                byte b = InversePolynom(d);
                res[i] = Convert.ToByte(b ^ RotL(b, 1) ^ RotL(b, 2) ^ RotL(b, 3) ^ RotL(b, 4) ^ 99);
                d++;
                int j = b ^ RotL(b, 1) ^ RotL(b, 2) ^ RotL(b, 3) ^ RotL(b, 4) ^ 99;
            }
            return res;
        }


        private static byte RotL(byte value, byte bits)
        {
            return Convert.ToByte((value << bits | value >> (8 - bits)) & 255);
        }
       
        public static byte[] GenerateInvSbox()
        {
            byte[] res = new byte[256];
            byte d = 0;
            for (int i = 0; i < 256; i++)
            {
                byte s = 0;
                s = Convert.ToByte(RotL(d, 1) ^ RotL(d, 3) ^ RotL(d, 6) ^ 5);
                s = InversePolynom(s);
                res[i] = s;
                d++;
            }
            return res;
        }

        public static void Check()
        {
            var sbox = GenerateSbox();
            var invsbox = GenerateInvSbox();


            for (int i = 0; i < 256; i++)
            {
                if(i % 16 == 0)
                    Console.WriteLine();
                Console.Write(sbox[i].ToString("X") + " ");
            }

            Console.WriteLine();
            Console.WriteLine();
            
            for (int i = 0; i < 256; i++)
            {
                if(i % 16 == 0)
                    Console.WriteLine();
                Console.Write(invsbox[i].ToString("X") + " ");
            }

            // byte d = 0;
            // byte b = 0;
            // for (int i = 0; i < 256; i++)
            // {
            //     for (int j = 0; j < 256; j++)
            //     {
            //         if (InversePolynom(Convert.ToByte(MultyplyPolynoms(0, b) ^ d)) == 82 &&
            //             InversePolynom(Convert.ToByte(MultyplyPolynoms(1, b) ^ d)) == 9 &&
            //             InversePolynom(Convert.ToByte(MultyplyPolynoms(2, b) ^ d)) == 106
            //             )
            //         {
            //             Console.WriteLine(b);
            //             Console.WriteLine(d);
            //         }
            //         b++;
            //     }
            //     d++;
            // }
        }
        
    }
}