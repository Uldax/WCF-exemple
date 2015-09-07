using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using USherbrooke.ServiceModel.Sondage;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int a = 1;
            int b = 2;

            int c = a + b;

            Assert.AreEqual(3, c, "Pas ok :(");
            
        }
    }
}
