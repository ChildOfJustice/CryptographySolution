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
            var result = fn.CipherTemplateMethod(st, true);
            result.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
	        
            var decrypted = fn.CipherTemplateMethod(result, false);
            decrypted.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
        }
    }
}