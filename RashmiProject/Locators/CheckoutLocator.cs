using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace RashmiProject.Locators
{
    internal class CheckoutLocator
    {
        public static By checkout = By.XPath("//button[@id='checkout']");
        public static By checkoutpage = By.XPath("//span[@class='title']");
        public static By firstname = By.XPath("//input[@id='first-name']");
        public static By lastname = By.XPath("//input[@id='last-name']");
        public static By zipcode = By.XPath("//input[@id='postal-code']");
        public static By countinue = By.XPath("//input[@id='continue']");
    }
}
