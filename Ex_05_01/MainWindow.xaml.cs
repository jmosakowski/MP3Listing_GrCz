using System;               // needed for AppContext
using System.IO;            // needed for File
using System.Windows;       // needed for RoutedEventArgs, Window
using Microsoft.Win32;      // needed for OpenFileDialog

namespace Ex_05_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuFileLoadPath_Click(object sender, RoutedEventArgs e)
        {
            // Load a file with the MP3 folder path (copy text from a file to TextBox)
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.InitialDirectory = AppContext.BaseDirectory; // or @"C:\..."
            openFileDlg.Filter = "txt files (*.txt)|*.txt|all files (*.*)|*.*";
            bool? result = openFileDlg.ShowDialog();

            if (result == true)
                TextPath.Text = File.ReadAllText(openFileDlg.FileName);
        }

        private void MenuFileSavePath_Click(object sender, RoutedEventArgs e)
        {
            // Create a file with the MP3 folder path (save TextBox text to a file)
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.InitialDirectory = AppContext.BaseDirectory; // or @"C:\..."
            saveFileDlg.FileName = "path";
            saveFileDlg.Filter = "txt files (*.txt)|*.txt|all files (*.*)|*.*";
            bool? result = saveFileDlg.ShowDialog();

            if (result == true)
                File.WriteAllText(saveFileDlg.FileName, TextPath.Text);
        }

        private void MenuFileExit_Click(object sender, RoutedEventArgs e)
        {
            // Prepare settings for the Message Box
            string text = "The program will be closed.";
            string windowName = "Exit";
            var buttons = MessageBoxButton.OKCancel;
            var iconInside = MessageBoxImage.Information;

            var whatsClicked = MessageBox.Show(text, windowName, buttons, iconInside);
            if (whatsClicked == MessageBoxResult.OK)
                Application.Current.Shutdown(); // else - the window will auto close
        }

        private void MenuInfoAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MP3 Listing with WPF\nJacek Mosakowski, WSEI Krakow");
        }

        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            // Load path from the TextBox, adding "\" at the end for safety
            string dir = TextPath.Text;
            if (dir[dir.Length - 1] != '\\') // or dir[^1] since C# 8.0
                dir += '\\';

            // Include subfolders or not
            SearchOption searchOpt = (CheckBoxSubfolders.IsChecked == true) ?
            SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            // Get an array with names of all files ending with ".mp3"
            string[] allMP3Files = Directory.GetFiles(dir, "*.mp3", searchOpt);

            // Save the array with names to a file
            string outputFile = dir + "mp3_list.txt";
            using StreamWriter writer = new StreamWriter(outputFile);
            foreach (string file in allMP3Files)
                writer.WriteLine(file);

            // Inform the user about the result
            MessageBox.Show(allMP3Files.Length + " files listed!", "Status info");
        }
    }
}
