using NUnit.Framework;
using NUnit.Framework.Internal;
using TestsReadabilityDemos;

namespace TestAutomation;

[TestFixture]
public class LoginTest : TestBase
{
    [Test]
    [TestCase(Browser.Chrome)]
    [TestCase(Browser.Firefox)]
    [TestCase(Browser.Edge)]
    public void VerifyPages(Browser browser)
    {
        var basePage = BasePage.GoToHomePage(browser);
 
        //Verify the main menu links
        basePage.WebDriver.TakeScreenShot("Before login" + Test.IdPrefix + browser);
        var textLogin = basePage.HomePage.MainMenuSection.LoginLink.Text;
        var textAbout = basePage.HomePage.MainMenuSection.AboutLink.Text;
        var textHome = basePage.HomePage.MainMenuSection.HomeLink.Text;
        var textDashboard = basePage.HomePage.MainMenuSection.DashboardLink.Text;
 
        Assert.That(textAbout, Is.EqualTo("About"), "Incorrect About link text");
        Assert.That(textHome, Is.EqualTo("Home"), "Incorrect Home link text");
 
        //go to About page and verify title and notification button
        basePage.HomePage.MainMenuSection.GoToAboutPage();
        basePage.WebDriver.TakeScreenShot("About page"+ Test.IdPrefix + browser);
 
        var AboutTitle = basePage.AboutePage.Title;
        var AboutButtonText = basePage.AboutePage.NotificationButton.Text;
        Assert.That(AboutTitle, Is.EqualTo("About Page"), "Incorrect About title text");
        Assert.That(AboutButtonText, Is.EqualTo("Notification"), "Incorrect Notification button text");
 
        basePage.AboutePage.CreateNotification();
    }
}
