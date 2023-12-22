namespace Fx_converter.Services.DataCollector
{

    // paste special -> as JSON
    public class CurrencyData
    {
        public HeaderData Header { get; set; }
        public DataSet[] DataSets { get; set; }
        public StructureData Structure { get; set; }
    }

    public class HeaderData
    {
        public string Id { get; set; }
        public bool Test { get; set; }
        public string Prepared { get; set; }
        public SenderData Sender { get; set; }
    }

    public class SenderData
    {
        public string Id { get; set; }
    }

    public class DataSet
    {
        public string Action { get; set; }
        public string ValidFrom { get; set; }
        public SeriesData Series { get; set; }
    }

    public class SeriesData
    {
        public Dictionary<string, ObservationData> Observations { get; set; }
    }

    public class ObservationData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public ValueData[] Values { get; set; }
    }

    public class ValueData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class StructureData
    {
        public LinkData[] Links { get; set; }
        public string Name { get; set; }
        public DimensionData Dimensions { get; set; }
    }

    public class LinkData
    {
        public string Title { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class DimensionData
    {
        public SeriesData[] Series { get; set; }
        public ObservationData[] Observation { get; set; }
    }

}

