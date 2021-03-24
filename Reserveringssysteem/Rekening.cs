namespace Reserveringssysteem
{
    public class Rekening
    {
        public double TeBetalen { get; set; }
        public Rekening(double teBetalen)
        {
            TeBetalen = teBetalen;
        }
    }
}