
using System.Collections.Generic;
using System.Drawing;
using OpenQA.Selenium;

namespace TestAutomation;

public class DriverDecorator : Driver
{
    protected Driver Driver;

    protected DriverDecorator(Driver driver)
    {
        Driver = driver;
    }

    public override void Start(Browser browser)
    {
        Driver?.Start(browser);
    }

    public override void Quit()
    {
        Driver?.Quit();
    }
    public void QuitAllBrowsers()
    {
        
    }

    public override void GoToUrl(string url)
    {
        Driver?.GoToUrl(url);
    }

    public override Element FindElement(By locator)
    {
        return Driver?.FindElement(locator);
    }

    public override List<Element> FindElements(By locator)
    {
        return Driver?.FindElements(locator);
    }

    public override void WaitUntilPageLoadsCompletely()
    {
        Driver?.WaitUntilPageLoadsCompletely();
    }

    public override void WaitForAjax()
    {
        throw new System.NotImplementedException();
    }
    
}
