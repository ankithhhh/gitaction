using System;
using System.IO;
using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace RashmiProject.Utilities
{
    [Binding]
    public class Hooks
    {
        public static IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;
        private static ExtentReports _extent;
        private static ExtentTest _feature;
        private ExtentTest _scenario;
        private static ExtentSparkReporter _sparkReporter;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "ExtentReport.html");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

            _sparkReporter = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(_sparkReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _feature = _extent.CreateTest($"Feature: {featureContext.FeatureInfo.Title}");
        }

        [BeforeScenario]
        public void Setup()
        {
            TestContext.Progress.WriteLine("Initializing WebDriver...");

            if (driver == null)
            {
                driver = new ChromeDriver();
            }

            _scenarioContext["WebDriver"] = driver;
            _scenario = _feature.CreateNode($"Scenario: {_scenarioContext.ScenarioInfo.Title}");
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            string stepText = _scenarioContext.StepContext.StepInfo.Text;
            string screenshotPath = CaptureScreenshot(_scenarioContext.ScenarioInfo.Title, stepText);

            if (_scenarioContext.TestError == null)
            {
                _scenario.Log(Status.Pass, stepText);
            }
            else
            {
                _scenario.Log(Status.Fail, stepText);
                _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);

                // ✅ Attach Screenshot *Below* the Failed Test Case
                if (screenshotPath != null)
                {
                    _scenario.Fail("Screenshot of the failed step:", 
                        MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
            }
        }

        [AfterScenario]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _extent.Flush();
        }

        // ✅ FIX: Save Screenshots Inside "Reports/Screenshots" for Organization
        private string CaptureScreenshot(string scenarioName, string stepName)
        {
            try
            {
                if (driver == null || driver.WindowHandles.Count == 0)
                {
                    TestContext.Progress.WriteLine("No active browser window. Skipping screenshot.");
                    return null;
                }

                Thread.Sleep(500); // Small delay before capturing

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                // ✅ Ensure Screenshots Folder Exists Inside Reports
                string screenshotFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Screenshots");
                Directory.CreateDirectory(screenshotFolder);

                // ✅ Use Scenario Name + Step Name for Screenshot File Name
                string sanitizedStepName = string.Join("_", stepName.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(screenshotFolder, $"{scenarioName}_{sanitizedStepName}.png");

                // ✅ Save Screenshot
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
                TestContext.Progress.WriteLine($"Screenshot saved at: {filePath}");

                return filePath;
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to capture screenshot: {ex.Message}");
                return null;
            }
        }
    }
}
