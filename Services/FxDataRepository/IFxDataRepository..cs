using Fx_converter.Models;

namespace Fx_converter
{
    public interface IFxDataRepository
    {
        Task<Observation> GetAsync(DateTime date);
        Task<CurrencyRate> GetAsyncCurrencyRate(DateTime date, string symbol);
		IEnumerable<Observation> GetAll();
        void Add(Observation observation);
        Task Update(Observation observation);
        void Remove(Observation observation);
	}
}
