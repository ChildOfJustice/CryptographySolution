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

            FeistelNet fn = new NotAbstractFeistelNet();
            fn.FeistelRoundQuantity = 16;
            fn.Key = 100;
            var result = fn.Encrypt(st);
            result.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
	        
            var decrypted = fn.Decrypt(result);
            decrypted.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
        }
    }
}