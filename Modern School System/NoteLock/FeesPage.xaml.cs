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
    /// Interaction logic for FeesPage.xaml
    /// </summary>
    public partial class FeesPage : Window
    {
        string mount;
        int selectedIdd;
        


        public FeesPage()
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


        public void Insert(int id, string name, string date,string status , string fees) 
        {
            if (Name.Text == "" || Mount.Text=="")
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

                int number1 = int.Parse(fees);
                string mounttt = Mount.Text;
                int number2 = int.Parse(mounttt);
                
                if (number1 > number2)
                {
                    this.IsEnabled = false;
                    payingMoreThanPage payingmoreThanPage = new payingMoreThanPage();
                    payingmoreThanPage.ShowDialog();
                    if (payingmoreThanPage.OkButtonClicked)
                    {
                        this.IsEnabled = true;
                    }
                }
                else
                {
                    int res = number2 - number1;
                    string result = res.ToString();

                    string sqlQuery = $"INSERT INTO Fees (StudentID,StudentName, Mount,Date,Status,Fees) Values (@studentId,@studentName, @mount,@date,@status,@fees);";
                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                    sqlCommand.Parameters.AddWithValue("@studentId", id);
                    sqlCommand.Parameters.AddWithValue("@studentName", name);
                    sqlCommand.Parameters.AddWithValue("@mount", result);
                    sqlCommand.Parameters.AddWithValue("@date", date);
                    sqlCommand.Parameters.AddWithValue("@status", status);
                    sqlCommand.Parameters.AddWithValue("@fees", fees);



                    sqlCommand.CommandType = CommandType.Text;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    LoadIdData();
                    LoadData();

                }


            
            }
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Feess.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)Feess.SelectedItem;
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

                    string query = "SELECT StudentID, Name, Fees FROM Students"; // Replace YourTable with your actual table name
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    Dictionary<int, Tuple<string, string>> idDataDictionary = new Dictionary<int, Tuple<string, string>>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string stdfees = reader.GetString(2);
                        idDataDictionary[id] = new Tuple<string, string>(name, stdfees);
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
                    Mount.Text = selectedData.Item2; // Display Class
                }
                else
                {
                    Name.Clear();
                    Mount.Clear();


                }
            }
        }
        private void UpdateFeesDatabase(int studentID, string date, string status, string fees)
        {
            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            int number1 = int.Parse(fees);
            string mounttt = Mount.Text;
            int number2 = int.Parse(mounttt);


            if (number1 > number2)
            {
                
            }
            else
            {
                int res = number2 - number1;
                string result = res.ToString();

                connection.Open();
                string query = "UPDATE Students SET Fees=@fees WHERE StudentID=@StudentID";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@fees", result);
                command.Parameters.AddWithValue("@StudentID", studentID);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    this.IsEnabled = false;
                    DoneCheckTheFees doneCheckTheFees = new DoneCheckTheFees();
                    doneCheckTheFees.ShowDialog();
                    if (doneCheckTheFees.OkButtonClicked)
                    {
                        this.IsEnabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Failed ");
                }

            }
           

        }
        public void LoadData()
        {

            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);
            connection.Open();
            string query = "SELECT * FROM Fees";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            Feess.ItemsSource = dataTable.DefaultView;
        }


        #endregion


        #region Button
        private void PayFeesbtn(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string date = Date.SelectedDate?.ToString("yyyy-MM-dd");
            string status = (Status.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string fee = Fees.Text;
            Insert(selectedIdd, name, date, status, fee);
            UpdateFeesDatabase(selectedIdd, date, status, fee);

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
