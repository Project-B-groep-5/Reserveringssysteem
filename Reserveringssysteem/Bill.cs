namespace Reserveringssysteem
{
    public class Bill
    {
        public double ToPay { get; set; }
        public Bill(double toPay)
        {
            ToPay = toPay;
        }
        private Bill() {}
    }
}