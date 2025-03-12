using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using RashmiProject.Utilities;
using RashmiProject.Locators;
using NUnit.Framework;



namespace RashmiProject.Pages
{
    internal class CheckOverviewPage
    {
        private IWebDriver driver = Hooks.driver;

       /* By checkoutoverview = By.XPath("//span[@class='title']");
        By finish = By.XPath("//button[@id='finish']");
        By orderconfirm = By.XPath("//h2[normalize-space()='Thank you for your order!']");*/

        public void CheckOutOverview()
        {
            if (driver.FindElement(CheckutOverviewLocator.checkoutoverview).Text == "Checkout: Overview")
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
            driver.FindElement(CheckutOverviewLocator.finish).Click();
        }

        public void OrderConfirm()
        {
            /*            if (driver.FindElement(CheckOutOverviewLocator.orderconfirm).Text == "Thank you for your o")
                        {
                            Console.WriteLine(driver.FindElement(CheckOutOverviewLocator.orderconfirm).Text);
                        }

                        else
                        {
                            Console.WriteLine("driver.FindElement(CheckOutOverviewLocator.orderconfirm).Text");
                        }
                        driver.Dispose();*/



           // NUnit.Framework.Assert.That(driver.FindElement(CheckutOverviewLocator.orderconfirm).Text,Is.EqualTo("Thank you for your o"));

        }

    }
}
