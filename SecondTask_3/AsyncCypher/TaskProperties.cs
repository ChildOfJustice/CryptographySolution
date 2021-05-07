

using AES;
using SecondTask_3.AsyncCypher;
using Task_4;

namespace Task_8.AsyncCypher
{
    public class TaskProperties
    {
        public int BlockNumber;
        public int BlocksQuantity;
        public AesCore RijndaelFramework;
        public byte[] Data;

        public TaskProperties(int blockNumber, int blocksQuantity, AesCore rijndaelFramework, byte[] data)
        {
            BlockNumber = blockNumber;
            BlocksQuantity = blocksQuantity;
            RijndaelFramework = rijndaelFramework;
            Data = data;
        }
    }
}