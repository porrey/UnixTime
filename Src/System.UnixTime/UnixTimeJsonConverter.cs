using Newtonsoft.Json;

namespace System
{
	/// <summary>
	/// Handles conversion of the Time object when using the 
	/// Newtonsoft.Json library.
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
		/// Gets a value indicating whether this System.UnixTimeJsonConverter can read JSON.
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
				returnValue = UnixTime.Parse((string)reader.Value);
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