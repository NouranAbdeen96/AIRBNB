// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;

using System.Collections;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {
        static IWebDriver webDriver;
        [SetUp]
        public void Initialize()
        {
            webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
        }
        public static IWebElement WaitUntilElementClickable(By elementLocator, int timeout = 10)
        {
            //Source of code: https://stackoverflow.com/questions/43203243/how-to-get-webdriver-to-wait-for-page-to-load-c-selenium-project
            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
                return wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }
        public void SearchActions()
        {
            //Variable Declarations
            //WebElement for Location TextBox
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

            try
            {
                //Open airbnb website
                System.TimeSpan maxWaitTime = new System.TimeSpan(0, 0, 120);
                webDriver.Manage().Timeouts().PageLoad = maxWaitTime;
                webDriver.Url = "https://www.airbnb.com/";
                //Wait for 5 seconds for the website to load
                webDriver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);
            }
            catch (TimeoutException e) {Assert.Fail(e.Message);}

            //Entering Location = Rome, Italy:
            try
            {
                //Get the TextBox of location and set the text to "Rome, Italy"
                By Locator = By.Id("bigsearch-query-detached-query");
                LocationInput = webDriver.FindElement(Locator);
                LocationInput.SendKeys("Rome, Italy");
            }
            catch (Exception e) {Assert.Fail("Exception Thrown when entering the location. Error Message: " + e.Message);}

            //Check-in and Check-out:
            try
            {
                checkInOutDiv = webDriver.FindElement(By.ClassName("_j8gg2a"));
                checkInInput = checkInOutDiv.FindElement(By.ClassName("_1akb2mdw"));
                checkInInput.Click();
                //Get the DIV that contains the Check in and out dates and click on it to open the calender
                //Choose Check-in and Check-out dates
                chosenCheckInDate = webDriver.FindElement(By.XPath(@"/html/body/div[4]/div/div/div/div[1]/div[1]/div/header/div/div[2]/div[2]/div/div/div/form/div/div/div[3]/div[4]/section/div/div/div[1]/div/div/div/div[2]/div[2]/div/div[2]/div/table/tbody/tr[3]/td[6]"));
                chosenCheckOutDate = webDriver.FindElement(By.XPath(@"/html/body/div[4]/div/div/div/div[1]/div[1]/div/header/div/div[2]/div[2]/div/div/div/form/div/div/div[3]/div[4]/section/div/div/div[1]/div/div/div/div[2]/div[2]/div/div[2]/div/table/tbody/tr[4]/td[7]"));
                chosenCheckInDate.Click();
                chosenCheckOutDate.Click();
            }
            catch (Exception e) {Assert.Fail("Exception thrown during entering the Check-in and Check-out dates. Error Message: "+e.Message);}

            //Entering the number of guests:
            try
            {
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
                System.Threading.Thread.Sleep(2000);
                searchButton.Submit();

                webDriver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);
            }
            catch (System.Exception e) {Assert.Fail("Exception thown during selecting the number of guests. Error Message: "+e.Message);}
        }
        [Test]
        public void TestCase1()
        {
            SearchActions();
            //Waiting for a web element on the page to be clickable to be sure that the page is loaded
            WaitUntilElementClickable(By.ClassName("_1usxwsg6"));

            //Checking that the 3 filters appear correctly in the following page
            List<IWebElement> chosenFilters = new List<IWebElement>(webDriver.FindElements(By.ClassName("_1g5ss3l")));
            if (chosenFilters[0].Text != "Rome")         Assert.Fail("The location filter is incorrect with value: " + chosenFilters[0].Text);
            if (chosenFilters[1].Text != "Jan 15 – 23")  Assert.Fail("The Date filter is incorrect with value: " + chosenFilters[1].Text);
            if (chosenFilters[2].Text != "3 guests")     Assert.Fail("The Number of guests filter is incorrect with value: " + chosenFilters[2].Text);

            //Checking that the results can accomodate atleast 3 guests
            List<IWebElement> roomsDiv = new List<IWebElement>(webDriver.FindElements(By.ClassName("_tmwq9g")));
            IWebElement numberOfGuestsInRoom;
            string numberOfGuests;
            //Loop through the offered rooms and check that the min number of guests is 3
            foreach (IWebElement room in roomsDiv)
            {
                numberOfGuestsInRoom = webDriver.FindElement(By.ClassName("_kqh46o"));
                numberOfGuests = numberOfGuestsInRoom.Text.Split(' ')[0];
                if (int.TryParse(numberOfGuests,out int x) &&  int.Parse(numberOfGuests) < 3)
                    Assert.Fail("The number of guests in an offered room is less than 3");
            }
        }
        [Test]
        public void TestCase2()
        {
            SearchActions();
            //Wait for page buttons to be clickable
           // WaitUntilElementClickable(By.ClassName("_t6p96s"),20);

            //Clicking on more filters button
            IWebElement moreFiltersParentDiv = webDriver.FindElement(By.Id("menuItemButton-dynamicMoreFilters"));
            IWebElement moreFiltersButton = moreFiltersParentDiv.FindElement(By.ClassName("_t6p96s"));
            moreFiltersButton.Click();

            //Adding 5 bedrooms
            try
            {
                IWebElement bedroomFilterParentDiv = webDriver.FindElements(By.ClassName("_jwbbkz"))[1];
                IWebElement bedroomFilterStepper = bedroomFilterParentDiv.FindElements(By.ClassName("_7hhhl3"))[1];
                int numOfBedrooms = 5;
                for (int i = 0; i < numOfBedrooms; i++) bedroomFilterStepper.Click();
            }
            catch (Exception e) {Assert.Fail("Exception thrown when choosing the number of bedrooms. Error Message: "+e.Message);}

            //Choose Pool From Facilities
            try
            {
                IWebElement poolCheckBox = webDriver.FindElement(By.Id("filterItem-facilities-checkbox-amenities-7"));
                poolCheckBox.Click();
            }
            catch (Exception e) {Assert.Fail("Exception thrown when choosing Pool facility. Error Message: "+e.Message);}

            //Show Stays
            IWebElement showStaysButton = webDriver.FindElement(By.ClassName("_m095vcq"));
            showStaysButton.Click();

            //Check no of bedrooms in first page have 5 bedrooms
            List<IWebElement> roomsDiv = new List<IWebElement>(webDriver.FindElements(By.ClassName("_tmwq9g")));
            IWebElement numberOfBedroomsInRoom;
            string numberOfBedrooms;
            WaitUntilElementClickable(By.ClassName("_kqh46o"),30);
            //Loop through the offered rooms and check that the number of bedrooms is 5
            foreach (IWebElement room in roomsDiv)
            {
                numberOfBedroomsInRoom = webDriver.FindElement(By.ClassName("_kqh46o"));
                numberOfBedrooms = numberOfBedroomsInRoom.Text.Split(' ')[3];
                if (int.TryParse(numberOfBedrooms, out int x) && int.Parse(numberOfBedrooms) < 5)
                    Assert.Fail("The number of bedrooms in the room is less than 5. The number of bedrooms: "+ numberOfBedrooms);
            }

            //Open the first property
            //IWebElement firstProperty = webDriver.FindElement(By.ClassName("_tmwq9g"));
            IWebElement firstPropertyLink = webDriver.FindElement(By.XPath("//*[@id='ExploreLayoutController']/div/div[1]/div[1]/div[2]/div/div[2]/div/div/div[2]/div/div/div/div/div[1]/div/div/div/div/div[2]/a"));
            firstPropertyLink.Click();

            System.Threading.Thread.Sleep(1000);

            var browserTabs = webDriver.WindowHandles;
            webDriver.SwitchTo().Window(browserTabs[1]);

            //Check Pool facility
            List<IWebElement> amenitiesParentDivs = new List<IWebElement>(webDriver.FindElements(By.ClassName("_1nlbjeu")));
            IWebElement childDiv;
            bool poolIsAvailable = false;
            foreach (IWebElement parentDiv in amenitiesParentDivs)
            {
                childDiv = parentDiv.FindElement(By.CssSelector("div"));
                if (childDiv.Text == "Pool")
                    poolIsAvailable = true;
            }
            if (poolIsAvailable == false)
                Assert.Fail("Pool Amenity is not available");
            System.Threading.Thread.Sleep(10000);

        }
        [Test]
        public void TestCase3()
        {
            //Variable Declarations
            Actions hoverAction = new Actions(webDriver);
            IWebElement firstProperty;
            IWebElement firstPropertyButton;
            string firstPropertyName;
            string expectedColor;
            string[] firstPropertySearchDetails;
            string[] firstPropertyMapDetails;

            SearchActions();
            System.Threading.Thread.Sleep(20000);

            //Hovering over the first property
            firstProperty = webDriver.FindElement(By.ClassName("_tmwq9g"));
            hoverAction.MoveToElement(firstProperty).Perform();

            //Checking that the first property id displayed on the map by its name
            firstPropertyName = getFirstPropertyName();
            firstPropertyButton = getFirstPropertyFromMap(firstPropertyName);
            if (firstPropertyButton == null) 
                Assert.Fail("First property is not displayed on the map");

            //Checking the color of the property on the map
            expectedColor = "rgba(72, 72, 72, 1)";
            checkColor(firstPropertyButton, expectedColor);
            if (checkColor(firstPropertyButton, expectedColor) != true)
                Assert.Fail("The color on the map is incorrect.");

            //Opening the property on the map
            System.Threading.Thread.Sleep(2000);

            //Getting property details in search page
            //firstPropertyButton.Click();
            //firstPropertySearchDetails = getFirstPropertyDetails();
            firstPropertyButton.Click();
            firstPropertyMapDetails = getFirstPropertyDetailsMap();

            //for (int i = 0; i < 6; i++)
            //{
            //    if (i == 1) continue;
            //    if (firstPropertySearchDetails[i] != firstPropertyMapDetails[i])
            //        Assert.Fail("Search and Map details do not match. Search Info: " + firstPropertySearchDetails[i] + " Map Info:" + firstPropertyMapDetails[i]);
            //}
            System.Threading.Thread.Sleep(20000);

        }

        private string[] getFirstPropertyDetailsMap()
        {
            IWebElement map = webDriver.FindElement(By.ClassName("_1q6k59c"));
            IWebElement parent = map.FindElement(By.ClassName("_1jqckyi"));
            List<IWebElement> spans = new List<IWebElement>(map.FindElements(By.CssSelector("span")));

            IWebElement mapParentDiv = webDriver.FindElement(By.ClassName("_1x0fg6n"));
            string[] details = new string[6];

            TimeSpan t = new TimeSpan(0,0,30);
            WebDriverWait wait = new WebDriverWait(webDriver, t);
            //wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("_olc9rf0")));

            Assert.Fail(spans[2].Text);
            details[0] = spans[2].Text;
            details[2] = (mapParentDiv.FindElement(By.ClassName("_1isz8pdq"))).Text;
            details[3] = (mapParentDiv.FindElement(By.ClassName("_11ry7lz"))).Text;
            details[4] = (mapParentDiv.FindElement(By.ClassName("_a7a5sx"))).Text;
            details[5] = (mapParentDiv.FindElement(By.ClassName("_16shi2n"))).Text;

            return details;
        }

        private string[] getFirstPropertyDetails()
        {
            IWebElement parent = webDriver.FindElement(By.ClassName("_1jqckyi"));
            List<IWebElement> spans = new List<IWebElement>(parent.FindElements(By.CssSelector("span")));

            string[] details = new string[6];
            System.Threading.Thread.Sleep(2000);

            TimeSpan t = new TimeSpan(0, 0, 30);
            WebDriverWait wait = new WebDriverWait(webDriver, t);
            //wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("_olc9rf0")));

            details[0] = spans[2].Text;
            details[1] = (webDriver.FindElement(By.ClassName("_b14dlit"))).Text;
            details[2] = (webDriver.FindElement(By.ClassName("_bzh5lkq"))).Text;
            details[3] = (webDriver.FindElement(By.ClassName("_10fy1f8"))).Text;
            details[4] = (webDriver.FindElement(By.ClassName("_a7a5sx"))).Text;
            details[5] = (webDriver.FindElement(By.ClassName("_16shi2n"))).Text;

            return details;
        }

        private bool checkColor(IWebElement firstPropertyButton, string expectedColor)
        {
            string displayedColor = firstPropertyButton.GetCssValue("color");
            if (displayedColor != expectedColor)
                return false;
            return true;
        }

        private string getFirstPropertyName()
        {
            IWebElement firstPropertyNameDiv = webDriver.FindElement(By.ClassName("_8s3ctt"));
            IWebElement firstPropertyLink = firstPropertyNameDiv.FindElement(By.ClassName("_gjfol0"));
            return firstPropertyLink.GetAttribute("aria-label");
        }

        private IWebElement getFirstPropertyFromMap(string firstPropertyName)
        {
            IWebElement firstPropertyButton = null;
            IWebElement mapButtonsParentDiv = webDriver.FindElement(By.ClassName("gm-style"));
            List<IWebElement> mapButtons = new List<IWebElement>(mapButtonsParentDiv.FindElements(By.TagName("Button")));
            //getFirstPropertyFromMap(firstPropertyButton);
            foreach (IWebElement button in mapButtons)
            {
                if (button.GetAttribute("aria-label").Contains(firstPropertyName))
                {
                    firstPropertyButton = button;
                    break;
                }
            }
            return firstPropertyButton;
        }

        [TearDown]
        public void EndTest()
        {
            //I had an issue with chromedriver.exe so I put close+quit both which helped in addition to killing the task from cmd if it keeps running in the background 
            webDriver.Close();
            webDriver.Quit();
        }
    }
}
