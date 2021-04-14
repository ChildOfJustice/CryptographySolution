using System;
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

        protected override uint AbstractFeistelFunction(uint R, ulong RoundKey)
        {
            throw new NotImplementedException();
        }

        public override ulong Key
        {
            set => throw new NotImplementedException();
        }

        #region MagentaFunctions

        public byte f(byte x)
        {
            return MagentaSbox[x];
        }

        public byte A(byte x, byte y)
        {
            return f((byte)(x ^ f(y)));
        }

        public int PE(byte x, byte y)
        {
            return (A(x, y) << byteSize) | A(y, x);
        }
        
        public ulong Pi(ulong from0to15)
        {
            ulong res = 0;
            for (int i = 0; i < 2*byteSize; i++)
            {
                var pe = PE(GetBit(from0to15, (byte)i), GetBit(from0to15, (byte)(i+byteSize)));
                ulong tempPe = (ulong)pe << i * 2 * byteSize;
                res <<= 2 * byteSize;
                res |= tempPe;
            }

            return res;
        }
        public byte GetBit(ulong value, byte i)
        {
            var res = value;
            for (int j = 0; j < i; j++)
            {
                res >>= 1;
            }

            return (byte)(res & 1);
        }


        public ulong T(ulong from0to15)
        {
            return Pi(Pi(Pi(Pi(from0to15))));
        }
        
        public ulong S(ulong from0to15)
        {
            ulong res = 0;
            for (int i = 0; i < 2*byteSize; i+=2)
            {
                res <<= 1;
                res |= ((from0to15 >> i) & 1);
            }
            for (int i = 1; i < 2*byteSize; i+=2)
            {
                res <<= 1;
                res |= ((from0to15 >> i) & 1);
            }

            return res;
        }

        public ulong C(int n, ulong w)
        {
            if (n == 1)
                return T(w);

            return T(w ^ S(C(n-1, w)));
        }

        #endregion
    }
}