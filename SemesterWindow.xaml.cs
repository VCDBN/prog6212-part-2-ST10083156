using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace PROG6212_POE
{
    /// <summary>
    /// Interaction logic for SemesterWindow.xaml
    /// </summary>
    public partial class SemesterWindow : Window
    {
        //Varibales used to hold details about semester
        public static int StudyHrs { get; set; }
        public DateTime startDate { get; set; }
        public DateTime currentDate { get; set; }
public TimeSpan semesterLength { get; set; }
        Week week = new Week();

//initializer for page
        public SemesterWindow()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(Connection.connectionString);

        //Confirm button click, when used, completes calculation for self study hours and adds it to the IDictionary in the Week class
        //Afterwards, transfers users to the display for the self study hours per week

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            DbData.NumWeeks = Convert.ToInt32(numWeeksTxt.Text);
            string selectQuery = "SELECT Code, Username, Credits, ClassHrs FROM Module";
            SqlCommand selectCmd = new SqlCommand(selectQuery, conn);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string moduleCode = reader["Code"].ToString();
                    string username = reader["Username"].ToString();
                    int credits = Convert.ToInt32(reader["Credits"]);
                    int classHrs = Convert.ToInt32(reader["ClassHrs"]);

                    int studyHrs = (credits * 10) / DbData.NumWeeks - classHrs;

                    using (SqlConnection updateConnection = new SqlConnection(Connection.connectionString))
                    {
                        string updateQuery = "UPDATE Module SET StudyHrs = @studyHrs WHERE Code = @moduleCode AND Username = @username";
                        updateConnection.Open();

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, updateConnection))
                        {
                            updateCmd.Parameters.AddWithValue("@studyHrs", studyHrs);
                            updateCmd.Parameters.AddWithValue("@moduleCode", moduleCode);
                            updateCmd.Parameters.AddWithValue("@username", username);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

        

        string semAddQuery = "Insert Into Semester(Week,Username,Code,StudyHrs) VALUES (@week,@username,@code,@studyHrs)";


            if (int.TryParse(numWeeksTxt.Text,out int numWeeks1))
            {
                DbData.NumWeeks = numWeeks1;
                string dataRetrievalQuery = "SELECT * FROM Module WHERE Username = @username";
SqlCommand semCmd = new SqlCommand(dataRetrievalQuery, conn);
                semCmd.Parameters.AddWithValue("@username", DbData.Username);
                SqlDataReader reader= semCmd.ExecuteReader();
                while (reader.Read())
                {
                    int studyHrs = Convert.ToInt32(reader["StudyHrs"]);
                    string modCode = reader["Code"].ToString();
                    string username = reader["Username"].ToString();
                    using(SqlConnection updateConn = new SqlConnection(Connection.connectionString)) 
                    {
                        updateConn.Open();
                        
                        for (int i = 0; i < numWeeks1; i++)
                        {
                            SqlCommand cmd = new SqlCommand(semAddQuery, updateConn);
                            cmd.Parameters.AddWithValue("@studyHrs", studyHrs);
                            cmd.Parameters.AddWithValue("@week", (i + 1));
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@code", modCode);
                            cmd.ExecuteNonQuery();
                        }
                        updateConn.Close();
                    }
                    
                }
                conn.Close();



            }
            else { MessageBox.Show("Please enter a valid number of weeks for the semester", "Input error!", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            conn.Close();
            StudyHrsDisplay studyHrsDisplay = new StudyHrsDisplay();
            this.Hide();
            studyHrsDisplay.Show();
            this.Close();
        
        } 
    }
}


/* private void confirmBtn_Click(object sender, RoutedEventArgs e)
 {
     string usernameToUpdate = DbData.Username;
     DbData.NumWeeks = Convert.ToInt32(numWeeksTxt.Text);
     int credits, classHrs;

     conn.Open();

     string selectQuery = "SELECT Credits, ClassHrs, Username FROM Module WHERE Username = @username";
     SqlCommand selectCmd = new SqlCommand(selectQuery, conn);
     selectCmd.Parameters.AddWithValue("@username", usernameToUpdate);

     using (SqlDataReader reader = selectCmd.ExecuteReader())
     {
         while (reader.Read())
         {
             credits = Convert.ToInt32(reader["Credits"]);
             classHrs = Convert.ToInt32(reader["ClassHrs"]); 

             int studyHrs = (credits * 10) / DbData.NumWeeks - classHrs;

             using (SqlConnection updateConnection = new SqlConnection(Connection.connectionString))
             {
                 string updateQuery = "UPDATE Module SET StudyHrs = @studyHrs WHERE Username = @username";
                 updateConnection.Open();

                 using (SqlCommand updateCmd = new SqlCommand(updateQuery, updateConnection))
                 {
                     updateCmd.Parameters.AddWithValue("@studyHrs", studyHrs);
                     updateCmd.Parameters.AddWithValue("@username", usernameToUpdate);
                     updateCmd.ExecuteNonQuery();
                 }
                 updateConnection.Close();

             }
         }
     }
*/