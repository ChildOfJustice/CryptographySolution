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
            RsaCore rsa = new RsaCore(516);

            Console.WriteLine("Public key as hex string:");
            Console.WriteLine(rsa.GetPublicKeyAsString(16));
            Console.WriteLine();
            
            Console.WriteLine("Private key as hex string:");
            Console.WriteLine(rsa.GetPrivateKeyAsString(16));
            Console.WriteLine();


            Console.WriteLine("Data to be encrypted:");
            var dataS = "Secret 2";
            Console.WriteLine(dataS);
            var data = new ASCIIEncoding().GetBytes(dataS);

            
            
            
            var encrypted = rsa.Encrypt(data);
            Console.WriteLine();
            Console.WriteLine("Encrypted data:");
            foreach (var VARIABLE in encrypted)
            {
                Console.WriteLine(VARIABLE);
            }
            Console.WriteLine();

            var decrypted = rsa.Decrypt(encrypted);
            Console.WriteLine("Decrypted data: " + new ASCIIEncoding().GetString(decrypted));
        }
    }
}