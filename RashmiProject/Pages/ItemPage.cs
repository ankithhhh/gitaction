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
    internal class ItemPage
    {
        private IWebDriver driver =Hooks.driver ;

        /*By item = By.XPath("//div[normalize-space()='Sauce Labs Backpack']");
        By item_details = By.XPath("//button[@id='back-to-products']");*/
        public void Item()
        {
            driver.FindElement(ItemLocator.item).Click();
        }

        public void ItemDetail()
        {
            if (driver.FindElement(ItemLocator.item_details).Text == "Back to products")
            {
                Console.WriteLine("User In Product detail");
            }

            else
            {
                Console.WriteLine("User Not in Product detail");
            }
        }
    }
}
