using System;
using System.Linq;
using EncryptionModes;
using Task_5;

namespace ProjectForTesting
{
    internal class Program
    {
        public static void Main(string[] args)
        {
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




            TestDesEncryptionModeCbc();
            //TestDesEncryptionModeCfb();
            //TestDesEncryptionModeOfb();
        }



        public static void TestDesEncryptionModeCbc()
        {
            byte[] IV = new byte[8];
            IV[1] = 1;
            // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            
            byte[] block1 = new byte[8];
            block1[5] = 1;
            // block1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            byte[] block2 = new byte[8];
            block2[6] = 1;
            // block2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            
            byte[] block3 = new byte[8];
            block3[1] = 1;
            // block3.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            byte[] block4 = new byte[8];
            block4[2] = 1;
            // block4.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            //
            // Console.WriteLine();

            
            
            
            
            
            
            byte[] key = new byte[8];
            key[5] = 3;
            DES des = new DES();
            des.Key = key;

            Cbc mode = new Cbc(IV, des);
            byte[][] allBlocks1 = 
            {
                block1,
                block1,
                block1,
                block1,
                block1,
                block1,
                block1,
                block2
            };
            Console.WriteLine("Data1:");
            foreach (var block in allBlocks1)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
            
            
            Console.WriteLine("Encrypted Data1:");
            var res1 = mode.EncryptAll(allBlocks1);
            foreach (var block in res1)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
            
            
            
            
            
            
            // byte[][] allBlocks2 = 
            // {
            //     block3,
            //     block3,
            //     block4
            // };
            // Console.WriteLine("Data2:");
            // foreach (var block in allBlocks2)
            // {
            //     foreach (var x in block)
            //     {
            //         Console.Write(x + " ");
            //     }
            //
            //     Console.WriteLine();
            // }
            // Console.WriteLine();
            
            
            // Console.WriteLine("Encrypted Data2:");
            // mode = new Cbc(res1[res1.Length-1], des);
            // var res2 = mode.EncryptAll(allBlocks2);
            // foreach (var block in res2)
            // {
            //     foreach (var x in block)
            //     {
            //         Console.Write(x + " ");
            //     }
            //
            //     Console.WriteLine();
            // }
            // Console.WriteLine();

            
            
            
            
            
            
            Console.WriteLine("Decrypted Data1:");
            mode = new Cbc(IV, des);
            var decrypted1 = mode.DecryptAll(res1);
            foreach (var block in decrypted1)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }
            
                Console.WriteLine();
            }
            
            // Console.WriteLine("Decrypted Data2:");
            // mode = new Cbc(res1[res1.Length-1], des);
            // var decrypted2 = mode.DecryptAll(res2);
            // foreach (var block in decrypted2)
            // {
            //     foreach (var x in block)
            //     {
            //         Console.Write(x + " ");
            //     }
            //
            //     Console.WriteLine();
            // }
            
        }
        public static void TestDesEncryptionModeCfb()
        {
            byte[] IV = new byte[8];
            IV[1] = 1;
            // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            
            byte[] block1 = new byte[8];
            block1[5] = 1;
            block1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            byte[] block2 = new byte[8];
            block2[6] = 1;
            block2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            
            byte[] block3 = new byte[8];
            block3[1] = 1;
            block3.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            byte[] block4 = new byte[8];
            block4[7] = 1;
            block4.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            
            Console.WriteLine();

            
            
            
            
            
            
            byte[] key = new byte[8];
            key[5] = 3;
            DES des = new DES();
            des.Key = key;

            Cfb mode = new Cfb(IV, des);
            byte[][] allBlocks1 = 
            {
                block1,
                block2
            };
            var res1 = mode.EncryptAll(allBlocks1);
            foreach (var block in res1)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
            
            
            
            
            
            
            byte[][] allBlocks2 = 
            {
                block3,
                block4
            };
            mode = new Cfb(res1[res1.Length-1], des);
            var res2 = mode.EncryptAll(allBlocks2);
            foreach (var block in res2)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();

            
            
            
            
            
            
            
            mode = new Cfb(IV, des);
            var decrypted1 = mode.DecryptAll(res1);
            foreach (var block in decrypted1)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }
            
                Console.WriteLine();
            }
            
            mode = new Cfb(res1[res1.Length-1], des);
            var decrypted2 = mode.DecryptAll(res2);
            foreach (var block in decrypted2)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }
            
                Console.WriteLine();
            }
            
        }
        public static void TestDesEncryptionModeOfb()
        {
            byte[] IV = new byte[8];
            IV[1] = 1;
            // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            
            byte[] block1 = new byte[8];
            block1[5] = 1;
            block1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            byte[] block2 = new byte[8];
            block2[6] = 1;
            block2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            
            byte[] block3 = new byte[8];
            block3[1] = 1;
            block3.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            byte[] block4 = new byte[8];
            block4[0] = 1;
            block4.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            
            Console.WriteLine();

            
            
            
            
            
            
            byte[] key = new byte[8];
            key[5] = 3;
            DES des = new DES();
            des.Key = key;

            Ofb mode = new Ofb(IV, des);
            byte[][] allBlocks1 = 
            {
                block1,
                block2
            };
            var res1 = mode.EncryptAll(allBlocks1);
            foreach (var block in res1)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
            
            
            
            
            
            
            byte[][] allBlocks2 = 
            {
                block3,
                block4
            };
            mode = new Ofb(res1[res1.Length-1], des);
            var res2 = mode.EncryptAll(allBlocks2);
            foreach (var block in res2)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();

            
            
            
            
            
            
            
            mode = new Ofb(IV, des);
            var decrypted1 = mode.DecryptAll(res1);
            foreach (var block in decrypted1)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }
            
                Console.WriteLine();
            }
            
            mode = new Ofb(res1[res1.Length-1], des);
            var decrypted2 = mode.DecryptAll(res2);
            foreach (var block in decrypted2)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }
            
                Console.WriteLine();
            }
            
        }
    }
}