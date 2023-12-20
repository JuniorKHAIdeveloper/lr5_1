using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using Path = System.IO.Path;

namespace lr5_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

         private void ChooseFileButtonClick(object sender, RoutedEventArgs e)
         {
             var opnFile = new OpenFileDialog();
             opnFile.Filter = "7zip files (*.7z)|*.7z|All files (*.*)|*.*";
             opnFile.FilterIndex = 2;
             if ((bool)opnFile.ShowDialog())
             {
                 currentFile.Text = opnFile.FileName;
             }
         }

        private void zipunzipClick(object sender, RoutedEventArgs e)
        {
            var sourcePath = currentFile.Text;
            var pathExtension = Path.GetExtension(sourcePath);
            var pInfo = new ProcessStartInfo();
            pInfo.FileName = "7za.exe";
            pInfo.RedirectStandardOutput = true;
            pInfo.UseShellExecute = false;
            try
            {
                if (pathExtension != ".7z")
                {
                    var archivePath = sourcePath.Replace(pathExtension, "");
                    pInfo.Arguments = $"a {archivePath}  {sourcePath}";
                }
                else
                {
                    var extractPath = sourcePath.Replace(Path.GetFileName(sourcePath), "");
                    pInfo.Arguments = $"x {sourcePath} -o{extractPath}";
                }
            }
            catch
            {
                logBox.Items.Add("Неправильный путь к файл.");
            }
            try
            {
                logBox.Items.Add("Начало работы");
                var proc = Process.Start(pInfo);
                var reader = proc.StandardOutput;
                proc.WaitForExit();
                logBox.Items.Add("Готово.");
                var outText = reader.ReadToEnd();
                if (outText.Contains("Внимание"))
                {
                    logBox.Items.Add("Ошибка архивации файла.");
                }
                var myRun = new Run(outText);
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(myRun);
                var flowDoc = new FlowDocument();
                flowDoc.Blocks.Add(paragraph);
            }
            catch
            {
                logBox.Items.Add("Ошибка.");
            }
        }   
    }
}