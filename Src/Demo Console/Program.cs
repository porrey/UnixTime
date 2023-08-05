using System;

namespace Demo_Console
{
	class Program
	{
		static void Main(string[] args)
		{
			// ***
			// *** Display Unix Epoch
			// ***
			Console.WriteLine("Unix Epoch is {0:f}.", UnixTime.Epoch);

			// ***
			// *** Capture the current date and time
			// ***
			DateTime d1 = DateTime.Now;

			// ***************************
			// *** Converting to Unix Time
			// ***
			long u1 = d1.ToUnixTime();
			Console.WriteLine("{0:f} in Unix Time is {1}.", d1, u1);

			// ***
			// *** Or...
			// ***
			long l1 = UnixTime.FromDateTime(DateTime.Now);
			Console.WriteLine("{0:f} in Unix Time is {1}.", DateTime.Now, u1);

			// ***
			// *** Converting back to local date/time
			// ***
			DateTime d2 = UnixTime.ToLocalDateTime(u1);
			Console.WriteLine("Unix Time {0} is {1:f}.", u1, d2);

			// ***************************
			// *** Converting back to UTC date/time
			// ***
			DateTime d3 = UnixTime.ToUniversalDateTime(u1);
			Console.WriteLine("Unix Time {0} is {1:f}.", u1, d3);

			// ***************************
			// *** Using System.UnixTime as a type
			// ***
			UnixTime u2 = new UnixTime(DateTime.Now);
			Console.WriteLine("{0:f} in Unix Time is {1}.", u2.DateTime, u2);

			UnixTime u3 = new UnixTime(1204343210);
			Console.WriteLine("{0} in Unix Time is {1:f}.", u3, u3.DateTime);

			// ***************************
			// *** Use implicit conversions
			// ***
			DateTime d4 = u2;
			long l2 = u2;
			UnixTime u4 = 1204343210;
			UnixTime u5 = DateTime.Now;

			// ***************************
			// *** Overload operators
			// ***
			UnixTime u6 = u4 + u5;						// Add Two UnixTime objects
			UnixTime u7 = u4 + 102345;					// Add seconds to a UnixTime
			UnixTime u8 = u4 + DateTime.Now;			// Add a DateTime to a UnixTime
			UnixTime u9 = u4 + TimeSpan.FromDays(1);	// Add a TimeSpan to a UnixTime
			UnixTime u10 = TimeSpan.FromDays(1) + u4;	// Add a TimeSpan to a UnixTime

			// ***************************
			// *** Parse values
			// ***

			// ***
			// *** Initializes by a string date and time
			// ***
			UnixTime u11 = UnixTime.Parse("January 1, 2013 12:34 AM");

			// ***
			// *** Initializes to 4/27/1970 4:43:13 AM CST
			// ***
			UnixTime u12 = UnixTime.Parse("10039393");

			// ***
			// *** Initializes a Unix Time 123 days, 15 hours, 19 minutes
			// *** and 20 seconds after Unix Epoch
			// ***
			UnixTime u13 = UnixTime.Parse("123.15:19:20");

			// ***************************
			// *** Determine Max dates
			// ***

			// ***
			// *** 32-bit based time stamp
			// ***
			DateTime maxdate1 = UnixTime.ToUniversalDateTime(Int32.MaxValue);
			Console.WriteLine("Maximum date with 32-bit Unix times is {0:f}.", maxdate1);

			// ***
			// *** Maximum in library. The limit within the library is NOT based on a 64-bit value.
			// *** Using int64.MaxValue will throw an exception because it results in an out of
			// *** range date. The maximum can be determined as follows:
			// ***
			// *** Subtract 1 second from the maximum date
			// ***
			UnixTime utmax = DateTime.MaxValue.ToUniversalTime().Subtract(TimeSpan.FromSeconds(1));
			Console.WriteLine("Maximum date in this library is {0:f} local time (or {1:f} UTC) [Unix time = {2}].", utmax.DateTime, utmax.DateTime.ToUniversalTime(), utmax);
		}
	}
}
