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
namespace System
{
	/// <summary>
	/// Provides extension methods for Sysem.DateTime to work with System.UnixTime
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Convert a System.DateTime value to a Unix Time.
		/// </summary>
		/// <param name="value">System.DateTime object.</param>
		/// <returns>A 64-bit integer representing the Unix Time.</returns>
		public static long ToUnixTime(this DateTime value)
		{
			return UnixTime.FromDateTime(value);
		}
	}
}
