using Fx_converter.Entities;
using Fx_converter.Models;
using Fx_converter.Services.DataCollector;

namespace Fx_converter
{
    public class FxDataRepository : IFxDataRepository
    {
        public FxDataRepository(FxDbContext context, IDataCollector dataCollector) {
            _context = context;
            _dataCollector = dataCollector;
        }
		private readonly FxDbContext _context;
        private readonly IDataCollector _dataCollector;
		public void Add(Observation observation) {
			// logic to handle before adding -> unique currencies and observation dates -> index with constraint
			// msg 2601 for duplicates
			_context.Observations.Add(observation);
            _context.SaveChanges();
        }
        public async Task<Observation> GetAsync(DateTime date) {
            var observation = _context.Observations.FirstOrDefault(x => x.Date == date);
			if (observation == null) {
				observation =  await _dataCollector.GetRates(date);
                Add(observation);
            }
            return observation;
        }

        public IEnumerable<Observation> GetAll() {
            return _context.Observations;
        }

        public void Remove(Observation observation) {
            _context.Remove(observation);
            _context.SaveChanges();
        }

        public async Task Update(Observation observation) {
            Observation updatedObservation = await GetAsync(observation.Date);
            if (updatedObservation != null) {
                observation.Date = updatedObservation.Date; 
                observation.CurrencyRates = updatedObservation.CurrencyRates;
            }
            _context.SaveChanges();
        }
	}
}