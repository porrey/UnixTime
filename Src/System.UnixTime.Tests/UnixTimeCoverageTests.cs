//
// Copyright(C) 2014-2026, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System.Globalization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace System.Tests
{
	/// <summary>
	/// Additional tests targeting code paths not exercised by UnixTimeTests,
	/// bringing line and branch coverage to near 100 %.
	/// </summary>
	[TestFixture]
	public class UnixTimeCoverageTests
	{
		// Wrapper used by Newtonsoft.Json converter tests.
		private class TimestampModel
		{
			[JsonConverter(typeof(UnixTimeJsonConverter))]
			public UnixTime Timestamp { get; set; }
		}

		#region DateTimeExtensions Tests
		[Test]
		public void ToUnixTimeExtensionUtcTest()
		{
			DateTime utc = new DateTime(2023, 11, 14, 22, 13, 20, DateTimeKind.Utc);
			long result = utc.ToUnixTime();
			Assert.That(result, Is.EqualTo(1_700_000_000L));
		}

		[Test]
		public void ToUnixTimeExtensionLocalTest()
		{
			// Round-trip: convert from UnixTime to local DateTime then back.
			UnixTime original = new(1_700_000_000L);
			DateTime local = original.DateTime;                // local-time DateTime
			long roundTripped = local.ToUnixTime();
			Assert.That(roundTripped, Is.EqualTo(1_700_000_000L));
		}

		[Test]
		public void ToUnixTimeExtensionUnspecifiedKindThrowsTest()
		{
			DateTime unspecified = new(2023, 11, 14, 22, 13, 20); // Kind == Unspecified
			Assert.That(() => unspecified.ToUnixTime(), Throws.TypeOf<ArgumentException>());
		}
		#endregion

		#region Timestamp Setter Test
		[Test]
		public void TimestampSetterTest()
		{
			UnixTime target = new(0D);
			target.Timestamp = 1_700_000_000D;
			Assert.That(target.Timestamp, Is.EqualTo(1_700_000_000D));
		}
		#endregion

		#region TimeSpan → UnixTime Implicit Conversion Test
		[Test]
		public void ImplicitTimeSpanToUnixTimeTest()
		{
			TimeSpan span = TimeSpan.FromSeconds(86400);
			UnixTime target = span;  // implicit operator UnixTime(TimeSpan)
			Assert.That(target.Timestamp, Is.EqualTo(86400D));
		}

		[Test]
		public void ImplicitTimeSpanToUnixTimeRoundTripTest()
		{
			double seconds = 123456789D;
			TimeSpan span = TimeSpan.FromSeconds(seconds);
			UnixTime target = span;
			Assert.That(target.Timestamp, Is.EqualTo(seconds).Within(1e-9));
		}
		#endregion

		#region Equals(object) Override Tests
		[Test]
		public void EqualsObjectBoxedUnixTimeTest()
		{
			UnixTime t1 = new(1_700_000_000D);
			object t2 = new UnixTime(1_700_000_000D);
			Assert.That(t1.Equals(t2), Is.True);
		}

		[Test]
		public void EqualsObjectDifferentTimestampTest()
		{
			UnixTime t1 = new(1_700_000_000D);
			object t2 = new UnixTime(1_700_000_001D);
			Assert.That(t1.Equals(t2), Is.False);
		}

		[Test]
		public void EqualsObjectNonUnixTypeReturnsFalseTest()
		{
			UnixTime t = new(1_700_000_000D);
			object other = "1700000000";
			Assert.That(t.Equals(other), Is.False);
		}

		[Test]
		public void EqualsObjectNullReturnsFalseTest()
		{
			UnixTime t = new(1_700_000_000D);
			Assert.That(t.Equals(null), Is.False);
		}
		#endregion

		#region UnixTime + UnixTime / UnixTime - UnixTime Operator Tests
		[Test]
		public void AdditionOperatorTwoUnixTimesTest()
		{
			UnixTime t1 = new(1_000_000D);
			UnixTime t2 = new(500_000D);
			UnixTime result = t1 + t2;
			Assert.That(result.Timestamp, Is.EqualTo(1_500_000D));
		}

		[Test]
		public void SubtractionOperatorTwoUnixTimesTest()
		{
			UnixTime t1 = new(1_000_000D);
			UnixTime t2 = new(400_000D);
			UnixTime result = t1 - t2;
			Assert.That(result.Timestamp, Is.EqualTo(600_000D));
		}

		[Test]
		public void SubtractionOperatorTwoUnixTimesNegativeResultTest()
		{
			UnixTime t1 = new(100D);
			UnixTime t2 = new(200D);
			UnixTime result = t1 - t2;
			Assert.That(result.Timestamp, Is.EqualTo(-100D));
		}
		#endregion

		#region UnixTime + TimeSpan / UnixTime - TimeSpan Operator Tests
		[Test]
		public void AdditionOperatorUnixTimePlusTimeSpanTest()
		{
			UnixTime t = new(1_700_000_000D);
			TimeSpan span = TimeSpan.FromHours(1);      // 3600 seconds
			UnixTime result = t + span;
			Assert.That(result.Timestamp, Is.EqualTo(1_700_003_600D));
		}

		[Test]
		public void SubtractionOperatorUnixTimeMinusTimeSpanTest()
		{
			UnixTime t = new(1_700_003_600D);
			TimeSpan span = TimeSpan.FromHours(1);      // 3600 seconds
			UnixTime result = t - span;
			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
		}

		[Test]
		public void AdditionOperatorUnixTimePlusZeroTimeSpanTest()
		{
			UnixTime t = new(1_700_000_000D);
			UnixTime result = t + TimeSpan.Zero;
			Assert.That(result.Timestamp, Is.EqualTo(t.Timestamp));
		}
		#endregion

		#region TimeSpan + UnixTime / TimeSpan - UnixTime Operator Tests
		[Test]
		public void AdditionOperatorTimeSpanPlusUnixTimeTest()
		{
			TimeSpan span = TimeSpan.FromSeconds(1_000D);
			UnixTime t = new(500D);
			UnixTime result = span + t;
			Assert.That(result.Timestamp, Is.EqualTo(1_500D));
		}

		[Test]
		public void SubtractionOperatorTimeSpanMinusUnixTimeTest()
		{
			TimeSpan span = TimeSpan.FromSeconds(2_000D);
			UnixTime t = new(500D);
			UnixTime result = span - t;
			Assert.That(result.Timestamp, Is.EqualTo(1_500D));
		}

		[Test]
		public void AdditionOperatorTimeSpanPlusZeroUnixTimeTest()
		{
			TimeSpan span = TimeSpan.FromSeconds(86400D);
			UnixTime zero = UnixTime.Zero;
			UnixTime result = span + zero;
			Assert.That(result.Timestamp, Is.EqualTo(86400D));
		}
		#endregion

		#region CanParse Tests
		[Test]
		public void CanParseValidNumericStringTest()
		{
			Assert.That(UnixTime.CanParse("1700000000"), Is.True);
		}

		[Test]
		public void CanParseValidDateStringTest()
		{
			Assert.That(UnixTime.CanParse("2023-11-14T22:13:20Z"), Is.True);
		}

		[Test]
		public void CanParseValidTimeSpanStringTest()
		{
			Assert.That(UnixTime.CanParse("1.00:00:00"), Is.True);
		}

		[Test]
		public void CanParseInvalidStringReturnsFalseTest()
		{
			Assert.That(UnixTime.CanParse("not-a-unixtime-xyz"), Is.False);
		}

		[Test]
		public void CanParseEmptyStringReturnsFalseTest()
		{
			Assert.That(UnixTime.CanParse(string.Empty), Is.False);
		}

		[Test]
		public void CanParseNullReturnsFalseTest()
		{
			Assert.That(UnixTime.CanParse(null), Is.False);
		}
		#endregion

		#region Parse(string) Success Path Tests
		[Test]
		public void ParseStringNumericSuccessTest()
		{
			UnixTime result = UnixTime.Parse("1700000000");
			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
		}

		[Test]
		public void ParseStringDateSuccessTest()
		{
			UnixTime result = UnixTime.Parse("2023-11-14T22:13:20Z");
			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D).Within(1));
		}

		[Test]
		public void ParseStringTimeSpanSuccessTest()
		{
			UnixTime result = UnixTime.Parse("1.00:00:00");
			Assert.That(result.Timestamp, Is.EqualTo(86400D).Within(1));
		}
		#endregion

		#region Parse(string, IFormatProvider) Null-Provider Fallthrough Test
		[Test]
		public void ParseWithNullProviderFallsThroughToBasicParseTest()
		{
			// When provider is null the method falls through to Parse(string).
			UnixTime result = UnixTime.Parse("1700000000", null);
			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
		}

		[Test]
		public void ParseWithNullProviderThrowsOnInvalidTest()
		{
			Assert.That(() => UnixTime.Parse("not-parseable-xyz", null), Throws.TypeOf<FormatException>());
		}
		#endregion

		#region TryParse(string, IFormatProvider, out) TimeSpan Branch Test
		[Test]
		public void TryParseWithProviderTimeSpanStringTest()
		{
			// "1.00:00:00" cannot be parsed as double or DateTime, but can as TimeSpan.
			bool success = UnixTime.TryParse("1.00:00:00", CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.True);
				Assert.That(result.Timestamp, Is.EqualTo(86400D).Within(1));
			});
		}

		[Test]
		public void TryParseWithProviderInvalidReturnsDefaultTest()
		{
			bool success = UnixTime.TryParse("not-a-time-xyz", CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.False);
				Assert.That(result.Timestamp, Is.EqualTo(UnixTime.Zero.Timestamp));
			});
		}
		#endregion

		#region Parse(ReadOnlySpan<char>, IFormatProvider) Success Path Tests
		[Test]
		public void ParseSpanNumericSuccessTest()
		{
			UnixTime result = UnixTime.Parse("1700000000".AsSpan(), CultureInfo.InvariantCulture);
			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
		}

		[Test]
		public void ParseSpanDateStringSuccessTest()
		{
			UnixTime result = UnixTime.Parse("2023-11-14T22:13:20Z".AsSpan(), CultureInfo.InvariantCulture);
			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D).Within(1));
		}
		#endregion

		#region TryParse(ReadOnlySpan<char>, IFormatProvider, out) TimeSpan Branch Test
		[Test]
		public void TryParseSpanTimeSpanStringTest()
		{
			// "1.00:00:00" falls through to TimeSpan parsing.
			bool success = UnixTime.TryParse("1.00:00:00".AsSpan(), CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.True);
				Assert.That(result.Timestamp, Is.EqualTo(86400D).Within(1));
			});
		}

		[Test]
		public void TryParseSpanInvalidReturnsDefaultTest()
		{
			bool success = UnixTime.TryParse("not-a-time-xyz".AsSpan(), CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.False);
				Assert.That(result.Timestamp, Is.EqualTo(UnixTime.Zero.Timestamp));
			});
		}
		#endregion

		#region UnixTimeJsonConverter (Newtonsoft.Json) Tests
		[Test]
		public void NewtonsoftJsonCanConvertStringTypeTest()
		{
			var converter = new UnixTimeJsonConverter();
			Assert.That(converter.CanConvert(typeof(string)), Is.True);
		}

		[Test]
		public void NewtonsoftJsonCanConvertNonStringTypeReturnsFalseTest()
		{
			var converter = new UnixTimeJsonConverter();
			// The converter is designed to be applied via [JsonConverter] attribute;
			// CanConvert intentionally returns false for non-string types.
			Assert.That(converter.CanConvert(typeof(int)), Is.False);
		}

		[Test]
		public void NewtonsoftJsonReadJsonStringTest()
		{
			// When the [JsonConverter] attribute is used, ReadJson receives the token
			// for the property value. A JSON string "1700000000" should round-trip.
			string json = "{\"Timestamp\":\"1700000000\"}";
			TimestampModel result = JsonConvert.DeserializeObject<TimestampModel>(json)!;

			Assert.That(result.Timestamp.Timestamp, Is.EqualTo(1_700_000_000D));
		}

		[Test]
		public void NewtonsoftJsonReadJsonDateStringTest()
		{
			string json = "{\"Timestamp\":\"2023-11-14T22:13:20Z\"}";
			TimestampModel result = JsonConvert.DeserializeObject<TimestampModel>(json)!;

			Assert.That(result.Timestamp.Timestamp, Is.EqualTo(1_700_000_000D).Within(1));
		}

		[Test]
		public void NewtonsoftJsonReadJsonLocalDateStringUnspecifiedKindTest()
		{
			// A date string without timezone designator is parsed by Newtonsoft.Json as
			// DateTimeKind.Unspecified. The converter should treat it as UTC.
			string json = "{\"Timestamp\":\"2023-11-14T22:13:20\"}";
			TimestampModel result = JsonConvert.DeserializeObject<TimestampModel>(json)!;

			// The converter maps Unspecified → UTC so the timestamp should equal the UTC reading.
			Assert.That(result.Timestamp.Timestamp, Is.EqualTo(1_700_000_000D).Within(1));
		}

		[Test]
		public void NewtonsoftJsonWriteJsonTest()
		{
			var model = new TimestampModel { Timestamp = new UnixTime(1_700_000_000D) };
			string json = JsonConvert.SerializeObject(model);

			Assert.That(json, Does.Contain("1700000000"));
		}

		[Test]
		public void NewtonsoftJsonRoundTripTest()
		{
			var original = new TimestampModel { Timestamp = new UnixTime(1_700_000_000D) };
			string json = JsonConvert.SerializeObject(original);
			TimestampModel deserialized = JsonConvert.DeserializeObject<TimestampModel>(json)!;

			Assert.That(deserialized.Timestamp.Timestamp, Is.EqualTo(original.Timestamp.Timestamp));
		}

		[Test]
		public void NewtonsoftJsonReadJsonNonUnixTimeTypeReturnsNullTest()
		{
			// When objectType != typeof(UnixTime) ReadJson returns null.
			var converter = new UnixTimeJsonConverter();
			using var stringReader = new IO.StringReader("\"1700000000\"");
			using var jsonReader = new JsonTextReader(stringReader);
			jsonReader.Read(); // advance to the string token

			object result = converter.ReadJson(jsonReader, typeof(string), null, new JsonSerializer());

			Assert.That(result, Is.Null);
		}

		[Test]
		public void NewtonsoftJsonWriteJsonNonUnixTimeValueIsNoopTest()
		{
			// When value is not a UnixTime WriteJson should not throw and not write anything.
			var converter = new UnixTimeJsonConverter();
			var sb = new Text.StringBuilder();
			using var sw = new IO.StringWriter(sb);
			using var writer = new JsonTextWriter(sw);

			Assert.That(() => converter.WriteJson(writer, "not-a-unixtime", new JsonSerializer()), Throws.Nothing);
		}
		#endregion
	}
}
