using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace TestProject1;

[TestFixture]
public class TestBasics
{
    protected IWebDriver? driver;
    protected static ExtentReports Reports;
    protected static ExtentTest Test;
    protected static string reportPath;

    //Ignores ads because fuck em
    protected void JsClick(IWebElement element)

    {
        //Throws a meaningful exception if either (driver and element) is null. Will crash if it fails and will throw error code.
        ArgumentNullException.ThrowIfNull(driver);
        ArgumentNullException.ThrowIfNull(element);
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
    }

    [OneTimeSetUp]
    public void SingleUse()
    {
        //ToDo: look into how to check if a file is present, check to see the version, and then increment the version file name by 1.
        //Creates report and determines its location
        reportPath = Path.Combine(System.Environment.ExpandEnvironmentVariables("%userprofile%/downloads/"), "TestResults.html");
        var reporter = new ExtentSparkReporter(reportPath);
        Reports = new ExtentReports();
        Reports.AttachReporter(reporter);
    }

    [OneTimeTearDown]
    public void SingleUseTear()
    {
        Reports.Flush();
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = reportPath,
            UseShellExecute = true
        });
    }

    [SetUp]
    public void Setup()
    {

       

        //Runs before EACH test
        driver = new FirefoxDriver();

        ArgumentNullException.ThrowIfNull(driver);
        driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

        //Creates the object which will keep track of test results
        Test = Reports.CreateTest(TestContext.CurrentContext.Test.Name);
    }

    [TearDown]
    public void Teardown()
    {
        //Checking if exceptions are null
        ArgumentNullException.ThrowIfNull(driver);
        //Retrieves test status from run - if failed takes a screenshot.
        //If the test passes the status will be updated alone
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            //Log the actual failure reason fron Nunit before taking the screenshot
            Test.Log(Status.Fail, $"Assertion failed: {message}");

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            Test.Fail("Test failed — see screenshot")
                .AddScreenCaptureFromBase64String(screenshot.AsBase64EncodedString);
        }
        else
        {
            Test.Pass("Test passed");
        }

        //Runs after each test
        //Disposes of previous cache before a new test is run
        driver?.Quit();
        driver?.Dispose();
        driver = null;

    }

    protected IWebElement findElementByCSS(string selector)
    {
        //Checking if exceptions are null
        ArgumentNullException.ThrowIfNull(driver);
        IWebElement foundCSSElement = driver.FindElement(By.CssSelector(selector));
        return foundCSSElement;
    }

    protected IWebElement findElementByXPath(string xpath)
    {
        //Checking if exceptions are null
        ArgumentNullException.ThrowIfNull(driver);
        IWebElement foundXPathElement = driver.FindElement(By.XPath(xpath));
        return foundXPathElement;
    }

    protected ReadOnlyCollection < IWebElement> findElementSByXPath(string xPathMulti)
    {
        //Checking if exceptions are null
        ArgumentNullException.ThrowIfNull(driver);
        var foundMultipleEle = driver.FindElements(By.XPath(xPathMulti));
        return foundMultipleEle;

    }

    protected void CheckDriverNull()
    {
        //Throws a meaningful exception if either (driver and element) is null. Will crash if it fails and will throw error code.
        //If null exception is in a method the warnings will still show up - need to find a fix for later
        ArgumentNullException.ThrowIfNull(driver);
    }
 
}
