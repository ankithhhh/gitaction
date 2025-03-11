using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace RashmiProject.Locators
{
    public class ItemLocator
    {
        public static By item = By.XPath("//div[normalize-space()='Sauce Labs Backpack']");
        public static By item_details = By.XPath("//button[@id='back-to-products']");
    }
}
