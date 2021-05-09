using System;

namespace ThirdTask_3
{
    public class RsaCore
    {
        private const Int32 rabinMillerConfidence = 10;
        
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
        
        
        public RsaCore(Object[] keyData)
        {
            this.p = 0;
            this.q = 0;
            this.phi = 0;

            if (keyData != null && keyData.Length == 3)
            {
                if (keyData[0] != null && keyData[2] != null)
                {
                    this.eC = new BigInteger((String)(keyData[0]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    this.d = new BigInteger((String)(keyData[2]), 10);
                    //this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
                }

                if (keyData[0] != null && keyData[2] == null)
                {
                    this.eC = new BigInteger((String)(keyData[0]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    //.actions = ActionsAvailableEnum.Encrypt;
                }

                if (keyData[0] == null && keyData[2] != null)
                {
                    this.d = new BigInteger((String)(keyData[2]), 10);
                    this.n = new BigInteger((String)(keyData[1]), 10);
                    //this.actions = ActionsAvailableEnum.Decrypt;
                }

                this.keySize = (uint)this.n.bitCount();
            }
            else
            {
                this.eC = 0;
                this.n = 0;
                this.keySize = 1024;
                //this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
            }
        }
        public void PrepareAlgorithm()
        {
            bool algorithmReady = false;
            //this.actions = ActionsAvailableEnum.Decrypt | ActionsAvailableEnum.Encrypt;
            BigInteger messageToTest = new BigInteger(77);

            while (!algorithmReady)
            {
                this.Generate_Primes();
                //this.Generate_Primes2();
                this.CalculateN();
                this.CalculatePhi();
                this.SelectE();

                // For a key of 1024 bits, the d was found in 192 miliseconds
                this.CalculateD_2();
                
                //check:
                {
                    BigInteger encryptedMessage = Encrypt(messageToTest);
                    BigInteger decryptedMessage = Decrypt(encryptedMessage);

                    if (decryptedMessage != messageToTest) continue;
                }
                algorithmReady = true;
            }
        }
        public String GetPublicKeyAsString(Int32 keyBase)
        {
            return String.Format("Public key: {0},{1}", this.eC.ToString(keyBase), this.n.ToString(keyBase));
        }
        public String GetPrivateKeyAsString(Int32 keyBase)
        {
            return String.Format("Private key: {0},{1}", this.d.ToString(keyBase), this.n.ToString(keyBase));
        }
        public BigInteger Encrypt(BigInteger mToPow)
        {
            // if ((Int32)(ActionsAvailable & ActionsAvailableEnum.Encrypt) == 0)
            //     return -1;

            BigInteger exp = new BigInteger(this.eC);
            BigInteger modN = new BigInteger(this.n);
            BigInteger modRes = mToPow.modPow(exp, modN);

            return modRes;
        }
        
        public BigInteger Decrypt(BigInteger c)
        {
            // if ((Int32)(ActionsAvailable & ActionsAvailableEnum.Decrypt) == 0)
            //     return -1;

            BigInteger cToPow = new BigInteger(c);
            BigInteger exp = new BigInteger(this.d);
            BigInteger modN = new BigInteger(this.n);
            BigInteger modRes = cToPow.modPow(exp, modN);

            return modRes;
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        private void Generate_Primes()
        {
            uint pLength = (uint)(this.keySize / 2.0), qLength = keySize - pLength;

            // Finding number of cores
            uint coreCount = 0;
            //NumberOfCores
            // foreach (System.Management.ManagementObject item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            // {
            //     coreCount += uint.Parse(item["NumberOfCores"].ToString());
            // }
            coreCount = 8;

            if (coreCount >= 2)
            {
                // Finding primes with two threads is more efficient on multicore systems
                System.Threading.Thread th = new System.Threading.Thread(delegate()
                {
                    do
                    {
                        this.p = GenerateOddNBitNumber(pLength);
                        // p = ThirdTask_2.Program.GeneratePrimeNumber(pLength, rabinMillerConfidence);

                    } while (!ThirdTask_2.PrimeTests.RabinMillerTest(this.p, rabinMillerConfidence));//while (!this.p.RabinMillerTest(rabinMillerConfidence));                
                });

                System.Threading.Thread th2 = new System.Threading.Thread(delegate()
                {
                    do
                    {
                        this.q = GenerateOddNBitNumber(qLength);
                        //q = ThirdTask_2.Program.GeneratePrimeNumber(qLength, rabinMillerConfidence);
                    } while (!ThirdTask_2.PrimeTests.RabinMillerTest(this.q, rabinMillerConfidence)); //while (!this.q.RabinMillerTest(rabinMillerConfidence) || this.p == this.q) ;
                });

                th.Start();
                th2.Start();
                th.Join();
                th2.Join();
            }
            else
            {
                do
                {
                    this.p = GenerateOddNBitNumber(pLength);
                    //this.p.genRandomBits((Int32)pLength, new Random((Int32)DateTime.Now.Ticks));
                    //res = this.p.RabinMillerTest(rabinMillerConfidence);

                } /*while (!RabinMillerPrimeTest(this.p, rabinMillerConfidence) || res == false);*/while (!this.p.RabinMillerTest(rabinMillerConfidence));
                ThirdTask_2.PrimeTests.RabinMillerTest(this.p, rabinMillerConfidence);
                do
                {
                    this.q = GenerateOddNBitNumber(qLength);

                } while (!ThirdTask_2.PrimeTests.RabinMillerTest(this.q, rabinMillerConfidence)); //while (!this.q.RabinMillerTest(rabinMillerConfidence) || this.p == this.q) ;
            }

            if(this.p < this.q)
            {
                BigInteger tmp = new BigInteger(this.p);
                this.p = this.q;
                this.q = tmp;
            }
        }

        /// <summary>
        /// Method to generate the primes used in this algorithm
        /// If the number is not prime, then it is incremented by 2 and tested again for primality.
        /// Tested for generation of 512 bit key, generation run for 10 minutes, no success. (P.S. I verify
        /// in PrepareAlgorithm() if encryption/decryption succeeds before claiming that the key is ready)
        /// </summary>
        /// <remarks></remarks>
        private void Generate_Primes2()
        {
            uint pLength = (uint)(this.keySize / 2.0), qLength = keySize - pLength;

            this.p = GenerateOddNBitNumber(pLength);
            this.q = GenerateOddNBitNumber(qLength);

            // Finding number of cores
            uint coreCount = 0;
            //NumberOfCores
            // foreach (System.Management.ManagementObject item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            // {
            //     coreCount += uint.Parse(item["NumberOfCores"].ToString());
            // }
            coreCount = 8;

            if (coreCount > 2)
            {
                // Finding primes with two threads is more efficient on multicore systems
                System.Threading.Thread th = new System.Threading.Thread(delegate()
                {
                    while (!ThirdTask_2.PrimeTests.RabinMillerTest(this.p, rabinMillerConfidence*10))
                    {
                        this.p += 2;
                    }               
                });

                System.Threading.Thread th2 = new System.Threading.Thread(delegate()
                {
                    while (!ThirdTask_2.PrimeTests.RabinMillerTest(this.q, rabinMillerConfidence*10))
                    {
                        this.q += 2;
                    }
                });

                th.Start();
                th2.Start();
                th.Join();
                th2.Join();
            }
            else
            {
                while (!ThirdTask_2.PrimeTests.RabinMillerTest(this.p, rabinMillerConfidence*10))
                {
                    this.p += 2;
                }

                while (!ThirdTask_2.PrimeTests.RabinMillerTest(this.q, rabinMillerConfidence*10))
                {
                    this.q += 2;
                }
            }

            if (this.p < this.q)
            {
                BigInteger tmp = new BigInteger(this.p);
                this.p = this.q;
                this.q = tmp;
            }
        }

        /// <summary>
        /// Method to calculate n
        /// </summary>
        private void CalculateN()
        {
            this.n = this.p * this.q;
        }

        /// <summary>
        /// Method to calculate phi
        /// </summary>
        private void CalculatePhi()
        { 
            this.phi = (this.p - 1) * (this.q - 1);
        }

        /// <summary>
        /// Method to select eC from the interval 1 &lt; e &lt; φ(n)
        /// </summary>
        private void SelectE()
        {
            BigInteger generatedE = GenerateOddNBitNumber((uint)(keySize / 2.0));

            for (; ; generatedE++)
            {
                if (AreRelativePrime(this.phi, generatedE))
                {
                    this.eC = generatedE;
                    break;
                }
            }

            if (this.eC == 0)
            {
                throw new Exception("Cannnot select e!");
            }
        }

        /// <summary>
        /// The method is the standart one used to find d, based on the RSA algorithm
        /// TO be used only with small numbers. With a key of 1024 bit it was executing for 15 minutes and no success
        /// </summary>
        private void CalculateD()
        {
            BigInteger tmp_D;

            for (int k = 2; ; k++)
            {
                this.d = (this.phi * k + 1) / this.eC;
                tmp_D = (this.phi * k + 1) % this.eC;

                // We verify that d is integral, so in this case tmp_D must be 0
                if (tmp_D == 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// This method calculates d using modulo inverse algorithm
        /// </summary>
        private void CalculateD_2()
        {
            this.d = (Extended_GDC(this.eC, this.phi, true))[1];
        }
        
        
        
        
        
        /// <summary>
        /// Two integers are termed relative prime if the only common factor between them is 1
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static System.Boolean AreRelativePrime(BigInteger phi, BigInteger e)
        {
            // The numbers are relatively prime only if x equals 1. The cycle above will end anyway
            return GreatestCommonDivizor(phi, e) == 1;
        }

        /// <summary>
        /// Algorithm to find greatest common divisor
        /// </summary>
        /// <param name="X">First number</param>
        /// <param name="Y">Second number</param>
        /// <returns>True if no common divisor greater than 1 is possible, False otherwise</returns>
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

        /// <summary>
        /// This method implements the extended euclidian GDC algorithm and the modulo inverse algorithm
        /// </summary>
        /// <param name="a">The number for which to apply the chosen algorithm</param>
        /// <param name="modulus">The modulus</param>
        /// <param name="calcOnlyModuloInverse">If = true, then modulo inverse algorithm will be applied, else EEGCD</param>
        /// <returns>array[3]. In the case of EEGCD: array[0] = multiplicative inverse of a to modulus
        /// array[1] = multiplicative inverse of modulus to a
        /// array[2] = the GCD
        /// 
        /// so that array[2] = a*array[0] + modulus*array[1];
        /// 
        /// In the case of modulo inverse algorithm: array[0] == array[2] = 0; array[1] = positive modulo inverse of a to modulus.
        /// This is the difference from the EEGCD algorithm, which can return negative. This one is used for RSA
        /// </returns>
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
                    quotient = b_ / a_;     // If not BigInteger used, then use direct cast to Int32

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
                    quotient = b_ / a_;     // If not BigInteger used, then use direct cast to Int32
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

        /// <summary>
        /// Method to write de n-1 as pow(2,s)*d, d odd
        /// </summary>
        /// <param name="n">The number from which to extract pow(2,s)*d</param>
        /// <returns>n-1 as pow(2,s)*d. [0] is s, [1] is d</returns>
        /// <remarks>
        /// This transformation can be done more easily, if to use 'uint mask = 0x01' and to do
        /// bit comparison, like in BigInteger RabinMiller test, but since I have no access to the 
        /// data that represents the BigInteger number, I will do a more costly operation
        /// </remarks>
        public static BigInteger[] Get2sdFromNminus1(BigInteger n)
        {
            // Even number passed
            if (n % 2 == 0) return new BigInteger[] { 0, 0 };

            BigInteger tmp = n - new BigInteger(1);
            Int32 counter = 0;
            BigInteger remainder;

            while (true)
            {
                tmp = tmp / 2;
                remainder = tmp % 2;
                counter++;

                // if remainder is different from 0, then we reached an odd number
                if (remainder != 0) break;
            }

            // counter is s, tmp is d, odd
            return new BigInteger[] { counter, tmp };
        }

        /// <summary>
        /// A method that will generate a random number of n bits
        /// Created this method because the one in the BigInteger 
        /// class is generating only 32 bit numbers, even if we specify more. 
        /// Didn't want to touch the BigInteger code
        /// </summary>
        /// <remarks>Reccomendations Reference http://www.di-mgt.com.au/rsa_alg.html</remarks>
        /// <param name="nrBits">number of bits in the generated number</param>
        /// <returns>A BigInteger instance of the n bit number</returns>
        public static BigInteger GenerateOddNBitNumber(uint nrBits)
        {
            //String tmp = "";
            Int32 nr;
            BigInteger b = new BigInteger();

            Random rand = new Random((Int32)DateTime.Now.Ticks);

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