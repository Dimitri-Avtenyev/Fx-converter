namespace Fx_converter.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<CurrencyRate> CurrencyRates { get; set; }
        public int CurrencyRateId { get; set; }
    }
}
