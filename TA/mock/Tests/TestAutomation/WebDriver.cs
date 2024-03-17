using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace TestAutomation;

public class WebDriver : Driver
{
    public IWebDriver _webDriver;
    private WebDriverWait _webDriverWait;

    public static string GetDriversPath(string path)
    {
        var basePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath.Split(new string[] { "\\TestAutomation" }, StringSplitOptions.None)[0];
        var filePath = Path.Combine(basePath, path);
        return filePath;
    }

    public static string chromePath = @"\chromedriver.exe";
    public static string chromeExePath = GetDriversPath($"Drivers") + chromePath;

    public static string fireFoxPath = @"\geckodriver.exe";
    public static string fireFoxExePath = GetDriversPath($"Drivers") + fireFoxPath;

    public static string edgePath = @"\msedgedriver.exe";
    public static string edgeExePath = GetDriversPath($"Drivers") + edgePath;

    public override void Start(Browser browser)
    {
        switch (browser)
        {
            case Browser.Chrome:
                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                var optionsChrome = new ChromeOptions
                {
                    AcceptInsecureCertificates = true,
                };
                optionsChrome.AddArgument("--disable-extensions");
                optionsChrome.AddArgument("no-sandbox");
                optionsChrome.AddArgument("--ignore-certificate-errors");
                optionsChrome.AddUserProfilePreference("download.default_directory", @"C:\Downoads");
                //optionsChrome.AddArgument("--headless");
                _webDriver = new ChromeDriver(chromeExePath, optionsChrome);
                break;
            case Browser.Firefox:
                new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                var optionsFireFox = new FirefoxOptions
                {
                    AcceptInsecureCertificates = true,
                };

                optionsFireFox.SetPreference("plugins.hide_infobar_for_missing_plugin", true);
                optionsFireFox.SetPreference("plugin.default.state", 0);
                optionsFireFox.SetPreference("media.navigator.permission.disabled", true);
                optionsFireFox.SetPreference("permissions.default.microphone", 1);
                optionsFireFox.SetPreference("permissions.default.camera", 1);
                optionsFireFox.SetPreference("browser.link.open_newwindow", 3);
                //optionsFireFox.AddArgument("--headless");

                optionsFireFox.SetPreference("browser.download.dir", @"C:\Downoads");
                optionsFireFox.SetPreference("browser.download.folderList", 2);
                optionsFireFox.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/zip, application/octet-stream");
                _webDriver = new FirefoxDriver(fireFoxExePath, optionsFireFox);
                               
                break;
            case Browser.Edge:
                new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                var optionsEdge = new EdgeOptions
                {
                    AcceptInsecureCertificates = true,
                };
                optionsEdge.AddArgument("--disable-extensions");
                optionsEdge.AddArgument("no-sandbox");
                optionsEdge.AddArgument("--ignore-certificate-errors");
                //optionsEdge.AddArgument("--headless");
                optionsEdge.AddUserProfilePreference("download.default_directory", @"C:\Downoads");
                _webDriver = new EdgeDriver(edgeExePath, optionsEdge);
                break;
            case Browser.Safari:
                _webDriver = new SafariDriver(Environment.CurrentDirectory);
                break;
            case Browser.InternetExplorer:
                new WebDriverManager.DriverManager().SetUpDriver(new InternetExplorerConfig());
                _webDriver = new InternetExplorerDriver(Environment.CurrentDirectory);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(browser), browser, null);
        }

        _webDriver.Manage().Window.Maximize();

        _webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(30));
    }

    public override void Quit()
    {
        _webDriver.Quit();
    }

    public override void GoToUrl(string url)
    {
        _webDriver.Navigate().GoToUrl(url);
    }

    public override Element FindElement(By locator)
    {
        IWebElement nativeWebElement = 
            _webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
        Element element = new WebElement(_webDriver, nativeWebElement, locator);

        // If we use log decorator.
        Element logElement = new LogElement(element);

        return logElement;
    }

    public override List<Element> FindElements(By locator)
    {
        ReadOnlyCollection<IWebElement> nativeWebElements = 
            _webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
        var elements = new List<Element>();
        foreach (var nativeWebElement in nativeWebElements)
        {
            Element element = new WebElement(_webDriver, nativeWebElement, locator);
            elements.Add(element);
        }

        return elements;
    }

    public override void WaitForAjax()
    {
        var js = (IJavaScriptExecutor)_webDriver;
        _webDriverWait.Until(wd => js.ExecuteScript("return jQuery.active").ToString() == "0");
    }

    public override void WaitUntilPageLoadsCompletely()
    {
        var js = (IJavaScriptExecutor)_webDriver;
        _webDriverWait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
    }


    public static string GetRoute(string file)
    {
        string name = string.Empty;
        try
        {
            name = GetDriversPath($"Screenshots") +"\\"+ file + ".png";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

        }

        return name;

    }

    public void SaveImage(Image image, string name)
    {
        try
        {
            if (File.Exists(GetRoute(name)))
            {
                File.Delete(GetRoute(name));
            }

            image.Save(GetRoute(name), System.Drawing.Imaging.ImageFormat.Png);
            image.Dispose();

            Console.WriteLine("Save image");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    public Image byteArrayToImage(byte[] byteArrayIn)
    {

        MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
        ms.Write(byteArrayIn, 0, byteArrayIn.Length);
        return Image.FromStream(ms);

    }

    public void TakeScreenShot(string name)
    {
        Screenshot ss = ((ITakesScreenshot)_webDriver).GetScreenshot();

        byte[] screenshotAsByteArray = ss.AsByteArray;

        var a = byteArrayToImage(screenshotAsByteArray);
        Console.WriteLine("saving image");
        
        SaveImage(a, name);
        Console.WriteLine("Ready");
    }
}
