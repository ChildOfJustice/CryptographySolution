﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SecondTask_3.AsyncCypher;

namespace Task_8.AsyncCypher
{
    public class TaskManager
    {
        public string outputFilePath = "result.txt";
        public string inputFilePath = "./resources/text.txt";
        public string keyFilePath = "./resources/key";
        public string pubKeyFilePath = "keypub";
        public string privateKeyFilePath = "./resources/keyprivate";

        public byte[] TempKey;

        public CypherAlgorithm algorithm;
        private int blockSize;
        private int keySize;
        private int blocksQuantity;
        
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        
        public TaskManager(CypherAlgorithm _algorithm, int _blockSize, int _keySize)
        {
            algorithm = _algorithm;
            blockSize = _blockSize;
            keySize = _keySize;
            
            //new ASCIIEncoding().GetBytes(sData);
            //MessageBox.Show("Decrypted data: " + new ASCIIEncoding().GetString(Final));
        }
        
        public void RunEncryptionProcess()
        {
            ImportCypherKey();
            
            //https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource-1?view=net-5.0
            CancellationToken ct = tokenSource.Token;
            
            blocksQuantity = DetermineBlocksQuantity(true);

            MessageBox.Show("Split the file into "+blocksQuantity);
            
            Task<TaskProperties>[] allTasks = new Task<TaskProperties>[blocksQuantity];
            for (int i = 0; i < blocksQuantity; i++)
            {
                TaskCompletionSource<TaskProperties> taskCompletionSource = new TaskCompletionSource<TaskProperties>();
                Task<TaskProperties> task = taskCompletionSource.Task;

                var i1 = i;
                Task.Factory.StartNew(() =>
                {
                    // Were we already canceled?
                    ct.ThrowIfCancellationRequested();

                    try
                    {
                        int endPositionToRead = (i1 + 1) * blockSize;
                        string keyFilePathToUse = keyFilePath;

                        if (i1 == blocksQuantity-1)
                        {
                            //encrypt the size and add it
                            byte[] block = new byte[blockSize];
                            var tempArr = new ASCIIEncoding().GetBytes(new FileInfo(inputFilePath).Length.ToString());
                            Array.Copy(tempArr, block, tempArr.Length);
                            
                            taskCompletionSource.SetResult(CypherMethods.encryptBlock(new TaskProperties(i1,
                                    blocksQuantity, algorithm,
                                    block), TempKey, keySize,
                                keyFilePathToUse));
                        }
                        else
                        {
                            using (FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                                taskCompletionSource.SetResult(CypherMethods.encryptBlock(new TaskProperties(i1,
                                        blocksQuantity, algorithm,
                                        ReadDesiredPart(fs, i1 * blockSize, endPositionToRead)), TempKey, keySize,
                                    keyFilePathToUse));
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                    
                }, tokenSource.Token);

                allTasks[i] = task;
            }

            if (ct.IsCancellationRequested)
            {
                // Clean up here
                ct.ThrowIfCancellationRequested();
            }
            else
            {
                outputFilePath = "./resources/" + outputFilePath;
                using (var outputStream = File.Open(outputFilePath, FileMode.Create))
                {
                    foreach (var promise in allTasks)
                    {
                        try
                        {
                            //WILL BE LOCKED ON THIS AND WAIT UNTIL A PROMISE WILL BE RESOLVED
                            outputStream.Write(promise.Result.Data, 0, promise.Result.Data.Length);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                    MessageBox.Show("Output file is " + outputFilePath);
                    outputStream.Close();
                }
            }
        }

        private void ImportCypherKey()
        {
            MessageBox.Show("Importing secret key...");
            var temp = new byte[keySize];
            using (var inputStream = File.OpenRead(keyFilePath))
                inputStream.Read(temp, 0, keySize);
            TempKey = temp;
        }

        public void RunDecryptionProcess()
        {
            ImportCypherKey();
            
            //https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource-1?view=net-5.0
            CancellationToken ct = tokenSource.Token;

            blocksQuantity = DetermineBlocksQuantity(false);

            Task<TaskProperties>[] allTasks = new Task<TaskProperties>[blocksQuantity];
            for (int i = 0; i < blocksQuantity; i++)
            {
                TaskCompletionSource<TaskProperties> taskCompletionSource = new TaskCompletionSource<TaskProperties>();
                Task<TaskProperties> task = taskCompletionSource.Task;

                var i1 = i;
                Task.Factory.StartNew(() =>
                {
                    ct.ThrowIfCancellationRequested();

                    try
                    {
                        int endPositionToRead = (i1 + 1) * blockSize;
                        string keyFilePathToUse = keyFilePath;

                        using (FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                        {
                            var result = CypherMethods.decryptBlock(new TaskProperties(i1, blocksQuantity, algorithm,
                                ReadDesiredPart(fs, i1 * blockSize, endPositionToRead)), TempKey, keySize, keyFilePathToUse);
                            taskCompletionSource.SetResult(result);
                        }
                    }catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                });
                
                allTasks[i] = task;
            }

            if (ct.IsCancellationRequested)
            {
                // Clean up here
                ct.ThrowIfCancellationRequested();
            }
            else
            {
                outputFilePath = "./resources/" + outputFilePath;
                
                int blockNumber = 0;
                int actualFileSize = 0;
                int fullArrSize = 0;
                byte[] plainTextBytes = null;
                using (BinaryWriter binWriter = new BinaryWriter(new MemoryStream()))
                {
                    foreach (var promise in allTasks)
                    {
                       
                        try
                        {
                            if (blockNumber == blocksQuantity - 1)
                            {
                                //MessageBox.Show("blocknumber : " +blockNumber+" blocks: " + blocksQuantity + "file SIZE IS: " + new ASCIIEncoding().GetString(promise.Result.Data));
                                actualFileSize = Int32.Parse(new ASCIIEncoding().GetString(promise.Result.Data));
                            }
                            else
                            {
                                //WILL BE LOCKED ON THIS AND WAIT UNTIL A PROMISE WILL BE RESOLVED
                                binWriter.Write(promise.Result.Data, 0, promise.Result.Data.Length);
                                fullArrSize = promise.Result.Data.Length;
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        blockNumber++;
                    }
                    
                    BinaryReader binReader =
                        new BinaryReader(binWriter.BaseStream);
                    // Set Position to the beginning of the stream.
                    binReader.BaseStream.Position = 0;
                    
                    if (algorithm != CypherAlgorithm.RSA)
                    {
                        plainTextBytes = binReader.ReadBytes(actualFileSize);
                        //var count = memStream.Read(plainTextBytes, 0, actualFileSize);
                    }
                    else
                    {
                        plainTextBytes = binReader.ReadBytes(fullArrSize);
                    }
                    
                }
                
                using (var outputStream = File.Open(outputFilePath, FileMode.Create))
                {
                    MessageBox.Show("Output file is " + outputFilePath);
                    outputStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    outputStream.Close();
                }
            }
            
        }

        private int DetermineBlocksQuantity(bool encrypt)
        {
            
            FileInfo fi = new FileInfo(inputFilePath);
            //MessageBox.Show("SIZE IS " + fi.Length + " blocks will be " + (int) (fi.Length / blockSize + 1));
            int res;
            if (fi.Length % blockSize == 0)
                res = (int) (fi.Length / blockSize);
            else
                res = (int) (fi.Length / blockSize + 1);
            if (encrypt)
                res++; // + 1 more for the file size block
            return res; 
        }

        public byte[] ReadDesiredPart(FileStream fs, int startPosition, int endPosition) {
            byte[] buffer = new byte[endPosition - startPosition];

            int arrayOffset = 0;

            //lock (fsLock) {
                fs.Seek(startPosition, SeekOrigin.Begin);

                int numBytesRead = fs.Read(buffer, arrayOffset , endPosition - startPosition);

                //  Typically used if you're in a loop, reading blocks at a time
                arrayOffset += numBytesRead;
            //}

            return buffer;
        }


        public void Cancel()
        {
            tokenSource.Cancel();
        }
    }
}