// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {
        IWebDriver webDriver;
        [SetUp]
        public void Initialize()
        {
            webDriver = new ChromeDriver();
        }
        [Test]
        public void TestMethod()
        {
            webDriver.Url = "https://www.airbnb.com/";
        }
        [TearDown]
        public void EndTest()
        {
            webDriver.Close();
            webDriver.Quit();
        }
    }
}
