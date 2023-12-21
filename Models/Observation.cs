namespace Fx_converter.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Currency> Currency { get; set; }
        public int CurrencyId { get; set; }
    }
}
