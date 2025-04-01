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

            // Process the first file (you can modify this to handle multiple files)
            if (files.Length > 0)
            {
                string filePath = files[0];
                FileDropTextBlock.Text = filePath;
                // Check the type of the sender
                // if (sender is Border border && border.Child is TextBlock textBlock)
                // {
                //     // Update the TextBlock inside the Border with the file path
                //     textBlock.Text = filePath;
                //     ((TextBlock)((Border)sender).Child).Text = filePath;
                // }
                // else if (sender is Rectangle rectangle)
                // {
                //     // Handle the case where the sender is a Rectangle
                //     //MessageBox.Show($"File dropped on a Rectangle: {filePath}", "File Dropped", MessageBoxButton.OK, MessageBoxImage.Information);
                // }
                // else if (sender is TextBlock textBlock)
                // {
                //     // Handle the case where the sender is a TextBlock
                //     textBlock.Text = filePath;
                // }
                // else
                // {
                //     // Handle other cases
                //     //MessageBox.Show($"File dropped: {filePath}", "File Dropped", MessageBoxButton.OK, MessageBoxImage.Information);
                // }
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
        string filePath = FileDropTextBlock.Text;
        // string filePath = ((TextBlock)((Border)FindName("FileDropBorder")).Child).Text;
        string password = PasswordBox.Password; // Use PasswordBox.Password instead of PasswordBox.Text

        // Determine the operation based on the file extension
        bool isZipOperation = !(filePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) || 
                                filePath.EndsWith(".7z", StringComparison.OrdinalIgnoreCase));

        string resultMessage;
        if (isZipOperation)
        {
            resultMessage = _zipService.ZipFile(filePath, password);
        }
        else
        {
            resultMessage = _zipService.UnzipFile(filePath, password);
        }

        MessageBox.Show(resultMessage);
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