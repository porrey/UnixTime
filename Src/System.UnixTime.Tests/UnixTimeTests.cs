//
// Copyright(C) 2014-2025, Daniel M. Porrey. All rights reserved.
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
using System.Text.Json;
using NUnit.Framework;

namespace System.Tests
{
    [TestFixture]
	public class UnixTimeTests
	{
		public static Random Rnd { get; } = new();

		private static IEnumerable<TestDataItem> _items = Array.Empty<TestDataItem>();
		private static readonly IEnumerable<TestDataItem> Items = LoadItems();

		private static IEnumerable<TestDataItem> LoadItems()
		{
			_items = Enumerable.Range(0, 2000).Select(x => new TestDataItem() { UnixTimestampDouble = Rnd.Next(-1 * int.MaxValue, int.MaxValue) }).ToArray();
			return _items;
		}

		#region Constructor Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void DoubleConstructorTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(target.DateTime.ToUniversalTime(), Is.EqualTo(data.DateTime));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LongConstructorTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampLong);
			Assert.That(target.DateTime.ToUniversalTime(), Is.EqualTo(data.DateTime));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void IntConstructorTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampInt);
			Assert.That(target.DateTime.ToUniversalTime(), Is.EqualTo(data.DateTime));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void DateTimeConstructorTest(TestDataItem data)
		{
			UnixTime target = new(new DateTime(data.DateTime.Ticks, DateTimeKind.Utc));
			Assert.That(target.Timestamp, Is.EqualTo(data.UnixTimestampDouble));
		}
		#endregion

