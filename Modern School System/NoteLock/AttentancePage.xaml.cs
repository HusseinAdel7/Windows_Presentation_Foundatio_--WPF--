using Microsoft.Extensions.Configuration;
using NoteLock.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace NoteLock
{
    /// <summary>
    /// Interaction logic for AttentancePage.xaml
    /// </summary>
    public partial class AttentancePage : Window
    {
        string stdClss;
        int selectedIdd;

        public AttentancePage()
        {
            InitializeComponent();
            LoadIdData();
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


        public void LoadData()
        {

            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "SELECT * FROM Attentances";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            Attentance.ItemsSource = dataTable.DefaultView;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Attentance.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Attentance.SelectedItem;
                Name.Text = selectedRow["StudentName"].ToString();


            }
            else
            {

                Name.Clear();

            }
        }
        private void LoadIdData()
        {
            try
            {
                var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string connectionString = stublishingConnection.GetSection("ConnectionString").Value;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT StudentID, Name, Class FROM Students"; // Replace YourTable with your actual table name
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    Dictionary<int, Tuple<string, string>> idDataDictionary = new Dictionary<int, Tuple<string, string>>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string studentClass = reader.GetString(2);
                        idDataDictionary[id] = new Tuple<string, string>(name, studentClass);
                    }

                    reader.Close();

                    // Bind the dictionary of ID and Name to the ComboBox
                    Id.ItemsSource = idDataDictionary.Keys;

                    // Store the dictionary for later use
                    Id.Tag = idDataDictionary;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Id.SelectedItem != null && Id.Tag is Dictionary<int, Tuple<string, string>> idDataDictionary)
            {
                int selectedId = (int)Id.SelectedItem;
                selectedIdd = (int)Id.SelectedItem;

                if (idDataDictionary.TryGetValue(selectedId, out Tuple<string, string> selectedData))
                {
                    Name.Text = selectedData.Item1; // Display Name
                    stdClss = selectedData.Item2; // Display Class
                }
                else
                {
                    Name.Clear();

                }
            }
        }
        private void UpdateAttentanceDatabase(int studentID, string status)
        {
            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "UPDATE Attentances SET Status=@Status WHERE StudentID=@StudentID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@StudentID", studentID);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                UpdateDonePage updateDonePage = new UpdateDonePage();
                updateDonePage.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to update Attentance.");
            }

        }
        public void Insert(int id, string name, string classs, string status)
        {
            if (Name.Text == "")
            {

                this.IsEnabled = false;
                SelectPersonPage selectPersonPage = new SelectPersonPage();
                selectPersonPage.ShowDialog();
                if (selectPersonPage.OkButtonClicked)
                {
                    this.IsEnabled = true;
                }
            }

            else
            {
                var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);

                string sqlQuery = $"INSERT INTO Attentances (StudentID,StudentName, Class,Status) Values (@studentId,@name, @classs,@status);";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                sqlCommand.Parameters.AddWithValue("@studentId", id);
                sqlCommand.Parameters.AddWithValue("@name", name);
                sqlCommand.Parameters.AddWithValue("@status", status);
                sqlCommand.Parameters.AddWithValue("@classs", classs);


                sqlCommand.CommandType = CommandType.Text;

                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                LoadIdData();
                LoadData();
            }
        }


        #endregion




        #region Buttons


        private void Addbtn(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string studentclass = stdClss;

            string status = (Status.SelectedItem as ComboBoxItem)?.Content?.ToString();
            Insert(selectedIdd, name, studentclass, status);
        }
        private void Updatebtn(object sender, RoutedEventArgs e)
        {
            if (Attentance.SelectedItem != null)
            {
                int studentID;
                DataRowView selectedRow = (DataRowView)Attentance.SelectedItem;
                studentID = (int)(selectedRow["StudentID"]);
                string name = Name.Text;
                string status = (Status.SelectedItem as ComboBoxItem)?.Content?.ToString();

                UpdateAttentanceDatabase(studentID, status);
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
            if (Attentance.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Attentance.SelectedItem;
                int studentID = Convert.ToInt32(selectedRow["StudentID"]);

                DataView teachersDataView = (DataView)Attentance.ItemsSource;

                // Find the row to delete based on the StudentID
                DataRow[] rowsToDelete = teachersDataView.Table.Select($"StudentID = {studentID}");

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

                    string deleteQuery = "DELETE FROM Attentances WHERE StudentID = @studentID";
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@studentID", studentID);
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
