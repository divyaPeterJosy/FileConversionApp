using BusinessLayer;
using System.Threading.Tasks;
using System.Windows;


namespace FileConversion
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

        /// <summary>
        /// button functionality for selecting source folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SourceButton_Click(object sender, RoutedEventArgs e)
        {

            sourcePath.Text = GetFolderPath();
        }

        /// <summary>
        /// button functionality for selecting destination folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DestinationButton_Click(object sender, RoutedEventArgs e)
        {
            destPath.Text = GetFolderPath();
        }


        /// <summary>
        ///  functionality for selecting folder for source and destination
        /// </summary>
        /// <returns></returns>
        private string GetFolderPath()
        {
            string path = string.Empty;
            System.Windows.Forms.FolderBrowserDialog pathSelector = new System.Windows.Forms.FolderBrowserDialog();
            // This is what will execute if the user selects a folder and hits OK.
            if (pathSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = pathSelector.SelectedPath;
            }
            return path;
        }

        /// <summary>
        /// asynchronous button functionality for writing file in xml/html format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void startConversion_Click(object sender, RoutedEventArgs e)
        {
            string source = sourcePath.Text;
            string destination = destPath.Text;
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
            {
                message.Content = Constants.SELECT_FOLDERS;
            }
            else
            {
                string selectedType = typeSelected.Text;

                string conversionType = typeSelected.Text;
                Task<int> task = new Task<int>(() => ReadWriteData(source, destination, conversionType));
                task.Start();

                message.Content = Constants.CONVERSION_PROCESS;

                int status = await task;
                if (status == 1)
                {
                    message.Content = Constants.FILE_CONVERSION_SUCCESS;
                }
                else
                {
                    message.Content = Constants.FILE_CONVERSION_FAIL;
                }
            }


        }

        /// <summary>
        ///  functionality for reading and writing file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private int ReadWriteData(string source, string destination, string conversionType)
        {
            IBussinessProcess textWriter;
            if (conversionType == Constants.CONVERSION_TYPE)
            {
                textWriter = new HtmlWriter();
            }
            else
            {
                textWriter = new XmlWriter();
            }

            ReaderClass content = textWriter.ReadWriteTextDataFiles(source);
            if (content != null && content.Code != string.Empty)
            {
                textWriter.WriteToOutPutFile(content, destination);
            }
            else
            {
                return 0;
            }

            return 1;
        }

    }
}