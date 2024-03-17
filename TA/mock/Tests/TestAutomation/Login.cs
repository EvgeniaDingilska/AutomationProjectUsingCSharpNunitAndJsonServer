
using OpenQA.Selenium;
using TestAutomation;

public class Login
{
    public Login(BasePage basePage)
    {
        _basePage = basePage;
    }

    private readonly BasePage _basePage;
    private string _baseAddressUrl = "http://localhost:3000";
    
    public BasePage GoToLoginPageAndLogin(string username = "basic",
        string password = "!Milestonesys2")
    {
        GoToHomePage();

        _basePage.LoginPage.LogIn(_baseAddressUrl, username, password);

        return _basePage;
    }

    public void GoToHomePage()
    {
        _basePage.WebDriver.GoToUrl(_baseAddressUrl);
    }

    private Element LoginLink => _basePage.WebDriver.FindElement(By.CssSelector("#LoginLink"));

    //protected override string Url => "http://localhost:3000/";

    public void GoToLoginPage()
    {
        LoginLink.Click();
    }
}