using Fx_converter.Entities;
using Fx_converter.Models;
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
        public Observation Observation { get; set; }
 
       
        public async Task<Observation> GetRates(DateTime startDate) {
            string startPeriod = string.Empty;
            string endPeriod = string.Empty;
            startDate = this.WeekDayCheckAndAdjust(startDate);

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

        public DateTime WeekDayCheckAndAdjust(DateTime date) {
    
            DayOfWeek dayOfWeek = date.DayOfWeek;
            if(this.AtLeastOneDayOlder(date)) {
                Console.WriteLine($"{date} is too recent(needs to be at least one day older), no data yet, adjusting...");
            }
            if (dayOfWeek == DayOfWeek.Sunday) {
                date = date.AddDays(-2);
            } else if (dayOfWeek == DayOfWeek.Saturday) {
                date = date.AddDays(-1);
            }
            return date;
        }
        public bool AtLeastOneDayOlder(DateTime date) {
            DateTime today = DateTime.Now;
            TimeSpan timeDiff = today - date;
            const int DAY_IN_MILLISECONDS = 24 * 60 * 60 * 1000;

            return timeDiff.TotalMilliseconds > DAY_IN_MILLISECONDS;
        }
        private Currency GetOrCreateCurrency(string symbol) {
			List<Currency> currencies = _context.Currencies.ToList();
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

            for (int i = 0; i<observations; i++) {
                for (int j = 0; j<dataSeries; j++) {
					// check needed, data not always uniform -> some observations/values missing mostly going over from 2022 to 2023
					// e.g. 2022-12-28 to 2023-01-05
                    // todo condition
                    string symbol = data.Structure.Dimensions.Series[1].Values[j].Id;
					double rate = data.DataSets[0].Series[$"0:{j}:0:0:0"].Observations[i.ToString()][0];
                    CurrencyRate currencyRate = new CurrencyRate()
                    {
                        Currency = GetOrCreateCurrency(symbol),
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
