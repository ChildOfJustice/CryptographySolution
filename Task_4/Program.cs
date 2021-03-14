using System;
using System.Linq;

namespace Task_4
{
    internal class Program
    {
        
        
        public static void Main(string[] args)
        {
            byte[] st = new byte[8];
            st[0] = 1;
            st.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();

            FeistelNet fn = new FeistelNet();
            fn.Key = 255;
            var result = fn.Cipher(st, true);
            result.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
	        
            var decrypted = fn.Cipher(result, false);
            decrypted.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
        }
    }
}