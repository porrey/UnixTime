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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System
{
	/// <summary>
	/// Handles conversion of System.UnixTime when using System.Text.Json.
	/// Numeric JSON values are read and written as the raw Unix timestamp (seconds since epoch).
	/// String JSON values are parsed using <see cref="UnixTime.TryParse(string, out UnixTime)"/>.
	/// </summary>
	public class UnixTimeSystemTextJsonConverter : JsonConverter<UnixTime>
	{
		/// <summary>
		/// Reads and converts a JSON value to a System.UnixTime.
		/// Accepts a JSON number (raw timestamp) or a JSON string (any format accepted by <see cref="UnixTime.TryParse(string, out UnixTime)"/>).
		/// </summary>
		/// <param name="reader">The reader to read from.</param>
		/// <param name="typeToConvert">The type being converted.</param>
		/// <param name="options">Serializer options.</param>
		/// <returns>The deserialized System.UnixTime value.</returns>
		public override UnixTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Number && reader.TryGetDouble(out double numericValue))
			{
				return new UnixTime(numericValue);
			}

			if (reader.TokenType == JsonTokenType.String)
			{
				string s = reader.GetString() ?? string.Empty;

				if (UnixTime.TryParse(s, out UnixTime result))
				{
					return result;
				}

				throw new JsonException($"The string '{s}' could not be parsed as a UnixTime value.");
			}

			throw new JsonException($"Cannot convert JSON token of type '{reader.TokenType}' to UnixTime.");
		}

		/// <summary>
		/// Writes a System.UnixTime value as a JSON number (the raw Unix timestamp).
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="value">The System.UnixTime value to write.</param>
		/// <param name="options">Serializer options.</param>
		public override void Write(Utf8JsonWriter writer, UnixTime value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Timestamp);
		}
	}
}
