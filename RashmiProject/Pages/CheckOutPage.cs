using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using RashmiProject.Utilities;
using RashmiProject.Locators;

namespace RashmiProject.Pages
{
    internal class CheckOutPage
    {
        private IWebDriver driver = Hooks.driver;


/*
        By checkout = By.XPath("//button[@id='checkout']");
        By checkoutpage = By.XPath("//span[@class='title']");
        By firstname = By.XPath("//input[@id='first-name']");
        By lastname = By.XPath("//input[@id='last-name']");
        By zipcode = By.XPath("//input[@id='postal-code']");
        By countinue = By.XPath("//input[@id='continue']");*/

        public void CheckOut()
        {
            driver.FindElement(CheckoutLocator.checkout).Click();
        }

        public void CheckOutpage()
        {
            if (driver.FindElement(CheckoutLocator.checkoutpage).Text == "Checkout: Your Information")
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
            driver.FindElement(CheckoutLocator.firstname).SendKeys(fname);
            driver.FindElement(CheckoutLocator.lastname).SendKeys(lname);
            driver.FindElement(CheckoutLocator.zipcode).SendKeys(zip);

        }

        public void Countniue()
        {
            driver.FindElement(CheckoutLocator.countinue).Click();
        }


    }
}
