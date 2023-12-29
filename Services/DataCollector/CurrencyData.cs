namespace Fx_converter.Services.DataCollector
{

	public class CurrencyData
	{
		public Header Header { get; set; }
		public DataSet[] DataSets { get; set; }
		public Structure Structure { get; set; }
	}

	public  class DataSet
	{
		public string Action { get; set; }
		public DateTimeOffset ValidFrom { get; set; }
		public Dictionary<string, SeriesValue> Series { get; set; }
	}

	public  class SeriesValue
	{
		public Dictionary<string, double[]> Observations { get; set; }
	}

	public  class Header
	{
		public Guid Id { get; set; }
		public bool Test { get; set; }
		public DateTimeOffset Prepared { get; set; }
		public Sender Sender { get; set; }
	}

	public  class Sender
	{
		public string Id { get; set; }
	}

	public  class Structure
	{
		public Link[] Links { get; set; }
		public string Name { get; set; }
		public Dimensions Dimensions { get; set; }
	}

	public  class Dimensions
	{
		public SeriesElement[] Series { get; set; }
		public ObservationData[] Observation { get; set; }
	}

	public  class ObservationData
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
		public ObservationValue[] Values { get; set; }
	}

	public  class ObservationValue
	{
		public DateTimeOffset Id { get; set; }
		public DateTimeOffset Name { get; set; }
		public DateTimeOffset Start { get; set; }
		public DateTimeOffset End { get; set; }
	}

	public  class SeriesElement
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public SeriesValueClass[] Values { get; set; }
	}

	public  class SeriesValueClass
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}

	public  class Link
	{
		public string Title { get; set; }
		public string Rel { get; set; }
		public Uri Href { get; set; }
	}
}
