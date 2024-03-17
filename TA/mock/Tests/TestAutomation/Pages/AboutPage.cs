using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.Pages;

public class AboutPage : BasePage
{
    public AboutPage(Driver driver)
        : base(driver)
    {
    }

    public string Title => Driver.FindElement(By.CssSelector(".XoMdyuU_zFPxAYxWGd4d")).Text;
    public Element NotificationButton => Driver.FindElement(By.CssSelector(".R3vHBoNppBwcCyJeEQMl"));
   
    public void CreateNotification()
    {
        NotificationButton.Click();
    }
}
