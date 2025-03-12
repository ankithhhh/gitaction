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
        public static IWebDriver? driver;  
        private readonly ScenarioContext _scenarioContext;
        private static ExtentReports? _extent;  
        private static ExtentTest? _feature;  
        private ExtentTest? _scenario;  
        private static ExtentSparkReporter? _sparkReporter;  

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "ExtentReport.html");

            string? directoryPath = Path.GetDirectoryName(reportPath);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            _sparkReporter = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(_sparkReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            if (_extent != null)
            {
                _feature = _extent.CreateTest($"Feature: {featureContext.FeatureInfo.Title}");
            }
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
            if (_feature != null)
            {
                _scenario = _feature.CreateNode($"Scenario: {_scenarioContext.ScenarioInfo.Title}");
            }
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            if (_scenario == null) return;  

            string stepText = _scenarioContext.StepContext.StepInfo.Text;
            string? screenshotPath = CaptureScreenshot(_scenarioContext.ScenarioInfo.Title, stepText);

            if (_scenarioContext.TestError == null)
            {
                _scenario.Log(Status.Pass, stepText);
            }
            else
            {
                _scenario.Log(Status.Fail, stepText);
                _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);

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
            driver?.Quit();
            driver = null;
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _extent?.Flush();
        }

        private string? CaptureScreenshot(string scenarioName, string stepName)
        {
            try
            {
                if (driver == null || driver.WindowHandles.Count == 0)
                {
                    TestContext.Progress.WriteLine("No active browser window. Skipping screenshot.");
                    return null;
                }

                Thread.Sleep(500); 

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                string screenshotFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Screenshots");
                Directory.CreateDirectory(screenshotFolder);

                string sanitizedStepName = string.Join("_", stepName.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(screenshotFolder, $"{scenarioName}_{sanitizedStepName}.png");

                // ✅ FIX: ScreenshotImageFormat Issue Resolved
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
