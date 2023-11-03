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
using System.Windows.Navigation;

namespace PROG6212_POE
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        SqlConnection conn = new SqlConnection(Connection.connectionString);
        public SignUp()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
            this.Close();
        }


        private void signUpBtn_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            string checkQuery = "SELECT * FROM UserDetails WHERE username = @username";
            SqlCommand cmd = new SqlCommand(checkQuery, conn);
            cmd.Parameters.AddWithValue("@username", usernameTxt.Text);
            int userCount = Convert.ToInt32(cmd.ExecuteScalar());
            if (passwrdTxt.Text == passwrdCnfrmTxt.Text)
            {
                if (userCount < 1)
                {
                    string hashPassword = BCrypt.Net.BCrypt.HashPassword(passwrdTxt.Text);
                    string addQuery = $"INSERT INTO UserDetails (username, password) VALUES ('" + usernameTxt.Text + "','" + hashPassword + "')";
                    if (UsernameCheck(usernameTxt.Text))
                    {
                        SqlCommand addCmd = new SqlCommand(addQuery, conn);
                        try { addCmd.ExecuteNonQuery(); }
                        catch (Exception E)
                        { MessageBox.Show("Error adding details to database" + E.Message); }
                        conn.Close();
                        Login login = new Login();
                        this.Hide();
                        login.Show();
                        this.Close();
                    }
                    else
                    {
                        usernameTxt.Clear();
                        passwrdTxt.Clear();
                        passwrdCnfrmTxt.Clear();
                        
                    }
                   

                }
                else
                {
                    usernameTakenLbl.Visibility = Visibility.Visible;
                    usernameTxt.Clear();
                    passwrdTxt.Clear();
                    passwrdCnfrmTxt.Clear();
                    conn.Close();
                }

            }
            else
            {
                passwrdNoMatchLbl.Visibility = Visibility.Visible;
                passwrdTxt.Clear();
                passwrdCnfrmTxt.Clear();
                conn.Close();
            }
            conn.Close();
        }
        public bool UsernameCheck(string username)
        {
            using (SqlConnection conn = new SqlConnection(Connection.connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM UserDetails WHERE username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                MessageBox.Show("Username is taken, please try a different username");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("An error occurred while checking the username: " + e.Message);
                        return false;
                    }
                }
            }
        }

    }
}

