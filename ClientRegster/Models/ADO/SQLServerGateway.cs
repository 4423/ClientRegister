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
            this.context.School.InsertAllOnSubmit(schoolList);
            this.SecureSubmitChanges();
        }
        
        public School InsertSchool(School school)
        {
            this.context.School.InsertOnSubmit(school);
            this.SecureSubmitChanges();

            //この段階でIDとかがschoolのインスタンスに反映されている
            return school;
        }


        public void ImportStudentList(IEnumerable<Student> studentList)
        {
            this.context.Student.InsertAllOnSubmit(studentList);
            this.SecureSubmitChanges();
        }

        public Student InsertStudent(Student student)
        {
            this.context.Student.InsertOnSubmit(student);
            this.SecureSubmitChanges();
            return student;
        }

        /// <summary>
        /// 学生を学校と関連付けてデータベースに挿入します。
        /// 学校が存在しない場合は新規に学校を登録します。
        /// </summary>
        /// <param name="student"></param>
        /// <param name="schoolName"></param>
        public Student InsertStudent(Student student, string schoolName)
        {
            if (this.HasSchool(schoolName) == false)
            {
                this.InsertSchool(new School() { Name = schoolName });
            }

            //IDの関連付け
            var school = this.GetSchool(schoolName);
            student.SchoolId = this.GetSchool(schoolName).Id;

            this.context.Student.InsertOnSubmit(student);
            this.SecureSubmitChanges();
            return student;
        }


        public bool HasSchool(string schoolName)
        {
            return this.context.School.Any(x => x.Name == schoolName);
        }

        public bool HasStudent(string studentName)
        {
            return this.context.Student.Any(x => x.Name == studentName);
        }


        public School GetSchool(string schoolName)
        {
            return this.context.School.Single(x => x.Name.IndexOf(schoolName) != -1);
        }

        public Student GetStudent(string studentName)
        {
            return this.context.Student.Single(x => x.Name.IndexOf(studentName) != -1);
        }


        public IEnumerable<School> GetAllSchool()
        {
            return this.context.School;
        }

        public IEnumerable<Student> GetAllStudent()
        {
            return this.context.Student;
        }
    }
}
