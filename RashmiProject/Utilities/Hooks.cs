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
        public static IWebDriver? driver;  // ✅ Nullable Fix
        private readonly ScenarioContext _scenarioContext;
        private static ExtentReports? _extent;  // ✅ Nullable Fix
        private static ExtentTest? _feature;  // ✅ Nullable Fix
        private ExtentTest? _scenario;  // ✅ Nullable Fix
        private static ExtentSparkReporter? _sparkReporter;  // ✅ Nullable Fix

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "ExtentReport.html");

            // ✅ Null Check Before Creating Directory
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
            if (_scenario == null) return;  // ✅ Avoid Null Reference

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

        // ✅ FIX: ScreenshotImageFormat Error & Nullable Fix
        private string? CaptureScreenshot(string scenarioName, string stepName)
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

                string screenshotFolder = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Screenshots");
                Directory.CreateDirectory(screenshotFolder);

                string sanitizedStepName = string.Join("_", stepName.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(screenshotFolder, $"{scenarioName}_{sanitizedStepName}.png");

                // ✅ Fix: Use Proper ScreenshotImageFormat
                screenshot.SaveAsFile(filePath, OpenQA.Selenium.ScreenshotImageFormat.Png);
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
