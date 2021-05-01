using System;
using System.Linq;
using EncryptionModes;

namespace Task_5
{
    internal class Program
    {
        public static void Main(string[] args)
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
            // DES des = new DES();
            // des.Key = 41827491293;
            // var result = des.Encrypt(st);
            // result.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
            //
            // var decrypted = des.Decrypt(result);
            // decrypted.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            // Console.WriteLine();
        }
    }
}