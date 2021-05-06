

using SecondTask_3.AsyncCypher;

namespace Task_8.AsyncCypher
{
    public class TaskProperties
    {
        public int BlockNumber;
        public int BlocksQuantity;
        // TODO public AesCore RijndaelFramework; 
        public CypherAlgorithm Algorithm;
        public byte[] Data;

        public TaskProperties(int blockNumber, int blocksQuantity, CypherAlgorithm algorithm, byte[] data)
        {
            BlockNumber = blockNumber;
            BlocksQuantity = blocksQuantity;
            Algorithm = algorithm;
            Data = data;
        }
    }
}