namespace Vendering_Machine_GUI
{
    interface IVendingMachine
    {
        void ShowProducts();
        Product SelectProduct(int choice);
        void AcceptPayment(double cost);
        void ReleaseItem(Product product);
    }
}
