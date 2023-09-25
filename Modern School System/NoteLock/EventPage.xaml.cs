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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace NoteLock
{
    /// <summary>
    /// Interaction logic for EventPage.xaml
    /// </summary>
    public partial class EventPage : Window
    {


        public EventPage()
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


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Events.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Events.SelectedItem;
                Name.Text = selectedRow["Name"].ToString();
                Address.Text = selectedRow["Address"].ToString();
                NoHours.Text = selectedRow["NoHours"].ToString();
                Date.Text = selectedRow["Date"].ToString();
                Status.Text = selectedRow["Status"].ToString();
                ForWho.Text = selectedRow["ForWho"].ToString();
            }
            else
            {

                Name.Clear();
                Address.Clear();
                NoHours.Clear();
            }
        }
        private void UpdateEventsrInDatabase(int eventID, string name, string address, string noHours, string date, string status, string forWho)
        {
            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "UPDATE Events SET Name=@Name, Address=@Address, NoHours=@NoHours, Date=@Date, Status=@Status, ForWho=@ForWho WHERE EventID=@EventID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@NoHours", noHours);
            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@ForWho", forWho);
            command.Parameters.AddWithValue("@EventID", eventID);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                UpdateDonePage updateDonePage = new UpdateDonePage();
                updateDonePage.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to update Event.");
            }

        }
        public void LoadData()
        {

            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "SELECT * FROM Events";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            Events.ItemsSource = dataTable.DefaultView;
        }
        public void Insert(string name, string address, string noHours, string date, string status, string forWho)
        {
            if (Name.Text == "" || Address.Text == "" || Address.Text == "" || NoHours.Text == "")
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
                var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);

                string sqlQuery = $"INSERT INTO Events (Name, Address,NoHours,Date,Status,ForWho) Values (@name, @address,@noHours, @date,@status, @forWho);";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                sqlCommand.Parameters.AddWithValue("@name", name);
                sqlCommand.Parameters.AddWithValue("@address", address);
                sqlCommand.Parameters.AddWithValue("@noHours", noHours);
                sqlCommand.Parameters.AddWithValue("@date", date);
                sqlCommand.Parameters.AddWithValue("@status", status);
                sqlCommand.Parameters.AddWithValue("@forWho", forWho);


                sqlCommand.CommandType = CommandType.Text;

                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                LoadData();
            }
        }


        #endregion


        #region Buttons

        private void Addbtn(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string address = Address.Text;
            string noofhours = NoHours.Text;
            string date = Date.SelectedDate?.ToString("yyyy-MM-dd");
            string status = (Status.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string forwho = (ForWho.SelectedItem as ComboBoxItem)?.Content?.ToString();


            Insert(name, address, noofhours, date, status, forwho);
        }
        private void Updatebtn(object sender, RoutedEventArgs e)
        {
            if (Events.SelectedItem != null)
            {
                int eventID;
                DataRowView selectedRow = (DataRowView)Events.SelectedItem;
                eventID = (int)(selectedRow["EventID"]);
                string name = Name.Text;
                string address = Address.Text;
                string noofhours = NoHours.Text;
                string date = Date.SelectedDate?.ToString("yyyy-MM-dd");
                string status = (Status.SelectedItem as ComboBoxItem)?.Content?.ToString();
                string forwho = (ForWho.SelectedItem as ComboBoxItem)?.Content?.ToString();
                UpdateEventsrInDatabase(eventID, name, address, noofhours, date, status, forwho);
                LoadData(); // Reload data to refresh the DataGrid

            }
            else
            {
                this.IsEnabled = false;
                SelectanEvent SslectanEvent = new SelectanEvent();
                SslectanEvent.ShowDialog();
                if (SslectanEvent.OkButtonClicked)
                {
                    this.IsEnabled = true;
                }
            }

        }
        private void Deletebtn(object sender, RoutedEventArgs e)
        {
            if (Events.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Events.SelectedItem;
                int eventID = Convert.ToInt32(selectedRow["EventID"]);

                // Assuming that Students.ItemsSource is a DataView
                DataView teachersDataView = (DataView)Events.ItemsSource;

                // Find the row to delete based on the StudentID
                DataRow[] rowsToDelete = teachersDataView.Table.Select($"EventID = {eventID}");

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

                    string deleteQuery = "DELETE FROM Events WHERE EventID = @eventID";
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@EventID", eventID);
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
            Address.Text = "";
            NoHours.Text = "";


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
