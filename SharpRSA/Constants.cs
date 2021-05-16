using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SharpRSA
{
    public class Constants
    {
        //The "e" value for low compute time RSA encryption.
        //Only has two bits of value 1.
        public static BigInteger e = 0x10001;
    }
}
