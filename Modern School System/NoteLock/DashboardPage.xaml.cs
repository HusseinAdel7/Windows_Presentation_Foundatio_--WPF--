using System;
using System.Collections.Generic;
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
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Window
    {


        static string GetTotalFees(string connectionString, string tableName, string columnName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Construct the SQL query to sum the values
                string query = $"SELECT SUM(CASE WHEN ISNUMERIC({columnName}) = 1 THEN CAST({columnName} AS DECIMAL(18, 2)) ELSE 0 END) FROM {tableName}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // ExecuteScalar returns the sum as an object
                    object result = command.ExecuteScalar();

                    // Convert the result to string and return
                    return result != DBNull.Value ? result.ToString() : "0";
                }
            }
        }

        static string GetRowCount(string connectionString, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM {tableName}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int rowCount = (int)command.ExecuteScalar();
                    return rowCount.ToString(); // Convert row count to string and return
                }
            }
        }

        public DashboardPage()
        {
            InitializeComponent();
            string connectionString = "Server=DESKTOP-B019U82\\SQLEXPRESS;Database=ModernSchool;Integrated Security=SSPI;TrustServerCertificate=True";
            string tableName1 = "Students"; 
            string rowCount1 = GetRowCount(connectionString, tableName1);
            Student.Content = rowCount1.ToString();
            string tableName2 = "Teachers";
            string rowCount2 = GetRowCount(connectionString, tableName2);
            Teacher.Content = rowCount2.ToString(); 
            string tableName3 = "Events";
            string rowCount3 = GetRowCount(connectionString, tableName3);
            Event.Content = rowCount3.ToString();





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



        private void Backbtn(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage();
            homePage.Show();
            this.Hide();    
        }



    }
}
