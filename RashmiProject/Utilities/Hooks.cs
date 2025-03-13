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
            try
            {
                string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "ExtentReport.html");

                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

                // Initialize reporter
                _sparkReporter = new ExtentSparkReporter(reportPath);
                _sparkReporter.Config.DocumentTitle = "Test Execution Report";
                _sparkReporter.Config.ReportName = "Automation Test Results";
                _sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;

                // Initialize extent reports
                _extent = new ExtentReports();
                _extent.AttachReporter(_sparkReporter);

                // Add system info
                _extent.AddSystemInfo("Environment", "Testing");
                _extent.AddSystemInfo("Browser", "Chrome");
                _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());

                TestContext.Progress.WriteLine($"Report will be generated at: {reportPath}");
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
            }
        }

        [BeforeScenario]
        public void Setup()
        {
            if (driver == null)
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }

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
        if (_scenario == null)
        {
            TestContext.Progress.WriteLine("Scenario is null. Skipping step logging.");
            return;
        }

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

            if (!string.IsNullOrEmpty(screenshotPath))
            {
                _scenario.Fail("Screenshot of failure:", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
            }
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
    try
    {
        if (driver != null)
        {
            driver.Quit();
            driver = null;
        }

        if (_extent != null)
        {
            _extent.Flush();
        }
    }
    catch (Exception ex)
    {
        TestContext.Progress.WriteLine($"Error during teardown: {ex.Message}");
    }
}


        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (_extent != null)
            {
                _extent.Flush();
            }
        }

        
        private string CaptureScreenshot(string scenarioName, string stepName)
{
    try
    {
        if (driver == null || driver.WindowHandles.Count == 0)
        {
            TestContext.Progress.WriteLine("WebDriver is not available. Skipping screenshot.");
            return null;
        }

        Thread.Sleep(500);
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

        string screenshotsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
        Directory.CreateDirectory(screenshotsFolder);

        string sanitizedStepName = new string(stepName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
        string sanitizedScenarioName = new string(scenarioName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());

        string filePath = Path.Combine(screenshotsFolder, $"{sanitizedScenarioName}_{sanitizedStepName}.png");
        screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

        TestContext.Progress.WriteLine($"Screenshot saved at: {filePath}");

        return filePath;
    }
    catch (Exception ex)
    {
        TestContext.Progress.WriteLine($"Error capturing screenshot: {ex.Message}");
        return null;
    }
}

    }
}



// using System;
// using System.IO;
// using System.Threading;
// using AventStack.ExtentReports;
// using AventStack.ExtentReports.Reporter;
// using NUnit.Framework;
// using OpenQA.Selenium;
// using OpenQA.Selenium.Chrome;
// using TechTalk.SpecFlow;

// namespace RashmiProject.Utilities
// {
//     [Binding]
//     public class Hooks
//     {
//         public static IWebDriver driver;
//         private readonly ScenarioContext _scenarioContext;
//         private static ExtentReports _extent;
//         private static ExtentTest _feature;
//         private ExtentTest _scenario;
//         private static ExtentSparkReporter _sparkReporter;

//         public Hooks(ScenarioContext scenarioContext)
//         {
//             _scenarioContext = scenarioContext;
//         }

//         [BeforeTestRun]
//         public static void BeforeTestRun()
//         {
//             try
//             {
//                 string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "ExtentReport.html");

//                 // Ensure directory exists
//                 Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

//                 // Initialize reporter
//                 _sparkReporter = new ExtentSparkReporter(reportPath);
//                 _sparkReporter.Config.DocumentTitle = "Test Execution Report";
//                 _sparkReporter.Config.ReportName = "Automation Test Results";
//                 _sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;

//                 // Initialize extent reports
//                 _extent = new ExtentReports();
//                 _extent.AttachReporter(_sparkReporter);

//                 // Add system info
//                 _extent.AddSystemInfo("Environment", "Testing");
//                 _extent.AddSystemInfo("Browser", "Chrome");
//                 _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());

//                 TestContext.Progress.WriteLine($"Report will be generated at: {reportPath}");
//             }
//             catch (Exception ex)
//             {
//                 TestContext.Progress.WriteLine($"Failed to initialize extent report: {ex.Message}");
//                 TestContext.Progress.WriteLine($"Stack trace: {ex.StackTrace}");
//             }
//         }

//         [BeforeFeature]
//         public static void BeforeFeature(FeatureContext featureContext)
//         {
//             try
//             {
//                 if (_extent != null)
//                 {
//                     _feature = _extent.CreateTest(featureContext.FeatureInfo.Title);
//                     TestContext.Progress.WriteLine($"Created feature test: {featureContext.FeatureInfo.Title}");
//                 }
//                 else
//                 {
//                     TestContext.Progress.WriteLine("ExtentReport instance is null. Cannot create feature test.");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 TestContext.Progress.WriteLine($"Failed to create feature test: {ex.Message}");
//             }
//         }

