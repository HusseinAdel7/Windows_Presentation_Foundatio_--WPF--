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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {


        public HomePage()
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



        private void StudentsPicturebtn(object sender, RoutedEventArgs e)
        {
            StudentPage studentpage = new StudentPage();
            studentpage.Show();
            this.Hide();    
        }
        private void TeachersPicturebtn(object sender, RoutedEventArgs e)
        {
            TeacherPage teacherpage = new TeacherPage();
            teacherpage.Show();
            this.Hide();
        }
        private void AttentancePicturebtn(object sender, RoutedEventArgs e)
        {
            AttentancePage attentancePage = new AttentancePage();
            attentancePage.Show();
            this.Hide();
        }
        private void EventPicturebtn(object sender, RoutedEventArgs e)
        {
            EventPage eventPage = new EventPage();
            eventPage.Show();
            this.Hide();
        }
        private void FeesPicturebtn(object sender, RoutedEventArgs e)
        {
            FeesPage feesPage = new FeesPage();
            feesPage.Show();
            this.Hide();
        }
        private void DashboardPicturebtn(object sender, RoutedEventArgs e)
        {
            DashboardPage dashboardPage = new DashboardPage();
            dashboardPage.Show();
            this.Hide();
        }



        private void StudentsTextbtn(object sender, RoutedEventArgs e)
        {
            StudentPage studentpage = new StudentPage();
            studentpage.Show();
            this.Hide();
        }
        private void TeachersTextbtn(object sender, RoutedEventArgs e)
        {
            TeacherPage teacherpage = new TeacherPage();
            teacherpage.Show();
            this.Hide();
        }
        private void AttentanceTextbtn(object sender, RoutedEventArgs e)
        {
            AttentancePage attentancePage = new AttentancePage();
            attentancePage.Show();
            this.Hide();
        }
        private void EventTextbtn(object sender, RoutedEventArgs e)
        {
            EventPage eventPage = new EventPage();
            eventPage.Show();
            this.Hide();
        }
        private void FeesTextbtn(object sender, RoutedEventArgs e)
        {
            FeesPage feesPage = new FeesPage();
            feesPage.Show();
            this.Hide();
        }
        private void DashboardTextbtn(object sender, RoutedEventArgs e)
        {
            DashboardPage dashboardPage = new DashboardPage();
            dashboardPage.Show();
            this.Hide();
        }



        private void CloseAppbtn(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Settingsbtn(object sender, RoutedEventArgs e)
        {
           
        }
        private void Profilebtn(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
