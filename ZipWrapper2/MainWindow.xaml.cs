using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZipWrapper2.Services;

namespace ZipWrapper2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ZipService _zipService;

    public MainWindow()
    {
        InitializeComponent();
        _zipService = new ZipService();
    }

    private void OnFileDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            // Get the dropped files
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0)
            {
                // Clear the ListBox and add each file path
                FileDropListBox.Items.Clear();
                foreach (string filePath in files)
                {
                    FileDropListBox.Items.Add(filePath);
                }
            }
        }

        e.Handled = true;
    }

    private void OnDragEnter(object sender, DragEventArgs e)
    {
        // Check if the data being dragged is a file
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            // Allow the drop operation
            e.Effects = DragDropEffects.Copy;
        }
        else
        {
            // Disallow the drop operation
            e.Effects = DragDropEffects.None;
        }

        e.Handled = true;
    }
    private void OnDragOver(object sender, DragEventArgs e)
    {
        // Check if the data being dragged is a file
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effects = DragDropEffects.Copy;
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }

        e.Handled = true;
    }

    private void OnExecuteClick(object sender, RoutedEventArgs e)
    {
        // Retrieve all file paths from the ListBox
        var filePaths = FileDropListBox.Items?.Cast<string>().ToList();

        if (filePaths == null)
        {
            filePaths = new List<string>();
        }
        
        int pathsCount = filePaths.Count();
        // Ensure there are files to process
        if (pathsCount == 0)
        {
            MessageBox.Show("No files selected. Please drag and drop files into the drop area.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        bool isZipOperation = (!(filePaths[0].EndsWith(".zip", StringComparison.OrdinalIgnoreCase) || 
                                    filePaths[0].EndsWith(".7z", StringComparison.OrdinalIgnoreCase)));

        string password = PasswordBox.Password; // Use PasswordBox.Password for the passwordz
        string resultMessage = "No operation performed.";
        if(isZipOperation)
        {
            if (pathsCount == 1)
            {
                resultMessage = _zipService.ZipFile(filePaths[0], password);
            }
            else if(pathsCount > 1)
            {
                resultMessage = _zipService.ZipMultipleFiles(filePaths, password);
            }

        }
        else //it is an unzip operation
        {
            if(pathsCount > 1)
            {
                MessageBox.Show("Please select only one zip file to unzip.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            resultMessage = _zipService.UnzipFile(filePaths[0], password);
        }
        // Show the result message in a MessageBox
        MessageBox.Show(resultMessage, "Result", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OnShowPasswordChecked(object sender, RoutedEventArgs e)
    {
        // Show the plain text password box and hide the obfuscated password box
        PlainTextPasswordBox.Text = PasswordBox.Password;
        PlainTextPasswordBox.Visibility = Visibility.Visible;
        PasswordBox.Visibility = Visibility.Collapsed;
    }

    private void OnShowPasswordUnchecked(object sender, RoutedEventArgs e)
    {
        // Show the obfuscated password box and hide the plain text password box
        PasswordBox.Password = PlainTextPasswordBox.Text;
        PasswordBox.Visibility = Visibility.Visible;
        PlainTextPasswordBox.Visibility = Visibility.Collapsed;
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        // Sync the password from the PasswordBox to the PlainTextPasswordBox
        if (PasswordBox.Visibility == Visibility.Visible)
        {
            PlainTextPasswordBox.Text = PasswordBox.Password;
        }
    }

    private void OnPlainTextPasswordChanged(object sender, TextChangedEventArgs e)
    {
        // Sync the password from the PlainTextPasswordBox to the PasswordBox
        if (PlainTextPasswordBox.Visibility == Visibility.Visible)
        {
            PasswordBox.Password = PlainTextPasswordBox.Text;
        }
    }
}