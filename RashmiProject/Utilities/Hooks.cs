using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using WebDriverManager;

namespace RashmiProject.Utilities
{
    [Binding]
    public class Hooks
    {
        public  static IWebDriver driver;
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
            _feature = _extent.CreateTest(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void Setup()
        {
            TestContext.Progress.WriteLine("Initializing WebDriver...");

            if (driver == null)
            {
                driver = new ChromeDriver();
            }

            // ✅ Store WebDriver in ScenarioContext
            _scenarioContext["WebDriver"] = driver;
            _scenario = _feature.CreateNode(_scenarioContext.ScenarioInfo.Title);
        }
        [AfterStep]
        public void InsertReportingSteps()
        {
            string stepText = _scenarioContext.StepContext.StepInfo.Text;
            string screenshotPath = CaptureScreenshot(_scenarioContext.ScenarioInfo.Title, stepText);

            if (_scenarioContext.TestError == null)
            {
                // ✅ Attach Screenshot Inline for Passed Steps
                if (screenshotPath != null)
                {
                    _scenario.Log(Status.Pass, stepText); //"MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build()");
                }
                else
                {
                    _scenario.Log(Status.Pass, stepText);
                }
            }
            else
            {
                // ✅ Attach Screenshot Inline for Failed Steps
                if (screenshotPath != null)
                {
                    _scenario.Log(Status.Fail, stepText, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
                else
                {
                    _scenario.Log(Status.Fail, stepText);
                }

                // ✅ Log the actual error message
                _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);
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

        // ✅ FIX: Use Step Name Instead of Timestamp for Overwriting
        private string CaptureScreenshot(string scenarioName, string stepName)
        {
            try
            {
                if (driver == null)
                {
                    TestContext.Progress.WriteLine("WebDriver is null. Cannot capture screenshot.");
                    return null;
                }

                if (driver.WindowHandles.Count == 0)
                {
                    TestContext.Progress.WriteLine("No active browser window. Skipping screenshot.");
                    return null;
                }

                // ✅ Introduce Small Wait Before Capturing Screenshot
                Thread.Sleep(500);

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                // ✅ Ensure Screenshots Folder Exists
                string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                Directory.CreateDirectory(screenshotPath);

                // ✅ Use Scenario Name + Step Name for Screenshot File Name
                string sanitizedStepName = string.Join("_", stepName.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(screenshotPath, $"{scenarioName}_{sanitizedStepName}.png");

                // ✅ Save Screenshot (Overwrites Previous One for Same Step)
                screenshot.SaveAsFile(filePath);
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