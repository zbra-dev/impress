using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress.Tests
{
    public class TestEnumerableExtensions
    {
        [Test]
        public void TestRandom()
        {
            var ar = new string[] { "a", "b", "c" };

            var m = ar.ElementAtRandom(new System.Random());

            Assert.IsTrue(m.HasValue);

            Assert.IsTrue(ar.Contains(m.Value));

            ar = new string[] { "a" };

            m = ar.ElementAtRandom(new System.Random());

            Assert.IsTrue(m.HasValue);

            Assert.IsTrue(ar.Contains(m.Value));

            var r = Enumerable.Empty<int>().Concat(1);

            var y = r.ElementAtRandom(new System.Random());

            Assert.IsTrue(y.HasValue);

            Assert.IsTrue(r.Contains(y.Value));

            r = Enumerable.Empty<int>().Concat(1).Concat(2).Concat(3);

            y = r.ElementAtRandom(new System.Random());

            Assert.IsTrue(y.HasValue);

            Assert.IsTrue(r.Contains(y.Value));
        }


        [Test]
        public void TestAssociate()
        {
            var dictionary = new Dictionary<int, string>()
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" }
            };

            var pairList = Enumerable.Range(1, 10).Associate(dictionary, n => n).ToList();

            Assert.AreEqual(pairList.Count, 4);

            Assert.AreEqual(pairList[0].Item1, 1);
            Assert.AreEqual(pairList[0].Item2, "One");

            Assert.AreEqual(pairList[1].Item1, 2);
            Assert.AreEqual(pairList[1].Item2, "Two");

            Assert.AreEqual(pairList[2].Item1, 3);
            Assert.AreEqual(pairList[2].Item2, "Three");

            Assert.AreEqual(pairList[3].Item1, 4);
            Assert.AreEqual(pairList[3].Item2, "Four");
        }

        [Test]
        public void TestAssociateWithConstructor()
        {
            var dictionary = new Dictionary<int, string>()
            {
                { 1, "One" },
                { 2, "Two" },
                { 3, "Three" },
                { 4, "Four" }
            };

            var pairList = Enumerable.Range(1, 10).Associate(dictionary, n => n, (n, s) => new { Number = n, NumberName = s }).ToList();

            Assert.AreEqual(pairList.Count, 4);

            Assert.AreEqual(pairList[0].Number, 1);
            Assert.AreEqual(pairList[0].NumberName, "One");

            Assert.AreEqual(pairList[1].Number, 2);
            Assert.AreEqual(pairList[1].NumberName, "Two");

            Assert.AreEqual(pairList[2].Number, 3);
            Assert.AreEqual(pairList[2].NumberName, "Three");

            Assert.AreEqual(pairList[3].Number, 4);
            Assert.AreEqual(pairList[3].NumberName, "Four");
        }


    }
}
