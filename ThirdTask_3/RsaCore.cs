using System.Numerics;

namespace ThirdTask_3
{
    public class RsaCore
    {
        //Vinner Attack:
        BigInteger startIntForTheEnryptionExponent = BigInteger.Pow(2, 500);

        private BigInteger EncryptionExponent;
        
        public RsaCore(BigInteger p, BigInteger q)
        {
            var n = p * q;

            var tempExp = startIntForTheEnryptionExponent;
            while (ThirdTask_1.Program.ExtendedEuclideanAlgorithm(tempExp, 
                ThirdTask_1.Program.EulerFunction(n),
                out BigInteger x, out BigInteger y) != 1)
            {
                tempExp++;
            }

            EncryptionExponent = tempExp;

        }
    }
}