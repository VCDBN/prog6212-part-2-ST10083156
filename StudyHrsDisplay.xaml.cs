using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace PROG6212_POE
{
    /// <summary>
    /// Interaction logic for StudyHrsDisplay.xaml
    /// </summary>
    public partial class StudyHrsDisplay : Window
    {
        SqlConnection conn=new SqlConnection(Connection.connectionString);
        //initializer for page
        public StudyHrsDisplay()
        {
            InitializeComponent();
            //populating combo box with weeks and their numbers
            selfStudyGrid.IsReadOnly= true;
            PopulateWeekComboBox();
            //displays week 1 as default
            selectedWeekComboBox.SelectedIndex = 0;
            selfStudyGrid.Items.Refresh();
        }


        //update button transfer users to the update hours page to record hours studied
        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateHrs updateHrs = new UpdateHrs();
            this.Hide();
            updateHrs.Show();
            this.Close();
        }

        private void selectedWeekComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = selectedWeekComboBox.SelectedIndex;
            string dataQuery = "SELECT m.Name, m.StudyHrs FROM Module m JOIN Semester s ON m.Code = s.Code AND m.Username= s.Username WHERE s.Username = @username AND s.Week = @week";
            conn.Open();
            SqlCommand dataCmd = new SqlCommand(dataQuery, conn);
            dataCmd.Parameters.AddWithValue("@username", DbData.Username);
            dataCmd.Parameters.AddWithValue("@week", (i + 1));
            SqlDataAdapter dataAdapter= new SqlDataAdapter(dataCmd);
            DataTable semesterTable = new DataTable();
            dataAdapter.Fill(semesterTable);
            conn.Close();
            selfStudyGrid.ItemsSource =semesterTable.DefaultView;
            selfStudyGrid.Items.Refresh();

        }
         
        private void PopulateWeekComboBox()
        {
            string weekQuery = "SELECT COUNT(*) FROM Semester WHERE Username = @username AND Code = @code";
            string codeQuery = "SELECT TOP 1 Code FROM Module WHERE Username = @username";
            SqlCommand codeCmd = new SqlCommand(codeQuery, conn);
            codeCmd.Parameters.AddWithValue("@username", DbData.Username);
            conn.Open();
            string code = codeCmd.ExecuteScalar()?.ToString();
            SqlCommand weekCmd = new SqlCommand(weekQuery, conn);
            weekCmd.Parameters.AddWithValue("@username", DbData.Username);
            weekCmd.Parameters.AddWithValue("@code", code);
            DbData.NumWeeks = (int)weekCmd.ExecuteScalar();
            for (int i = 0; i < DbData.NumWeeks; i++)
            {
                selectedWeekComboBox.Items.Add($"Week {i + 1}");
            }
            conn.Close();
        }

        private void addModuleBtn_Click(object sender, RoutedEventArgs e)
        {
            ModuleWindow moduleWindow = new ModuleWindow();
            this.Hide();
            moduleWindow.Show();
            this.Close();
        }

        private void signOutBtn_Click(object sender, RoutedEventArgs e)
        {
            DbData.Username = null;
            Login login = new Login();
            this.Hide();
            login.Show();
            this.Close();
        }
    }
}
