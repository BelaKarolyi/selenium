using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;

namespace FULABV_Sauce_Firefox
{
    [TestClass]
    public sealed class SauceTests
    {
        IWebDriver? driver;

        [TestInitialize]
        public void Setup()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();

            /* weboldalt megnyitom */
            driver.Navigate().GoToUrl("https://www.saucedemo.com");
        }

        [TestCleanup]
        public void Teardown() /* bezárja a drivert */
        {
            Thread.Sleep(3000);
            driver.Quit();
        }

        [TestMethod]
        public void SauceBuyTest()
        {
            /* ellenőrizzük az oldalt */
            Assert.AreEqual("Swag Labs", driver.Title);

            /* belépés, most standard_userrel, nem akarunk galibát problem_userrel :) */
            var usernameField = driver.FindElement(By.Id("user-name"));
            usernameField.Clear();
            usernameField.SendKeys("standard_user");
            Thread.Sleep(800);

            var passwordField = driver.FindElement(By.Name("password"));
            passwordField.Clear();
            passwordField.SendKeys("secret_sauce");
            Thread.Sleep(800);
            var loginButton = driver.FindElement(By.Id("login-button"));
            loginButton.Click();

            /* inventory megjelent? */
            var inventoryContainer = driver.FindElement(By.Id("inventory_container"));
            Assert.IsTrue(inventoryContainer.Displayed);
            Thread.Sleep(800);
            /* vegyük meg a legolcsóbb terméket! */
            var sortDropdown = driver.FindElement(By.ClassName("product_sort_container"));
            sortDropdown.Click();
            Thread.Sleep(800);
            var lowToHighOption = driver.FindElement(By.XPath("//option[@value='lohi']"));
            lowToHighOption.Click();
            Thread.Sleep(1000); /* adjunk időt a sorbarendezésnek */
            Thread.Sleep(800);
            /* kosárba rakom */
            var firstAddToCartButton = driver.FindElement(By.XPath("(//div[@class='inventory_item']//button)[1]"));
            firstAddToCartButton.Click();
            Thread.Sleep(800);
            /* kosár megkeresése shopping-cart alapján */
            var cartContainer = driver.FindElement(By.Id("shopping_cart_container"));

            /* ellenőrizhető, valóban 1 termék van-e a kosárban */
            var cartBadge = cartContainer.FindElement(By.ClassName("shopping_cart_badge"));
            Assert.AreEqual("1", cartBadge.Text);
            cartContainer.Click();
            Thread.Sleep(800);
            /* checkout */
            var checkoutButton = driver.FindElement(By.Id("checkout"));
            checkoutButton.Click();
            Thread.Sleep(800);
            /* megrendelő adatai */
            var firstNameField = driver.FindElement(By.Id("first-name"));
            firstNameField.Click();
            firstNameField.SendKeys("FULABV");
            /* tabbal lépünk tovább */
            firstNameField.SendKeys(Keys.Tab);
            Thread.Sleep(800);

            var lastNameField = driver.FindElement(By.Id("last-name"));
            lastNameField.Click();
            lastNameField.Clear(); /* biztos, ami tuti */
            lastNameField.SendKeys("bb");
            lastNameField.SendKeys(Keys.Tab);
            Thread.Sleep(800);

            var postalCodeField = driver.FindElement(By.Id("postal-code"));
            postalCodeField.Click();
            postalCodeField.Clear();
            postalCodeField.SendKeys("1214");
            Thread.Sleep(800);

            /* continue gomb */
            var continueButton = driver.FindElement(By.Id("continue"));
            continueButton.Click();
            Thread.Sleep(1500);
            /* továbbmentünk az összegző oldalra? */
            Assert.Contains("checkout-step-two.html", driver.Url);

            /* finish gomb */
            var finishButton = driver.FindElement(By.Id("finish"));
            finishButton.Click();
            Thread.Sleep(1500);
            /* visszaigazolták a rendelést? */
            var completeHeader = driver.FindElement(By.ClassName("complete-header"));
            Assert.AreEqual("Thank you for your order!", completeHeader.Text);

            /* back gomb */
            var backHomeButton = driver.FindElement(By.Id("back-to-products"));
            backHomeButton.Click();
            Thread.Sleep(800);
            /* hamburgermenü */
            var burgerMenuButton = driver.FindElement(By.Id("react-burger-menu-btn"));
            burgerMenuButton.Click();
            Thread.Sleep(1000); /* menü animációnak adunk időt */

            /* logout gomb */
            var logoutLink = driver.FindElement(By.Id("logout_sidebar_link"));
            logoutLink.Click();
            Thread.Sleep(800);
            /* ha működött a kijelentkezés, akkor látni fogjuk a login gombot */
            var loginButtonAfterLogout = driver.FindElement(By.Id("login-button"));
            Assert.IsTrue(loginButtonAfterLogout.Displayed);
        }
    }
}