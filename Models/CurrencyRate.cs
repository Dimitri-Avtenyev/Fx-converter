namespace Fx_converter.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }
        public double Rate { get; set; }
        public int ObservationId { get; set; }
		public Observation Observation { get; set; }

	}

}
