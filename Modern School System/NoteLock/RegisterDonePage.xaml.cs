using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegisterDonePage.xaml
    /// </summary>
    public partial class RegisterDonePage : Window
    {


        public RegisterDonePage()
        {
            InitializeComponent();
        }



        private void Loginbtn(object sender, RoutedEventArgs e)
        {
            LoginPage loginpage = new LoginPage();
            this.Hide();
            loginpage.Show();
        }


    }
}
