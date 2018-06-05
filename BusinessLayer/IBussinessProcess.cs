using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IBussinessProcess
    {
        ReaderClass ReadWriteTextDataFiles(string inputPath);

        void WriteToOutPutFile(ReaderClass content, string outPutPath);
    }
}
