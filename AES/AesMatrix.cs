using System;
using System.Linq;

namespace AES
{
    public class AesMatrix
    {
        //4 x 4
        protected ulong R;
        protected ulong L;
        
        //4 x 6
        protected ulong Tail;
        
        //4 x 8
        protected AesMatrix SecondPart;


        public int Nb;

        
        
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
                Nb = 4;
            }
            else if (DataSize == 24)
            {
                L = BitConverter.ToUInt64(DataBytes, 0);
                R = BitConverter.ToUInt64(DataBytes, 8);
                Tail = BitConverter.ToUInt64(DataBytes, 16);
                Nb = 6;
            }
            else if (DataSize == 32)
            {
                L = BitConverter.ToUInt64(DataBytes, 0);
                R = BitConverter.ToUInt64(DataBytes, 8);
                byte[] _DataBytes = new byte[16];
                for (int i = 16; i < 32; i++)
                {
                    _DataBytes[i-16] = DataBytes[i];
                }
                SecondPart = new AesMatrix(_DataBytes);
                Nb = 8;
            }
            else
            {
                throw new ArgumentException("Wrong data block size");
            }
        }

        public byte[] ToByteArray()
        {
            var res = new byte[Nb * 4];
            int counter = 0;
            for (int j = 0; j < Nb; j++)
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
                for (int j = 0; j < Nb; j++)
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
                for (int j = 0; j < Nb; j++)
                {
                    Console.Write(Get(i, j).ToString("X") + " ");
                }

                Console.WriteLine();
            }
        }
        
        public byte Get(int i, int j)
        {
            ulong res = 0;
            if (Nb == 4)
            {
                
                if (j < 2)
                {
                    //left part
                    ulong mask4Bytes = (((ulong) 1 << (bytesize * 4)) - 1);
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;
                    res = L;

                    for (int k = 0; k < j; k++)
                    {
                        res >>= bytesize * 4;
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
                    ulong mask4Bytes = ((ulong) 1 << (bytesize * 4)) - 1;
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;
                    res = R;
                    //PrintDigitAsByteArray(res);
                    for (int k = 2; k < j; k++)
                    {
                        res >>= bytesize * 4;
                    }

                    //PrintDigitAsByteArray(res);
                    res &= mask4Bytes;

                    for (int k = 0; k < i; k++)
                    {
                        res >>= bytesize;
                    }

                    res &= mask1Byte;
                }
            }
            else if (Nb == 6)
            {
                
                if (j < 2)
                {
                    //left part
                    ulong mask4Bytes = (((ulong) 1 << (bytesize * 4)) - 1);
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;
                    res = L;

                    for (int k = 0; k < j; k++)
                    {
                        res >>= bytesize * 4;
                    }

                    res &= mask4Bytes;

                    for (int k = 0; k < i; k++)
                    {
                        res >>= bytesize;
                    }

                    res &= mask1Byte;

                }
                else if (j < 4)
                {
                    //right part
                    ulong mask4Bytes = ((ulong) 1 << (bytesize * 4)) - 1;
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;
                    res = R;
                    //PrintDigitAsByteArray(res);
                    for (int k = 2; k < j; k++)
                    {
                        res >>= bytesize * 4;
                    }

                    //PrintDigitAsByteArray(res);
                    res &= mask4Bytes;

                    for (int k = 0; k < i; k++)
                    {
                        res >>= bytesize;
                    }

                    res &= mask1Byte;
                }
                else
                {
                    //the third part
                    ulong mask4Bytes = ((ulong) 1 << (bytesize * 4)) - 1;
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;
                    res = Tail;
                    for (int k = 0; k < j-4; k++)
                    {
                        res >>= bytesize * 4;
                    }

                    res &= mask4Bytes;

                    for (int k = 0; k < i; k++)
                    {
                        res >>= bytesize;
                    }

                    res &= mask1Byte;
                }
            }
            else if (Nb == 8)
            {
                
                if (j < 2)
                {
                    //left part
                    ulong mask4Bytes = (((ulong) 1 << (bytesize * 4)) - 1);
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;
                    res = L;

                    for (int k = 0; k < j; k++)
                    {
                        res >>= bytesize * 4;
                    }

                    res &= mask4Bytes;

                    for (int k = 0; k < i; k++)
                    {
                        res >>= bytesize;
                    }

                    res &= mask1Byte;

                }
                else if (j < 4)
                {
                    //right part
                    ulong mask4Bytes = ((ulong) 1 << (bytesize * 4)) - 1;
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;
                    res = R;
                    //PrintDigitAsByteArray(res);
                    for (int k = 2; k < j; k++)
                    {
                        res >>= bytesize * 4;
                    }

                    //PrintDigitAsByteArray(res);
                    res &= mask4Bytes;

                    for (int k = 0; k < i; k++)
                    {
                        res >>= bytesize;
                    }

                    res &= mask1Byte;
                }
                else
                {
                    res = SecondPart.Get(i, j - 4);
                }
                
            }
            return (byte)res;
        }
        public void Set(byte value, int i, int j)
        {
            if (Nb == 4)
            {
                if (j < 2)
                {
                    //left part
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;

                    var eraser = mask1Byte;
                    ulong newValue = value;

                    for (int k = 1; k > j; k--)
                    {
                        eraser <<= bytesize * 4;
                        newValue <<= bytesize * 4;
                    }
                   

                    for (int k = 3; k > i; k--)
                    {
                        eraser <<= bytesize;
                        newValue <<= bytesize;
                    }

                    eraser = ~eraser;


                    eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                    newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                   
                    L &= eraser;
                    L |= newValue;
                }
                else
                {
                    //right part
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;

                    var eraser = mask1Byte;
                    ulong newValue = value;

                    for (int k = 1; k > j - 2; k--)
                    {
                        eraser <<= bytesize * 4;
                        newValue <<= bytesize * 4;
                    }

                    for (int k = 3; k > i; k--)
                    {
                        eraser <<= bytesize;
                        newValue <<= bytesize;
                    }

                    eraser = ~eraser;

                    eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                    newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                   
                    R &= eraser;
                    R |= newValue;
                }
            }
            else if (Nb == 6)
            {
                if (j < 2)
                {
                    //left part
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;

                    var eraser = mask1Byte;
                    ulong newValue = value;

                    for (int k = 1; k > j; k--)
                    {
                        eraser <<= bytesize * 4;
                        newValue <<= bytesize * 4;
                    }
                   

                    for (int k = 3; k > i; k--)
                    {
                        eraser <<= bytesize;
                        newValue <<= bytesize;
                    }

                    eraser = ~eraser;


                    eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                    newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                   
                    L &= eraser;
                    L |= newValue;
                }
                else if (j < 4)
                {
                    //right part
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;

                    var eraser = mask1Byte;
                    ulong newValue = value;

                    for (int k = 1; k > j - 2; k--)
                    {
                        eraser <<= bytesize * 4;
                        newValue <<= bytesize * 4;
                    }

                    for (int k = 3; k > i; k--)
                    {
                        eraser <<= bytesize;
                        newValue <<= bytesize;
                    }

                    eraser = ~eraser;

                    eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                    newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                   
                    R &= eraser;
                    R |= newValue;
                }
                else 
                {
                    //the third part
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;

                    var eraser = mask1Byte;
                    ulong newValue = value;

                    for (int k = 1; k > j - 4; k--)
                    {
                        eraser <<= bytesize * 4;
                        newValue <<= bytesize * 4;
                    }
                    

                    for (int k = 3; k > i; k--)
                    {
                        eraser <<= bytesize;
                        newValue <<= bytesize;
                    }

                    eraser = ~eraser;
                    

                    eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                    newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                    //Console.WriteLine("Will be:");
                    //PrintDigitAsByteArray(eraser);
                    //PrintDigitAsByteArray(newValue);
                    Tail &= eraser;
                    Tail |= newValue;
                    //PrintDigitAsByteArray(Tail);
                }
            }
            else if (Nb == 8)
            {
                if (j < 2)
                {
                    //left part
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;

                    var eraser = mask1Byte;
                    ulong newValue = value;

                    for (int k = 1; k > j; k--)
                    {
                        eraser <<= bytesize * 4;
                        newValue <<= bytesize * 4;
                    }
                   

                    for (int k = 3; k > i; k--)
                    {
                        eraser <<= bytesize;
                        newValue <<= bytesize;
                    }

                    eraser = ~eraser;


                    eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                    newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                   
                    L &= eraser;
                    L |= newValue;
                }
                else if (j < 4)
                {
                    //right part
                    ulong mask1Byte = ((ulong) 1 << (bytesize)) - 1;

                    var eraser = mask1Byte;
                    ulong newValue = value;

                    for (int k = 1; k > j - 2; k--)
                    {
                        eraser <<= bytesize * 4;
                        newValue <<= bytesize * 4;
                    }

                    for (int k = 3; k > i; k--)
                    {
                        eraser <<= bytesize;
                        newValue <<= bytesize;
                    }

                    eraser = ~eraser;

                    eraser = BitConverter.ToUInt64(BitConverter.GetBytes(eraser).Reverse().ToArray(), 0);
                    newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newValue).Reverse().ToArray(), 0);
                   
                    R &= eraser;
                    R |= newValue;
                }
                else
                {
                    SecondPart.Set(value, i, j-4);
                }
            }
        }

        public void Xor(AesMatrix operand)
        {
            if (Nb == 4)
            {
                L ^= operand.L;
                R ^= operand.R;
            } else if (Nb == 6)
            {
                L ^= operand.L;
                R ^= operand.R;
                Tail ^= operand.Tail;
            }
            else  if (Nb == 8)
            {
                L ^= operand.L;
                R ^= operand.R;
                SecondPart.L ^= operand.L;
                SecondPart.R ^= operand.R;
            }
            
        }
    }
}