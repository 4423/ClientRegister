using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientRegster.Models.ADO;
using Microsoft.Win32;

namespace ClientRegster.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLServerGateway sql;
        private ObservableCollection<Student> observableStudent;

        public MainWindow()
        {
            InitializeComponent();

            this.sql = SQLServerGateway.Instance;
            this.observableStudent = new ObservableCollection<Student>(sql.GetAllStudent());
            this.dataGrid.ItemsSource = this.observableStudent;

            this.textBoxSchoolName.Candidates = sql.GetAllSchool().Select(x => x.Name);
        }


        private void buttonInputSchools_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                FilterIndex = 1,
                Filter = "テキスト ファイル(.txt)|*.txt|All Files (*.*)|*.*"
            };
            bool? result = ofd.ShowDialog();
            if (result == false)
            {
                return;
            }

            var schoolList = new TextSchoolCollection(ofd.FileName);
            sql.ImportSchoolList(schoolList);
        }


        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            //入力のチェック
            if (this.CanRegister() == false)
            {
                return;
            }

            var student = new Student()
            {
                Name = this.textBoxStudentName.Text
            };
            student = sql.InsertStudent(student, this.textBoxSchoolName.Text);

            this.observableStudent.Add(student);
            this.ClearTextBox();
        }


        private bool CanRegister()
        {
            return !String.IsNullOrWhiteSpace(this.textBoxSchoolName.Text) 
                && !String.IsNullOrWhiteSpace(this.textBoxStudentName.Text);
        }

        private void ClearTextBox()
        {
            this.textBoxSchoolName.Text = "";
            this.textBoxStudentName.Text = "";
        }
    }
}
