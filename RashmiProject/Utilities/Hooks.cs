using System;
using System.IO;
using System.Net.Mail;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace RashmiProject.Utilities
{
    [Binding]
    public class Hooks
    {
        public static IWebDriver? driver { get; private set; }
        private readonly ScenarioContext _scenarioContext;
        private static ExtentReports _extent;
        private static ExtentTest _feature;
        private ExtentTest _scenario;
        private static ExtentSparkReporter _sparkReporter;
        private static string reportPath = "";
        private static string screenshotsDir = "";

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            driver = sauceLabs_PageObject.Utils.WebDriverManager.GetDriver();
            
            string reportsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            Directory.CreateDirectory(reportsDir);
            reportPath = Path.Combine(reportsDir, "ExtentReport.html");
            
            screenshotsDir = Path.Combine(reportsDir, "Screenshots");
            Directory.CreateDirectory(screenshotsDir);

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
            _scenario = _feature.CreateNode(_scenarioContext.ScenarioInfo.Title);
            _scenarioContext["WebDriver"] = driver;
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            string stepText = _scenarioContext.StepContext.StepInfo.Text;
            string? screenshotBase64 = null;

            if (_scenarioContext.TestError != null)
            {
                screenshotBase64 = CaptureScreenshot();
                string imgTag = screenshotBase64 != null ? $"<img src='data:image/png;base64,{screenshotBase64}' width='600px' />" : "";
                _scenario.Log(Status.Fail, stepText + "<br>" + imgTag);
                _scenario.Log(Status.Fail, _scenarioContext.TestError.Message);
            }
            else
            {
                _scenario.Log(Status.Pass, stepText);
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
            _extent.Flush();
            SendEmailReport();
        }

        private static void SendEmailReport()
        {
            if (File.Exists(reportPath))
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("your-email@example.com");
                    mail.To.Add("ankiamin77@gmail.com");
                    mail.Subject = "Automation Test Results Report";
                    mail.Body = "Hello,\n\nAttached is the latest test execution report.\n\nRegards,\nCI/CD Automation";
                    mail.Attachments.Add(new Attachment(reportPath));
                    
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"),
                        EnableSsl = true
                    };
                    
                    smtp.Send(mail);
                    TestContext.Progress.WriteLine("✅ Attached Extent Report and sent email.");
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine("❌ Failed to send email: " + ex.Message);
                }
            }
            else
            {
                TestContext.Progress.WriteLine("❌ Report file not found!");
            }
        }

        private string? CaptureScreenshot()
        {
            try
            {
                if (driver == null || driver.WindowHandles.Count == 0)
                {
                    return null;
                }
                
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotPath = Path.Combine(screenshotsDir, $"Screenshot_{Guid.NewGuid()}.png");
                screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                return Convert.ToBase64String(File.ReadAllBytes(screenshotPath));
            }
            catch
            {
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
