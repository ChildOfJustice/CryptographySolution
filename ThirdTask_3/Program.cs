using System;
using System.IO;
using System.Numerics;
using System.Text;
//using BigInteger = System.Numerics.BigInteger;

namespace ThirdTask_3
{
    internal class Program
    {
        //new ASCIIEncoding().GetBytes(sData);
        //MessageBox.Show("Decrypted data: " + new ASCIIEncoding().GetString(Final));
        
        public static void Main(string[] args)
        {
            //TestWithFile();
            //Console.WriteLine(LogPow2(System.Numerics.BigInteger.Pow(16,19),4));
            //return;
            
            
            RsaCore rsa = new RsaCore(512);

            byte[] raw = {0x00, 0x01, 0x02, 0x03, 0x04};

//These bytes are now encrypted using RSA, of the bitlength specified before.
            byte[] encrypted = rsa.EncryptBytes(raw);
            Console.WriteLine("Encrypted:");
            foreach (var VARIABLE in encrypted)
            {
                Console.Write(VARIABLE);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Decrypted: ");
            byte[] decrypted = rsa.DecryptBytes(encrypted);
            foreach (var VARIABLE in decrypted)
            {
                Console.Write(VARIABLE);
            }
            
            return;
            //
            //
            //
            //
            // Console.WriteLine(rsa.EncryptOneByte(0));
            //
            //
            // Console.WriteLine("Public key as hex string:");
            // Console.WriteLine(rsa.GetPublicKeyAsString());
            // Console.WriteLine();
            //
            // Console.WriteLine("Private key as hex string:");
            // Console.WriteLine(rsa.GetPrivateKeyAsString());
            // Console.WriteLine();
            //
            //
            // Console.WriteLine("Data to be encrypted:");
            // var dataS = "Secret 2 123456789";
            // Console.WriteLine(dataS);
            // var data = new ASCIIEncoding().GetBytes(dataS);
            //
            //
            //
            //
            // var encrypted = rsa.Encrypt(data);
            // Console.WriteLine();
            // Console.WriteLine("Encrypted data:");
            // for (int i = 0; i < encrypted.Length; i++)
            // {
            //     Console.WriteLine("Byte number " + i + ", |"+data[i]+" size is " + encrypted[i].ToByteArray().Length);
            //     Console.WriteLine(encrypted[i]);
            // }
            //
            // Console.WriteLine();
            //
            // var decrypted = rsa.Decrypt(encrypted);
            // Console.WriteLine("Decrypted data: " + new ASCIIEncoding().GetString(decrypted));
        }


        public static int LogPow2(System.Numerics.BigInteger number, int powOfTwo)
        {
            int res = 0;
            
            var temp = number;
            while (temp != 0)
            {
                temp >>= powOfTwo;
                res++;
            }

            return res-1;
        }
        
        public static void TestWithFile()
        {
            RsaCore rsa = new RsaCore(516);

            // Console.WriteLine("Public key as hex string:");
            // Console.WriteLine(rsa.GetPublicKeyAsString(16));
            // Console.WriteLine();
            //
            // Console.WriteLine("Private key as hex string:");
            // Console.WriteLine(rsa.GetPrivateKeyAsString(16));
            // Console.WriteLine();


            EncryptKey(rsa, "key", "encKey");
            DecryptKey(rsa, "encKey", "decKey");
        }
        
        
        
        
        public static void EncryptKey(RsaCore rsaCore, string keyFileName, string outputFilePath)
        {
            FileInfo fi = new FileInfo(keyFileName);
            int blocks = (int)fi.Length / 8;
            
            if (fi.Length < 8 || fi.Length % 8 != 0)
                Console.WriteLine("Wrong key file");
            int blockSize = 8;
            
            var outputStream = File.Open(outputFilePath, FileMode.Create);
            FileStream fs = new FileStream(keyFileName, FileMode.Open, FileAccess.Read);
            
            for (int i = 0; i < blocks; i++)
            {
                int endPositionToRead = (i + 1) * blockSize;

                var plainText = ReadDesiredPart(fs, i * blockSize, endPositionToRead);
                
                var encryptedBigIntegers = new BigInteger[plainText.Length];
                var encryptedAllBytes = new byte[plainText.Length][];
                for (int j = 0; j < encryptedBigIntegers.Length; j++)
                {
                    encryptedBigIntegers[j] = rsaCore.EncryptOneByte(plainText[j]);
                    Console.WriteLine("Encrypted byte: " + plainText[j] + " length is " + encryptedBigIntegers[j].ToByteArray().Length);
                    
                    encryptedAllBytes[j] = encryptedBigIntegers[j].ToByteArray();
                    if (encryptedAllBytes[j].Length != rsaCore.numberSize)
                    {
                        //MessageBox.Show("Error : " + encrypted[j].getBytes().Length + " != " + rsaCore.numberSize);
                        Console.WriteLine("Found wrong data size: " + encryptedAllBytes[j].Length);
                        var temp = encryptedBigIntegers[j].ToByteArray();

                        var fixedWithPadding = new byte[rsaCore.numberSize];
                        if (temp.Length == 1)
                        {
                            Console.WriteLine("ONE!!!");
                            for (int k = 0; k < fixedWithPadding.Length-1; k++)
                            {
                                fixedWithPadding[k] = 0;
                            }

                            fixedWithPadding[fixedWithPadding.Length-1] = temp[0];
                            Console.WriteLine("ready one: " + arrToStr(fixedWithPadding));
                        }
                        else
                        {
                            
                            for (int k = 1; k < temp.Length+1; k++)
                            {
                                fixedWithPadding[k] = temp[k-1];
                            }

                            fixedWithPadding[0] = 0;
                        }
                        
                        // MessageBox.Show("2: " + arrToStr(newTemp));
                        // List<byte> temp2 = new List<byte>();
                        // for (int k = 0; k < newTemp.Length; k++)
                        // {
                        //     if (newTemp[k] != 0)
                        //     {
                        //         temp2.Add(newTemp[k]);
                        //         //MessageBox.Show("ADD "+ newTemp[k])
                        //     }
                        //         
                        // }
                        // MessageBox.Show("3: " + arrToStr(temp2.ToArray()));
                        // MessageBox.Show("Need to be eq: " + new ASCIIEncoding().GetString(temp2.ToArray()) + " | " +
                        //                 new ASCIIEncoding().GetString(temp));
                        encryptedAllBytes[j] = fixedWithPadding;
                        //encrypted[j] = new BigInteger(newTemp);
                        
                    }
                }
                
                
                foreach (var encryptedByte in encryptedAllBytes)
                {
                    try
                    {
                        outputStream.Write(encryptedByte, 0, encryptedByte.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                
            }
            outputStream.Close();
            fs.Close();
        }

        public static string arrToStr(byte[] arr)
        {
            string res = "";
            foreach (var VARIABLE in arr)
            {
                res += VARIABLE;
            }

            return res;
        }
        public static void DecryptKey(RsaCore rsaCore, string encryptedKeyFileName, string outputFilePath)
        {
            
            var outputStream = File.Open(outputFilePath, FileMode.Create);
            FileStream fs = new FileStream(encryptedKeyFileName, FileMode.Open, FileAccess.Read);

            var fi = new FileInfo(encryptedKeyFileName);
            var numbersQuantity = fi.Length/rsaCore.numberSize;
            for (int i = 0; i < numbersQuantity; i++)
            {
                int endPositionToRead = (i + 1) * rsaCore.numberSize;
                var block = ReadDesiredPart(fs, (i) * rsaCore.numberSize, endPositionToRead);
                //MessageBox.Show("Dec size: " + block.Length);
                //MessageBox.Show("read: " + arrToStr(block));
                byte decryptedByte;
                if (block[0] == 0)
                {
                    Console.WriteLine("!HMM!");
                    Console.WriteLine(arrToStr(block));
                    bool isOne = true;
                    for (int j = 1; j < block.Length-1; j++)
                    {
                        if (block[j] != 0)
                        {
                            isOne = false;
                            break;
                        }
                    }

                    if (isOne)
                    {
                        Console.WriteLine("ITS ONE");
                        decryptedByte = 1;
                    }
                    else
                    {
                        var temp = new byte[block.Length - 1];
                        for (int j = 0; j < block.Length - 1; j++)
                        {
                            temp[j] = block[j + 1];
                        }
                        decryptedByte = (byte)rsaCore.DecryptOneByte(new BigInteger(temp));
                    }
                    
                }
                else
                {
                    decryptedByte = (byte)rsaCore.DecryptOneByte(new BigInteger(block));
                }
                // List<byte> temp = new List<byte>();
                // for (int j = 0; j < block.Length; j++)
                // {
                //     if(block[j] != 0)
                //         temp.Add(block[j]);
                //     else
                //     {
                //         MessageBox.Show("!!");
                //     }
                // }
                
                
                //MessageBox.Show("dec: " + (decryptedByte-'0'));
                try
                {
                    //var encryptedBytes = new ASCIIEncoding().GetString(wtf);
                    outputStream.WriteByte(decryptedByte);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
            
            
            
            // StreamReader sr = new StreamReader(encryptedKeyFileName);
            // string line;
            // while ((line = sr.ReadLine()) != null)
            // {
            //     var decryptedByte = (byte)rsaCore.DecryptOneByte(new BigInteger(line, 10)).IntValue();
            //     // byte[] wtf = new byte[1];
            //     // wtf[0] = decryptedByte;
            //     try
            //     {
            //         //var encryptedBytes = new ASCIIEncoding().GetString(wtf);
            //         outputStream.WriteByte(decryptedByte);
            //     }
            //     catch (Exception e)
            //     {
            //         MessageBox.Show(e.Message);
            //     }
            // }

            
            outputStream.Close();
            //sr.Close();
            
        }
        
        public static byte[] ReadDesiredPart(FileStream fs, int startPosition, int endPosition) {
            byte[] buffer = new byte[endPosition - startPosition];

            int arrayOffset = 0;

            //lock (fsLock) {
            fs.Seek(startPosition, SeekOrigin.Begin);

            int numBytesRead = fs.Read(buffer, arrayOffset , endPosition - startPosition);

            //  Typically used if you're in a loop, reading blocks at a time
            arrayOffset += numBytesRead;
            //}

            return buffer;
        }
    }
}