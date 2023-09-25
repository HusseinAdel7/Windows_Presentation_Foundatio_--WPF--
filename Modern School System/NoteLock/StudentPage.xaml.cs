using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace NoteLock
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Window
    {
        private string connectionString = "Server=DESKTOP-B019U82\\SQLEXPRESS;Database=ModernSchool;Integrated Security=SSPI;TrustServerCertificate=True";
        public StudentPage()
        {
            InitializeComponent();
            LoadData();
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);

            if (currentWindow != null)
            {
                currentWindow.WindowState = WindowState.Minimized;
            }
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }




        #region Functions

        private bool CheckDataExists(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                return false; 
            }

            string query = "SELECT COUNT(*) FROM Students WHERE Email = @InputData"; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InputData", inputData);

                    int rowCount = (int)command.ExecuteScalar();

                    return rowCount > 0; 
                }
            }
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Students.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Students.SelectedItem;
                Name.Text = selectedRow["Name"].ToString();
                Email.Text = selectedRow["Email"].ToString();
                Address.Text = selectedRow["Address"].ToString();
                Fees.Text = selectedRow["Fees"].ToString();
                Gender.Text = selectedRow["Gender"].ToString();
                Class.Text = selectedRow["Class"].ToString();
            }
            else
            {

                Name.Clear();
                Email.Clear();
                Address.Clear();
                Fees.Clear();
            }
        }
        private void UpdateStudentInDatabase(int studentID, string name, string email, string address, string fees, string gender, string studentClass)
        {
            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "UPDATE Students SET Name=@Name, Email=@Email, Address=@Address, Fees=@Fees, Gender=@Gender, Class=@Class WHERE StudentID=@StudentID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@Fees", fees);
            command.Parameters.AddWithValue("@Gender", gender);
            command.Parameters.AddWithValue("@Class", studentClass);
            command.Parameters.AddWithValue("@StudentID", studentID);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                UpdateDonePage updateDonePage = new UpdateDonePage();
                updateDonePage.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to update student.");
            }

        }
        public void LoadData()
        {

            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "SELECT * FROM Students";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            Students.ItemsSource = dataTable.DefaultView;
        }
        public void Insert(string name, string email, string address, string fees, string gender, string classs)
        {
            if (Name.Text == "" || Email.Text == "" || Address.Text == "" || Fees.Text == "")
            {
                 
                this.IsEnabled = false;
                DataFieldsPage dataFieldsPage = new DataFieldsPage();
                dataFieldsPage.ShowDialog();
                if (dataFieldsPage.OkButtonClicked)
                {
                    this.IsEnabled = true;
                }

            }

            else
            {
                string inputData = Email.Text.Trim(); 

                bool dataExists = CheckDataExists(inputData);
                if (dataExists)
                {

                    this.IsEnabled = false;
                    EmailExist emailExist = new EmailExist();
                    emailExist.ShowDialog();
                    if (emailExist.OkButtonClicked)
                    {
                        this.IsEnabled = true;
                    }
                }
                else
                {
                    var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);

                    string sqlQuery = $"INSERT INTO Students (Name, Email,Address,Fees,Gender,Class) Values (@name, @email,@address, @fees,@gender, @classs);";
                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    sqlCommand.Parameters.AddWithValue("@address", address);
                    sqlCommand.Parameters.AddWithValue("@fees", fees);
                    sqlCommand.Parameters.AddWithValue("@gender", gender);
                    sqlCommand.Parameters.AddWithValue("@classs", classs);


                    sqlCommand.CommandType = CommandType.Text;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    LoadData();
                }
            }
        }


        #endregion



        #region Buttons


        private void Addbtn(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string email = Email.Text;
            string address = Address.Text;
            string fees = Fees.Text;
            string gender = (Gender.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string classs = (Class.SelectedItem as ComboBoxItem)?.Content?.ToString();


            Insert(name, email, address, fees, gender, classs);
        }
        private void Updatebtn(object sender, RoutedEventArgs e)
        {
            if (Students.SelectedItem != null)
            {
                int studentID;
                DataRowView selectedRow = (DataRowView)Students.SelectedItem;
                studentID = (int)(selectedRow["StudentID"]);
                string name = Name.Text;
                string email = Email.Text;
                string address = Address.Text;
                string fees = Fees.Text;
                string gender = Gender.Text;
                string studentClass = Class.Text;

                UpdateStudentInDatabase(studentID, name, email, address, fees, gender, studentClass);
                LoadData(); // Reload data to refresh the DataGrid

            }
            else
            {
                this.IsEnabled = false;
                SelectPersonPage selectPersonPage = new SelectPersonPage();
                selectPersonPage.ShowDialog();
                if (selectPersonPage.OkButtonClicked)
                {
                    this.IsEnabled = true;
                }
            }

        }
        private void Deletebtn(object sender, RoutedEventArgs e)
        {
            if (Students.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Students.SelectedItem;
                int studentID = Convert.ToInt32(selectedRow["StudentID"]);

                // Assuming that Students.ItemsSource is a DataView
                DataView studentsDataView = (DataView)Students.ItemsSource;

                // Find the row to delete based on the StudentID
                DataRow[] rowsToDelete = studentsDataView.Table.Select($"StudentID = {studentID}");

                if (rowsToDelete.Length > 0)
                {
                    foreach (DataRow row in rowsToDelete)
                    {
                        // Step 1: Remove the row from the DataView
                        studentsDataView.Table.Rows.Remove(row);
                    }

                    var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
                    connection.Open();

                    string deleteQuery = "DELETE FROM Students WHERE StudentID = @StudentID";
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            this.IsEnabled = false;
                            DeleteDonePage deleteDonePage = new DeleteDonePage();
                            deleteDonePage.ShowDialog();
                            if (deleteDonePage.OkButtonClicked)
                            {
                                this.IsEnabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete row from the database.");
                        }
                    }
                }
            }
        }
        private void Resetbtn(object sender, RoutedEventArgs e)
        {
            Name.Text = "";
            Email.Text = "";
            Address.Text = "";
            Fees.Text = "";


        }


        #endregion



        private void Backbtn(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage(); 
            homePage.Show();
            this.Hide();
        }
        private void Back2btn(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage();
            homePage.Show();
            this.Hide();
        }
    }
}
