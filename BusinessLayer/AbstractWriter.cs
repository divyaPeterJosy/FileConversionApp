using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BusinessLayer
{
    /// <summary>
    /// abstract class implementing the interface
    /// </summary>
    public abstract class AbstractWriter : IBussinessProcess
    {
        /// <summary>
        /// function for reading text file
        /// </summary>
        /// <param name="inputPath"></param>
        /// <returns></returns>
        public ReaderClass ReadWriteTextDataFiles(string inputPath)
        {
            ReaderClass readData = new ReaderClass();

            var result = readAllFiles(readData, inputPath);
            result.Wait();            
            
            return readData;
        }

        /// <summary>
        /// abstract method for writing text file in xml/html format
        /// </summary>
        /// <param name="content"></param>
        /// <param name="outPutPath"></param>
        public abstract void WriteToOutPutFile(ReaderClass content, string outputPath);

        /// <summary>
        /// Method for reading the files asynchronously 
        /// </summary>
        /// <param name="readData"></param>
        /// <param name="inputPath"></param>
        /// <returns></returns>
        public async Task readAllFiles(ReaderClass readData, string inputPath)
        {
            List<string> fileNames = new List<string>();
            List<string> codes = new List<string>();
            List<string> descriptions = new List<string>();
            FileInfo file;
            List<string> datas = new List<string>();

            string[] fileAry = Directory.GetFiles(inputPath, "*.txt");
            foreach (string filePath in fileAry)
            {
                file = new FileInfo(filePath);
                fileNames.Add(file.Name);

                Task task = ReadTextAsync(filePath, codes, descriptions, datas);
                await task;
            }
            readData.FileNames = string.Join(",", fileNames);
            readData.Code = string.Join(",", codes);
            readData.FileDescription = string.Join(",", descriptions);
            readData.Datas = datas;
           
        }

        private async Task ReadTextAsync(string filePath,List<string> codes, List<string> descriptions, List<string> datas)
        {
            await Task.Run(() =>
            {
                using (TextReader tr = new StreamReader(filePath))
                {
                    string line;
                    string lineCode = "";
                    bool nextLine = true;
                    while ((line = tr.ReadLine()) != null)
                    {
                        nextLine = true;
                        if (line == "[Code]")
                        {
                            nextLine = false;
                            lineCode = "Code";
                        }
                        else if (line == "[Description]")
                        {
                            nextLine = false;
                            lineCode = "Desc";
                        }
                        else if (line == "[Data]")
                        {
                            nextLine = false;
                            lineCode = "Data";
                        }
                        if (nextLine)
                        {
                            if (lineCode == "Code")
                            {
                                codes.Add(line);
                            }
                            else if (lineCode == "Desc")
                            {
                                descriptions.Add(line);
                            }
                            else if (lineCode == "Data")
                            {
                                datas.Add(line);
                            }
                        }
                    }

                    tr.Close();
                    tr.Dispose();
                }
            });
                      
        }
    }
}