using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
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
using System.Data.SqlClient;
using System.Data;
using System.Threading;

namespace NoteLock
{
    /// <summary>
    /// Interaction logic for RegisterationPage.xaml
    /// </summary>
    public partial class RegisterationPage : Window
    {

        public RegisterationPage()
        {
            InitializeComponent();
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




        private void Loginbtn(object sender, RoutedEventArgs e)
        {
            LoginPage loginpage = new LoginPage();
            this.Hide();
            loginpage.Show();
        }

        public  void Insert(string Fullname, string Username, string Email, string Password)
        {

            var stublishingConnection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SqlConnection connection = new SqlConnection(stublishingConnection.GetSection("ConnectionString").Value);

            string sqlQuery = $"INSERT INTO Users (Fullname, Username,Email,Password) Values (@Fullname, @Username,@Email, @Password);";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
            sqlCommand.Parameters.AddWithValue("@Fullname", Fullname);
            sqlCommand.Parameters.AddWithValue("@Username", Username);
            sqlCommand.Parameters.AddWithValue("@Email", Email);
            sqlCommand.Parameters.AddWithValue("@Password", Password);

            sqlCommand.CommandType = CommandType.Text;

            connection.Open();
            sqlCommand.ExecuteNonQuery();
            connection.Close();

            RegisterDonePage registerDonePage = new RegisterDonePage();
            this.Hide();
            registerDonePage.Show();
        }
        private void SignUpbtn(object sender, RoutedEventArgs e)
        {
            string fullnamee = FullName.Text;
            string usernamee = Username.Text;
            string emaill = Email.Text;
            string passwordd = Password.Password;
            string passwordConfirmm = PasswordConfirm.Password;
            Validation obj = new Validation();
            string fname = obj.FullNameError(fullnamee);
            FullNameError.Content = fname.ToString();
            string usernameer = obj.FullNameError(usernamee);
            UsernameError.Content = usernameer.ToString();
            string emailer = obj.EmailError(emaill);
            EmailError.Content = emailer.ToString();
            string passworder = obj.passwoedError(passwordd);
            PasswordError.Content = passworder.ToString();
            string passwordconfer = obj.passwoedConfirmationError(passwordd, passwordConfirmm);
            PasswordConfirmError.Content = passwordconfer.ToString();
            if ( string.IsNullOrEmpty(FullNameError.Content?.ToString()) && 
                 string.IsNullOrEmpty(UsernameError.Content?.ToString()) && 
                 string.IsNullOrEmpty(EmailError.Content?.ToString()) &&
                 string.IsNullOrEmpty(PasswordError.Content?.ToString()) &&
                 string.IsNullOrEmpty(PasswordConfirmError.Content?.ToString()))
            {
                Insert(fullnamee, usernamee, emaill, passwordd);
            }
        }



        private void FullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FullName.Text == string.Empty)
            {
                FullNameError.Content = "";
            }
        }
        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Username.Text == string.Empty)
            {
                UsernameError.Content = "";
            }
        }
        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Email.Text == string.Empty)
            {
                EmailError.Content = "";
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password == string.Empty)
            {
                PasswordError.Content = "";
            }
        }
        private void PasswordBox_PasswordChangedcon(object sender, RoutedEventArgs e)
        {
            if (PasswordConfirm.Password == string.Empty)
            {
                PasswordConfirmError.Content = "";
            }
        }

        
    }
}
