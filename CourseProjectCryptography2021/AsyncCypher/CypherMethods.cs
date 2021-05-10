using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThirdTask_3;

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


        public static void EncryptKey(RsaCore rsaCore, string keyFileName, string outputFilePath)
        {
            FileInfo fi = new FileInfo(keyFileName);
            int blockSize = 8;
            int blocks = (int)fi.Length / 8;
           
            outputFilePath = "./resources/" + outputFilePath;
            var outputStream = File.Open(outputFilePath, FileMode.Create);
            FileStream fs = new FileStream(keyFileName, FileMode.Open, FileAccess.Read);
            

            for (int i = 0; i < blocks; i++)
            {
                int endPositionToRead = (i + 1) * blockSize;

                
                var encrypted = rsaCore.Encrypt(TaskManager.ReadDesiredPart(fs, i * blockSize, endPositionToRead));

                foreach (var encryptedBigInt in encrypted)
                {
                    try
                    {
                        var encryptedBytes = new ASCIIEncoding().GetBytes(encryptedBigInt.ToString());
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                        var newLine = new ASCIIEncoding().GetBytes("\n");
                        outputStream.Write(newLine, 0, newLine.Length);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                
            }
            outputStream.Close();
            fs.Close();
            
        }
        public static void DecryptKey(RsaCore rsaCore, string encryptedKeyFileName, string outputFilePath)
        {

            outputFilePath = "./resources/" + outputFilePath;
            var outputStream = File.Open(outputFilePath, FileMode.Create);
            //FileStream fs = new FileStream(encryptedKeyFileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(encryptedKeyFileName);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var decryptedByte = (byte)rsaCore.DecryptOneByte(new BigInteger(line, 10)).IntValue();
                // byte[] wtf = new byte[1];
                // wtf[0] = decryptedByte;
                try
                {
                    //var encryptedBytes = new ASCIIEncoding().GetString(wtf);
                    outputStream.WriteByte(decryptedByte);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            
            outputStream.Close();
            sr.Close();
            
        }
    }
}