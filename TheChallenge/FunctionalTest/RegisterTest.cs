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
using Dapper;
using System.Data.SqlClient;
using System.Configuration;

namespace FunctionalTest
{
    [TestFixture]
    class RegisterTest
    {
        private IWebDriver driver = null;
        [TearDown]
        public void TearDown()
        {
            SqlConnection connection = null;
            using (connection= new SqlConnection(ConfigurationManager.ConnectionStrings["TheChallenge"].ConnectionString))
            {
                connection.Open();
                connection.Execute(@"delete from thechallenge.dimitryushakov.[user] where UserName = 'func_test'");
            }

            driver.Quit();
        }

        [Test]
        public void RegisterWithValidUserIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            Register(driver, "func_test", "functest", true, "functest");
            ValidateRegisterWithValidUser(driver);
        }

        [Test]
        public void AttemptRegisterWithNotConfirmedPasswordIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Register(driver, "wrongname", "wrongpassword", false);
            ValidateAttemptRegisterWithNotConfirmedPassword(driver);
        }

        [Test]
        public void AttemptRegisterWithoutEnteringUserIdIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Register(driver, pswd: "wrongpassword", confirmPswd: "wrongpassword");
            ValidateAttemptRegisterWithoutEnteringUserId(driver);
        }

        [Test]
        public void AttemptRegisterWithoutEnteringPasswordIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Register(driver, "wrongname", isConfirmCorrect: false);
            ValidateAttemptRegisterWithoutEnteringPassword(driver);
        }

        [Test]
        public void AttemptRegisterWithIncorrectConfirmedPasswordIETest()
        {
            driver = new InternetExplorerDriver(new InternetExplorerOptions() { IntroduceInstabilityByIgnoringProtectedModeSettings = true });
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Register(driver, "wrongname", "password", true, "notpassword");
            ValidateAttemptRegisterWithIncorrectConfirmedPassword(driver);
        }

        [Test]
        public void RegisterWithValidUserChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            Register(driver, "func_test", "functest", true, "functest");
            ValidateRegisterWithValidUser(driver);
        }

        [Test]
        public void AttemptRegisterWithNotConfirmedPasswordChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            Register(driver, "wrongname", "wrongpassword", false);
            ValidateAttemptRegisterWithNotConfirmedPassword(driver);
        }

        [Test]
        public void AttemptRegisterWithoutEnteringUserIdChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            Register(driver, pswd: "wrongpassword", confirmPswd: "wrongpassword");
            ValidateAttemptRegisterWithoutEnteringUserId(driver);
        }

        [Test]
        public void AttemptRegisterWithoutEnteringPasswordChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            Register(driver, "wrongname", isConfirmCorrect: false);
            ValidateAttemptRegisterWithoutEnteringPassword(driver);
        }

        [Test]
        public void AttemptRegisterWithIncorrectConfirmedPasswordChromeTest()
        {
            driver = new ChromeDriver();
            NavigateToHomePage(driver);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Register(driver, "wrongname", "password", true, "notpassword");
            ValidateAttemptRegisterWithIncorrectConfirmedPassword(driver);
        }

        [Test]
        public void RegisterWithValidUserFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            Register(driver, "func_test", "functest", true, "functest");
            ValidateRegisterWithValidUser(driver);
        }

        [Test]
        public void AttemptRegisterWithNotConfirmedPasswordFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            Register(driver, "wrongname", "wrongpassword", false);
            ValidateAttemptRegisterWithNotConfirmedPassword(driver);
        }

        [Test]
        public void AttemptRegisterWithoutEnteringUserIdFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            Register(driver, pswd: "wrongpassword", confirmPswd: "wrongpassword");
            ValidateAttemptRegisterWithoutEnteringUserId(driver);
        }

        [Test]
        public void AttemptRegisterWithoutEnteringPasswordFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            Register(driver, "wrongname", isConfirmCorrect: false);
            ValidateAttemptRegisterWithoutEnteringPassword(driver);
        }

        [Test]
        public void AttemptRegisterWithIncorrectConfirmedPasswordFirefoxTest()
        {
            driver = new FirefoxDriver();
            NavigateToHomePage(driver);
            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Register(driver, "wrongname", "password", true, "notpassword");
            ValidateAttemptRegisterWithIncorrectConfirmedPassword(driver);
        }

        private void NavigateToHomePage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://thechallenge.dimitryushakov.com");
        }

        private void Register(IWebDriver driver, String id = null, String pswd = null, bool isConfirmCorrect = true, String confirmPswd = null)
        {
            IWebElement registerWindow = driver.FindElement(By.Id("sign-in-window"));
            Assert.IsTrue(registerWindow.Displayed);
            IWebElement registerButton = driver.FindElement(By.Id("view-register"));
            registerButton.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            if (!String.IsNullOrEmpty(id))
            {
                IWebElement userName = driver.FindElement(By.Id("register-name"));
                userName.SendKeys(id);
            }

            if (!String.IsNullOrEmpty(pswd))
            {
                IWebElement password = driver.FindElement(By.Id("register-password"));
                password.SendKeys(pswd);
            }

            if (isConfirmCorrect)
            {
                IWebElement confirmPassword = driver.FindElement(By.Id("confirm-register-password"));
                confirmPassword.SendKeys(confirmPswd);
            }

            IWebElement registerClickButton = driver.FindElement(By.Id("register"));
            registerClickButton.Click();
        }

        private void ValidateAttemptRegisterWithoutEnteringPassword(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            IWebElement error = driver.FindElement(By.XPath("//div[@id='register-information']/span[1]"));
            Assert.AreEqual("Please enter your password", error.Text);
            Assert.IsTrue(error.Displayed);
        }

        private void ValidateAttemptRegisterWithIncorrectConfirmedPassword(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            IWebElement error = driver.FindElement(By.XPath("//div[@id='register-information']/span[1]"));
            Assert.AreEqual("Please verify that confirmed password and password match.", error.Text);
            Assert.IsTrue(error.Displayed);
        }

        private void ValidateAttemptRegisterWithoutEnteringUserId(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            IWebElement error = driver.FindElement(By.XPath("//div[@id='register-information']/span[1]"));
            Assert.AreEqual("Please enter your name", error.Text);
            Assert.IsTrue(error.Displayed);
        }

        private void ValidateAttemptRegisterWithNotConfirmedPassword(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            IWebElement registerWindow = driver.FindElement(By.Id("register-window"));
            Assert.IsTrue(registerWindow.Displayed);
        }

        private void ValidateRegisterWithValidUser(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            IWebElement signInUser = driver.FindElement(By.TagName("h2"));
            Assert.AreEqual("Dimitry Ushakov", signInUser.Text);
            IWebElement statsList = driver.FindElement(By.XPath("//table[@id='currentLiftsTable']/tbody/tr[1]/td[1]/p"));
            Assert.AreEqual("EventType: MAX_LIFT", statsList.Text);

            IWebElement signOut = driver.FindElement(By.Id("sign-out"));
            signOut.Click();
        }
    }
}
