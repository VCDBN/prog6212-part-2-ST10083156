using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PROG6212_POE
{
    public class DbData
    {
        public static string ModCode { get; set; }
        public static int StudyHrs { get; set; }
        public static int Week { get; set; }
        public static string Username { get; set; }
        public static int NumWeeks { get; set; }
        public DbData() { }

        SqlConnection conn = new SqlConnection(Connection.connectionString);
        public DbData(string username, string modCode,int week) 
        {
            conn.Open();
            string query = $"SELECT * FROM Semester WHERE Username = @username AND Code = @code AND Week = @week";
            string weekQuery = "SELECT COUNT(*) FROM Semester WHERE Username = @username ";
            SqlCommand weekCmd = new SqlCommand(weekQuery, conn);
            weekCmd.Parameters.AddWithValue("@username", username);
            NumWeeks = (int)weekCmd.ExecuteScalar();
            SqlCommand cmd= new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@code", modCode);
            cmd.Parameters.AddWithValue("@week", week);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ModCode = reader["Code"].ToString();
                Username = reader["Username"].ToString();
                StudyHrs = Convert.ToInt32(reader["StudyHrs"]);
                Week = Convert.ToInt32(reader["Week"]);
            }

            conn.Close();
        }


    }
}
