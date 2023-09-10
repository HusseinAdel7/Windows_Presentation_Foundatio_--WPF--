namespace Vendering_Machine_GUI
{
    abstract class Product
    {
        public string Name { get; set; }
        public double Cost { get; set; }

        public abstract void DisplayInfo();
    }
}
