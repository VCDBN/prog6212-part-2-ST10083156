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
using BCrypt.Net;
using System.Data.SqlClient;

namespace PROG6212_POE
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public SqlConnection conn = new SqlConnection(Connection.connectionString);
        private bool passwordMatch;
        public string Username { get; set; }
        public string Password { get; set; }
        public Login()
        {
            InitializeComponent();
        }

        private bool UserCheck(string username, string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            conn.Open();

            string query = "SELECT password FROM UserDetails WHERE username = @username";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            string storedPasswordHash = cmd.ExecuteScalar() as string;

            conn.Close();

            if (!string.IsNullOrEmpty(storedPasswordHash))
            {
                return BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
            }
            return false;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            Username = usernameTxt.Text;
            Password = passwordTxt.Text;
            if (UserCheck(Username, Password))
            {
                MessageBox.Show("Login Successful!\nWelcome back!");
                DbData.Username = Username;

                conn.Open();
                string userCheckQuery = "SELECT * FROM Module Where Username = @username";
                SqlCommand checkCmd = new SqlCommand(userCheckQuery, conn);
                checkCmd.Parameters.AddWithValue("@username", Username);
                SqlDataReader modReader = checkCmd.ExecuteReader();

               
                if (modReader.HasRows) 
                {
                    modReader.Close();
                    string semQuery = "SELECT * FROM Semester Where Username = @username";
                    SqlCommand semCmd = new SqlCommand(semQuery, conn);
                    semCmd.Parameters.AddWithValue("@username", Username);
                    SqlDataReader semReader = semCmd.ExecuteReader();
                    if (semReader.HasRows) 
                    {
                        conn.Close();
                        StudyHrsDisplay studyHrsDisplay = new StudyHrsDisplay();
                        this.Hide();
                        studyHrsDisplay.Show();
                        this.Close();
                    }
                    else
                    {
                        conn.Close();
                        SemesterWindow semesterWindow = new SemesterWindow();
                        this.Hide();
                        semesterWindow.Show();
                        this.Close();
                    }
                    
                }
                else 
                {
                    conn.Close();

                    ModuleWindow moduleWindow = new ModuleWindow(); 
                    this.Hide();
                    moduleWindow.Show();
                    this.Close();
                }
                
            }
            else 
            {

                MessageBox.Show("User not found\nCheck username and password and try again");
                usernameTxt.Clear();
                passwordTxt.Clear();
            }
        }

        private void signUpBtn_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            this.Hide();
            signUp.Show();
            this.Close();

        }
    }
}
