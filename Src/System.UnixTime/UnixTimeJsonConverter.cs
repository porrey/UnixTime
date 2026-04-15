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
using Newtonsoft.Json;

namespace System
{
	/// <summary>
	/// Handles JSON serialization and deserialization of System.UnixTime values when using Newtonsoft.Json.
	/// </summary>
	public class UnixTimeJsonConverter : JsonConverter
	{
		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>True if this instance can convert the specified object type; False otherwise.</returns>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(string);
		}

		/// <summary>
		/// Reads a JSON value and converts it to a System.UnixTime instance.
		/// If Newtonsoft.Json has pre-parsed the value as a System.DateTime (its default
		/// DateParseHandling behaviour), it is converted directly; otherwise the raw string
		/// is parsed via <see cref="UnixTime.Parse(string)"/>.
		/// </summary>
		/// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object returnValue = null;

			if (objectType == typeof(UnixTime))
			{
				// Newtonsoft.Json may have already parsed an ISO date string into a DateTime
				// (its default DateParseHandling.DateTime behaviour). Handle that case directly
				// so that timezone information is preserved.
				if (reader.Value is DateTime dt)
				{
					// If Kind is Unspecified (e.g. the string had no timezone designator),
					// treat it as UTC to stay consistent with the rest of the library.
					DateTime utcDt = dt.Kind == DateTimeKind.Unspecified
						? DateTime.SpecifyKind(dt, DateTimeKind.Utc)
						: dt;
					returnValue = new UnixTime(utcDt);
				}
				else
				{
					returnValue = UnixTime.Parse(Convert.ToString(reader.Value));
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
		/// <param name="value">The value to be written.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is UnixTime)
			{
				writer.WriteValue(((UnixTime)value).ToString());
			}
		}
	}
}