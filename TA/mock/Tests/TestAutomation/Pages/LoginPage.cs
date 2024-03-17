using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestAutomation.Section;

namespace TestAutomation.Pages;

public class LoginPage : BasePage
{
    
    public LoginPage(Driver driver)
        : base(driver)
    {
        BreadcrumbSection = new BreadcrumbSection(driver);
    }

    public BreadcrumbSection BreadcrumbSection { get; }

    //protected override string Url => "http://localhost:3000/login";

    public Element Title => Driver.FindElement(By.TagName("h1"));
    private Element ServerInputField => Driver.FindElement(By.CssSelector("#ServerUrl"));
    private Element UserNameInputField => Driver.FindElement(By.CssSelector("#Username"));
    private Element PasswordInputField => Driver.FindElement(By.CssSelector("#Password"));
    private Element LoginButton => Driver.FindElement(By.CssSelector("#LoginButton"));
    
    public void LogIn(string host, string userName, string password) 
    {
        ServerInputField.TypeText(host);
        UserNameInputField.TypeText(userName);
        PasswordInputField.TypeText(password);

        LoginButton.Click();
    }

    public Element Sites => Driver.FindElement(By.CssSelector("#simple-tab-0"));
    public Element BasicUsers => Driver.FindElement(By.CssSelector("#simple-tab-1"));
    public Element CameraGroups => Driver.FindElement(By.CssSelector("#simple-tab-2"));

}
