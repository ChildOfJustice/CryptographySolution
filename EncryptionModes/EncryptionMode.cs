using Task_4;
using Task_5;

namespace EncryptionModes
{
    public class EncryptionMode
    {
        protected byte[] IV;
        protected byte[] Key;

        protected ICypherAlgorithm algorithm;

        protected int blockSize;
    }
}