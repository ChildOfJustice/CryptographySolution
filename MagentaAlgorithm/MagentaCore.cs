﻿using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using Task_4;

namespace MagentaAlgorithm
{
    public class MagentaCore : FeistelNet
    {
        const byte byteSize = 8;
        
        static byte[] MagentaSbox =
        {
            1, 2, 4, 8, 16, 32, 64, 128, 101,
            202, 241, 135, 107, 214, 201, 247, 139, 115,
            230, 169, 55, 110, 220, 221, 223, 219, 211,
            195, 227, 163, 35, 70, 140, 125, 250, 145,
            71, 142, 121, 242, 129, 103, 206, 249, 151,
            75, 150, 73, 146, 65, 130, 97, 194, 225,
            167, 43, 86, 172, 61, 122, 244, 141, 127,
            254, 153, 87, 174, 57, 114, 228, 173, 63,
            126, 252, 157, 95, 190, 25, 50, 100, 200,
            245, 143, 123, 246, 137, 119, 238, 185, 23,
            46, 92, 184, 21, 42, 84, 168, 53, 106,
            212, 205, 255, 155, 83, 166, 41, 82, 164,
            45, 90, 180, 13, 26, 52, 104, 208, 197,
            239, 187, 19, 38, 76, 152, 85, 170, 49,
            98, 196, 237, 191, 27, 54, 108, 216, 213,
            207, 251, 147, 67, 134, 105, 210, 193, 231,
            171, 51, 102, 204, 253, 159, 91, 182, 9,
            18, 36, 72, 144, 69, 138, 113, 226, 161,
            39, 78, 156, 93, 186, 17, 34, 68, 136,
            117, 234, 177, 7, 14, 28, 56, 112, 224,
            165, 47, 94, 188, 29, 58, 116, 232, 181,
            15, 30, 60, 120, 240, 133, 111, 222, 217,
            215, 203, 243, 131, 99, 198, 233, 183, 11,
            22, 44, 88, 176, 5, 10, 20, 40, 80,
            160, 37, 74, 148, 77, 154, 81, 162, 33,
            66, 132, 109, 218, 209, 199, 235, 179, 3,
            6, 12, 24, 48, 96, 192, 229, 175, 59,
            118, 236, 189, 31, 62, 124, 248, 149, 79,
            158, 89, 178, 0
        };
        

        protected override ulong AbstractFeistelFunction(ulong R, ulong RoundKey)
        {
            // 64  64
            //Each Feistel round is expressed as taking (X(1),X(2)), where each X is half the block, 64 bits in length, and replacing it with (X(2),X(1) xor F(X(2),SK(n))), where n is the round number, and SK(n) the n-th subkey.
            //
            //The F function equals the first eight bytes of S(C(3,(X(2),SK(n)))).
            //(X(2),X(1) xor F(X(2),SK(n)))
            //Console.WriteLine(F(R, RoundKey).ToString("X"));
            return F(R, RoundKey);
        }

        public override ulong Key
        {
            set
            {
                // K1, K2, and the subkeys for the six rounds in order are K1, K1, K2, K2, K1, K1.
                ulong mask = ((ulong)1 << 64) - 1;
                var K1 = (value >> 64);
                var K2 = value & mask;
                RoundKeys = new ulong[FeistelRoundQuantity];
                RoundKeys[0] = K1;
                RoundKeys[1] = K1;
                RoundKeys[2] = K2;
                RoundKeys[3] = K2;
                RoundKeys[4] = K1;
                RoundKeys[5] = K1;
            }
        }

        #region MagentaFunctions

        public byte f(byte x)
        {
            // Console.Write("f(" + x + ")=");
            // Console.WriteLine(Convert.ToString(MagentaSbox[x], 2));
            return MagentaSbox[x];
        }

        public byte A(byte x, byte y)
        {
            // Console.Write("A(" + x + ","+y+")=");
            // Console.WriteLine(Convert.ToString(f((byte)(x ^ f(y))), 2));
            return f((byte)(x ^ f(y)));
        }

