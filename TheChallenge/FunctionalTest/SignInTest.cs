using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Android;

namespace FunctionalTest
{
    [TestFixture]
    class SignInTest
    {
        private IWebDriver driver = null;
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void SignInWithValidUserIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            SignIn(driver, "unit-test", "unittest");
            ValidateSignInWithValidUser(driver);
        }

        [Test]
        public void AttemptSignInWithInvalidUserIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            SignIn(driver, "wrongname", "wrongpassword");
            ValidateAttemptSignInWithInvalidUser(driver);
        }

        [Test]
        public void AttemptSignInWithoutEnteringUserIdIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            SignIn(driver, pswd: "wrongpassword");
            ValidateAttemptSignInWithoutEnteringUserId(driver);
        }

        [Test]
        public void AttemptSignInWithoutEnteringPasswordIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            SignIn(driver, "wrongname");
            ValidateAttemptSignInWithoutEnteringPassword(driver);
        }

        [Test]
        public void SignInWithValidUserChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            SignIn(driver, "unit-test", "unittest");
            ValidateSignInWithValidUser(driver);
        }

        [Test]
        public void AttemptSignInWithInvalidUserChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            SignIn(driver, "wrongname", "wrongpassword");
            ValidateAttemptSignInWithInvalidUser(driver);
        }

        [Test]
        public void AttemptSignInWithoutEnteringUserIdChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            SignIn(driver, pswd: "wrongpassword");
            ValidateAttemptSignInWithoutEnteringUserId(driver);
        }

        [Test]
        public void AttemptSignInWithoutEnteringPasswordChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            SignIn(driver, "wrongname");
            ValidateAttemptSignInWithoutEnteringPassword(driver);
        }

        [Test]
        public void SignInWithValidUserFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            SignIn(driver, "unit-test", "unittest");
            ValidateSignInWithValidUser(driver);
        }

        [Test]
        public void AttemptSignInWithInvalidUserFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            SignIn(driver, "wrongname", "wrongpassword");
            ValidateAttemptSignInWithInvalidUser(driver);
        }

        [Test]
        public void AttemptSignInWithoutEnteringUserIdFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            SignIn(driver, pswd: "wrongpassword");
            ValidateAttemptSignInWithoutEnteringUserId(driver);
        }

        [Test]
        public void AttemptSignInWithoutEnteringPasswordFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            SignIn(driver, "wrongname");
            ValidateAttemptSignInWithoutEnteringPassword(driver);
        }

        private void NavigateToHomePage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://thechallenge.unit-testushakov.com");
        }

        private void SignIn(IWebDriver driver, String id = null, String pswd = null)
        {
            IWebElement signInWindow = driver.FindElement(By.Id("sign-in-window"));
            Assert.IsTrue(signInWindow.Displayed);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            if (!String.IsNullOrEmpty(id))
            {
                IWebElement userName = driver.FindElement(By.Id("sign-in-name"));
                userName.SendKeys(id);
            }

            if (!String.IsNullOrEmpty(pswd))
            {
                IWebElement password = driver.FindElement(By.Id("sign-in-password"));
                password.SendKeys(pswd);
            }

            IWebElement signInButton = driver.FindElement(By.Id("sign-in"));
            signInButton.Click();
        }

        private void ValidateAttemptSignInWithoutEnteringPassword(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            IWebElement error = driver.FindElement(By.XPath("//div[@id='sign-in-information']/span[1]"));
            Assert.AreEqual("Please enter your password", error.Text);
            Assert.IsTrue(error.Displayed);
        }

        private void ValidateAttemptSignInWithoutEnteringUserId(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            IWebElement error = driver.FindElement(By.XPath("//div[@id='sign-in-information']/span[1]"));
            Assert.AreEqual("Please enter your name", error.Text);
            Assert.IsTrue(error.Displayed);
        }

        private void ValidateAttemptSignInWithInvalidUser(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            IWebElement error = driver.FindElement(By.Id("sign-in-error"));
            Assert.AreEqual("Unable to log this user in. Please verify your credentials.", error.Text);
        }

        private void ValidateSignInWithValidUser(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            IWebElement signInUser = driver.FindElement(By.TagName("h2"));
            Assert.AreEqual("unit-test Ushakov", signInUser.Text);
            IWebElement statsList = driver.FindElement(By.XPath("//table[@id='currentLiftsTable']/tbody/tr[1]/td[1]/p"));
            Assert.AreEqual("EventType: MAX_LIFT", statsList.Text);

            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
        }
    }
}
