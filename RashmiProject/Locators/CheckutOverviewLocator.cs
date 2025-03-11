using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace RashmiProject.Locators
{
    internal class CheckutOverviewLocator
    {
        public static By checkoutoverview = By.XPath("//span[@class='title']");
        public static By finish = By.XPath("//button[@id='finish']");
        public static By orderconfirm = By.XPath("//h2[normalize-space()='Thank you for your order!']");
    }
}
