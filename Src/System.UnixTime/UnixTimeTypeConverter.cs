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
#if NET20 || NET35 || NET40 || NET45 || NET451
using System.ComponentModel;
using System.Globalization;

namespace System
{
	/// <summary>
	/// Provides a unified way of converting types of values to other types, as well
	/// as for accessing standard values and sub-properties.
	/// </summary>
	public class UnixTimeTypeConverter : TypeConverter 
	{
		/// <summary>
		/// Returns whether this converter can convert an object of the given type to
		/// the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
		/// <param name="sourceType">A System.Type that represents the type you want to convert from.</param>
		/// <returns>True if this converter can perform the conversion, False otherwise.</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
			return converter.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type,
		/// using the specified context.
		/// </summary>
		/// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
		/// <param name="destinationType">A System.Type that represents the type you want to convert to.</param>
		/// <returns>True if this converter can perform the conversion, False otherwise.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
			return converter.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified
		/// context and culture information.
		/// </summary>
		/// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
		/// <param name="culture">The System.Globalization.CultureInfo to use as the current culture.</param>
		/// <param name="value">The System.Object to convert.</param>
		/// <returns>An System.Object that represents the converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
			return converter.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified
		/// context and culture information.
		/// </summary>
		/// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
		/// <param name="culture">A System.Globalization.CultureInfo. If null is passed, the current culture
		/// is assumed.</param>
		/// <param name="value">The System.Object to convert.</param>
		/// <param name="destinationType">The System.Type to convert the value parameter to.</param>
		/// <returns>An System.Object that represents the converted value.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
			return converter.ConvertTo(context, culture, value, destinationType);
		}
	}
}
#endif