using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CourseProjectCryptography2021.Spinner;
using MagentaAlgorithm;
using Task_4;
using Task_8;
using Task_8.AsyncCypher;
using ThirdTask_3;

namespace CourseProjectCryptography2021
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainWindowViewModel _mainWindowViewModel;
        private RsaCore rsaCore;
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
        }
        
        
        
        private void ComboBoxMagentaKeySizeSelected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            _mainWindowViewModel.MagentaKeySize = Int32.Parse(selectedItem.Text)/8;
        }
        private void ComboBoxEncryptionModeSelected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            _mainWindowViewModel.EncryptionMode = selectedItem.Text;
        }
        private void OnStartSessionButtonClick(object sender, RoutedEventArgs e)
        {
            //get key path to export them
            var pubKeyFileName = OutPutPubKeyFilePathHolder.Text;
            var privateKeyFileName = OutPutPrivateKeyFilePathHolder.Text;
            // get RSA key size
            uint rsaKeySize = 516;
            try
            {
                rsaKeySize = UInt32.Parse(RsaKeySizeHolder.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Wrong RSA key size, will be used <516>");
            }
            Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        setContent();
                    });
                    
                    rsaCore = new RsaCore(rsaKeySize);
                    //export keys
                    rsaCore.ExportPubKey(pubKeyFileName);
                    rsaCore.ExportPrivateKey(privateKeyFileName);
                    
                    
                    //
                    //
                    //
                    // CypherMethods.EncryptKey(rsaCore, _mainWindowViewModel.SymmetricKeyFile, _mainWindowViewModel.SymmetricKeyFile+"Encrypted");
                    // CypherMethods.DecryptKey(rsaCore, _mainWindowViewModel.SymmetricKeyFile+"Encrypted", _mainWindowViewModel.SymmetricKeyFile+"Decrypted");
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        removeContent();
                    });
                }
                catch (Exception exception)
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        removeContent();
                    });
                    
                    MessageBox.Show(exception.Message);
                }
            });
        }
        private void OnSendFileButtonClick(object sender, RoutedEventArgs e)
        {
            var outputFileName = OutPutFilePathHolder.Text;

            var pubKeyFileName = _mainWindowViewModel.PublicKeyFile;
            
            //generate symmetric key for MAGENTA
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] magentaKey = new byte[_mainWindowViewModel.MagentaKeySize];
            rng.GetBytes(magentaKey);
            
            byte[] IV = new byte[16];
            rng.GetBytes(IV);

            Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        setContent();
                    });
                    //export IV and this key and encrypt them with the RSA pub key
                    using (var outputStream = File.Open(_mainWindowViewModel.SymmetricKeyFile, FileMode.Create))
                        outputStream.Write(magentaKey, 0, magentaKey.Length);
                    using (var outputStream = File.Open(_mainWindowViewModel.IvFilePath, FileMode.Create))
                        outputStream.Write(IV, 0, IV.Length);
                    
                    rsaCore = new RsaCore(generateKeys:false);
                    rsaCore.ImportPubKey(pubKeyFileName);
                    
                    CypherMethods.EncryptKey(rsaCore, _mainWindowViewModel.SymmetricKeyFile, _mainWindowViewModel.SymmetricKeyFile+"Encrypted");
                    CypherMethods.EncryptKey(rsaCore, _mainWindowViewModel.IvFilePath, _mainWindowViewModel.IvFilePath+"Encrypted");
                    
                    
                    _mainWindowViewModel.MainTaskManager = new TaskManager(_mainWindowViewModel.SymmetricKeyFile,_mainWindowViewModel.MagentaKeySize, _mainWindowViewModel.EncryptionMode);
                   
                    //CypherMethods.EncryptKey(rsaCore, _mainWindowViewModel.SymmetricKeyFile, "./resources/EncryptedSymmetricKey");
                   
                    // if (_mainWindowViewModel.SymmetricKeyFile == null)
                    //     throw new NullReferenceException("The key file path is empty");
                    // if (_mainWindowViewModel.IvFilePath == null && _mainWindowViewModel.MainTaskManager.EncryptionMode != "ECB")
                    //     throw new NullReferenceException("The IV file path is empty");
                    // if (_mainWindowViewModel.DataFile == null)
                    //     throw new NullReferenceException("The data file path is empty");
                    // if (outputFileName == null || outputFileName == "")
                    //     throw new NullReferenceException("The output file name is empty");
                    
                    //_mainWindowViewModel.MainTaskManager.keyFilePath = _mainWindowViewModel.SymmetricKeyFile;
                    //_mainWindowViewModel.MainTaskManager.ivFilePath = _mainWindowViewModel.IvFilePath;
                    _mainWindowViewModel.MainTaskManager.inputFilePath = _mainWindowViewModel.DataFile;
                    _mainWindowViewModel.MainTaskManager.outputFilePath = outputFileName;
                    
                    _mainWindowViewModel.MainTaskManager.RunEncryptionProcess();

                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        removeContent();
                    });
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        removeContent();
                    });
                }
            });
        }
        private void OnReceiveFileButtonClick(object sender, RoutedEventArgs e)
        {
            var outputFileName = OutPutFilePathHolder.Text;
            var privateKeyFileName = OutPutPrivateKeyFilePathHolder.Text;
           
            
            
            Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        setContent();
                    });

                    //decrypt symmetric key with private RSA key
                    rsaCore = new RsaCore(generateKeys:false);
                    rsaCore.ImportPrivateKey(privateKeyFileName);
                    
                    var decryptedKeyFilePath = "./resources/DecryptedSymmetricKey";
                    CypherMethods.DecryptKey(rsaCore, _mainWindowViewModel.SymmetricKeyFile, decryptedKeyFilePath);
                    
                    
                    //decrypt file with MAGENTA
                    // _mainWindowViewModel.MainTaskManager = new TaskManager(decryptedKeyFilePath,_mainWindowViewModel.MagentaKeySize, _mainWindowViewModel.EncryptionMode);
                    _mainWindowViewModel.MainTaskManager = new TaskManager(decryptedKeyFilePath,_mainWindowViewModel.MagentaKeySize, _mainWindowViewModel.EncryptionMode);

                    // if (_mainWindowViewModel.SymmetricKeyFile == null)
                    //     throw new NullReferenceException("The key file path is empty");
                    // if (_mainWindowViewModel.IvFilePath == null && _mainWindowViewModel.MainTaskManager.EncryptionMode != "ECB")
                    //     throw new NullReferenceException("The IV file path is empty");
                    // if (_mainWindowViewModel.DataFile == null)
                    //     throw new NullReferenceException("The data file path is empty");
                    // if (outputFileName == null || outputFileName == "")
                    //     throw new NullReferenceException("The output file name is empty");
                    
                    //_mainWindowViewModel.MainTaskManager.keyFilePath = decryptedKeyFilePath;
                    //_mainWindowViewModel.MainTaskManager.ivFilePath = _mainWindowViewModel.IvFilePath;
                    _mainWindowViewModel.MainTaskManager.inputFilePath = _mainWindowViewModel.DataFile;
                    _mainWindowViewModel.MainTaskManager.outputFilePath = outputFileName;
                    
                    _mainWindowViewModel.MainTaskManager.RunDecryptionProcess();

                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        removeContent();
                    });
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        removeContent();
                    });
                }
            });
        }
        private void OnChooseSymmetricKeyFileButtonClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                _mainWindowViewModel.SymmetricKeyFile = filename;
            }
        }
        private void OnChoosePublicKeyFileButtonClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                _mainWindowViewModel.PublicKeyFile = filename;
            }
        }
        private void OnChoosePrivateKeyFileButtonClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                _mainWindowViewModel.PrivateKeyFile = filename;
            }
        }
        private void OnChooseIvFileButtonClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                _mainWindowViewModel.IvFilePath = filename;
            }
        }
        private void OnChooseDataFileButtonClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                _mainWindowViewModel.DataFile = filename;
            }
        }
        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.MainTaskManager.Cancel();
            
        }

        private void setContent()
        {
            var pb = new CircularProgressBar();
            //pb.Message = "Wait...";
            ProgressBar.Content = pb;
        }
        
        private void removeContent()
        {
            ProgressBar.Content = null;
        }
    }
}