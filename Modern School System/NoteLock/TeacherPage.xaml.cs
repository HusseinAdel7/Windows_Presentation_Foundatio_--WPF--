using Microsoft.Extensions.Configuration;
using NoteLock.Entities;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Window
    {
        private string connectionString = "Server=DESKTOP-B019U82\\SQLEXPRESS;Database=ModernSchool;Integrated Security=SSPI;TrustServerCertificate=True";


        public TeacherPage()
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

            string query = "SELECT COUNT(*) FROM Teachers WHERE Email = @InputData";

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
            if (Teachers.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Teachers.SelectedItem;
                Name.Text = selectedRow["Name"].ToString();
                Email.Text = selectedRow["Email"].ToString();
                Address.Text = selectedRow["Address"].ToString();
                Phone.Text = selectedRow["Phone"].ToString();
                Gender.Text = selectedRow["Gender"].ToString();
                Class.Text = selectedRow["Class"].ToString();
            }
            else
            {

                Name.Clear();
                Email.Clear();
                Address.Clear();
                Phone.Clear();
            }
        }
        private void UpdateTeacherInDatabase(int teacherID, string name, string email, string address, string phone, string gender, string teacherClass)
        {
            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "UPDATE Teachers SET Name=@Name, Email=@Email, Address=@Address, Phone=@Phone, Gender=@Gender, Class=@Class WHERE TeacherID=@TeacherID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Gender", gender);
            command.Parameters.AddWithValue("@Class", teacherClass);
            command.Parameters.AddWithValue("@TeacherID", teacherID);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                UpdateDonePage updateDonePage = new UpdateDonePage();
                updateDonePage.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to update Teacher.");
            }

        }
        public void LoadData()
        {

            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "SELECT * FROM Teachers";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            Teachers.ItemsSource = dataTable.DefaultView;
        }
        public void Insert(string name, string email, string address, string phone, string gender, string classs)
        {
            if (Name.Text == "" || Email.Text == "" || Address.Text == "" || Phone.Text == "")
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

                    string sqlQuery = $"INSERT INTO Teachers (Name, Email,Address,Phone,Gender,Class) Values (@name, @email,@address, @phone,@gender, @classs);";
                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    sqlCommand.Parameters.AddWithValue("@address", address);
                    sqlCommand.Parameters.AddWithValue("@phone", phone);
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
            string phone = Phone.Text;
            string gender = (Gender.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string classs = (Class.SelectedItem as ComboBoxItem)?.Content?.ToString();


            Insert(name, email, address, phone, gender, classs);
        }
        private void Updatebtn(object sender, RoutedEventArgs e)
        {
            if (Teachers.SelectedItem != null)
            {
                int teacherID;
                DataRowView selectedRow = (DataRowView)Teachers.SelectedItem;
                teacherID = (int)(selectedRow["TeacherID"]);
                string name = Name.Text;
                string email = Email.Text;
                string address = Address.Text;
                string phone = Phone.Text;
                string gender = Gender.Text;
                string teacherClass = Class.Text;

                UpdateTeacherInDatabase(teacherID, name, email, address, phone, gender, teacherClass);
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
            if (Teachers.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Teachers.SelectedItem;
                int teacherID = Convert.ToInt32(selectedRow["TeacherID"]);

                // Assuming that Students.ItemsSource is a DataView
                DataView teachersDataView = (DataView) Teachers.ItemsSource;

                // Find the row to delete based on the StudentID
                DataRow[] rowsToDelete = teachersDataView.Table.Select($"TeacherID = {teacherID}");

                if (rowsToDelete.Length > 0)
                {
                    foreach (DataRow row in rowsToDelete)
                    {
                        // Step 1: Remove the row from the DataView
                        teachersDataView.Table.Rows.Remove(row);
                    }

                    var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
                    connection.Open();

                    string deleteQuery = "DELETE FROM Teachers WHERE TeacherID = @teacherID";
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TeacherID", teacherID);
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
            Phone.Text = "";


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
