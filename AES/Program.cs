﻿using System;

namespace AES
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            byte[] data = 
            {
                0x19, 0x3d, 0xe3, 0xbe, 0xa0, 0xf4, 0xe2, 0x2b, 0x9a, 0xc6, 0x8d, 0x2a, 0xe9, 0xf8, 0x48, 0x08
            };
            
            AesCore aes = new AesCore();
            aes.Cipher(data);

            return;

            
            AesMatrix testMatrix = new AesMatrix(data);
            // i is row, j - column
            //Console.WriteLine(testMatrix.Get(1, 2));
            ;
            testMatrix.PrintMatrix();
            testMatrix.Set(99,1,3);
            //byte counter = 101;
            // for (int i = 0; i < 4; i++)
            // {
            //     for (int j = 0; j < 4; j++)
            //     {
            //         testMatrix.Set(counter, i, j);
            //         counter++;
            //     }
            //     
            // }
            testMatrix.PrintMatrix();
            //Console.WriteLine(testMatrix.Get(2, 1));
            
            //Console.WriteLine(testMatrix.Get(3,3));
        }
    }
}