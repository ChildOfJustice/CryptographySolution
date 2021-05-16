using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ThirdTask_3
{
    public class RsaCore
    {
        
        //Log 256 (N)   + 1 
        
        public const int testConfidence = 20;
        
        private BigInteger p;
        private BigInteger q;

        
        // Encryption exponent
        public BigInteger eC;
        
        public BigInteger n;
        // (p-1)(q-1)
        private BigInteger phi;
        // (1+k*phi)/eC
        public BigInteger d;
        
        
        public uint keySize;
        public bool CanEncrypt;
        public bool CanDecrypt;

        public int numberSize;
        
        public RsaCore(uint _keySize=516, bool generateKeys=true, bool weak=false)
        {
            p = 0;
            q = 0;
            phi = 0;
            keySize = _keySize;

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
                CanEncrypt = true;
                CanDecrypt = true;
                GenerateKeys(weak);
                //}
            }
        }
        public void GenerateKeys(bool  weak=false)
        {
            bool algorithmReady = false;
            byte messageToTest = 77;

            while (!algorithmReady)
            {
                Generate_Primes();
                
                n = q * p;
                CalculatePhi();
                if (weak)
                {
                    GenerateWeakDecryptionExponent();
                    GenerateWeakEncryptionExponent();
                }
                else
                {
                    GenerateEncryptionExponent();
                    GenerateDecryptionExponent();
                }

                
                
                //TODO use your own log_256()
                //numberSize = (int)Math.Round(System.Numerics.BigInteger.Log(new System.Numerics.BigInteger(n.getBytes()), 256));
                
                
                //check:
                // {
                //     BigInteger encryptedTest = EncryptOneByte(messageToTest);
                //     //numberSize = encryptedTest.getBytes().Length;
                //     byte decryptedTest = DecryptOneByte(encryptedTest);
                //     MessageBox.Show("pt: " + messageToTest + " dec: " + decryptedTest);
                //     if (decryptedTest != messageToTest) continue;
                // }
                // MessageBox.Show("Max bigint size is: " + numberSize);
                algorithmReady = true;
            }
        }
        
        private void GenerateWeakDecryptionExponent()
        {
            BigInteger generatedD;
            
            generatedD = 125;

            for (; ; generatedD++)
            {
                if (GreatestCommonDivizor(phi, generatedD) == 1)
                {
                    d = generatedD;
                    break;
                }
            }

            if (d == 0)
            {
                throw new Exception("Cannnot select d!");
            }
        }
        
        private void GenerateWeakEncryptionExponent()
        {
            eC = (Extended_GDC(d, phi, true))[1];
        }
        


        


        public String GetPublicKeyAsString()
        {
            return eC.ToString("X") + "," + n.ToString("X");
        }
        public String GetPrivateKeyAsString()
        {
            return d.ToString("X") + "," + n.ToString("X");
        }
        public void ExportPubKey(string outPutFile)
        {
            var exportedKey = GetPublicKeyAsString();
            var exportedKeyBytes= new ASCIIEncoding().GetBytes(exportedKey);
            using (var outputStream = File.Open(outPutFile, FileMode.Create))
                outputStream.Write(exportedKeyBytes, 0, exportedKeyBytes.Length);
        }
        public void ExportPrivateKey(string outPutFile)
        {
            var exportedKey = GetPrivateKeyAsString();
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
            sbE.Append("0");
            sbN.Append("0");// to convert to BigInteger
            
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
            
            
            eC = BigInteger.Parse(sbE.ToString(),NumberStyles.AllowHexSpecifier);
            n = BigInteger.Parse(sbN.ToString(),NumberStyles.AllowHexSpecifier);
            CanEncrypt = true;
        }
        public void ImportPrivateKey(string inPutFile)
        {
           
            StreamReader sr = new StreamReader(inPutFile);
            string line = sr.ReadLine();
            StringBuilder sbD = new StringBuilder();
            StringBuilder sbN = new StringBuilder();
            sbD.Append("0");
            sbN.Append("0");// to convert to BigInteger

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

            d = BigInteger.Parse(sbD.ToString(),NumberStyles.AllowHexSpecifier);
            n = BigInteger.Parse(sbN.ToString(),NumberStyles.AllowHexSpecifier);
            CanDecrypt = true;
        }
        public BigInteger EncryptOneByte(byte mToPow)
        {
            if (!CanEncrypt)
                throw new CryptographicException("Dont have a public key imported to do encryption process");

            BigInteger exp = eC;
            BigInteger modN = n;
            BigInteger modRes = BigInteger.ModPow(mToPow, exp, modN);

            return modRes;
        }
        public BigInteger[] Encrypt(byte[] data)
        {
            BigInteger[] encrypted = new BigInteger[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                encrypted[i] = EncryptOneByte(data[i]);
            }

            return encrypted;
        }
        
        public byte DecryptOneByte(BigInteger c)
        {
            if (!CanDecrypt)
                throw new CryptographicException("Dont have a private key imported to do decryption process");

            BigInteger cToPow = c;
            BigInteger exp = d;
            BigInteger modN = n;
            var powed = BigInteger.ModPow(cToPow, exp, modN);
            byte modRes = (byte)(int) powed;
            return modRes;
        }
        public byte[] Decrypt(BigInteger[] data)
        {
            byte[] decrypted = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                decrypted[i] = (byte)(int)DecryptOneByte(data[i]);
            }

            return decrypted;
        }













        private void Generate_Primes()
        {
            uint pLength = (uint)(keySize / 2.0), qLength = keySize - pLength;

            var tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                p = ThirdTask_2.Program.FindPrime((int)pLength, testConfidence);
            }));
            tasks.Add(Task.Run(() =>
            {
                q = ThirdTask_2.Program.FindPrime((int)qLength, testConfidence);
            }));

            Task t = Task.WhenAll(tasks);
            try
            {
                t.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }   


            if(p < q)
            {
                BigInteger tmp = p;
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
            // RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            //
            // byte[] _a = new byte[(uint)(keySize / 2.0)];
            //
            // rng.GetBytes(_a);
            // generatedE = new BigInteger(_a);
            
            var eSize = (int) (keySize / 2.0);
            // var minE = BigInteger.Pow(2, eSize);
            // var maxE = BigInteger.Pow(2, eSize+1)-1;
            // RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            
            // byte[] randomBytes = new byte[(eSize / 8)+1];
            byte[] randomBytes = new byte[eSize];
            Random rand = new Random(Environment.TickCount);
            
            BigInteger generatedE;
            
            do
            {
                rand.NextBytes(randomBytes);
                //Making the extra byte 0x0 so the BigInts are unsigned (little endian).
                randomBytes[randomBytes.Length - 1] = 0x0;

                //Setting the bottom bit and top two bits of the number.
                //This ensures the number is odd, and ensures the high bit of N is set when generating keys.
                SetBitInByte(0, ref randomBytes[0]);
                SetBitInByte(7, ref randomBytes[randomBytes.Length - 2]);
                SetBitInByte(6, ref randomBytes[randomBytes.Length - 2]);
                generatedE = new BigInteger(randomBytes);
            }
            while (GreatestCommonDivizor(phi, generatedE) != 1);
            
            //generatedE = GenerateOddNBitNumber((uint)(keySize / 2.0));

            // for (; ; generatedE++)
            // {
            //     if (GreatestCommonDivizor(phi, generatedE) == 1)
            //     {
            //         eC = generatedE;
            //         break;
            //     }
            // }
            eC = generatedE;

            MessageBox.Show("Generated e: " + eC);
            MessageBox.Show("Condition: GCD(phi, eC)=" + GreatestCommonDivizor(phi, eC));
        }
        public static void SetBitInByte(int bitNumFromRight, ref byte toSet)
        {
            byte mask = (byte)(1 << bitNumFromRight);
            toSet |= mask;
        }
        private void GenerateDecryptionExponent()
        {
            // d = (Extended_GDC(eC, phi, false))[0];
            // if (d < 0)
            //     d = -d;
            
            //d = (Extended_GDC(eC, phi, true))[1];
            
            
            // d = (ExtendedEuclideanAlgorithm(eC, phi))[0];
            // while(d < 0)
            //     d += phi;
            
            BigInteger x;
            BigInteger y;
            ThirdTask_1.Program.ExtendedEuclideanAlgorithm(eC, phi, out x, out y);
            d = x;
            while(d < 0)
                d += phi;
            
            MessageBox.Show("Generated d: " + d);

            MessageBox.Show("Condition e*d + phi*y = GCD(eC,phi) = " + (eC * x + phi * y));
        }
        public static BigInteger ModularInverse(BigInteger u, BigInteger v)
        {
            //Declaring new variables on the heap.
            BigInteger inverse, u1, u3, v1, v3, t1, t3, q = new BigInteger();
            //Staying on the stack, quite small, so no need for extra memory time.
            BigInteger iteration;

            //Stating initial variables.
            u1 = 1;
            u3 = u;
            v1 = 0;
            v3 = v;

            //Beginning iteration.
            iteration = 1;
            while (v3 != 0)
            {
                //Divide and sub q, t3 and t1.
                q = u3 / v3;
                t3 = u3 % v3;
                t1 = u1 + q * v1;

                //Swap variables for next pass.
                u1 = v1; v1 = t1; u3 = v3; v3 = t3;
                iteration = -iteration;
            }

            if (u3 != 1)
            {
                //No inverse, return 0.
                return 0;
            }
            else if (iteration < 0)
            {
                inverse = v - u1;
            }
            else
            {
                inverse = u1;
            }

            //Return.
            return inverse;
        }
        //________________________________________________________________________________________________
        public byte[] EncryptBytes(byte[] bytes)
        {
            //Checking that the size of the bytes is less than n, and greater than 1.
            // if (1>bytes.Length || bytes.Length>=public_key.n.ToByteArray().Length)
            // {
            //     throw new Exception("Bytes given are longer than length of key element n (" + bytes.Length + " bytes).");
            // }

            //Padding the array to unsign.
            byte[] bytes_padded = new byte[bytes.Length+2];
            Array.Copy(bytes, bytes_padded, bytes.Length);
            bytes_padded[bytes_padded.Length-1] = 0x00;
            
            //Setting high byte right before the data, to prevent data loss.
            bytes_padded[bytes_padded.Length-2] = 0xFF;

            //Computing as a BigInteger the encryption operation.
            var cipher_bigint = new BigInteger();
            var padded_bigint = new BigInteger(bytes_padded);
            cipher_bigint = BigInteger.ModPow(padded_bigint, eC, n);

            //Returning the byte array of encrypted bytes.
            return cipher_bigint.ToByteArray();
        }

        //Decrypts a set of bytes when given a private key.
        public byte[] DecryptBytes(byte[] bytes)
        {
            //Checking that the private key is legitimate, and contains d.
            // if (private_key.type!=KeyType.PRIVATE)
            // {
            //     throw new Exception("Private key given for decrypt is classified as non-private in instance.");
            // }

            //Decrypting.
            var plain_bigint = new BigInteger();
            var padded_bigint = new BigInteger(bytes);
            plain_bigint = BigInteger.ModPow(padded_bigint, d, n);

            //Removing all padding bytes, including the marker 0xFF.
            byte[] plain_bytes = plain_bigint.ToByteArray();
            int lengthToCopy=-1;
            for (int i=plain_bytes.Length-1; i>=0; i--) 
            {
                if (plain_bytes[i]==0xFF)
                {
                    lengthToCopy = i;
                    break;
                }
            }

            //Checking for a failure to find marker byte.
            if (lengthToCopy==-1)
            {
                throw new Exception("Marker byte for padding (0xFF) not found in plain bytes.\nPossible Reasons:\n1: PAYLOAD TOO LARGE\n2: KEYS INVALID\n3: ENCRYPT/DECRYPT FUNCTIONS INVALID");
            }

            //Copying into return array, returning.
            byte[] return_array = new byte[lengthToCopy];
            Array.Copy(plain_bytes, return_array, lengthToCopy);
            return return_array;
        }
        //________________________________________________________________________________________________
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

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
    }
}