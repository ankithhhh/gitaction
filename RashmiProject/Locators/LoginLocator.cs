using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace RashmiProject.Locators
{
    public class LoginLocator
    {
        public static By usernameField = By.XPath("//input[@id='user-name']");
        public static By passwordField = By.XPath("//input[@id='password']");
        public static By loginField = By.XPath("//input[@id='login-button']");
        public static By homepage = By.XPath("//div[@class='app_logo']");
        /*public static By item = By.XPath("//div[normalize-space()='Sauce Labs Backpack']");
        public static By item_details = By.XPath("//button[@id='back-to-products']");*/
       /* public static By addtocart = By.XPath("//button[@id='add-to-cart']");
        public static By cart = By.XPath("//a[@class='shopping_cart_link']");
        public static By usercart = By.XPath("//span[@class='title']");*/
       /* public static By checkout = By.XPath("//button[@id='checkout']");
        public static By checkoutpage = By.XPath("//span[@class='title']");
        public static By firstname = By.XPath("//input[@id='first-name']");
        public static By lastname = By.XPath("//input[@id='last-name']");
        public static By zipcode = By.XPath("//input[@id='postal-code']");
        public static By countinue = By.XPath("//input[@id='continue']");*/
       /* public static By checkoutoverview = By.XPath("//span[@class='title']");
        public static By finish = By.XPath("//button[@id='finish']");
        public static By orderconfirm = By.XPath("//h2[normalize-space()='Thank you for your order!']");*/
    }
}
