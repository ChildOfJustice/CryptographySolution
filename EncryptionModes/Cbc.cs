using System;
using Task_4;
using Task_5;

namespace EncryptionModes
{
    public class Cbc : EncryptionMode
    {
        public Cbc(byte[] iv, ICypherAlgorithm _algorithm, int _blockSize)
        {
            IV = iv;
            algorithm = _algorithm;
            blockSize = _blockSize;
        }

        private void EncryptChain(byte[] prevBlock, byte[][] allData, int blockNumber, byte[][] result)
        {
            for (int i = 0; i < allData[blockNumber].Length; i++)
            {
                allData[blockNumber][i] ^= prevBlock[i];
            }

            result[blockNumber] = algorithm.Encrypt(allData[blockNumber]);
            
            if(blockNumber == allData.Length-1)
                return;
            EncryptChain(result[blockNumber], allData, blockNumber + 1, result);
        }

        public byte[][] EncryptAll(byte[][] allBlocks)
        {
            byte[][] result = new byte[allBlocks.Length][];
            EncryptChain(IV, allBlocks, 0, result);
            return result;
        }





        public byte[] Decrypt(byte[] prevCypherBlock, byte[] currentCypherBlock)
        {
            var data = algorithm.Decrypt(currentCypherBlock);
            byte[] res = new byte[prevCypherBlock.Length];
            for (int i = 0; i < prevCypherBlock.Length; i++)
            {
                res[i] = (byte)(prevCypherBlock[i] ^ data[i]);
            }

            return res;
        }
        
        public byte[][] DecryptAll(byte[][] allBlocks)
        {
            byte[][] result = new byte[allBlocks.Length][];
            result[0] = Decrypt(IV, allBlocks[0]);

            for (int i = 1; i < allBlocks.Length; i++)
                result[i] = Decrypt(allBlocks[i - 1], allBlocks[i]);
            
            
            return result;
        }
    }
}