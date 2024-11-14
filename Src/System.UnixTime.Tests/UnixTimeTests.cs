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
	}
}
