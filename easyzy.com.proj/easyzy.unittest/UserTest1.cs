using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using easyzy.bll;

namespace easyzy.unittest
{
    [TestClass]
    public class UserTest1
    {
        [TestMethod]
        public void AddMethodTest()
        {
            int d1 = 110;

            int d2 = 220;

            int d = B_Test.Add(d1, d2);

            Assert.AreEqual(d, 330);
            //CollectionAssert.
            
        }
    }
}
