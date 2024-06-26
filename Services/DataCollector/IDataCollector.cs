﻿using Fx_converter.Models;

namespace Fx_converter.Services.DataCollector
{
    public interface IDataCollector
    {
        public string EntryPointUrl { get; set; }
        public Task<Observation> GetRates(DateTime startDate);
        public Task<IEnumerable<Observation>> GetRates(DateTime startDate, DateTime endDate);
    }
}
