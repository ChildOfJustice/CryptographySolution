using System;
using System.Text;
using System.Windows;

namespace Task_8.AsyncCypher
{
    public class CypherMethods
    {
        public static TaskProperties encryptBlock(TaskProperties props, byte[] key, int keySize, string keyFilePath)
        {
            byte[] encryptedData = null;
            
            
            //MessageBox.Show("Data to be encrypted: " + new ASCIIEncoding().GetString(props.Data));
            encryptedData = props.CypherFramework.Encrypt(props.Data);
            //MessageBox.Show("encrypted data: " + new ASCIIEncoding().GetString(encryptedData));
            //EXPORT THE USED KEY
            // if(props.BlockNumber == props.BlocksQuantity - 1)
            //     rijndaelFramework.ExportKey(keyFilePath);
            return new TaskProperties(props.BlockNumber, props.BlocksQuantity,  props.CypherFramework, encryptedData);
        }
        public static TaskProperties decryptBlock(TaskProperties props, byte[] key, int keySize, string keyFilePath)
        {
            byte[] decryptedData = null;

            //MessageBox.Show("Data to be decrypted: " + new ASCIIEncoding().GetString(props.Data));
            decryptedData = props.CypherFramework.Decrypt(props.Data);
            //MessageBox.Show("decrypted data: " + new ASCIIEncoding().GetString(decryptedData));
            
            return new TaskProperties(props.BlockNumber, props.BlocksQuantity, props.CypherFramework, decryptedData);
         
        }
    }
}