using System.Data.SqlClient;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for ModuleWindow.xaml
    /// </summary>
    public partial class ModuleWindow : Window
    {
        //Varibales used to store user input for creation of Module objects and for use in further calculations
       public string modName { get; set; }
       public string modCode { get; set; }
 public int modCredits { get; set; }
        public int classHrs { get; set; }

        //initializer for page, also used to populate datagrid
        public ModuleWindow()
        {
            InitializeComponent();
            Module module = new Module();
            moduleDataGrid.Items.Clear();
            populateDataGrid();
            moduleDataGrid.IsReadOnly= true;
            moduleDataGrid.Items.Refresh();

        }
        
        //Add module button used to update datagrid and "Modules" list in Module class, first click of button also used to make "Done" button visible
        private void addModuleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(modNameTxt.Text)) { MessageBox.Show("Please fill in all the relevant fields before adding a module.", "Detail error", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            if (String.IsNullOrEmpty(modCodeTxt.Text)) { MessageBox.Show("Please fill in all the relevant fields before adding a module.", "Detail error", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            if (String.IsNullOrEmpty(modCreditsTxt.Text)) { MessageBox.Show("Please fill in all the relevant fields before adding a module.", "Detail error", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            if (String.IsNullOrEmpty(classHrsTxt.Text)) { MessageBox.Show("Please fill in all the relevant fields before adding a module.", "Detail error", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            modName = char.ToUpper(modNameTxt.Text[0]) + modNameTxt.Text.Substring(1);
            modCode = modCodeTxt.Text.ToUpper();
            modCredits = Convert.ToInt32(modCreditsTxt.Text);
            classHrs = Convert.ToInt32(classHrsTxt.Text);
            Module module = new Module(modName,modCode, modCredits,classHrs);
            Module.Modules.Add(module);
            module.AddToDatabase(module);

            moduleDataGrid.Items.Refresh();
            modNameTxt.Clear();
            modCodeTxt.Clear();
            modCreditsTxt.Clear();
            classHrsTxt.Clear();
            doneBtn.Visibility = Visibility.Visible;

        }

        //Used to transfer users to "Semester" window to enter details of the semester
        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            SemesterWindow semesterWindow = new SemesterWindow();
            this.Hide();
            semesterWindow.Show();
            this.Close();

        }

        public void populateDataGrid()
        {
            Module.UpdateDataTable();
            moduleDataGrid.ItemsSource = Module.ModuleData.DefaultView;
            
            
        }
    }
}
