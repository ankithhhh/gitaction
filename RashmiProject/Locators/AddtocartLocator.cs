using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace RashmiProject.Locators
{
    public  class AddtocartLocator
    {
        public static By addtocart = By.XPath("//button[@id='add-to-cart']");
        public static By cart = By.XPath("//a[@class='shopping_cart_link']");
        public static By usercart = By.XPath("//span[@class='title']");
    }
}
