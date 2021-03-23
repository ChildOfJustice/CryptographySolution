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
            var result = des.CipherTemplateMethod(st, true);
            result.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
	        
            var decrypted = des.CipherTemplateMethod(result, false);
            decrypted.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
        }
    }
}