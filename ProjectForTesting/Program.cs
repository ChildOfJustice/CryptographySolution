using System;
using System.IO;
using System.Linq;
using System.Numerics;
using SardorRsa;


namespace ProjectForTesting
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TestWithFile();
            return;
            RsaCore rsa = new RsaCore();
            var encr = rsa.EncryptOneByte(183);
            Console.WriteLine(encr);
            Console.WriteLine(rsa.DecryptOneByte(encr));


            // byte[] iv = new byte[8];
            // iv[1] = 1;
            // // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // // Console.WriteLine();
            //
            // byte[] block1 = new byte[8];
            // block1[5] = 1;
            // block1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            //
            // byte[] key = new byte[8];
            // key[5] = 3;
            //
            //
            // DES des = new DES();
            // des.Key = key;
            //
            // var encryptedIv = des.Encrypt(iv);
            // var temp = new byte[encryptedIv.Length];
            // for (int i = 0; i < encryptedIv.Length; i++)
            // {
            //     temp[i] = (byte)(block1[i] ^ encryptedIv[i]);
            // }
            //
            // temp.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            //
            //
            // var C0 = des.Encrypt(iv);
            // var decrypted = new byte[encryptedIv.Length];
            // for (int i = 0; i < encryptedIv.Length; i++)
            // {
            //     decrypted[i] = (byte)(C0[i] ^ temp[i]);
            // }
            // decrypted.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();




            //TestDesEncryptionModeCbc();
            //TestDesEncryptionModeCfb();
            //TestDesEncryptionModeOfb();
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
                    //Console.WriteLine("Encrypted byte: " + plainText[j] + " length is " + encryptedBigIntegers[j].ToByteArray().Length);
                    
                    encryptedAllBytes[j] = encryptedBigIntegers[j].ToByteArray();
                    if (encryptedAllBytes[j].Length != rsaCore.NumberSize)
                    {
                        //MessageBox.Show("Error : " + encrypted[j].getBytes().Length + " != " + rsaCore.numberSize);
                        //Console.WriteLine("Found wrong data size: " + encryptedAllBytes[j].Length);
                        var temp = encryptedBigIntegers[j].ToByteArray();

                        var fixedWithPadding = new byte[rsaCore.NumberSize];
                        if (temp.Length == 1)
                        {
                            //Console.WriteLine("ONE!!!");
                            for (int k = 0; k < fixedWithPadding.Length-1; k++)
                            {
                                fixedWithPadding[k] = 0;
                            }

                            fixedWithPadding[fixedWithPadding.Length-1] = temp[0];
                            //Console.WriteLine("ready one: " + arrToStr(fixedWithPadding));
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
        public static void DecryptKey(RsaCore rsaCore, string encryptedKeyFileName, string outputFilePath)
        {
            
            var outputStream = File.Open(outputFilePath, FileMode.Create);
            FileStream fs = new FileStream(encryptedKeyFileName, FileMode.Open, FileAccess.Read);

            var fi = new FileInfo(encryptedKeyFileName);
            var numbersQuantity = fi.Length/rsaCore.NumberSize;
            for (int i = 0; i < numbersQuantity; i++)
            {
                int endPositionToRead = (i + 1) * rsaCore.NumberSize;
                var block = ReadDesiredPart(fs, (i) * rsaCore.NumberSize, endPositionToRead);
                //MessageBox.Show("Dec size: " + block.Length);
                //MessageBox.Show("read: " + arrToStr(block));
                byte decryptedByte;
                if (block[0] == 0)
                {
                    //Console.WriteLine("!HMM!");
                    //Console.WriteLine(arrToStr(block));
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
                        //Console.WriteLine("ITS ONE");
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
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        // public static void TestDesEncryptionModeCbc()
        // {
        //     byte[] IV = new byte[8];
        //     IV[1] = 1;
        //     // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     // Console.WriteLine();
        //     
        //     byte[] block1 = new byte[8];
        //     block1[5] = 1;
        //     // block1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     // Console.WriteLine();
        //     byte[] block2 = new byte[8];
        //     block2[6] = 1;
        //     // block2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     // Console.WriteLine();
        //     
        //     byte[] block3 = new byte[8];
        //     block3[1] = 1;
        //     // block3.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     // Console.WriteLine();
        //     byte[] block4 = new byte[8];
        //     block4[2] = 1;
        //     // block4.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     // Console.WriteLine();
        //     //
        //     // Console.WriteLine();
        //
        //     
        //     
        //     
        //     
        //     
        //     
        //     byte[] key = new byte[8];
        //     key[5] = 3;
        //     DES des = new DES();
        //     des.Key = key;
        //
        //     Cbc mode = new Cbc(IV, des);
        //     byte[][] allBlocks1 = 
        //     {
        //         block1,
        //         block1,
        //         block1,
        //         block1,
        //         block1,
        //         block1,
        //         block1,
        //         block2
        //     };
        //     Console.WriteLine("Data1:");
        //     foreach (var block in allBlocks1)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //
        //         Console.WriteLine();
        //     }
        //     Console.WriteLine();
        //     
        //     
        //     Console.WriteLine("Encrypted Data1:");
        //     var res1 = mode.EncryptAll(allBlocks1);
        //     foreach (var block in res1)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //
        //         Console.WriteLine();
        //     }
        //     Console.WriteLine();
        //     
        //     
        //     
        //     
        //     
        //     
        //     // byte[][] allBlocks2 = 
        //     // {
        //     //     block3,
        //     //     block3,
        //     //     block4
        //     // };
        //     // Console.WriteLine("Data2:");
        //     // foreach (var block in allBlocks2)
        //     // {
        //     //     foreach (var x in block)
        //     //     {
        //     //         Console.Write(x + " ");
        //     //     }
        //     //
        //     //     Console.WriteLine();
        //     // }
        //     // Console.WriteLine();
        //     
        //     
        //     // Console.WriteLine("Encrypted Data2:");
        //     // mode = new Cbc(res1[res1.Length-1], des);
        //     // var res2 = mode.EncryptAll(allBlocks2);
        //     // foreach (var block in res2)
        //     // {
        //     //     foreach (var x in block)
        //     //     {
        //     //         Console.Write(x + " ");
        //     //     }
        //     //
        //     //     Console.WriteLine();
        //     // }
        //     // Console.WriteLine();
        //
        //     
        //     
        //     
        //     
        //     
        //     
        //     Console.WriteLine("Decrypted Data1:");
        //     mode = new Cbc(IV, des);
        //     var decrypted1 = mode.DecryptAll(res1);
        //     foreach (var block in decrypted1)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //     
        //         Console.WriteLine();
        //     }
        //     
        //     // Console.WriteLine("Decrypted Data2:");
        //     // mode = new Cbc(res1[res1.Length-1], des);
        //     // var decrypted2 = mode.DecryptAll(res2);
        //     // foreach (var block in decrypted2)
        //     // {
        //     //     foreach (var x in block)
        //     //     {
        //     //         Console.Write(x + " ");
        //     //     }
        //     //
        //     //     Console.WriteLine();
        //     // }
        //     
        // }
        // public static void TestDesEncryptionModeCfb()
        // {
        //     byte[] IV = new byte[8];
        //     IV[1] = 1;
        //     // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     // Console.WriteLine();
        //     
        //     byte[] block1 = new byte[8];
        //     block1[5] = 1;
        //     block1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     byte[] block2 = new byte[8];
        //     block2[6] = 1;
        //     block2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     
        //     byte[] block3 = new byte[8];
        //     block3[1] = 1;
        //     block3.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     byte[] block4 = new byte[8];
        //     block4[7] = 1;
        //     block4.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     
        //     Console.WriteLine();
        //
        //     
        //     
        //     
        //     
        //     
        //     
        //     byte[] key = new byte[8];
        //     key[5] = 3;
        //     DES des = new DES();
        //     des.Key = key;
        //
        //     Cfb mode = new Cfb(IV, des);
        //     byte[][] allBlocks1 = 
        //     {
        //         block1,
        //         block2
        //     };
        //     var res1 = mode.EncryptAll(allBlocks1);
        //     foreach (var block in res1)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //
        //         Console.WriteLine();
        //     }
        //     Console.WriteLine();
        //     
        //     
        //     
        //     
        //     
        //     
        //     byte[][] allBlocks2 = 
        //     {
        //         block3,
        //         block4
        //     };
        //     mode = new Cfb(res1[res1.Length-1], des);
        //     var res2 = mode.EncryptAll(allBlocks2);
        //     foreach (var block in res2)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //
        //         Console.WriteLine();
        //     }
        //     Console.WriteLine();
        //
        //     
        //     
        //     
        //     
        //     
        //     
        //     
        //     mode = new Cfb(IV, des);
        //     var decrypted1 = mode.DecryptAll(res1);
        //     foreach (var block in decrypted1)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //     
        //         Console.WriteLine();
        //     }
        //     
        //     mode = new Cfb(res1[res1.Length-1], des);
        //     var decrypted2 = mode.DecryptAll(res2);
        //     foreach (var block in decrypted2)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //     
        //         Console.WriteLine();
        //     }
        //     
        // }
        // public static void TestDesEncryptionModeOfb()
        // {
        //     byte[] IV = new byte[8];
        //     IV[1] = 1;
        //     // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     // Console.WriteLine();
        //     
        //     byte[] block1 = new byte[8];
        //     block1[5] = 1;
        //     block1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     byte[] block2 = new byte[8];
        //     block2[6] = 1;
        //     block2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     
        //     byte[] block3 = new byte[8];
        //     block3[1] = 1;
        //     block3.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     byte[] block4 = new byte[8];
        //     block4[0] = 1;
        //     block4.ToList().ForEach(i => Console.Write(i.ToString() + " "));
        //     Console.WriteLine();
        //     
        //     Console.WriteLine();
        //
        //     
        //     
        //     
        //     
        //     
        //     
        //     byte[] key = new byte[8];
        //     key[5] = 3;
        //     DES des = new DES();
        //     des.Key = key;
        //
        //     Ofb mode = new Ofb(IV, des);
        //     byte[][] allBlocks1 = 
        //     {
        //         block1,
        //         block2
        //     };
        //     var res1 = mode.EncryptAll(allBlocks1);
        //     foreach (var block in res1)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //
        //         Console.WriteLine();
        //     }
        //     Console.WriteLine();
        //     
        //     
        //     
        //     
        //     
        //     
        //     byte[][] allBlocks2 = 
        //     {
        //         block3,
        //         block4
        //     };
        //     mode = new Ofb(res1[res1.Length-1], des);
        //     var res2 = mode.EncryptAll(allBlocks2);
        //     foreach (var block in res2)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //
        //         Console.WriteLine();
        //     }
        //     Console.WriteLine();
        //
        //     
        //     
        //     
        //     
        //     
        //     
        //     
        //     mode = new Ofb(IV, des);
        //     var decrypted1 = mode.DecryptAll(res1);
        //     foreach (var block in decrypted1)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //     
        //         Console.WriteLine();
        //     }
        //     
        //     mode = new Ofb(res1[res1.Length-1], des);
        //     var decrypted2 = mode.DecryptAll(res2);
        //     foreach (var block in decrypted2)
        //     {
        //         foreach (var x in block)
        //         {
        //             Console.Write(x + " ");
        //         }
        //     
        //         Console.WriteLine();
        //     }
        //     
        // }
    }
}