//         [BeforeScenario]
//         public void Setup()
//         {
//             try
//             {
//                 TestContext.Progress.WriteLine("Initializing WebDriver...");

//                 if (driver == null)
//                 {
//                     driver = new ChromeDriver();
//                     driver.Manage().Window.Maximize();
//                     driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
//                 }

//                 // Store WebDriver in ScenarioContext
//                 _scenarioContext["WebDriver"] = driver;

//                 // Create scenario node
//                 if (_feature != null)
//                 {
//                     _scenario = _feature.CreateNode(_scenarioContext.ScenarioInfo.Title);
//                     TestContext.Progress.WriteLine($"Created scenario node: {_scenarioContext.ScenarioInfo.Title}");
//                 }
//                 else
//                 {
//                     TestContext.Progress.WriteLine("Feature test is null. Cannot create scenario node.");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 TestContext.Progress.WriteLine($"Failed to setup scenario: {ex.Message}");
//             }
//         }

//         [AfterStep]
//         public void InsertReportingSteps()
//         {
//             try
//             {
//                 string stepText = _scenarioContext.StepContext.StepInfo.Text;
//                 string screenshotPath = CaptureScreenshot(_scenarioContext.ScenarioInfo.Title, stepText);

//                 if (_scenario == null)
//                 {
//                     TestContext.Progress.WriteLine("Scenario is null. Cannot log step.");
//                     return;
//                 }

//                 if (_scenarioContext.TestError == null)
//                 {
//                     // For passed steps
//                     if (screenshotPath != null)
//                     {
//                         _scenario.Log(Status.Pass, stepText, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
//                     }
//                     else
//                     {
//                         _scenario.Log(Status.Pass, stepText);
//                     }
//                 }
//                 else
//                 {
//                     // For failed steps
//                     if (screenshotPath != null)
//                     {
//                         _scenario.Log(Status.Fail, stepText, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
//                         _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);
//                     }
//                     else
//                     {
//                         _scenario.Log(Status.Fail, stepText);
//                         _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 TestContext.Progress.WriteLine($"Failed to log step: {ex.Message}");
//             }
//         }

//         [AfterScenario]
//         public void TearDown()
//         {
//             try
//             {
//                 if (driver != null)
//                 {
//                     driver.Quit();
//                     driver = null;
//                     TestContext.Progress.WriteLine("WebDriver quit successfully.");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 TestContext.Progress.WriteLine($"Failed to tear down: {ex.Message}");
//             }
//         }

//         [AfterTestRun]
//         public static void AfterTestRun()
//         {
//             try
//             {
//                 if (_extent != null)
//                 {
//                     _extent.Flush();
//                     TestContext.Progress.WriteLine("ExtentReport has been generated successfully.");
//                 }
//                 else
//                 {
//                     TestContext.Progress.WriteLine("ExtentReport instance is null. Cannot generate report.");
//                 }
//             }
//             catch (Exception ex)
//             {
//                 TestContext.Progress.WriteLine($"Failed to generate extent report: {ex.Message}");
//                 TestContext.Progress.WriteLine($"Stack trace: {ex.StackTrace}");
//             }
//         }

//         private string CaptureScreenshot(string scenarioName, string stepName)
//         {
//             try
//             {
//                 if (driver == null)
//                 {
//                     TestContext.Progress.WriteLine("WebDriver is null. Cannot capture screenshot.");
//                     return null;
//                 }

//                 if (driver.WindowHandles.Count == 0)
//                 {
//                     TestContext.Progress.WriteLine("No active browser window. Skipping screenshot.");
//                     return null;
//                 }

//                 // Introduce small wait before capturing screenshot
//                 Thread.Sleep(500);

//                 Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

//                 // Ensure Screenshots folder exists
//                 string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
//                 Directory.CreateDirectory(screenshotPath);

//                 // Use Scenario Name + Step Name for Screenshot File Name
//                 string sanitizedStepName = string.Join("_", stepName.Split(Path.GetInvalidFileNameChars()));
//                 string sanitizedScenarioName = string.Join("_", scenarioName.Split(Path.GetInvalidFileNameChars()));
//                 string filePath = Path.Combine(screenshotPath, $"{sanitizedScenarioName}_{sanitizedStepName}.png");

//                 // Save Screenshot (Overwrites Previous One for Same Step)
//                 screenshot.SaveAsFile(filePath);
//                 TestContext.Progress.WriteLine($"Screenshot saved at: {filePath}");

//                 return filePath;
//             }
//             catch (Exception ex)
//             {
//                 TestContext.Progress.WriteLine($"Failed to capture screenshot: {ex.Message}");
//                 return null;
//             }
//         }
//     }
// }
