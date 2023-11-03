using Azure.Identity;
using System.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG6212_POE
{
    public class Module
    {
        //variables used to create properties for "Module" objects
        public static string Name { get; set; }
        public static string Code { get; set; }
        public static int Credits { get; set; }
        public static int ClassHrs { get; set; }
        public static DataTable ModuleData { get; set; }
        public static List<Module> Modules { get; set; }

        SqlConnection conn = new SqlConnection(Connection.connectionString);

        //empty constructor used to initialize static Modules List and DataTable
        public Module() {
           Modules = new List<Module>();
            ModuleData = new DataTable(); }

        //overloaded constructor used to create modules objects
        public Module(string name, string code, int credits, int classHrs)
        {
            Name = name;
            Code = code;
            Credits = credits;
            ClassHrs = classHrs;
            Login login = new Login();
            string username = login.Username;
           


        }

        public void AddToDatabase(Module module)
        {
            conn.Open();
            string dataQuery = "INSERT INTO Module(Code, Name, Credits, ClassHrs, Username) VALUES (@code, @name, @credits, @classHrs, @username)";
            SqlCommand dataCmd = new SqlCommand(dataQuery, conn);
            dataCmd.Parameters.AddWithValue("@code", Module.Code);
            dataCmd.Parameters.AddWithValue("@name", Module.Name);
            dataCmd.Parameters.AddWithValue("@credits", Module.Credits);
            dataCmd.Parameters.AddWithValue("@classHrs", Module.ClassHrs);
            dataCmd.Parameters.AddWithValue("@username", DbData.Username);
            dataCmd.ExecuteNonQuery();
            conn.Close();

        }

        public static void UpdateDataTable()
        {
            SqlConnection conn = new SqlConnection(Connection.connectionString);
            string query = "SELECT Code, Name, Credits,ClassHrs FROM Module Where Username = @username";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", DbData.Username);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ModuleData);
          

        }
    } }

