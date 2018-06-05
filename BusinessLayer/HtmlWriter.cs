using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace BusinessLayer
{
    /// <summary>
    /// class inherited from abstract class 
    /// </summary>
    public class HtmlWriter : AbstractWriter
    {
        /// <summary>
        /// function for writing text file into html format
        /// </summary>
        public override void WriteToOutPutFile(ReaderClass content, string outPutPath)
        {

            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);
            writer.WriteBeginTag("html");
            writer.Write(HtmlTextWriter.TagRightChar);
            GetHtmlToWrite(content, writer);
            writer.WriteEndTag("html");
            string fileName = "\\OutputHTML_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
            File.WriteAllText(outPutPath + fileName, writer.InnerWriter.ToString());

            writer.Close();
        }


        private HtmlTextWriter GetHtmlToWrite(ReaderClass content, HtmlTextWriter writer)
        {
            List<string> codes = !string.IsNullOrEmpty(content.Code) ? content.Code.Split(',').ToList() : new List<string>();
            List<string> description = !string.IsNullOrEmpty(content.FileDescription) ? content.FileDescription.Split(',').ToList() : new List<string>();
            List<string> metaHeader = new List<string> { "File", "Code", "Description" };
            List<string> dataHeader = new List<string> { "Time" };

            int fileCount = 0;
            GetHtmlHeader(writer, "Metadata");
            writer.WriteBeginTag("table");
            writer.WriteAttribute("border", "1");
            writer.Write(HtmlTextWriter.TagRightChar);
            GetTableTr(writer, metaHeader, "", "", "");
            foreach (string file in content.FileNames.Split(','))
            {
                dataHeader.Add(codes[fileCount]);
                GetTableTr(writer, null, file, codes[fileCount], description[fileCount]);
                fileCount++;
            }
            writer.WriteEndTag("table");


            GetHtmlHeader(writer, "Activity Data");
            writer.WriteBeginTag("table");
            writer.WriteAttribute("border", "1");
            writer.Write(HtmlTextWriter.TagRightChar);
            GetTableTr(writer, dataHeader, "", "", "");
            foreach (string data in content.Datas)
            {
                GetTableTrData(writer, data);
            }


            writer.WriteEndTag("table");
            return writer;
        }


        private void GetHtmlHeader(HtmlTextWriter writer, string data)
        {
            writer.WriteBeginTag("h1");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(data);
            writer.WriteEndTag("h1");
        }

        private void GetTableTh(HtmlTextWriter writer, string data)
        {
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(data);
            writer.WriteEndTag("th");
        }
        private void GetTableTd(HtmlTextWriter writer, string data)
        {
            writer.WriteBeginTag("td");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(data);
            writer.WriteEndTag("td");
        }

        private void GetTableTrData(HtmlTextWriter writer, string datas)
        {
            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
            foreach (string td in datas.Split(','))
            {
                GetTableTd(writer, td);
            }
            writer.WriteEndTag("tr");
        }

        private void GetTableTr(HtmlTextWriter writer, List<string> tableTh, string fileName, string code, string desc)
        {
            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
            if (tableTh != null)
            {
                foreach (string th in tableTh)
                {
                    GetTableTh(writer, th);
                }
            }
            if (fileName != "")
            {
                GetTableTd(writer, fileName);
            }
            if (code != "")
            {
                GetTableTd(writer, code);
            }
            if (desc != "")
            {
                GetTableTd(writer, desc);
            }

            writer.WriteEndTag("tr");
        }

    }
}