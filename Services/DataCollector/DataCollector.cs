using Fx_converter.Models;
using Newtonsoft.Json;

namespace Fx_converter.Services.DataCollector
{
    public class DataCollector : IDataCollector
    {
        public string EntryPointUrl { get; set; } = "https://data-api.ecb.europa.eu/service/data/EXR/D..EUR.SP00.A";
        public Observation Observation { get; set; }
 

        public async Task<Observation> GetRates(DateTime startDate, DateTime? endDate) {
            DateTime startPeriod = new DateTime();
            DateTime endPeriod = new DateTime();
            startDate = this.WeekDayCheckAndAdjust(startDate);

            if (!endDate.HasValue) {
                endPeriod = startDate;
            }
            using (var client = new HttpClient()) {
                string url = $"{EntryPointUrl}?startPeriod={startPeriod}&endPeriod={endPeriod}&detail=dataonly";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                // interface/class for json data?
                //Observation observation = JsonConvert.DeserializeObject<Observation>(result);
                var json = JsonConvert.DeserializeObject(result);
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
            date = date.AddDays(-1);
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
