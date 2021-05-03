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




            //TestDesEncryptionModeCbc();
            TestDesEncryptionModeCfb();
        }



        public static void TestDesEncryptionModeCbc()
        {
            byte[] st1 = new byte[8];
            st1[1] = 1;
            // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            
            byte[] st2 = new byte[8];
            st2[5] = 1;
            st2.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            
            byte[] st3 = new byte[8];
            st3[6] = 1;
            st3.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            Console.WriteLine();

            byte[] key = new byte[8];
            key[5] = 3;
            DES des = new DES();
            des.Key = key;

            Cbc mode = new Cbc(st1, des);
            byte[][] allBlocks = 
            {
                st2,
                st3
            };
            var res = mode.EncryptAll(allBlocks);


            foreach (var block in res)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();


            var decrypted = mode.DecryptAll(res);
            foreach (var block in decrypted)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }
            
                Console.WriteLine();
            }
            
        }
        public static void TestDesEncryptionModeCfb()
        {
            byte[] iv = new byte[8];
            iv[1] = 1;
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
            Console.WriteLine();

            byte[] key = new byte[8];
            key[5] = 3;
            DES des = new DES();
            des.Key = key;

            Cfb mode = new Cfb(iv, des);
            byte[][] allBlocks = 
            {
                block1,
                block2
            };
            var res = mode.EncryptAll(allBlocks);


            foreach (var block in res)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();


            var decrypted = mode.DecryptAll(res);
            foreach (var block in decrypted)
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