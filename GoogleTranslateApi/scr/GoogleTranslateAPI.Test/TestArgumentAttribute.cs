﻿/**
 * TestArgumentAttribute.cs
 *
 * Copyright (C) 2008,  iron9light
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using NUnit.Framework;

namespace Google.API.Translate.Test
{
    [TestFixture]
    public class TestArgumentAttribute
    {
        [Test]
        public void ConstructorTest()
        {
            string name = "Some name";
            ArgumentAttribute attribute = new ArgumentAttribute(name);
            Assert.AreEqual(name, attribute.Name);
            Assert.IsTrue(attribute.Optional);
            Assert.IsNull(attribute.DefaultValue);
            Assert.IsFalse(attribute.NeedEncode);

            attribute.Optional = false;
            Assert.IsFalse(attribute.Optional);

            attribute.NeedEncode = true;
            Assert.IsTrue(attribute.NeedEncode);
        }

        [Test]
        public void ConstructorTest2()
        {
            string name = "Some name";
            string defaultValue = "Some default value.";
            ArgumentAttribute attribute = new ArgumentAttribute(name, defaultValue);
            Assert.AreEqual(name, attribute.Name);
            Assert.IsFalse(attribute.Optional);
            Assert.AreEqual(defaultValue, attribute.DefaultValue);
            Assert.IsFalse(attribute.NeedEncode);

            attribute.Optional = true;
            Assert.IsTrue(attribute.Optional);

            attribute.NeedEncode = true;
            Assert.IsTrue(attribute.NeedEncode);
        }
    }
}
