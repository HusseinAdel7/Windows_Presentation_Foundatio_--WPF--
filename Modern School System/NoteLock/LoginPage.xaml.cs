using Microsoft.Extensions.Configuration;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
         bool resFromAdmin = false;
        bool resFromUser = false;
        public LoginPage()
        {
            InitializeComponent();
        }

        

        private void Grid_MouseDownn(object sender, MouseButtonEventArgs e)
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


        public bool SelectFromAdemin(string UserName, string Password)
        {
            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);


            var sqlquery = "SELECT * FROM Admins WHERE AdminName=@username AND Password=@password";
            SqlCommand sqlCommand = new SqlCommand(sqlquery, connection);
            sqlCommand.Parameters.AddWithValue("@username", UserName);
            sqlCommand.Parameters.AddWithValue("@password", Password);

            sqlCommand.CommandType = CommandType.Text;


            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows == true)
            {
                resFromAdmin = true;
            }
            else
            {
                resFromAdmin = false;
            }
            connection.Close();
            return resFromAdmin;
        }
        public bool SelectFromUsers(string UserName, string Password)
        {
            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);


            var sqlquery = "SELECT * FROM Users WHERE UserName=@username AND Password=@password";
            SqlCommand sqlCommand = new SqlCommand(sqlquery, connection);
            sqlCommand.Parameters.AddWithValue("@username", UserName);
            sqlCommand.Parameters.AddWithValue("@password", Password);

            sqlCommand.CommandType = CommandType.Text;


            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows == true)
            {
                resFromUser = true;
            }
            else
            {
                resFromUser = false;
            }
            connection.Close();
            return resFromUser;
        }
        private void Loginbtn(object sender, RoutedEventArgs e)
        {
            string usernamee = Username.Text;
            string passwordd = Password.Password;


            ComboBoxItem selectedItem = (ComboBoxItem)Role.SelectedItem;

            if (selectedItem != null)
            {

                string selectedRole = selectedItem.Content.ToString();
                if (selectedRole == "User")
                {
                    bool res = SelectFromUsers(usernamee, passwordd);
                    if (res == true)
                    {
                        HomePage homePage = new HomePage();
                        this.Hide();
                        homePage.Show();
                    }
                    else
                    {
                        Error.Content = "Username or Password Not Valid";
                    }
                }
                else if (selectedRole == "Admin")
                {
                    bool res = SelectFromAdemin(usernamee, passwordd);
                    if (res == true)
                    {
                        HomePage homePage = new HomePage();
                        this.Hide();
                        homePage.Show();
                    }
                    else
                    {
                        Error.Content = "Username or Password Not Valid";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid role selected");
                }
            }
        }
        private void SingUpbtn(object sender, RoutedEventArgs e)
        {
            RegisterationPage registerationPage = new RegisterationPage();
            this.Hide();
            registerationPage.Show();
        }
        

    }
}
