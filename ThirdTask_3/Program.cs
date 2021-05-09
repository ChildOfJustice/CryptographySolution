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
            RsaCore rsa = new RsaCore(null);
            rsa.keySize = 512;
            
            rsa.PrepareAlgorithm();

            Console.WriteLine(rsa.GetPrivateKeyAsString(16));
            Console.WriteLine(rsa.GetPublicKeyAsString(16));
            
            var data = new ASCIIEncoding().GetBytes("Secret");
            BigInteger[] enc = new BigInteger[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                enc[i] = rsa.Encrypt((int)data[i]);
            }

            Console.WriteLine();
            foreach (var VARIABLE in enc)
            {
                Console.Write(" " + VARIABLE);
            }
            Console.WriteLine();


            byte[] decr = new byte[enc.Length];
            for (int i = 0; i < data.Length; i++)
            {
                decr[i] =(byte) rsa.Decrypt(enc[i]).IntValue();
            }

            Console.WriteLine("Decrypted data: " + new ASCIIEncoding().GetString(decr));
        }
    }
}