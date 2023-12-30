using Fx_converter.Entities;
using Fx_converter.Models;
using Fx_converter.Utilities;
using Newtonsoft.Json;


namespace Fx_converter.Services.DataCollector
{
    public class DataCollector : IDataCollector
    {
        public DataCollector(IHttpClientFactory httpClient, FxDbContext context) {
            _httpClient = httpClient;
            _context = context;
        }
        private readonly IHttpClientFactory _httpClient;
        private readonly FxDbContext _context;
        public string EntryPointUrl { get; set; } = "https://data-api.ecb.europa.eu/service/data/EXR/D..EUR.SP00.A";
       
        public async Task<Observation> GetRates(DateTime startDate) {
            string startPeriod = string.Empty;
            string endPeriod = string.Empty;
            startDate = DateHelper.WeekDayCheckAndAdjust(startDate);

            startPeriod = startDate.ToString("yyyy-MM-dd");
            endPeriod = startDate.ToString("yyyy-MM-dd");
            using (var client = _httpClient.CreateClient()) {

                string url = $"{EntryPointUrl}?startPeriod={startPeriod}&endPeriod={endPeriod}&format=jsondata&detail=dataonly";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<CurrencyData>(result);
           
                Observation? observation = ObservationObjectBuilder(data);
                return observation;
            }
        }
        public async Task<IEnumerable<Observation>> GetRates(DateTime startDate, DateTime endDate) {
                // todo return range
                return [];
        }

        private Currency GetOrCreateCurrency(string symbol, List<Currency> currencies) {
			Currency? currency = currencies.FirstOrDefault(c => c.Symbol == symbol);
          
			if (currency == null) {
				return currency = new Currency() { Symbol = symbol };
			} else {
				return currency;
			}
        }
        private Observation ObservationObjectBuilder(CurrencyData data) {
            Observation observation = new Observation();
     
            int observations = data.Structure.Dimensions.Observation.Length;
            int dataSeries = data.DataSets[0].Series.Count;
			List<Currency> currencies = _context.Currencies.ToList();

			for (int i = 0; i<observations; i++) {
                for (int j = 0; j<dataSeries; j++) {
                    string symbol = data.Structure.Dimensions.Series[1].Values[j].Id;
					double rate = data.DataSets[0].Series[$"0:{j}:0:0:0"].Observations[i.ToString()][0];
                    CurrencyRate currencyRate = new CurrencyRate()
                    {
                        Currency = GetOrCreateCurrency(symbol, currencies),
                        Rate = rate, 
                    };
                    string isoDate = data.Structure.Dimensions.Observation[0].Values[i].Id.ToString("yyyy-MM-dd");
                    observation.Date = isoDate;
                    observation.CurrencyRates.Add(currencyRate);
				}
			}
            return observation;
        }
    }

}
