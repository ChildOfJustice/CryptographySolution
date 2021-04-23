using System;
using System.Linq;

namespace AES
{
    public class AesMatrix
    {
        //4 x 4
        protected ulong R;
        protected ulong L;

        private int bytesize = 8;

        public AesMatrix(byte[] DataBytes)
        {
            ProcessDataBytes(DataBytes);
        }
        
        
        
        protected void ProcessDataBytes(byte[] DataBytes)
        {
            var DataSize = DataBytes.Length;
            
            if(DataSize == 16)
            {
                L = BitConverter.ToUInt64(DataBytes, 0);
                R = BitConverter.ToUInt64(DataBytes, DataSize/2);
            }
            else
            {
                throw new ArgumentException("Wrong data block size");
            }
        }

        public byte[] ToByteArray()
        {
            var res = new byte[16];
            int counter = 0;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    res[counter] = Get(i, j);
                    counter++;
                }
            }

            return res;
        }
        
        
        public void PrintDigitAsByteArray(ulong digit)
        {
            var arr=BitConverter.GetBytes(digit);
            foreach (var VARIABLE in arr.Reverse())
            {
                Console.Write(VARIABLE + " ");
            }

            Console.WriteLine();
        }
        public void PrintMatrix()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(Get(i, j) + " ");
                }

                Console.WriteLine();
            }
        }
        
        public void PrintMatrixHex()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(Get(i, j).ToString("X") + " ");
                }

                Console.WriteLine();
            }
        }
        
        public byte Get(int i, int j)
        {

            ulong res = 0;
            if (j < 2)
            {
                //left part
                ulong mask4Bytes = (((ulong)1 << (bytesize*4)) - 1);
                ulong mask1Byte = ((ulong)1 << (bytesize)) - 1;
                res = L;
                
                for (int k = 0; k < j; k++)
                {
                    res >>= bytesize*4;
                }

                res &= mask4Bytes;

                for (int k = 0; k < i; k++)
                {
                    res >>= bytesize;
                }

                res &= mask1Byte;
                
            }
            else
            {
                //right part
                ulong mask4Bytes = ((ulong)1 << (bytesize*4)) - 1;
                ulong mask1Byte = ((ulong)1 << (bytesize)) - 1;
                res = R;
                //PrintDigitAsByteArray(res);
                for (int k = 2; k < j; k++)
                {
                    res >>= bytesize*4;
                }
                //PrintDigitAsByteArray(res);
                res &= mask4Bytes;

                for (int k = 0; k < i; k++)
                {
                    res >>= bytesize;
                }

                res &= mask1Byte;
            }
            return (byte)res;
        }
        public void Set(byte value, int i, int j)
        {

            ulong res = 0;
            if (j < 2)
            {
                //left part
                ulong mask4Bytes = (((ulong)1 << (bytesize*4)) - 1);
                ulong mask1Byte = ((ulong)1 << (bytesize)) - 1;

                var eraser = mask1Byte;
                ulong newValue = value;

                for (int k = 1; k > j; k--)
                {
                    eraser <<= bytesize*4;
                    newValue <<= bytesize*4;
                }
                //PrintDigitAsByteArray(eraser);

                //res &= mask4Bytes;

                for (int k = 3; k > i; k--)
                {
                    eraser <<= bytesize;
                    newValue <<= bytesize;
                }
                
                eraser = ~eraser;
                
                
                eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                //PrintDigitAsByteArray(eraser);
                //PrintDigitAsByteArray(newValue);
                
                //PrintDigitAsByteArray(L);
                L &= eraser;
                //PrintDigitAsByteArray(L);
                L |= newValue;
                //PrintDigitAsByteArray(L);
            }
            else
            {
                //right part
                // ulong mask4Bytes = ((ulong)1 << (bytesize*4)) - 1;
                // ulong mask1Byte = ((ulong)1 << (bytesize)) - 1;
                // res = R;
                // //PrintDigitAsByteArray(res);
                // for (int k = 2; k < j; k++)
                // {
                //     res >>= bytesize*4;
                // }
                // //PrintDigitAsByteArray(res);
                // res &= mask4Bytes;
                //
                // for (int k = 0; k < i; k++)
                // {
                //     res >>= bytesize;
                // }
                //
                // res &= mask1Byte;
                
                
                
                
                //right part
                ulong mask4Bytes = (((ulong)1 << (bytesize*4)) - 1);
                ulong mask1Byte = ((ulong)1 << (bytesize)) - 1;

                var eraser = mask1Byte;
                ulong newValue = value;

                for (int k = 1; k > j-2; k--)
                {
                    eraser <<= bytesize*4;
                    newValue <<= bytesize*4;
                }
                //PrintDigitAsByteArray(eraser);

                //res &= mask4Bytes;

                for (int k = 3; k > i; k--)
                {
                    eraser <<= bytesize;
                    newValue <<= bytesize;
                }
                
                eraser = ~eraser;
                
                
                eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                //PrintDigitAsByteArray(eraser);
                //PrintDigitAsByteArray(newValue);
                
                //PrintDigitAsByteArray(L);
                R &= eraser;
                //PrintDigitAsByteArray(L);
                R |= newValue;
                //PrintDigitAsByteArray(L);
            }
        }

        public void Xor(AesMatrix operand)
        {
            L ^= operand.L;
            R ^= operand.R;
        }
    }
}