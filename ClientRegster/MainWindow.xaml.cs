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
            var schoolList = new TextSchoolCollection(@"C:\Users\YUHI\school.txt");

            var sql = SQLServerGateway.Instance;
            sql.ImportSchoolList(schoolList);

            this.dataGrid.ItemsSource = sql.GetAllSchool();
        }
    }
}