        public int PE(byte x, byte y)
        {
            //Console.Write("INPUT: " + x + " " + y + " res: ");
            //Console.WriteLine((A(x, y) << byteSize) | A(y, x));
            return (A(x, y) << byteSize) | A(y, x);
        }
        
        public BigInteger Pi(BigInteger digit16bytes)
        {
            //Console.Write("INPUT: " + from0to15 + " res: ");
            //Console.WriteLine(digit16bytes.ToString("X"));
            //Console.WriteLine("0 byte: " + GetByte(digit16bytes, 0));
            //Console.WriteLine("8 byte: " + GetByte(digit16bytes, 8));
            BigInteger res = 0;
            for (int i = 0; i < byteSize; i++)
            {
                var pe = PE(GetByte(digit16bytes, i), GetByte(digit16bytes, (i+byteSize)));
                //Console.WriteLine(Convert.ToString((long)pe, 2));
                BigInteger tempPe = new BigInteger(pe);
                res <<= 2*byteSize;
                res |= tempPe;
            }
            //Console.Write("Pi(" + from0to15 + ")=");
            //Console.WriteLine(Convert.ToString(f((byte)(x ^ f(y))), 2));
            //Console.WriteLine(res);
            // Console.WriteLine(Convert.ToString((long)res, 2));
            return res;
        }
        public byte GetByte(BigInteger value, int i)
        {
            uint MaskByte = ((uint)1 << 8) - 1;
            var res = value;
            for (int j = 15-i; j > 0; j--)
            {
                res >>= byteSize;
                //Console.WriteLine("!!" + res.ToString("X"));
            }
            return (byte)(res & MaskByte);
        }


        public BigInteger T(BigInteger digit16bytes)
        {
            //Console.WriteLine(Convert.ToString((long)Pi(Pi(Pi(Pi(from0to15)))), 2));
            return Pi(Pi(Pi(Pi(digit16bytes))));
        }
        //
        public BigInteger S(BigInteger digit16bytes)
        {
            uint MaskByte = ((uint)1 << 8) - 1;
            
            BigInteger res = 0;
            for (int i = 15; i > 0; i-=2)
            {
                res <<= byteSize;
                res |= ((digit16bytes >> i*byteSize) & MaskByte);
            }
            for (int i = 14; i > 0; i-=2)
            {
                res <<= byteSize;
                res |= ((digit16bytes >> i*byteSize) & MaskByte);
            }
        
            
            return res;
        }
        //
        public BigInteger C(int n, BigInteger w)
        {
            //Console.WriteLine("INPUT: " + w.ToString("X"));
            if (n == 1)
                return T(w);
            var res = T(w ^ S(C(n - 1, w)));
            //Console.WriteLine("RES " + res.ToString("X"));
            return res;
        }
        //
        //
        public ulong F(BigInteger rightHalf, ulong subKey)
        {
            BigInteger R = rightHalf;
            R <<= byteSize*8;
            R |= subKey;
            //The F function equals the first eight bytes of S(C(3,(X(2),SK(n)))).
            var fValue = S(C(3, R));
            //var fValue = C(3, R);
            //Console.WriteLine(fValue.ToString("X"));
            fValue >>= 8 * byteSize;
            
            return (ulong)fValue;
        }
        #endregion
        
        
        
        
        
        public static string ToBinaryString(BigInteger bigint)
        {
            var bytes = bigint.ToByteArray();
            var idx = bytes.Length - 1;

            // Create a StringBuilder having appropriate capacity.
            var base2 = new StringBuilder(bytes.Length * 8);

            // Convert first byte to binary.
            var binary = Convert.ToString(bytes[idx], 2);

            // Ensure leading zero exists if value is positive.
            if (binary[0] != '0' && bigint.Sign == 1)
            {
                base2.Append('0');
            }

            // Append binary string to StringBuilder.
            base2.Append(binary);

            // Convert remaining bytes adding leading zeros.
            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }

            return base2.ToString();
        }
    }
    
}