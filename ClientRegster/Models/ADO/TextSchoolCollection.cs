using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegster.Models.ADO
{
    public class TextSchoolCollection : List<School>
    {
        public TextSchoolCollection(string excelFilePath)
        {
            //Shift-JISで読み込み
            using (var sr = new StreamReader(excelFilePath, Encoding.GetEncoding(932)))
            {
                var schoolList = sr.ReadToEnd()
                                    .Replace("\r\n", "\n")
                                    .Split('\n')
                                    .Select(x => new School() { Name = x });
                this.AddRange(schoolList);
            }
        }
    }
}
