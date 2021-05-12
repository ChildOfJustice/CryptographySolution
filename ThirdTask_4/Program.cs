using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Task_8.AsyncCypher;
using ThirdTask_3;

namespace ThirdTask_4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var rsaCore = new RsaCore(generateKeys:false);
            
            //rsaCore.GenerateVulnerableKeys(516);
           
            
            
            var rsaCore = new RsaCore(516,weak:true);
            
            
            //export keys
            rsaCore.ExportPubKey("./resources/pubkey");
            rsaCore.ExportPrivateKey("./resources/privkey");
                    
                    
                    
                    
                    
            CypherMethods.EncryptKey(rsaCore, "./resources/key", "./resources/keyEncrypted");
            //CypherMethods.DecryptKey(rsaCore, "./resources/keyEncrypted", "./resources/keyDecrypted");
            
            BigInteger WienerD = Hack(rsaCore.eC, rsaCore.n);
            
            
            var temp = new RsaCore(generateKeys: false);
            temp.d = WienerD;
            temp.n = rsaCore.n;
            temp.CanDecrypt = true;
            
            CypherMethods.DecryptKey(temp, "./resources/keyEncrypted", "./resources/keyDecryptedHacked");
        }

        // static IEnumerator<BigInteger> convergents(BigInteger[] cf)
        // {
        //     BigInteger r = 0;
        //     BigInteger s = 1;
        //     BigInteger p = 1;
        //     BigInteger q = 0;
        //     foreach (var c in cf)
        //     {
        //         r = p;
        //         s = q;
        //         p =  c*p+r;
        //         q = c*q+s;
        //         
        //         yield return p, q;
        //     }
        //     
        // }
        
        public static BigInteger Hack(BigInteger e, BigInteger N)
        {
            BigInteger hackedD = -1;
            
            ContinuedFraction continuedFraction = new ContinuedFraction(e, N);
            List<Tuple<BigInteger, BigInteger >> convergents = continuedFraction.GetConvergents();
            foreach (var pair in convergents)
            {
                if(pair.Item2 > Sqrt(Sqrt(N))/3)
                    break;
                Console.WriteLine("Fraction: " + pair.Item1 + " | " + pair.Item2);
                hackedD = pair.Item2;
            }
            if(hackedD == -1)
                throw new CryptographicException();
            return hackedD;
        }
        
        private static BigInteger Sqrt(BigInteger n)
        {
            int bitlength = n.bitCount();
            BigInteger a = bitlength / 2;
            BigInteger b = bitlength % 2;

            BigInteger x = BigInteger.Pow(2, (a + b));
            while (true)
            {
                BigInteger y = (x + n / x) / 2;
                if (y >= x) return x;
                x = y;
            }
        }

        private static BigInteger Square(BigInteger n)
        {
            BigInteger s = Sqrt(n);
            if (s*s == n) return s;
            return -1;
        }
    }
}