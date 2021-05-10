using System;
using System.Text;

namespace ThirdTask_3
{
    internal class Program
    {
        //new ASCIIEncoding().GetBytes(sData);
        //MessageBox.Show("Decrypted data: " + new ASCIIEncoding().GetString(Final));
        
        public static void Main(string[] args)
        {
            RsaCore rsa = new RsaCore(500);

            Console.WriteLine(rsa.GetPrivateKeyAsString(16));
            Console.WriteLine(rsa.GetPublicKeyAsString(16));
            
            var data = new ASCIIEncoding().GetBytes("Secret 2");

            var encrypted = rsa.Encrypt(data);
            
            
            Console.WriteLine();
            foreach (var VARIABLE in encrypted)
            {
                Console.Write(" " + VARIABLE);
            }
            Console.WriteLine();

            var decrypted = rsa.Decrypt(encrypted);
            
            
            Console.WriteLine("Decrypted data: " + new ASCIIEncoding().GetString(decrypted));
        }
    }
}