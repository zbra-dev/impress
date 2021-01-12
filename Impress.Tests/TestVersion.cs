using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Tests
{
    public class TestVersion
    {
        [Test]
        public void TestIncorrectParsing()
        {
            Assert.Throws<FormatException>(() =>
            {
                Version.Parse("23-41-5-alfa");
            });
        }

        [Test]
        public void TestCannotParseNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Version.Parse(null);
            });
        }

        [Test]
        public void TestCorrectParsing()
        {
            var version = Version.Parse("23.41.5-alfa");

            Assert.AreEqual(23, version.Major);
            Assert.AreEqual(41, version.Minor);
            Assert.AreEqual(5, version.Revision);
            Assert.AreEqual("alfa", version.Variant);
        }

        [Test]
        public void TestComparing()
        {
            var a = Version.Parse("0.1.0");
            var b = Version.Parse("1.2.3-alfa");
            var c = Version.Parse("1.2.3");

            Assert.IsTrue(a < b);
            Assert.IsTrue(c <= b);
            Assert.IsTrue(c >= b);
            Assert.IsTrue(c > a);
        }
    }
}
