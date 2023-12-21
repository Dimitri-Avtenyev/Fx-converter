using Fx_converter.Models;

namespace Fx_converter
{
    public interface IFxData
    {
        Observation Get(DateTime date);
        IEnumerable<Observation> GetAll();
        void Add(Observation observation);
        void Update(Observation observation);
        void Remove(Observation observation);
    }
}
