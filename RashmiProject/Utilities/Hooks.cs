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
        private static string reportDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestReports");
        private static string reportPath = Path.Combine(reportDirectory, "ExtentReport.html");

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            try
            {
                Directory.CreateDirectory(reportDirectory);  // Ensure directory exists

                _sparkReporter = new ExtentSparkReporter(reportPath);
                _sparkReporter.Config.DocumentTitle = "Test Execution Report";
                _sparkReporter.Config.ReportName = "Automation Test Results";
                _sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;

                _extent = new ExtentReports();
                _extent.AttachReporter(_sparkReporter);
                _extent.AddSystemInfo("Environment", "Testing");
                _extent.AddSystemInfo("Browser", "Chrome");
                _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());

                TestContext.Progress.WriteLine($"Report will be saved at: {reportPath}");
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to initialize extent report: {ex.Message}");
            }
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            if (_extent != null)
            {
                _feature = _extent.CreateTest(featureContext.FeatureInfo.Title);
                TestContext.Progress.WriteLine($"Created feature test: {featureContext.FeatureInfo.Title}");
            }
        }

        [BeforeScenario]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _scenarioContext["WebDriver"] = driver;

            if (_feature != null)
            {
                _scenario = _feature.CreateNode(_scenarioContext.ScenarioInfo.Title);
            }
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            try
            {
                string stepText = _scenarioContext.StepContext.StepInfo.Text;
                if (_scenario == null) return;

                if (_scenarioContext.TestError == null)
                {
                    _scenario.Log(Status.Pass, stepText);
                }
                else
                {
                    _scenario.Log(Status.Fail, stepText);
                    _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to log step: {ex.Message}");
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
            TestContext.Progress.WriteLine($"Extent report saved at: {reportPath}");
        }
    }
}
