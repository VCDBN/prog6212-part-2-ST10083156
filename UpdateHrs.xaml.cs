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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace PROG6212_POE
{
    /// <summary>
    /// Interaction logic for UpdateHrs.xaml
    /// </summary>
    public partial class UpdateHrs : Window
    {
        SqlConnection conn = new SqlConnection(Connection.connectionString);
        //initializer for page
        //calls "PopulateWeekComboBox" and "PopulateModuleComboBox" methods to populate the corresponding methods on runtime
        public UpdateHrs()
        {
            InitializeComponent();
            PopulateWeekComboBox();
            PopulateModuleComboBox();
        }

        //populates week combobox by using the number of "Week" objects in the "Weeks" list
        private void PopulateWeekComboBox()
        {
            weekComboBox.Items.Clear();
            int numWeeks = DbData.NumWeeks;
            for(int i = 0;i < numWeeks; i++) 
            {
                weekComboBox.Items.Add($"Week {i + 1}");

            }
        }

        //populates the module combobox by using the number of "Module" objects in the "Modules list
        private void PopulateModuleComboBox()
        {
            moduleComboBox.Items.Clear();
            string countQuery = "SELECT COUNT(*) FROM Module WHERE Username = @username";
            SqlCommand countCmd = new SqlCommand(countQuery, conn);
            countCmd.Parameters.AddWithValue("@username", DbData.Username);
            conn.Open();
            int numModules = countCmd.ExecuteNonQuery();
            string getModules = "SELECT Name FROM Module WHERE Username =@username";
            SqlCommand moduleCmd = new SqlCommand(getModules, conn);
            moduleCmd.Parameters.AddWithValue("@username",DbData.Username);
            SqlDataAdapter modAdapter = new SqlDataAdapter(moduleCmd);
            DataTable modTable = new DataTable();   
            modAdapter.Fill(modTable);
            moduleComboBox.ItemsSource =modTable.DefaultView;
            moduleComboBox.DisplayMemberPath = "Name";
            conn.Close();
        }
        int hrsStudied;
        private void updateHrsBtn_Click(object sender, RoutedEventArgs e)
        {
            //trying to assign all user input to corresponding variables
            
            try { hrsStudied = Convert.ToInt32(hrsTxt.Text); }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            hrsStudied= Convert.ToInt32(hrsTxt.Text);
            string moduleName = moduleComboBox.Text;
            int weekIndex = weekComboBox.SelectedIndex +1;

            //calculating the number of self study hours remaining in the selected week
            string query = "Select s.StudyHrs From Semester s JOIN Module m ON s.Code = m.Code AND m.Username = s.Username WHERE s.Username = @username AND m.Name = @name AND s.Week = @week";
            SqlConnection conn = new SqlConnection(Connection.connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", DbData.Username);
            cmd.Parameters.AddWithValue("@name", moduleName);
            cmd.Parameters.AddWithValue("@week", weekIndex);
            conn.Open();
            object result = cmd.ExecuteScalar();
            int selfStudyHrs = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
            conn.Close();
            int hrsRemaining = selfStudyHrs - hrsStudied;
            if (hrsRemaining < 0)
            {
                MessageBox.Show("You have entered more hours than you have left this week", "No hours left", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                hrsRemaining = 0;
                return;
            }
            else
            {
                //updating the self study hours in the Database
                string getCode = "Select m.Code from Module m JOIN Semester s on s.Username = m.Username WHERE m.Name = @name";

                SqlConnection getCodeConn = new SqlConnection(Connection.connectionString);
                SqlCommand getCodeCmd = new SqlCommand(getCode, getCodeConn);
                getCodeCmd.Parameters.AddWithValue("@name", moduleName);

                getCodeConn.Open();
                string code = getCodeCmd.ExecuteNonQuery().ToString();
                getCodeConn.Close();

                string studyHrsRem = "UPDATE Semester SET StudyHrs = @studyHrsRem WHERE Username = @username AND Code = @code AND Week = @week";
                
                SqlConnection updateCon = new SqlConnection(Connection.connectionString);
                SqlCommand updateCmd = new SqlCommand(studyHrsRem, updateCon);
                updateCmd.Parameters.AddWithValue("@studyHrsRem",hrsRemaining);
                updateCmd.Parameters.AddWithValue("@username", DbData.Username);
                updateCmd.Parameters.AddWithValue("@code", code);
                updateCmd.Parameters.AddWithValue("@week", weekIndex);

                updateCon.Open();
                updateCmd.ExecuteNonQuery();
                updateCon.Close();
            }
            //returning the user to the display for self study hours
            StudyHrsDisplay studyHrsDisplay = new StudyHrsDisplay();
            this.Hide();
            studyHrsDisplay.Show();
            this.Close();
        }
    }
}
