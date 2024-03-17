using System.Xml.Linq;
using TestAutomation.Pages;
using TestAutomation.Section;

namespace TestAutomation;

public class BasePage
{
    public Driver Driver;
    public const string DateTimeFormat = "yyyy-MM-d h:mm:ss tt";

    public const string DateFormat = "yyyy-MM-d";
    public const string HourMinuteFormat = "h:mm tt";
    public const string UrlAddress = "http://localhost:3000";

    protected BasePage(Driver driver)
    {
        Driver = driver;
        SearchSection = new SearchSection(driver);
        MainMenuSection = new MainMenuSection(driver);
    }

    protected BasePage(Browser browserType)
    {
        WebDriver = new WebDriver();
        WebDriver.Start(browserType); 

        Login = new Login(this);
        HomePage = new HomePage(WebDriver);
        LoginPage = new LoginPage(WebDriver);
        AboutePage = new AboutPage(WebDriver);
        
    }

    protected BasePage(Browser browserType, string urlAddress)
    {
        WebDriver = new WebDriver();
        WebDriver.Start(browserType);
        WebDriver.GoToUrl(urlAddress);
    }

    public HomePage HomePage { get;}
    public LoginPage LoginPage { get;}
    public AboutPage AboutePage { get;}
    public Login Login { get;}
    public SearchSection SearchSection { get; }
    public MainMenuSection MainMenuSection { get; }
    public WebDriver WebDriver { get; }

    public static BasePage Start(Browser browserType)
    {
        BasePage basePage = null;

        basePage = new BasePage(browserType);
        return basePage;
    }
    public static BasePage GoToHomePageAndLogin(Browser browser)
    {
        BasePage basePage = null;

        basePage = Start(browser);
        basePage.Login.GoToHomePage();
        basePage.Login.GoToLoginPage();

        return basePage;
    }

    public static BasePage GoToHomePage(Browser browser)
    {
        BasePage basePage = null;

        basePage = Start(browser);
        basePage.Login.GoToHomePage();

        return basePage;
    }
    //protected abstract string Url { get; }

    //public void Open()
    //{
    //    Driver.GoToUrl(Url);
    //}
}
