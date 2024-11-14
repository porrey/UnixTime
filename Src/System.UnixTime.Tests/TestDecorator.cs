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
using NUnit.Framework;

namespace System.Tests
{
    public class Assert2
    {
        public static void AreEqual(object x, object y)
        {
            Assert.That(x, Is.EqualTo(y));
        }

        public static void AreEqual(double x, double y, double delta)
        {
            Assert.Multiple(()=>
            {
                Assert.That(x, Is.AtLeast(y - delta));
                Assert.That(x, Is.AtMost(y + delta));
            });
        }

        public static void IsTrue(bool value)
        {
            Assert.That(value, Is.EqualTo(true));
        }

        public static void IsFalse(bool value)
        {
            Assert.That(value, Is.EqualTo(false));
        }

        public static void AreSame(object x, object y)
        {
            Assert.That(x, Is.SameAs(y));
        }
    }
}
