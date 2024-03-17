using NUnit.Framework;
using System.Diagnostics;

namespace TestsReadabilityDemos;

[TestFixture]
[SingleThreadedAttribute]
public class TestBase
{
    [SetUp]
    public static void Setup()
    {
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {


    }


    [TearDown]
    public static void Cleanup()
    {
        // Specify the process name to be killed
        List<string> processNames = new List<string> { "chromedriver", "chrome", "geckodriver", "firefox", "MicrosoftWebDriver", "msedge" };
        Process[] processes;

        foreach (var process in processNames)
        {
            processes = Process.GetProcessesByName(process);
            foreach (var p in processes)
            {
                p.Kill();
                Console.WriteLine($"Process {p} killed.");
            }
        }

        //if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
        //{
        //    driver.TakeScreenShot();
        //}
    }
}
