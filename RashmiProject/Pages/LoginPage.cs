using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RashmiProject.Utilities;
using RashmiProject.Locators;

namespace RashmiProject.Pages
{
    internal class LoginPage
    {
        private  IWebDriver driver=Hooks.driver;

        // Modify constructor to get WebDriver from ScenarioContext
        /*public LoginPage(ScenarioContext scenarioContext)
        {
            driver = scenarioContext.Get<IWebDriver>("WebDriver"); // Get WebDriver from ScenarioContext
        }*/
        

        /*By usernameField = By.XPath("//input[@id='user-name']");
        By passwordField = By.XPath("//input[@id='password']");
        By loginField = By.XPath("//input[@id='login-button']");
        By homepage = By.XPath("//div[@class='app_logo']");*/
       /* By item = By.XPath("//div[normalize-space()='Sauce Labs Backpack']");
        By item_details = By.XPath("//button[@id='back-to-products']");*/
       /* By addtocart = By.XPath("//button[@id='add-to-cart']");
        By cart = By.XPath("//a[@class='shopping_cart_link']");
        By usercart = By.XPath("//span[@class='title']");*/
       /* By checkout = By.XPath("//button[@id='checkout']");
        By checkoutpage = By.XPath("//span[@class='title']");
        By firstname = By.XPath("//input[@id='first-name']");
        By lastname = By.XPath("//input[@id='last-name']");
        By zipcode = By.XPath("//input[@id='postal-code']");
        By countinue = By.XPath("//input[@id='continue']");*/
       /* By checkoutoverview = By.XPath("//span[@class='title']");
        By finish = By.XPath("//button[@id='finish']");
        By orderconfirm = By.XPath("//h2[normalize-space()='Thank you for your order!']");*/

        public void LaunchBrowser()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        public void EnterUserNames(string username)
        {
            driver.FindElement(LoginLocator.usernameField).SendKeys(username);


        }
        public void EnterPassowrd(string password)
        {
            driver.FindElement(LoginLocator.passwordField).SendKeys(password);
        }
        public void Submit()
        {
            driver.FindElement(LoginLocator.loginField).Click();
        }

        public void HomePageDisplay()
        {


            if (driver.FindElement(LoginLocator.homepage).Text == "Swag Labs")
            {
                Console.WriteLine("User autincation passed");
            }

            else
            {
                Console.WriteLine("User autincation failed");
            }

        }
       /* public void Item()
        {
            driver.FindElement(item).Click();
        }

        public void ItemDetail()
        {
            if (driver.FindElement(item_details).Text == "Back to products")
            {
                Console.WriteLine("User In Product detail");
            }

            else
            {
                Console.WriteLine("User Not in Product detail");
            }
        }*/

       /* public void AddToCart()
        {
            driver.FindElement(addtocart).Click();
        }

        public void Cart()
        {
            driver.FindElement(cart).Click();

        }

        public void UserCart()
        {
            if (driver.FindElement(usercart).Text == "Your Cart")
            {
                Console.WriteLine("User In UserCArt");
            }

            else
            {
                Console.WriteLine("User Not in Usercart");
            }

        }*/

       /* public void CheckOut()
        {
            driver.FindElement(checkout).Click();
        }

        public void CheckOutpage()
        {
            if (driver.FindElement(checkoutpage).Text == "Checkout: Your Information")
            {
                Console.WriteLine("User In checkoutpage");
            }

            else
            {
                Console.WriteLine("User Not in checkout page");
            }
        }

        public void BuyerDetail(string fname, string lname, string zip)
        {
            driver.FindElement(firstname).SendKeys(fname);
            driver.FindElement(lastname).SendKeys(lname);
            driver.FindElement(zipcode).SendKeys(zip);

        }

        public void Countniue()
        {
            driver.FindElement(countinue).Click();
        }

*/
        /*public void CheckOutOverview()
        {
            if (driver.FindElement(checkoutoverview).Text == "Checkout: Overview")
            {
                Console.WriteLine("User In checkout Overview");
            }

            else
            {
                Console.WriteLine("User Not in checkout overview");
            }
        }

        public void Finish()
        {
            driver.FindElement(finish).Click();
        }

        public void OrderConfirm()
        {
            if (driver.FindElement(orderconfirm).Text == "Thank you for your order!")
            {
                Console.WriteLine("\"Thank you for your order!\"");
            }

            else
            {
                Console.WriteLine("Order was Not placed");
            }
        }*/
        


    }
}



