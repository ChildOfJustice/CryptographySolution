using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Task_8.AsyncCypher;
using ThirdTask_3;
using ThirdTask_2;

namespace ThirdTask_4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var rsaCore = new RsaCore(generateKeys:false);
            
            //rsaCore.GenerateVulnerableKeys(516);
           
            
            
            var rsaCore = new RsaCore(516,weak:false);
            
            
            //export keys
            rsaCore.ExportPubKey("./resources/pubkey");
            rsaCore.ExportPrivateKey("./resources/privkey");
                    
                    
                    
                    
                    
            CypherMethods.EncryptKey(rsaCore, "./resources/key", "./resources/keyEncrypted");
            CypherMethods.DecryptKey(rsaCore, "./resources/keyEncrypted", "./resources/keyDecrypted");
            
            // BigInteger WienerD = Hack(rsaCore.eC, rsaCore.n);
            //
            // var temp = new RsaCore(generateKeys: false);
            // temp.d = WienerD;
            // temp.n = rsaCore.n;
            //
            // CypherMethods.DecryptKey(temp, "./resources/keyEncrypted", "./resources/keyDecryptedHacked");
        }
        
        public static BigInteger ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            
    
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
 
            BigInteger gcd = ExtendedEuclideanAlgorithm(b % a, a, out x, out y);
    
            BigInteger newY = x;
            BigInteger newX = y - (b / a) * x;
    
            x = newX;
            y = newY;
            return gcd;
        }
        public static BigInteger Hack(BigInteger e, BigInteger N)
        {
            ContinuedFraction continuedFraction = new ContinuedFraction(e, N);
            List<Tuple<BigInteger, BigInteger >> convergents = continuedFraction.GetConvergents();
            foreach (var pair in convergents)
            {
                if (pair.Item1 == 0) continue;
                BigInteger f = (e * pair.Item2 - 1) / pair.Item1;
                BigInteger b = (N - f) + 1;
                BigInteger D = b * b - 4*N;
                if (D >= 0)
                {
                    BigInteger sqr = Square(D);
                    if (sqr != -1) {
                        BigInteger p = (-b - sqr) / 2;
                        BigInteger q = (-b + sqr) / 2;
                        if (p * q == N) return pair.Item2;
                    }
                }
            }

            throw new CryptographicException();
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