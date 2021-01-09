﻿// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;

using System.Collections;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

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
        public void TestCase1()
        {
            //Variable Declarations
            //Location TextBox web element
            IWebElement LocationInput;
            //Web Elements for the Check-in and Check-out dates
            IWebElement checkInOutDiv;
            IWebElement checkInInput;
            IWebElement chosenCheckInDate;
            IWebElement chosenCheckOutDate;
            //Web Elements for Number of Guests
            IWebElement guestsInput;
            IWebElement adultsStepperParent;
            IWebElement adultsStepper;
            IWebElement childrenStepperParent;
            IWebElement childrenStepper;
            //Web Elements for the search button
            IWebElement parentElement;
            IWebElement searchButton;

            //Open airbnb website
            System.TimeSpan maxWaitTime = new System.TimeSpan(0, 0, 60);
            webDriver.Manage().Timeouts().PageLoad = maxWaitTime;
            webDriver.Url = "https://www.airbnb.com/";

            webDriver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);

            try
            {
                //Get the TextBox of location and set the text to "Rome, Italy"
                By Locator = By.Id("bigsearch-query-detached-query");
                LocationInput = webDriver.FindElement(Locator);
                LocationInput.SendKeys("Rome, Italy");

                //Get the DIV that contains the Check in and out dates and click on it to open the calender
                checkInOutDiv = webDriver.FindElement(By.ClassName("_j8gg2a"));
                checkInInput = checkInOutDiv.FindElement(By.ClassName("_1akb2mdw"));
                checkInInput.Click();

                //Choose Check-in and Check-out dates
                chosenCheckInDate = webDriver.FindElement(By.XPath(@"/html/body/div[4]/div/div/div/div[1]/div[1]/div/header/div/div[2]/div[2]/div/div/div/form/div/div/div[3]/div[4]/section/div/div/div[1]/div/div/div/div[2]/div[2]/div/div[2]/div/table/tbody/tr[3]/td[6]"));
                chosenCheckOutDate = webDriver.FindElement(By.XPath(@"/html/body/div[4]/div/div/div/div[1]/div[1]/div/header/div/div[2]/div[2]/div/div/div/form/div/div/div[3]/div[4]/section/div/div/div[1]/div/div/div/div[2]/div[2]/div/div[2]/div/table/tbody/tr[4]/td[7]"));
                chosenCheckInDate.Click();
                chosenCheckOutDate.Click();

                //Add the number of guests
                guestsInput = webDriver.FindElement(By.ClassName("_37ivfdq"));
                guestsInput.Click();
                //Add two Adults
                adultsStepperParent = webDriver.FindElement(By.Id("stepper-adults"));
                adultsStepper = adultsStepperParent.FindElements(By.ClassName("_7hhhl3"))[1];
                adultsStepper.Click();
                adultsStepper.Click();
                //Add 1 child
                childrenStepperParent = webDriver.FindElement(By.Id("stepper-children"));
                childrenStepper = childrenStepperParent.FindElements(By.ClassName("_7hhhl3"))[1];
                childrenStepper.Click();

                //Submit the search
                parentElement = webDriver.FindElement(By.ClassName("_w64aej"));
                searchButton = parentElement.FindElement(By.ClassName("_1mzhry13"));
                searchButton.Submit();

                webDriver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);
            }
            catch (System.Exception e)
            {
                Assert.Fail();
            }
        }
        [TearDown]
        public void EndTest()
        {
            webDriver.Close();
            webDriver.Quit();
        }
    }
}
