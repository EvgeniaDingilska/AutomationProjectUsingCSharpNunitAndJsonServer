using OpenQA.Selenium;

namespace TestAutomation.Section;

public class MainMenuSection
{
    private readonly Driver _driver;

    public MainMenuSection(Driver driver)
    {
        _driver = driver;
    }

    public Element HomeLink => _driver.FindElement(By.CssSelector("#HomeLink"));
    public Element AboutLink => _driver.FindElement(By.CssSelector("#AboutLink"));
    public Element LoginLink => _driver.FindElement(By.CssSelector("#LoginLink"));
    public Element DashboardLink => _driver.FindElement(By.CssSelector("#DashboardLink"));

    public void OpenHomePage()
    {
        HomeLink.Click();
    }

    public void GoToAboutPage()
    {
        AboutLink.Click();
    }

    public void GoToLoginPage()
    {
        LoginLink.Click();
    }

    public void GoToDashboardPage()
    {
        DashboardLink.Click();
    }
}
