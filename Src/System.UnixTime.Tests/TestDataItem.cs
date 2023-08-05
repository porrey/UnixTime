namespace System.Tests
{
	public class TestDataItem
	{
		public double UnixTimestampDouble { get; set; }

		public DateTime DateTime
		{
			get
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(this.UnixTimestampDouble);
			}
		}

		public long UnixTimestampLong
		{
			get
			{
				return Convert.ToInt64(this.UnixTimestampDouble);
			}
		}

		public long UnixTimestampInt
		{
			get
			{
				return Convert.ToInt64(this.UnixTimestampDouble);
			}
		}
	}
}
