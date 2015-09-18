using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClientRegster.Models.ADO
{
    public class SQLServerGateway
    {
        private static SQLServerGateway instance = new SQLServerGateway();
        public static SQLServerGateway Instance => instance;

        private String connectionString;

        private ClientDataClassesDataContext context;


        private SQLServerGateway()
        {
            this.connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""{ Directory.GetCurrentDirectory() }\ClientDatabase.mdf"";Integrated Security=True";
            this.context = new ClientDataClassesDataContext(this.connectionString);
        }


        private void SecureSubmitChanges()
        {
            //Retryや例外処理の実装
            this.context.SubmitChanges();
        }


        public void ImportSchoolList(IEnumerable<School> schoolList)
        {
            foreach (var school in schoolList)
            {
                this.InsertSchool(school);
            }
        }


        public void InsertSchool(School school)
        {
            this.context.School.InsertOnSubmit(school);
            this.SecureSubmitChanges();
        }


        public IEnumerable<School> GetAllSchool()
        {
            return this.context.School;
        }
    }
}
