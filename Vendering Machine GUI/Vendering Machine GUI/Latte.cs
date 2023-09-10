using System;
using System.Windows;

namespace Vendering_Machine_GUI
{
    class Latte : Product
    {
        public Latte()
        {
            Name = "Latte";
            Cost = 2.50;
        }

        public override void DisplayInfo()
        {
            MessageBox.Show($"You Wanna Buy {Name}\t        The Cost Is : {Cost}$");
         

        }
    }
}
