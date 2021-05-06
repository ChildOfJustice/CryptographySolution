using System.IO;
using System.Text;
using System.Windows;
using AES;
using SecondTask_3.AsyncCypher;

namespace Task_8.AsyncCypher
{
    public class CypherMethods
    {
        public static TaskProperties encryptBlock(TaskProperties props, byte[] key, int keySize, string keyFilePath)
        {
            byte[] encryptedData = null;
            
            
            //MessageBox.Show("Data to be encrypted: " + new ASCIIEncoding().GetString(props.Data));
            encryptedData = props.RijndaelFramework.Encrypt(props.Data);
            //MessageBox.Show("encrypted data: " + new ASCIIEncoding().GetString(encryptedData));
            //EXPORT THE USED KEY
            // if(props.BlockNumber == props.BlocksQuantity - 1)
            //     rijndaelFramework.ExportKey(keyFilePath);
            return new TaskProperties(props.BlockNumber, props.BlocksQuantity,  props.RijndaelFramework, encryptedData);
        }
        public static TaskProperties decryptBlock(TaskProperties props, byte[] key, int keySize, string keyFilePath)
        {
            byte[] decryptedData = null;
            
            
            //MessageBox.Show("Data to be decrypted: " + new ASCIIEncoding().GetString(props.Data));
            decryptedData = props.RijndaelFramework.Decrypt(props.Data); 
            //MessageBox.Show("decrypted data: " + new ASCIIEncoding().GetString(decryptedData));
            
            return new TaskProperties(props.BlockNumber, props.BlocksQuantity, props.RijndaelFramework, decryptedData);
         
        }
    }
}