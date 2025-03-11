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
    internal class AddCart
    {
        private IWebDriver driver = Hooks.driver;
        /*By addtocart = By.XPath("//button[@id='add-to-cart']");
        By cart = By.XPath("//a[@class='shopping_cart_link']");
        By usercart = By.XPath("//span[@class='title']");*/

        public void AddToCart()
        {
            driver.FindElement(AddtocartLocator.addtocart).Click();
        }

        public void Cart()
        {
            driver.FindElement( AddtocartLocator.cart).Click();

        }

        public void UserCart()
        {
            if (driver.FindElement(AddtocartLocator.usercart).Text == "Your Cart")
            {
                Console.WriteLine("User In UserCArt");
            }

            else
            {
                Console.WriteLine("User Not in Usercart");
            }

        }
    }
}
