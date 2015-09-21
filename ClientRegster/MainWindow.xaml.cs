using System;
using System.Collections.Generic;
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

namespace ClientRegster
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonInputSchools_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                FilterIndex = 1,
                Filter = "テキスト ファイル(.txt)|*.txt|HTML File(*.html, *.htm)|*.html;*.htm|All Files (*.*)|*.*"
            };
            bool? result = ofd.ShowDialog();
            if (result == false)
            {
                return;
            }

            var schoolList = new TextSchoolCollection(ofd.FileName);

            var sql = SQLServerGateway.Instance;
            sql.ImportSchoolList(schoolList);

            this.dataGrid.ItemsSource = sql.GetAllSchool();
        }
    }
}