		#region Public Member Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void TimestampPropertyTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(target.Timestamp, Is.EqualTo(data.UnixTimestampDouble));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void DateTimePropertyTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(target.DateTime, Is.EqualTo(data.DateTime.ToLocalTime()));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void DateTimeUtcPropertyTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(target.DateTimeUtc, Is.EqualTo(data.DateTime));
		}
		#endregion

		#region Implicit Operator Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void UnixTimeToDoubleTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That((double)target, Is.EqualTo(data.UnixTimestampDouble));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void DoubleToUnixTimeTest(TestDataItem data)
		{
			UnixTime target = data.UnixTimestampDouble;
			Assert.That(target.Timestamp, Is.EqualTo(data.UnixTimestampDouble));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void UnixTimeToLongTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampLong);
			Assert.That((long)target, Is.EqualTo(data.UnixTimestampLong));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LongToUnixTimeTest(TestDataItem data)
		{
			UnixTime target = data.UnixTimestampLong;
			Assert.That((long)target.Timestamp, Is.EqualTo(data.UnixTimestampLong));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void UnixTimeToDateTimeTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(((DateTime)target).ToUniversalTime(), Is.EqualTo(data.DateTime));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void DateTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target = data.DateTime;
			Assert.That(target.Timestamp, Is.EqualTo(data.UnixTimestampDouble));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void UnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(((DateTime)target).ToUniversalTime(), Is.EqualTo(data.DateTime));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void TimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan actual = TimeSpan.FromSeconds(data.UnixTimestampDouble);
			Assert.That(actual.TotalSeconds, Is.EqualTo(data.UnixTimestampDouble));
		}
		#endregion

		#region Public Override Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void ToStringUnixTimeTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(target.ToString(), Is.EqualTo(data.UnixTimestampDouble.ToString()));
		}

		[Test]
		public void EqualsNullUnixTimeTest()
		{
			UnixTime target = new(DateTime.Now);
			Assert.That(target, Is.Not.EqualTo(null));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void EqualsUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			object target2 = new UnixTime(data.DateTime);
			Assert.That(target1, Is.EqualTo(target2));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void NotEqualUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			object target2 = new UnixTime(data.DateTime);
			Assert.That(target1, Is.Not.EqualTo(target2));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void GetHashCodeUnixTimeTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(data.UnixTimestampDouble.GetHashCode(), Is.EqualTo(target.GetHashCode()));
		}
		#endregion

		#region Static Member Tests
		[Test]
		public void EpochUnixTimeTest()
		{
			DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			Assert.That(UnixTime.Epoch, Is.EqualTo(epoch));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void FromDateTimeUnixTimeTest(TestDataItem data)
		{
			UnixTime target = UnixTime.FromDateTime(data.DateTime);
			Assert.That(data.UnixTimestampDouble, Is.EqualTo(target.Timestamp));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LongToUniversalDateTimeUnixTimeTest(TestDataItem data)
		{
			UnixTime target = UnixTime.ToUniversalDateTime(data.UnixTimestampLong);
			Assert.That(data.DateTime, Is.EqualTo(target.DateTimeUtc));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LongToLocalDateTimeUnixTimeTest(TestDataItem data)
		{
			UnixTime target = UnixTime.ToLocalDateTime(data.UnixTimestampLong);
			Assert.That(data.DateTime.ToLocalTime(), Is.EqualTo(target.DateTime));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void DoubleToUniversalDateTimeUnixTimeTest(TestDataItem data)
		{
			UnixTime target = UnixTime.ToUniversalDateTime(data.UnixTimestampDouble);
			Assert.That(data.DateTime, Is.EqualTo(target.DateTimeUtc));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void DoubleToLocalDateTimeUnixTimeTest(TestDataItem data)
		{
			UnixTime target = UnixTime.ToLocalDateTime(data.UnixTimestampDouble);
			Assert.That(data.DateTime.ToLocalTime(), Is.EqualTo(target.DateTime));
		}
		#endregion

		#region UnixTime/UnixTime Operator Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void AdditionOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			double value = Rnd.Next(1, 5000000);
			UnixTime target2 = target1 + value;

			Assert.That(target1.Timestamp + value, Is.EqualTo(target2.Timestamp));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void SubtractionOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			double value = Rnd.Next(1, 5000000);
			UnixTime target2 = target1 - value;

			Assert.That((target1.Timestamp - value) == target2.Timestamp, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void EqualsOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 == target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void NotEqualsOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 != target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LessThanOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble - 1);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 < target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LessThanOrEqualToOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble - 1);
			UnixTime target2 = new(data.DateTime.Subtract(TimeSpan.FromSeconds(1)));
			UnixTime target3 = new(data.UnixTimestampDouble);

			Assert.Multiple(() =>
			{
				Assert.That(target1 <= target2, Is.True);
				Assert.That(target1 <= target3, Is.True);
			});
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void GreaterThanOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 > target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void GreaterThanOrEqualToOperatorUnixTimeToUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			UnixTime target2 = new(data.DateTime.Add(TimeSpan.FromSeconds(1)));
			UnixTime target3 = new(data.UnixTimestampDouble);

			Assert.Multiple(() =>
			{
				Assert2.IsTrue(target1 >= target2);
				Assert2.IsTrue(target1 >= target3);
			});
		}
		#endregion

		#region UnixTime/TimeSpan Operator Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void AdditionOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			double value = Rnd.Next(1, 5000000);
			TimeSpan target2 = target1 + value;

			Assert.That(target1.Timestamp + value, Is.EqualTo(target2.TotalSeconds));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void SubtractionOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			double value = Rnd.Next(1, 5000000);
			TimeSpan target2 = target1 - value;

			Assert.That(target1.Timestamp - value, Is.EqualTo(target2.TotalSeconds));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void EqualsOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			TimeSpan target2 = TimeSpan.FromSeconds(data.UnixTimestampDouble);

			Assert.That(target1 == target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void NotEqualsOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			TimeSpan target2 = TimeSpan.FromSeconds(data.UnixTimestampDouble);

			Assert.That(target1 != target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LessThanOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble - 1);
			TimeSpan target2 = TimeSpan.FromSeconds(data.UnixTimestampDouble);

			Assert.That(target1 < target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LessThanOrEqualToOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble - 1);
			TimeSpan target2 = TimeSpan.FromSeconds(data.UnixTimestampDouble + 1);
			TimeSpan target3 = TimeSpan.FromSeconds(data.UnixTimestampDouble);

			Assert.Multiple(() =>
			{
				Assert.That(target1 <= target2, Is.True);
				Assert.That(target1 <= target3, Is.True);
			});
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void GreaterThanOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			TimeSpan target2 = TimeSpan.FromSeconds(data.UnixTimestampDouble);

			Assert.That(target1 > target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void GreaterThanOrEqualToOperatorUnixTimeToTimeSpanTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			TimeSpan target2 = TimeSpan.FromSeconds(data.UnixTimestampDouble + 1);
			TimeSpan target3 = TimeSpan.FromSeconds(data.UnixTimestampDouble);

			Assert.Multiple(() =>
			{
				Assert.That(target1 >= target2, Is.True);
				Assert.That(target1 >= target3, Is.True);
			});
		}
		#endregion

		#region TimeSpan/UnixTime Operator Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void AdditionOperatorTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble);
			double value = Rnd.Next(1, 5000000);
			UnixTime target2 = target1.TotalSeconds + value;

			Assert.That(target1.TotalSeconds + value == target2.Timestamp, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void SubtractionOperatorTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble);
			double value = Rnd.Next(1, 5000000);
			UnixTime target2 = target1.TotalSeconds - value;

			Assert.That(target1.TotalSeconds - value == target2.Timestamp, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void EqualsOperatorTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 == target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void NotEqualsOperatorTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble + 1);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 != target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LessThanOperatorTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble - 1);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 < target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void LessThanOperatorOrEqualToTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble - 1);
			UnixTime target2 = new(data.DateTime.Subtract(TimeSpan.FromSeconds(1)));
			UnixTime target3 = new(data.UnixTimestampDouble);

			Assert.Multiple(() =>
			{
				Assert.That(target1 <= target2, Is.True);
				Assert.That(target1 <= target3, Is.True);
			});
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void GreaterThanOperatorTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble + 1);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1 > target2, Is.True);
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void GreaterThanOperatorOrEqualToTimeSpanToUnixTimeTest(TestDataItem data)
		{
			TimeSpan target1 = TimeSpan.FromSeconds(data.UnixTimestampDouble + 1);
			UnixTime target2 = new(data.DateTime.Add(TimeSpan.FromSeconds(1)));
			UnixTime target3 = new(data.UnixTimestampDouble);

			Assert.Multiple(() =>
			{
				Assert.That(target1 >= target2, Is.True);
				Assert.That(target1 >= target3, Is.True);
			});
		}
		#endregion

		#region IComparable Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void IComparableTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			object target2 = new UnixTime(data.UnixTimestampDouble - 1);
			object target3 = new UnixTime(data.UnixTimestampDouble);
			object target4 = new UnixTime(data.UnixTimestampDouble + 1);

			Assert.Multiple(() =>
			{
				Assert.That(target1.CompareTo(target2), Is.EqualTo(1), "+1");
				Assert.That(target1.CompareTo(target3), Is.EqualTo(0), "0");
				Assert.That(target1.CompareTo(target4), Is.EqualTo(-1), "-1");
				Assert.That(target1.CompareTo(null), Is.EqualTo(-1), "+1");
			});
		}
		#endregion

		#region IFormattable Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void ToStringFormatTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(target.ToString("f"), Is.EqualTo(data.DateTime.ToLocalTime().ToString("f")));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void ToStringFormatProviderTest(TestDataItem data)
		{
			CultureInfo culture = new("fr-FR");
			UnixTime target = new(data.UnixTimestampDouble);
			Assert.That(target.ToString("f", culture), Is.EqualTo(data.DateTime.ToLocalTime().ToString("f", culture)));
		}
		#endregion

		#region IComparable<UnixTime> Test
		[Test]
		[TestCaseSource(nameof(Items))]
		public void IComparableUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			UnixTime target2 = new(data.UnixTimestampDouble - 1);
			UnixTime target3 = new(data.UnixTimestampDouble);
			UnixTime target4 = new(data.UnixTimestampDouble + 1);

			Assert.Multiple(() =>
			{
				Assert.That(target1.CompareTo(target2), Is.EqualTo(1), "+1");
				Assert.That(target1.CompareTo(target3), Is.EqualTo(0), "0");
				Assert.That(target1.CompareTo(target4), Is.EqualTo(-1), "-1");
				Assert.That(target1.CompareTo(null), Is.EqualTo(-1), "+1");
			});
		}
		#endregion

		#region IEquatable<UnixTime> Test
		[Test]
		public void IEquatableEqualsNullUnixTimeTest()
		{
			UnixTime target = new(DateTime.Now);
			Assert.That(target, Is.Not.EqualTo(null));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void IEquatableEqualsUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1, Is.EqualTo(target2));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void IEquatableNotEqualUnixTimeTest(TestDataItem data)
		{
			UnixTime target1 = new(data.UnixTimestampDouble + 1);
			UnixTime target2 = new(data.DateTime);

			Assert.That(target1, Is.Not.EqualTo(target2));
		}
		#endregion

		#region Static Boundary Property Tests
		[Test]
		public void ZeroPropertyTest()
		{
			Assert.That(UnixTime.Zero.Timestamp, Is.EqualTo(0D));
		}

		[Test]
		public void MinValuePropertyTest()
		{
			Assert.That(UnixTime.MinValue.DateTimeUtc, Is.EqualTo(new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Utc)));
		}

		[Test]
		public void MaxValuePropertyTest()
		{
			// DateTime.MaxValue sub-second precision is lost through integer-second conversion; verify the
			// timestamp is within 1 second of the expected boundary rather than round-tripping via DateTimeUtc.
			double expectedSeconds = (new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc) - UnixTime.Epoch).TotalSeconds;
			Assert.That(UnixTime.MaxValue.Timestamp, Is.EqualTo(expectedSeconds).Within(1));
		}

		[Test]
		public void MinValueLessThanMaxValueTest()
		{
			Assert.That(UnixTime.MinValue < UnixTime.MaxValue, Is.True);
		}

		[Test]
		public void ZeroBetweenMinAndMaxTest()
		{
			Assert.Multiple(() =>
			{
				Assert.That(UnixTime.Zero > UnixTime.MinValue, Is.True);
				Assert.That(UnixTime.Zero < UnixTime.MaxValue, Is.True);
			});
		}
		#endregion

		#region ISpanFormattable Tests
		[Test]
		[TestCaseSource(nameof(Items))]
		public void TryFormatNoFormatTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Span<char> buffer = stackalloc char[64];
			bool success = target.TryFormat(buffer, out int charsWritten, default, null);

			Assert.That(success, Is.True);
			Assert.That(double.Parse(buffer[..charsWritten].ToString(), CultureInfo.InvariantCulture), Is.EqualTo(data.UnixTimestampDouble).Within(1e-9));
		}

		[Test]
		[TestCaseSource(nameof(Items))]
		public void TryFormatWithFormatTest(TestDataItem data)
		{
			UnixTime target = new(data.UnixTimestampDouble);
			Span<char> buffer = stackalloc char[128];
			ReadOnlySpan<char> format = "o".AsSpan();
			CultureInfo culture = CultureInfo.InvariantCulture;
			bool success = target.TryFormat(buffer, out int charsWritten, format, culture);

			Assert.That(success, Is.True);
			Assert.That(buffer[..charsWritten].ToString(), Is.EqualTo(target.DateTime.ToString("o", culture)));
		}

		[Test]
		public void TryFormatBufferTooSmallTest()
		{
			UnixTime target = new(1_700_000_000D);
			Span<char> tinyBuffer = stackalloc char[2];

			bool success = target.TryFormat(tinyBuffer, out _, default, null);

			Assert.That(success, Is.False);
		}
		#endregion

		#region TryParse with IFormatProvider Tests
		[Test]
		public void TryParseWithProviderNumericTest()
		{
			string s = "1700000000";
			bool success = UnixTime.TryParse(s, CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.True);
				Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
			});
		}

		[Test]
		public void TryParseWithProviderDateStringTest()
		{
			string s = "2023-11-14T22:13:20Z";
			bool success = UnixTime.TryParse(s, CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.True);
				Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D).Within(1));
			});
		}

		[Test]
		public void TryParseWithProviderInvalidTest()
		{
			bool success = UnixTime.TryParse("not-a-time", CultureInfo.InvariantCulture, out _);
			Assert.That(success, Is.False);
		}
		#endregion

		#region IParsable<UnixTime> Tests
		[Test]
		public void IParsableParseNumericTest()
		{
			UnixTime result = UnixTime.Parse("1700000000", CultureInfo.InvariantCulture);

			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
		}

		[Test]
		public void IParsableTryParseNumericTest()
		{
			bool success = UnixTime.TryParse("1700000000", CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.True);
				Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
			});
		}

		[Test]
		public void IParsableTryParseInvalidTest()
		{
			bool success = UnixTime.TryParse("not-a-time", CultureInfo.InvariantCulture, out _);
			Assert.That(success, Is.False);
		}
		#endregion

		#region ISpanParsable<UnixTime> Tests
		[Test]
		public void ISpanParsableTryParseNumericTest()
		{
			bool success = UnixTime.TryParse("1700000000".AsSpan(), CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.True);
				Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
			});
		}

		[Test]
		public void ISpanParsableTryParseDateStringTest()
		{
			bool success = UnixTime.TryParse("2023-11-14T22:13:20Z".AsSpan(), CultureInfo.InvariantCulture, out UnixTime result);

			Assert.Multiple(() =>
			{
				Assert.That(success, Is.True);
				Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D).Within(1));
			});
		}

		[Test]
		public void ISpanParsableTryParseInvalidTest()
		{
			bool success = UnixTime.TryParse("not-a-time".AsSpan(), null, out _);
			Assert.That(success, Is.False);
		}

		[Test]
		public void ISpanParsableParseThrowsOnInvalidTest()
		{
			Assert.That(() => UnixTime.Parse("not-a-time".AsSpan(), null), Throws.TypeOf<FormatException>());
		}
		#endregion

		#region Parse FormatException Message Test
		[Test]
		public void ParseThrowsDescriptiveFormatExceptionTest()
		{
			string badInput = "definitely-not-parseable-xyz";
			FormatException ex = Assert.Throws<FormatException>(() => UnixTime.Parse(badInput));
			Assert.That(ex.Message, Does.Contain(badInput));
		}
		#endregion

		#region UnixTimeSystemTextJsonConverter Tests
		[Test]
		public void SystemTextJsonConverterWritesNumberTest()
		{
			UnixTime value = new(1_700_000_000D);
			var options = new JsonSerializerOptions();
			options.Converters.Add(new UnixTimeSystemTextJsonConverter());

			string json = JsonSerializer.Serialize(value, options);

			Assert.That(json, Is.EqualTo("1700000000"));
		}

		[Test]
		public void SystemTextJsonConverterReadsNumberTest()
		{
			string json = "1700000000";
			var options = new JsonSerializerOptions();
			options.Converters.Add(new UnixTimeSystemTextJsonConverter());

			UnixTime result = JsonSerializer.Deserialize<UnixTime>(json, options);

			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D));
		}

		[Test]
		public void SystemTextJsonConverterReadsStringTest()
		{
			string json = "\"2023-11-14T22:13:20Z\"";
			var options = new JsonSerializerOptions();
			options.Converters.Add(new UnixTimeSystemTextJsonConverter());

			UnixTime result = JsonSerializer.Deserialize<UnixTime>(json, options);

			Assert.That(result.Timestamp, Is.EqualTo(1_700_000_000D).Within(1));
		}

		[Test]
		public void SystemTextJsonConverterRoundTripTest()
		{
			UnixTime original = new(1_700_000_000D);
			var options = new JsonSerializerOptions();
			options.Converters.Add(new UnixTimeSystemTextJsonConverter());

			string json = JsonSerializer.Serialize(original, options);
			UnixTime deserialized = JsonSerializer.Deserialize<UnixTime>(json, options);

			Assert.That(deserialized.Timestamp, Is.EqualTo(original.Timestamp));
		}

		[Test]
		public void SystemTextJsonConverterThrowsOnInvalidStringTest()
		{
			string json = "\"not-a-unixtime-xyz\"";
			var options = new JsonSerializerOptions();
			options.Converters.Add(new UnixTimeSystemTextJsonConverter());

			Assert.That(() => JsonSerializer.Deserialize<UnixTime>(json, options), Throws.TypeOf<JsonException>());
		}

		[Test]
		public void SystemTextJsonConverterThrowsOnBooleanTokenTest()
		{
			string json = "true";
			var options = new JsonSerializerOptions();
			options.Converters.Add(new UnixTimeSystemTextJsonConverter());

			Assert.That(() => JsonSerializer.Deserialize<UnixTime>(json, options), Throws.TypeOf<JsonException>());
		}
		#endregion
	}
}
