using System;
using System.IO;
using Task_4;

namespace AES
{
    public class AesCore : ICypherAlgorithm
    {
        private AesMatrix State;
        private AesMatrix CipherKey;
        public AesMatrix[] RoundKeys;

        private int bytesize = 8;

        private int roundsQuantity;

        private byte[] Sbox = SecondTask_2.SboxesOptimizedVersion.GenerateSbox();
        private byte[] inversedSbox = SecondTask_2.SboxesOptimizedVersion.GenerateInvSbox();


        private int blockSize;
        private int keySize;

        public AesCore(byte[] key, bool generateRoundKeys = true, int _blockSize = 16)
        {
            blockSize = _blockSize;
            keySize = key.Length;

            if (_blockSize == 32 || keySize == 32)
                roundsQuantity = 14;
            else if (_blockSize == 24 || keySize == 24)
                roundsQuantity = 12;
            else if (_blockSize == 16 && keySize == 16)
                roundsQuantity = 10;

            if (generateRoundKeys)
            {
                Key = key;
                GenerateAllSubKeys(blockSize, keySize);
            }
        }
        
        
        public byte[] Key
        {
            set
            {
                CipherKey = new AesMatrix(value);
                if (CipherKey.Nb * 4 != keySize)
                    throw new ArgumentException("Wrong key size, should be " + keySize + " bytes");
            }
        }

        private AesMatrix GenerateNextRoundKey(AesMatrix previousRoundKey, byte NextRconByte)
        {
            ulong mask = ((ulong)1 << 2*bytesize) - 1;
            byte temp;
            
            
            var curr4Bytes = new byte[4];
            byte[] arrForNewSubKey = new byte[blockSize];

            for (int k = 0; k < 3; k++)
            {
                temp = previousRoundKey.Get(k+1, blockSize/4-1);
                curr4Bytes[k] = (byte) SecondTask_2.Program.GetSboxElement(
                    (byte) ConvertIndexesToByte((byte) (temp >> 2 * bytesize), (byte) (temp & mask)));
            }
            temp = previousRoundKey.Get(0, blockSize/4-1);
            curr4Bytes[3] =
                (byte) SecondTask_2.Program.GetSboxElement(
                    (byte) ConvertIndexesToByte((byte) (temp >> 2 * bytesize), (byte) (temp & mask)));


            var subForRcon4Bytes = new byte[4];

            for (int k = 0; k < 4; k++)
            {
                subForRcon4Bytes[k] = previousRoundKey.Get(k, 0);
            }

            for (int k = 0; k < 4; k++)
            {
                curr4Bytes[k] ^= subForRcon4Bytes[k];
            }

            curr4Bytes[0] ^= NextRconByte;

            for (int k = 0; k < 4; k++)
            {
                arrForNewSubKey[k] = curr4Bytes[k];
                //Console.WriteLine(curr4Bytes[k].ToString("X"));
                // time: 2.04
                //https://www.youtube.com/watch?v=CxU4ROAYGzs
            }

            //The last three columns of a new round key
            for (int columnIndex = 1; columnIndex < blockSize/4; columnIndex++)
            {
                for (int k = 0; k < 4; k++)
                {
                    arrForNewSubKey[columnIndex*4 + k] = (byte)(previousRoundKey.Get(k,columnIndex) ^ arrForNewSubKey[(columnIndex-1)*4 + k]);
                    //Console.WriteLine(arrForNewSubKey[columntIndex*4 + k].ToString("X"));
                }
            }

            return new AesMatrix(arrForNewSubKey);
        }
        
        
        
        
        
        # region Encryption
        public byte[] Encrypt(byte[] DataBytes)
        {
            State = new AesMatrix(DataBytes);

            for (int i = 0; i < roundsQuantity; i++)
            {
                // Console.WriteLine("////////////////ROUND " + i);
                // State.PrintMatrixHex();
                
                // Console.WriteLine("-----------SubBytes:");
                SubBytes();
                // State.PrintMatrixHex();
                
                // Console.WriteLine("-----------ShiftRows:");
                ShiftRows();
                // State.PrintMatrixHex();
                
                if (i != roundsQuantity-1)
                {
                    // Console.WriteLine("-----------MixColumns:");
                    MixColumns();
                    // State.PrintMatrixHex();
                }
                    
                // Console.WriteLine("-----------AddRoundKey:");
                AddRoundKey(RoundKeys[i]);
                // State.PrintMatrixHex();
            }

            return State.ToByteArray();
        }

        private void GenerateAllSubKeys(int dataSize, int keySize)
        {
            byte[] Rcon = 
            {
                0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
            };
            RoundKeys = new AesMatrix[roundsQuantity];


            RoundKeys[0] = GenerateNextRoundKey(CipherKey, Rcon[0]);

            for (int roundNumber = 1; roundNumber < roundsQuantity; roundNumber++)
                RoundKeys[roundNumber] = GenerateNextRoundKey(RoundKeys[roundNumber-1], Rcon[roundNumber%10]);


            // foreach (var VARIABLE in RoundKeys)
            // {
            //     VARIABLE.PrintMatrixHex();
            //     Console.WriteLine();
            // }
        }








        public void SubBytes()
        {
            //my S-box generation function does not generate any arrays
            //instead it can return the S-value for index from 0 to 255, so:
            //
            ulong mask = ((ulong)1 << bytesize/2) - 1;
            //State.PrintMatrixHex();
            for (int j = 0; j < State.Nb; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    
                    var currValue = State.Get(i, j);
                    var indexI = (byte)(currValue >> bytesize/2);
                    var indexJ = (byte)(currValue & mask);

                    State.Set(Sbox[ConvertIndexesToByte(indexI, indexJ)], i, j);
                }
            }
            
