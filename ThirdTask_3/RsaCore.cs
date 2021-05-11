using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ThirdTask_3
{
    public class RsaCore
    {
        public const int testConfidence = 10;
        
        private BigInteger p;
        private BigInteger q;

        
        // Encryption exponent
        private BigInteger eC;
        
        private BigInteger n;
        // (p-1)(q-1)
        private BigInteger phi;
        // (1+k*phi)/eC
        private BigInteger d;
        
        
        public uint keySize;
        public bool CanEncrypt;
        public bool CanDecrypt;
        
        
        public RsaCore(uint _keySize=516, bool generateKeys=true)
        {
            p = 0;
            q = 0;
            phi = 0;

            if (generateKeys)
            {
                // if (keyData != null && keyData.Length == 3)
                // {
                //     if (keyData[0] != null && keyData[2] != null)
                //     {
                //         eC = new BigInteger(keyData[0], 10);
                //         n = new BigInteger(keyData[1], 10);
                //         d = new BigInteger(keyData[2], 10);
                //         CanEncrypt = true;
                //         CanDecrypt = true;
                //     }
                //
                //     if (keyData[0] != null && keyData[2] == null)
                //     {
                //         eC = new BigInteger(keyData[0], 10);
                //         n = new BigInteger(keyData[1], 10);
                //         CanEncrypt = true;
                //         CanDecrypt = false;
                //     }
                //
                //     if (keyData[0] == null && keyData[2] != null)
                //     {
                //         d = new BigInteger(keyData[2], 10);
                //         n = new BigInteger(keyData[1], 10);
                //         CanEncrypt = false;
                //         CanDecrypt = true;
                //     }
                //
                //     keySize = (uint)n.bitCount();
                // }
                // else
                // {
                    eC = 0;
                    n = 0;
                    keySize = _keySize;
                    CanEncrypt = true;
                    CanDecrypt = true;
                    GenerateKeys();
                //}
            }
        }
        public void GenerateKeys()
        {
            bool algorithmReady = false;
            BigInteger messageToTest = new BigInteger(77);

            while (!algorithmReady)
            {
                Generate_Primes();
                n = q * p;
                CalculatePhi();
                GenerateEncryptionExponent();

                // For a key of 1024 bits, the d was found in 192 miliseconds
                GenerateDecryptionExponent();
                
                //check:
                {
                    BigInteger encryptedTest = EncryptOneByte(messageToTest);
                    BigInteger decryptedTest = DecryptOneByte(encryptedTest);

                    if (decryptedTest != messageToTest) continue;
                }
                algorithmReady = true;
            }
        }
        public String GetPublicKeyAsString(int keyBase)
        {
            return eC.ToString(keyBase) + "," + n.ToString(keyBase);
        }
        public String GetPrivateKeyAsString(int keyBase)
        {
            return d.ToString(keyBase) + "," + n.ToString(keyBase);
        }
        public void ExportPubKey(string outPutFile)
        {
            var exportedKey = GetPublicKeyAsString(16);
            var exportedKeyBytes= new ASCIIEncoding().GetBytes(exportedKey);
            using (var outputStream = File.Open(outPutFile, FileMode.Create))
                outputStream.Write(exportedKeyBytes, 0, exportedKeyBytes.Length);
        }
        public void ExportPrivateKey(string outPutFile)
        {
            var exportedKey = GetPrivateKeyAsString(16);
            var exportedKeyBytes= new ASCIIEncoding().GetBytes(exportedKey);
            using (var outputStream = File.Open(outPutFile, FileMode.Create))
                outputStream.Write(exportedKeyBytes, 0, exportedKeyBytes.Length);
        }
        public void ImportPubKey(string inPutFile)
        {
           
            StreamReader sr = new StreamReader(inPutFile);
            string line = sr.ReadLine();
            StringBuilder sbE = new StringBuilder();
            StringBuilder sbN = new StringBuilder();

            int i = 0;
            while (line[i] != ',')
            {
                sbE.Append(line[i]);
                i++;
            }

            i++;
            while (i != line.Length)
            {
                sbN.Append(line[i]);
                i++;
            }

            eC = new BigInteger(sbE.ToString(), 16);
            n = new BigInteger(sbN.ToString(), 16);
            CanEncrypt = true;
        }
        public void ImportPrivateKey(string inPutFile)
        {
           
            StreamReader sr = new StreamReader(inPutFile);
            string line = sr.ReadLine();
            StringBuilder sbD = new StringBuilder();
            StringBuilder sbN = new StringBuilder();

            int i = 0;
            while (line[i] != ',')
            {
                sbD.Append(line[i]);
                i++;
            }

            i++;
            while (i != line.Length)
            {
                sbN.Append(line[i]);
                i++;
            }

            d = new BigInteger(sbD.ToString(), 16);
            n = new BigInteger(sbN.ToString(), 16);
            CanDecrypt = true;
        }
        public BigInteger EncryptOneByte(BigInteger mToPow)
        {
            if (!CanEncrypt)
                return -1;

            BigInteger exp = new BigInteger(eC);
            BigInteger modN = new BigInteger(n);
            BigInteger modRes = mToPow.modPow(exp, modN);

            return modRes;
        }
        public BigInteger[] Encrypt(byte[] data)
        {
            BigInteger[] encrypted = new BigInteger[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                encrypted[i] = EncryptOneByte((int)data[i]);
            }

            return encrypted;
        }
        public BigInteger[] Encrypt(BigInteger[] data)
        {
            BigInteger[] encrypted = new BigInteger[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                encrypted[i] = EncryptOneByte(data[i]);
            }

            return encrypted;
        }
        
        public BigInteger DecryptOneByte(BigInteger c)
        {
            if (!CanDecrypt)
                return -1;

            BigInteger cToPow = new BigInteger(c);
            BigInteger exp = new BigInteger(d);
            BigInteger modN = new BigInteger(n);
            BigInteger modRes = cToPow.modPow(exp, modN);

            return modRes;
        }
        public byte[] Decrypt(BigInteger[] data)
        {
            byte[] decrypted = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                decrypted[i] =(byte) DecryptOneByte(data[i]).IntValue();
            }

            return decrypted;
        }
        public BigInteger[] DecryptToBigIntegers(BigInteger[] data)
        {
            BigInteger[] decrypted = new BigInteger[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                decrypted[i] = DecryptOneByte(data[i]);
            }
        
            return decrypted;
        }


        



       
        
        
        
        
        
        
        private void Generate_Primes()
        {
            uint pLength = (uint)(keySize / 2.0), qLength = keySize - pLength;
            
        
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                do 
                {
                    p = GenerateOddNBitNumber(pLength);
                } while (!ThirdTask_2.PrimeTests.RabinMillerTest(p, testConfidence));
            }));
            tasks.Add(Task.Run(() =>
            {
                do 
                {
                    q = GenerateOddNBitNumber(qLength);
                } while (!ThirdTask_2.PrimeTests.RabinMillerTest(q, testConfidence));
            }));

            Task t = Task.WhenAll(tasks);
            try {
                t.Wait();
            }
            catch {}   


            if(p < q)
            {
                BigInteger tmp = new BigInteger(p);
                p = q;
                q = tmp;
            }
        }

        
        private void CalculatePhi()
        { 
            phi = (p - 1) * (q - 1);
        }
        
        private void GenerateEncryptionExponent()
        {
            BigInteger generatedE = GenerateOddNBitNumber((uint)(keySize / 2.0));

            for (; ; generatedE++)
            {
                if (GreatestCommonDivizor(phi, generatedE) == 1)
                {
                    eC = generatedE;
                    break;
                }
            }

            if (eC == 0)
            {
                throw new Exception("Cannnot select e!");
            }
        }
        
        private void GenerateDecryptionExponent()
        {
            d = (Extended_GDC(eC, phi, true))[1];
        }
        
        
        
        
        public static BigInteger GreatestCommonDivizor(BigInteger x, BigInteger y)
        {
            BigInteger tmp;

            if (x < y)
            {
                tmp = x;
                x = y;
                y = tmp;
            }

            while (true)
            {
                tmp = x % y;
                x = y;
                y = tmp;

                if (y == 0) break;
            }

            // This will be the GCD
            return x;
        }

       
        public static BigInteger[] Extended_GDC(BigInteger a, BigInteger modulus, Boolean calcOnlyModuloInverse)
        {
            BigInteger x, lastX, b_, y, lastY, a_, quotient, temp, temp2, temp3;
            BigInteger[] result;

            if (modulus == 0) return new BigInteger[] { 1, 0, a };

            // We assure ourselves that the two algorithms below will give good results in any case
            if (a < modulus)
            {
                x = 0; lastX = 1; b_ = modulus;
                y = 1; lastY = 0; a_ = a;
            }
            else
            {
                x = 1; lastX = 0; b_ = a;
                y = 0; lastY = 1; a_ = modulus;
            }

            if (calcOnlyModuloInverse)
            {
                // modulo inverse calculation
                // http://snippets.dzone.com/posts/show/4256
                while (a_ > 0)
                {
                    temp = a_;
                    quotient = b_ / a_;     // If not BigInteger used, then use direct cast to int

                    a_ = b_ % temp;
                    b_ = temp;
                    temp = y;

                    y = lastY - quotient * temp;
                    lastY = temp;
                }

                lastY %= modulus;

                if (lastY < 0) lastY = (lastY + modulus) % modulus;
                result = new BigInteger[] { 0, lastY, 0 };
            }
            else
            {
                // Extended euclidian algorithm
                // http://everything2.com/title/Extended+Euclidean+algorithm
                // The only good implementation of the full Extended Euclidian Algorithm that I found
                while (a_ > 1)
                {
                    quotient = b_ / a_;     // If not BigInteger used, then use direct cast to int
                    temp = x - quotient * y;
                    temp2 = lastX - quotient * lastY;
                    temp3 = b_ - quotient * a_;

                    x = y; lastX = lastY; b_ = a_;
                    y = temp; lastY = temp2; a_ = temp3;
                }

                if (a_ == 0) result = new BigInteger[] { x, lastX, b_ };
                else result = new BigInteger[] { y, lastY, a_ };
            }

            return result;
        }

        //generate a random number of n bits
        public static BigInteger GenerateOddNBitNumber(uint nrBits)
        {
            //String tmp = "";
            int nr;
            BigInteger b = new BigInteger();

            Random rand = new Random((int)DateTime.Now.Ticks);

            // This cycle can be changed to generate not one bit, but more bits at a time
            for (uint i = 0; i < nrBits; i++)
            {
                nr = rand.Next(2);

                // The generated binary number will be calculated like this, because
                // the assignation of the bits is done from the lower to high ones
                // tmp = nr.ToString() + tmp;
                if (nr == 1) b.setBit(i);
            }

            // We assure ourselves that the number is odd, by setting the lower bit to 1
            b.setBit(0);

            // this ensures that the high bit of n is also set
            b.setBit(nrBits - 2);
            b.setBit(nrBits - 1);

            return b;
        }
    }
}