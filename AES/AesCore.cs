using System;

namespace AES
{
    public class AesCore
    {
        private AesMatrix State;
        private AesMatrix CipherKey;
        private AesMatrix[] RoundKeys;

        private int bytesize = 8;

        private int roundsQuantity = 10;
        
        public void Cipher(byte[] DataBytes)
        {
            State = new AesMatrix(DataBytes);

           
            
            byte[] testRoundKey = 
            {
                0xa0, 0xfa, 0xfe, 0x17, 0x88, 0x54, 0x2c, 0xb1, 0x23, 0xa3, 0x39, 0x39, 0x2a, 0x6c, 0x76, 0x05
            };
            
            for (int i = 0; i < roundsQuantity; i++)
            {
                SubBytes();
                ShiftRows();
                if(i != 9)
                    MixColumns();
                AddRoundKey(new AesMatrix(testRoundKey));
            }
        }

        public byte[] Key
        {
            set
            {
                ulong mask = ((ulong)1 << 2*bytesize) - 1;
                byte[] Rcon = 
                {
                    0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
                };
                byte temp;
                
                CipherKey = new AesMatrix(value);
                
                RoundKeys = new AesMatrix[10];


                
                
                
                var curr4Bytes = new byte[4];
                byte[] arrForNewSubKey = new byte[16];

                for (int k = 0; k < 3; k++)
                {
                    temp = CipherKey.Get(k+1, 3);
                    curr4Bytes[k] = (byte) SecondTask_2.Program.GetSboxElement(
                        (uint) ConvertIndexesToByte((byte) (temp >> 2 * bytesize), (byte) (temp & mask)));
                }
                temp = CipherKey.Get(0, 3);
                curr4Bytes[3] =
                    (byte) SecondTask_2.Program.GetSboxElement(
                        (uint) ConvertIndexesToByte((byte) (temp >> 2 * bytesize), (byte) (temp & mask)));


                var subForRcon4Bytes = new byte[4];

                for (int k = 0; k < 4; k++)
                {
                    subForRcon4Bytes[k] = CipherKey.Get(k, 0);
                }

                for (int k = 0; k < 4; k++)
                {
                    curr4Bytes[k] ^= subForRcon4Bytes[k];
                }

                curr4Bytes[0] ^= Rcon[0];

                for (int k = 0; k < 4; k++)
                {
                    arrForNewSubKey[k] = curr4Bytes[k];
                    Console.WriteLine(curr4Bytes[k].ToString("X"));
                }

            }
        }






        public void SubBytes()
        {
            //my S-box generation function does not generate any arrays
            //instead it can return the S-value for index from 0 to 255, so:
            //
            ulong mask = ((ulong)1 << 2*bytesize) - 1;
            //State.PrintMatrixHex();
            //Console.WriteLine(SecondTask_2.Program.GetSboxElement((uint)ConvertIndexesToByte(1, 9)).ToString("X"));
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    
                    var currValue = State.Get(i, j);
                    var indexI = (byte)(currValue >> 2*bytesize);
                    var indexJ = (byte)(currValue & mask);
                    
                    State.Set((byte)SecondTask_2.Program.GetSboxElement((uint)ConvertIndexesToByte(indexI, indexJ)), i, j);
                }
            }
            
            //State.PrintMatrixHex();
        }
        private byte ConvertIndexesToByte(byte i, byte j)  // i - the first byte, j - the second one
        {
            byte result = 0;
            result += (byte)(i * 16 % 255);
            result += j;
            return (byte)(result % 255);
        }

        public void ShiftRows()
        {
            //shift 0
            //...
            
            //shift 1
            var temp = State.Get(1, 0);
            for (int k = 1; k < 4; k++)
            {
                State.Set(State.Get(1,k), 1, k-1);
            }
            State.Set(temp, 1, 3);
            
            
            //shift 2
            var temp1 = State.Get(2, 0);
            var temp2 = State.Get(2, 1);
            for (int k = 2; k < 4; k++)
            {
                State.Set(State.Get(2,k), 2, k-2);
            }
            State.Set(temp1, 2, 2);
            State.Set(temp2, 2, 3);
            
            //shift 3
            temp = State.Get(3, 3);
            for (int k = 2; k >= 0; k--)
            {
                State.Set(State.Get(3,k), 3, k+1);
            }
            State.Set(temp, 3, 0);
        }
        public void MixColumns()
        {
            
            for (int j = 0; j < 4; j++)
            {
                byte value1 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 2) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 3) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 1) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j), 1));

                byte value2 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 1) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 2) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 3) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j),1));

                byte value3 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 1) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 1) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 2) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j), 3));

                byte value4 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 3) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 1) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 1) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j), 2));
                
                State.Set(value1,0,j);
                State.Set(value2,1,j);
                State.Set(value3,2,j);
                State.Set(value4,3,j);
            }
            
        }
        public void AddRoundKey(AesMatrix roundKey)
        {
            State.PrintMatrixHex();
            
            State.Xor(roundKey);
            
            State.PrintMatrixHex();
        }

    }
}