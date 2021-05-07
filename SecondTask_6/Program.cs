using System;
using System.Linq;
using EncryptionModes;
using Task_5;

namespace SecondTask_6
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Testing ECB encryption mode:");
            TestDesEncryptionModeEcb();
            Console.WriteLine("//////////////");
            
            Console.WriteLine("Testing CBC encryption mode:");
            TestDesEncryptionModeCbc();
            Console.WriteLine("//////////////");
            
            Console.WriteLine("Testing CFB encryption mode:");
            TestDesEncryptionModeCfb();
            Console.WriteLine("//////////////");
            
            Console.WriteLine("Testing OFB encryption mode:");
            TestDesEncryptionModeOfb();
            Console.WriteLine("//////////////");
        }
        
        public static void TestDesEncryptionModeEcb()
        {

            Console.WriteLine("Data to be encrypted:");
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

            Ecb mode = new Ecb(des);
            byte[][] allBlocks = 
            {
                st2,
                st3
            };
            var res = mode.EncryptAll(allBlocks);


            Console.WriteLine("Encrypted data:");
            foreach (var block in res)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();


            Console.WriteLine("Decrypted data:");
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
        public static void TestDesEncryptionModeCbc()
        {
            byte[] iv = new byte[8];
            iv[1] = 1;
            // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            
            Console.WriteLine("Data to be encrypted:");
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

            Cbc mode = new Cbc(iv, des);
            byte[][] allBlocks = 
            {
                st2,
                st3
            };
            var res = mode.EncryptAll(allBlocks);

            Console.WriteLine("Encrypted data:");
            foreach (var block in res)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Decrypted data:");
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
            
            Console.WriteLine("Data to be encrypted:");
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

            Console.WriteLine("Encrypted data:");
            foreach (var block in res)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Decrypted data:");
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
        public static void TestDesEncryptionModeOfb()
        {
            byte[] iv = new byte[8];
            iv[1] = 1;
            // st1.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            
            Console.WriteLine("Data to be encrypted:");
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
            
            byte[] block5 = new byte[8];
            block5[7] = 1;
            block5.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            
            Console.WriteLine();

            byte[] key = new byte[8];
            key[5] = 3;
            DES des = new DES();
            des.Key = key;

            Ofb mode = new Ofb(iv, des);
            byte[][] allBlocks = 
            {
                block1,
                block2,
                block3,
                block4,
                block5
            };
            var res = mode.EncryptAll(allBlocks);

            Console.WriteLine("Encrypted data:");
            foreach (var block in res)
            {
                foreach (var x in block)
                {
                    Console.Write(x + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Decrypted data:");
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