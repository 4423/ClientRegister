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
            using (var sr = new StreamReader(excelFilePath))
            {
                var schoolList = sr.ReadToEnd()
                                    .Split('\n')
                                    .Select(x => new School() { Name = x });
                this.AddRange(schoolList);
            }
        }
    }
}
