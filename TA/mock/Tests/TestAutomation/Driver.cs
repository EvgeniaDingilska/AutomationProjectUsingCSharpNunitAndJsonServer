
using System.Collections.Generic;
using System.Drawing;
using OpenQA.Selenium;

namespace TestAutomation;

public abstract class Driver
{
    public abstract void Start(Browser browser);
    public abstract void Quit();
    public abstract void GoToUrl(string url);
    public abstract Element FindElement(By locator);
    public abstract List<Element> FindElements(By locator);
    public abstract void WaitForAjax();
    public abstract void WaitUntilPageLoadsCompletely();
}
