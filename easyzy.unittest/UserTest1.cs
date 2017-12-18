using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace easyzy.unittest
{
    [TestClass]
    public class UserTest1
    {
        [TestMethod]
        public void AddMethod1()
        {
            double d1 = 110;

            double d2 = 220;

            Assert.AreEqual((d1 + d2), 230);
        }
    }
}
