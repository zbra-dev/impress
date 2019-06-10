using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Tests
{
    public class TestHash
    {
        [Test]
        public void TestHashWithNullValues()
        {
            Assert.AreEqual(0, Hash.Create(null).GetHashCode());
        }

        [Test]
        public void TestHashWithNullables()
        {
            int? testValue = null;
            Assert.AreEqual(0, Hash.Create(testValue).GetHashCode());
            testValue = 3;
            Assert.AreEqual(3, Hash.Create(testValue).GetHashCode());
        }

        [Test]
        public void TestHashWithMaybe()
        {
            Maybe<long> testValue = Maybe<long>.Nothing;
            Assert.AreEqual(0, Hash.Create(testValue).GetHashCode());
            testValue = 2L.ToMaybe();
            Assert.AreEqual(2, Hash.Create(testValue).GetHashCode());
        }

        [Test]
        public void TestHashWithEnumerables()
        {
            var enumerable = new List<string>() { "I'll send", "an SOS", "to the world" };
            var array = enumerable.ToArray();
            var hash1 = Hash.Create(array[0]).Add(array[1]).Add(array[2]).GetHashCode();
            var hash2 = Hash.Create(enumerable).GetHashCode();

            Assert.AreEqual(hash1, hash2);
        }
    }
}
