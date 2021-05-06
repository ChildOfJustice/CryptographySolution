using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SecondTask_3.AsyncCypher;
using SecondTask_3.Spinner;
using Task_3.ViewModels;
using Task_8;
using Task_8.AsyncCypher;

namespace SecondTask_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainWindowViewModel _mainWindowViewModel;
        
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
        }
        
        
        
        
        private void ComboBoxRijndaelBlockSizeSelected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            _mainWindowViewModel.RijndaelBlockSize = Int32.Parse(selectedItem.Text)/8;
            // MessageBox.Show(selectedItem.Text);
        }
        private void ComboBoxRijndaelKeySizeSelected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock selectedItem = (TextBlock)comboBox.SelectedItem;
            _mainWindowViewModel.RijndaelKeySize = Int32.Parse(selectedItem.Text)/8;
            // MessageBox.Show(selectedItem.Text);
        }
        
        
        
        private void OnEncryptButtonClick(object sender, RoutedEventArgs e)
        {
            var outputFileName = OutPutFilePathHolder.Text;

            Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        setContent();
                    });
                    
                    _mainWindowViewModel.MainTaskManager = new TaskManager(CypherAlgorithm.Rijndael,
                        _mainWindowViewModel.RijndaelBlockSize, _mainWindowViewModel.RijndaelKeySize);

                    if (_mainWindowViewModel.SymmetricKeyFile == null)
                        throw new NullReferenceException("The key file path is empty");
                    if (_mainWindowViewModel.DataFile == null)
                        throw new NullReferenceException("The data file path is empty");
                    if (outputFileName == null || outputFileName == "")
                        throw new NullReferenceException("The output file name is empty");
                    
                    _mainWindowViewModel.MainTaskManager.keyFilePath = _mainWindowViewModel.SymmetricKeyFile;
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
        private void OnDecryptButtonClick(object sender, RoutedEventArgs e)
        {
            var outputFileName = OutPutFilePathHolder.Text;
            
            Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        setContent();
                    });
                    
                    
                    _mainWindowViewModel.MainTaskManager = new TaskManager(CypherAlgorithm.Rijndael,
                        _mainWindowViewModel.RijndaelBlockSize, _mainWindowViewModel.RijndaelKeySize);

                    if (_mainWindowViewModel.SymmetricKeyFile == null)
                        throw new NullReferenceException("The key file path is empty");
                    if (_mainWindowViewModel.DataFile == null)
                        throw new NullReferenceException("The data file path is empty");
                    if (outputFileName == null || outputFileName == "")
                        throw new NullReferenceException("The output file name is empty");

                    _mainWindowViewModel.MainTaskManager.keyFilePath = _mainWindowViewModel.SymmetricKeyFile;
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
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        removeContent();
                    });
                    
                    MessageBox.Show(exception.Message);
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
            ProgressBar.Content = new CircularProgressBar();
        }
        
        private void removeContent()
        {
            ProgressBar.Content = null;
        }
    }
}