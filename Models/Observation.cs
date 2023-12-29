namespace Fx_converter.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public string Date { get; set; } //ISO yyyy-MM-dd
        public List<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();
    }

}
