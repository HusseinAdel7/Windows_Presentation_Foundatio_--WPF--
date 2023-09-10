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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vendering_Machine_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static void clear()
        {
            Console.Clear();

        }
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Border For Windows Min Max Close Move
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        #endregion


        private void LatteBuy(object sender, RoutedEventArgs e)
        {
            Window1 secondaryWindow = new Window1();
            secondaryWindow.Show();  // Open the secondary window

            Close();
            Window1 win1 = new Window1();
            win1.textBox1InSecondary.Text = "Latte";
            VendingMachine vendingMachine = new VendingMachine();
            Product selectedProduct = new Latte();
            if (selectedProduct != null)
            {
                
                //selectedProduct.DisplayInfo();
                //vendingMachine.AcceptPayment(selectedProduct.Cost);
                //vendingMachine.ReleaseItem(selectedProduct);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
        private void BlackcoffreBuy(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private void CappacinoBuy(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private void TeaBuy(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

    }
}
