using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            int val = 0xC177;
            for (int i = 0xC100; i < 0xc1FF; i++)
            {
                Debug.WriteLine("{0:X4}, {1:X4}", i, i & val);
            }
        }
    }
}
