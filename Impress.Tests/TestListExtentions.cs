using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Impress;

namespace Impress.Tests
{
  
    public class TestListExtentions
    {

        private List<int> SortedList = new List<int>() { 1, 2, 3, 4, 5 };

        [Test]
        public void TestCannotSortArray()
        {
            Assert.Throws<Exception>(() =>
            {
                int[] list = { 1, 3, 5, 4, 2 };
                list.Sort();
            });
      
        }

        [Test]
        public void TestSortList()
        {
         
            IList<int> list = new List<int>() { 1, 3, 5, 4, 2 };
            list.Sort();

            Assert.AreEqual(SortedList, list);
        }

        [Test]
        public void TestSortListWithRule()
        {

            IList<Maybe<int>> list = new List<int>() { 1, 3, 5, 4, 2 }.Select(it => it.ToMaybe()).ToList();
            list.Sort((a,b) => a.Value.CompareTo(b.Value));

            Assert.AreEqual(SortedList.Select(it => it.ToMaybe()).ToList(), list);
        }
    }
}
