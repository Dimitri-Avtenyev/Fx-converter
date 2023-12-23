using Fx_converter.Entities;
using Fx_converter.Models;

namespace Fx_converter
{
    public class FxDataRepository : IFxDataRepository
    {
        private readonly FxDbContext _context;
        public FxDataRepository(FxDbContext context) {
            _context = context;
        }
        public void Add(Observation observation) {
            _context.Add(observation);
            _context.SaveChanges();
        }

        public Observation Get(DateTime date) {
            // if obs is null -> fetch + add and return
            return _context.Observations.FirstOrDefault(x => x.Date == date);
        }

        public IEnumerable<Observation> GetAll() {
            return _context.Observations;
        }

        public void Remove(Observation observation) {
            _context.Remove(observation);
            _context.SaveChanges();
        }

        public void Update(Observation observation) {
            Observation updatedObservation = this.Get(observation.Date);
            if (updatedObservation != null) {
                observation.Date = updatedObservation.Date; 
                observation.CurrencyRates = updatedObservation.CurrencyRates;
            }
            _context.SaveChanges();
        }
    }
}