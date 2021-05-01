using System;
using System.Linq;

namespace Task_5
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            byte[] st = new byte[8];
            st[5] = 1;
            st.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            
            
            DES des = new DES();
            des.Key = 41827491293;
            var result = des.Encrypt(st);
            result.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
	        
            var decrypted = des.Decrypt(result);
            decrypted.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
        }
    }
}