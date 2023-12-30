namespace Fx_converter.Utilities
{
	public static class DateHelper
	{
		public static DateTime WeekDayCheckAndAdjust(DateTime date) {

			DayOfWeek dayOfWeek = date.DayOfWeek;

			if (!AtLeastOneDayOlder(date)) {
				Console.WriteLine($"{date} is too recent(needs to be at least one day older), no data yet, adjusting...");
				date = date.AddDays(-1);
			}
			if (dayOfWeek == DayOfWeek.Sunday) {
				date = date.AddDays(-2);
			} else if (dayOfWeek == DayOfWeek.Saturday) {
				date = date.AddDays(-1);
			}
			return date;
		}
		public static bool AtLeastOneDayOlder(DateTime date) {
			DateTime today = DateTime.Now;
			TimeSpan timeDiff = today - date;
			const int DAY_IN_MILLISECONDS = 24 * 60 * 60 * 1000;

			return timeDiff.TotalMilliseconds > DAY_IN_MILLISECONDS;
		}
	}
}
