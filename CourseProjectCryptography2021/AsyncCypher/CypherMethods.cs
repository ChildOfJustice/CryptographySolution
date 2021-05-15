using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
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
            //rsaCore.numberSize
            
            MessageBox.Show("Reading: " + keyFileName);
            FileInfo fi = new FileInfo(keyFileName);
            int blocks = (int)fi.Length / 8;

            MessageBox.Show("File length is " + fi.Length);
            //TODO create rsa multipart encryption with padding
            if (fi.Length < 8 || fi.Length % 8 != 0)
                MessageBox.Show("Very strange key file to encrypt");
            int blockSize = 8;
            
            
            var outputStream = File.Open(outputFilePath, FileMode.Create);
            FileStream fs = new FileStream(keyFileName, FileMode.Open, FileAccess.Read);

            //MessageBox.Show("will encrypt " + keyFileName + " to " +outputFilePath);

            for (int i = 0; i < blocks; i++)
            {
                int endPositionToRead = (i + 1) * blockSize;

                var plainText = TaskManager.ReadDesiredPart(fs, i * blockSize, endPositionToRead);
                //MessageBox.Show("PT: " + new ASCIIEncoding().GetString(plainText));
                //var encrypted = rsaCore.Encrypt(TaskManager.ReadDesiredPart(fs, i * blockSize, endPositionToRead));
                var encrypted = new BigInteger[plainText.Length];
                var encryptedBytes = new byte[plainText.Length][];
                for (int j = 0; j < encrypted.Length; j++)
                {
                    encrypted[j] = rsaCore.EncryptOneByte(new BigInteger(plainText[j]));
                    //MessageBox.Show("Dec: " + ((byte)rsaCore.DecryptOneByte(encrypted[j]).IntValue()-'0'));
                    encryptedBytes[j] = encrypted[j].getBytes();
                    if (encrypted[j].getBytes().Length != rsaCore.numberSize)
                    {
                        MessageBox.Show("Error : " + encrypted[j].getBytes().Length + " != " + rsaCore.numberSize);
                        var temp = encrypted[j].getBytes();

                        MessageBox.Show("1: " + arrToStr(temp));
                        
                        var newTemp = new byte[rsaCore.numberSize];
                        for (int k = 1; k < temp.Length+1; k++)
                        {
                            newTemp[k] = temp[k-1];
                        }

                        newTemp[0] = 0;
                        // MessageBox.Show("2: " + arrToStr(newTemp));
                        // List<byte> temp2 = new List<byte>();
                        // for (int k = 0; k < newTemp.Length; k++)
                        // {
                        //     if (newTemp[k] != 0)
                        //     {
                        //         temp2.Add(newTemp[k]);
                        //         //MessageBox.Show("ADD "+ newTemp[k])
                        //     }
                        //         
                        // }
                        // MessageBox.Show("3: " + arrToStr(temp2.ToArray()));
                        // MessageBox.Show("Need to be eq: " + new ASCIIEncoding().GetString(temp2.ToArray()) + " | " +
                        //                 new ASCIIEncoding().GetString(temp));
                        encryptedBytes[j] = newTemp;
                        //encrypted[j] = new BigInteger(newTemp);
                        
                    }
                    //MessageBox.Show("CT: " + encrypted[j] + " size is " + encrypted[j].dataLength);
                    
                    //MessageBox.Show("enc: " + arrToStr(encryptedBytes[j]));
                }
                
                
                foreach (var encryptedB in encryptedBytes)
                {
                    try
                    {
                        //var encryptedBytes = encryptedBigInt.getBytes();//new ASCIIEncoding().GetBytes(encryptedBigInt.ToString());
                        outputStream.Write(encryptedB, 0, encryptedB.Length);
                        //MessageBox.Show("Length is: " + encryptedBytes.Length);
                        // var newLine = new ASCIIEncoding().GetBytes("\n");
                        // outputStream.Write(newLine, 0, newLine.Length);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                
            }
            outputStream.Close();
            fs.Close();
            MessageBox.Show("Key was encrypted with: " + rsaCore.numberSize);
        }

        public static string arrToStr(byte[] arr)
        {
            string res = "";
            foreach (var VARIABLE in arr)
            {
                res += VARIABLE - '0';
            }

            return res;
        }
        public static void DecryptKey(RsaCore rsaCore, string encryptedKeyFileName, string outputFilePath)
        {
            
            var outputStream = File.Open(outputFilePath, FileMode.Create);
            FileStream fs = new FileStream(encryptedKeyFileName, FileMode.Open, FileAccess.Read);

            var fi = new FileInfo(encryptedKeyFileName);
            var numbersQuantity = fi.Length/rsaCore.numberSize;
            for (int i = 0; i < numbersQuantity; i++)
            {
                int endPositionToRead = (i + 1) * rsaCore.numberSize;
                var block = TaskManager.ReadDesiredPart(fs, (i) * rsaCore.numberSize, endPositionToRead);
                //MessageBox.Show("Dec size: " + block.Length);
                //MessageBox.Show("read: " + arrToStr(block));
                byte decryptedByte;
                if (block[0] == 0)
                {
                    var temp = new byte[block.Length - 1];
                    for (int j = 0; j < block.Length - 1; j++)
                    {
                        temp[j] = block[j + 1];
                    }
                    decryptedByte = (byte)rsaCore.DecryptOneByte(new BigInteger(temp)).IntValue();
                }
                else
                {
                    decryptedByte = (byte)rsaCore.DecryptOneByte(new BigInteger(block)).IntValue();
                }
                // List<byte> temp = new List<byte>();
                // for (int j = 0; j < block.Length; j++)
                // {
                //     if(block[j] != 0)
                //         temp.Add(block[j]);
                //     else
                //     {
                //         MessageBox.Show("!!");
                //     }
                // }
                
                
                //MessageBox.Show("dec: " + (decryptedByte-'0'));
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
            
            
            
            
            // StreamReader sr = new StreamReader(encryptedKeyFileName);
            // string line;
            // while ((line = sr.ReadLine()) != null)
            // {
            //     var decryptedByte = (byte)rsaCore.DecryptOneByte(new BigInteger(line, 10)).IntValue();
            //     // byte[] wtf = new byte[1];
            //     // wtf[0] = decryptedByte;
            //     try
            //     {
            //         //var encryptedBytes = new ASCIIEncoding().GetString(wtf);
            //         outputStream.WriteByte(decryptedByte);
            //     }
            //     catch (Exception e)
            //     {
            //         MessageBox.Show(e.Message);
            //     }
            // }

            
            outputStream.Close();
            //sr.Close();
            
        }
    }
}