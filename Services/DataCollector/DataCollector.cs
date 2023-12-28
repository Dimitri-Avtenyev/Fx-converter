using Fx_converter.Models;
using Newtonsoft.Json;


namespace Fx_converter.Services.DataCollector
{
    public class DataCollector : IDataCollector
    {
        public DataCollector(IHttpClientFactory httpClient) {
            _httpClient = httpClient;
        }
        private readonly IHttpClientFactory _httpClient;
        public string EntryPointUrl { get; set; } = "https://data-api.ecb.europa.eu/service/data/EXR/D..EUR.SP00.A";
        public Observation Observation { get; set; }
 
       
        public async Task<Observation> GetRates(DateTime startDate) {
            string startPeriod = String.Empty;
            string endPeriod = String.Empty;
            startDate = this.WeekDayCheckAndAdjust(startDate);

            startPeriod = startDate.ToString("yyy-MM-dd");
            endPeriod = startDate.ToString("yyy-MM-dd");
            using (var client = _httpClient.CreateClient()) {

                string url = $"{EntryPointUrl}?startPeriod={startPeriod}&endPeriod={endPeriod}&format=jsondata&detail=dataonly";
            /*    var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                // interface/class for json data?
                //Observation observation = JsonConvert.DeserializeObject<Observation>(result);
                var json = JsonConvert.DeserializeObject<CurrencyData>(result);*/
                // dummy obj
                Observation observation = new Observation();
                observation.Date = new DateTime(2023, 12, 20);
                observation.CurrencyRates = new List<CurrencyRate> {
                    new CurrencyRate { 
                        Currency = new Currency { Symbol = "USD" }, 
                        Rate = 1.0944, },
                    };
                return observation;
            }
        }
        public async Task<Observation> GetRates(DateTime startDate, DateTime endDate) {
            string startPeriod = String.Empty;
            string endPeriod = String.Empty;
            startDate = this.WeekDayCheckAndAdjust(startDate);

            startPeriod = startDate.ToString("yyy-MM-dd");
            endPeriod = endDate.ToString("yyyy-MM-dd");
            using (var client = new HttpClient()) {

                string url = $"{EntryPointUrl}?startPeriod={startPeriod}&endPeriod={endPeriod}&format=jsondata&detail=dataonly";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                // interface/class for json data?
                //Observation observation = JsonConvert.DeserializeObject<Observation>(result);
                var json = JsonConvert.DeserializeObject<CurrencyData>(result);
                Console.WriteLine(json);
                Observation observation = null;
                return observation;
            }
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

    }

}