            //State.PrintMatrixHex();
        }
        public byte ConvertIndexesToByte(byte i, byte j)  // i - the first byte, j - the second one
        {
            int result = 0;
            result += (i * 16);
            result += j;
            return (byte)(result);
        }

        public void ShiftRows()
        {
            //shift 0
            //...
            
            //shift 1
            var temp = State.Get(1, 0);
            for (int k = 1; k < State.Nb; k++)
            {
                State.Set(State.Get(1,k), 1, k-1);
            }
            State.Set(temp, 1, State.Nb-1);
            
            
            //shift 2
            var temp1 = State.Get(2, 0);
            var temp2 = State.Get(2, 1);
            for (int k = 2; k < State.Nb; k++)
            {
                State.Set(State.Get(2,k), 2, k-2);
            }
            State.Set(temp1, 2, State.Nb-2);
            State.Set(temp2, 2, State.Nb-1);
            
            //shift 3
            temp = State.Get(3, State.Nb-1);
            for (int k = State.Nb-2; k >= 0; k--)
            {
                State.Set(State.Get(3,k), 3, k+1);
            }
            State.Set(temp, 3, 0);
        }
        public void MixColumns()
        {
            
            for (int j = 0; j < State.Nb; j++)
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
            //State.PrintMatrixHex();
            
            State.Xor(roundKey);
            
            //State.PrintMatrixHex();
        }

        #endregion


        #region Decryption

        public byte[] Decrypt(byte[] DataBytes)
        {
            State = new AesMatrix(DataBytes);
            
            for (int i = 0; i < roundsQuantity; i++)
            {
                // Console.WriteLine("////////////////ROUND " + i);
                // State.PrintMatrixHex();
                
                // Console.WriteLine("-----------AddRoundKey:");
                AddRoundKey(RoundKeys[roundsQuantity-1 - i]);
                // State.PrintMatrixHex();
                
                if (i != 0)
                {
                    // Console.WriteLine("-----------InversedMixColumns:");
                    InversedMixColumns();
                    // State.PrintMatrixHex();
                }
                
                // Console.WriteLine("-----------InversedShiftRows:");
                InversedShiftRows();
                // State.PrintMatrixHex();
                
               
                    
                // Console.WriteLine("-----------InversedSubBytes:");
                InversedSubBytes();
                // State.PrintMatrixHex();
                
                
            }

            return State.ToByteArray();
        }
        
        
        public void InversedMixColumns()
        {
            
            for (int j = 0; j < State.Nb; j++)
            {
                byte value1 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 14) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 11) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 13) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j), 9));

                byte value2 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 9) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 14) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 11) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j),13));

                byte value3 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 13) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 9) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 14) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j), 11));

                byte value4 = (byte) (SecondTask_1.Program.GaloisMultiplication(State.Get(0, j), 11) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(1, j), 13) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(2, j), 9) ^
                                      SecondTask_1.Program.GaloisMultiplication(State.Get(3, j), 14));
                
                State.Set(value1,0,j);
                State.Set(value2,1,j);
                State.Set(value3,2,j);
                State.Set(value4,3,j);
            }
            
        }
        public void InversedShiftRows()
        {
            //shift 0
            //...
            
            //shift 1
            var temp = State.Get(1, State.Nb-1);
            for (int k = State.Nb-1; k > 0; k--)
            {
                State.Set(State.Get(1,k-1), 1, k);
            }
            State.Set(temp, 1, 0);
            
            
            //shift 2
            var temp1 = State.Get(2, State.Nb-2);
            var temp2 = State.Get(2, State.Nb-1);
            for (int k = State.Nb-3; k >= 0; k--)
            {
                State.Set(State.Get(2,k), 2, k+2);
            }
            State.Set(temp1, 2, 0);
            State.Set(temp2, 2, 1);
            
            //shift 3
            temp = State.Get(3, 0);
            for (int k = 1; k < State.Nb; k++)
            {
                State.Set(State.Get(3,k), 3, k-1);
            }
            State.Set(temp, 3, State.Nb-1);
        }
        public void InversedSubBytes()
        {
            //my S-box generation function does not generate any arrays
            //instead it can return the S-value for index from 0 to 255, so:
            //
            ulong mask = ((ulong)1 << bytesize/2) - 1;
            //State.PrintMatrixHex();
            for (int j = 0; j < State.Nb; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    
                    var currValue = State.Get(i, j);
                    var indexI = (byte)(currValue >> bytesize/2);
                    var indexJ = (byte)(currValue & mask);
                    
                    State.Set(inversedSbox[ConvertIndexesToByte(indexI, indexJ)], i, j);
                }
            }
            
            //State.PrintMatrixHex();
        }
        #endregion
        
        
        
        public void ExportKey(string outPutFile)
        {
            using (var outputStream = File.Open(outPutFile, FileMode.Create))
                outputStream.Write(CipherKey.ToByteArray(), 0, CipherKey.ToByteArray().Length);
        }
        public void ImportKey(string inPutFile, int keySize)
        {
           
            var temp = new byte[keySize];
            using (var inputStream = File.OpenRead(inPutFile))
                inputStream.Read(temp, 0, keySize); // 8 bytes = 64 bit key

            Key = temp;
            GenerateAllSubKeys(blockSize, keySize);
        }
    }
}