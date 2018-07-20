using IntranetApplication.Models.HtmlScrapping;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using IntranetApplication.Models.DashboardItem;

namespace IntranetApplication.Engines
{
    public class TransformEngine //: ITransformEngine
    {
        private readonly HtmlTargetContext _htmlTargetContext;
        private readonly ILogger<TransformEngine> _Log;

        public TransformEngine(HtmlTargetContext htmlTargetContext, ILoggerFactory loggerFactory)
        {
            _htmlTargetContext = htmlTargetContext;
            _Log = loggerFactory.CreateLogger<TransformEngine>();
        }

        public TransformEngine()
        {

        }

       // creates intial conversion of image
        public string Convert(DashItemScrap newItem)
        {

            string url = newItem.SourceURL;
            string userName = newItem.LogonUser;
            string password = newItem.LogonPwd;
            string cssSelector = newItem.CssSelector;

            string location = "wwwroot/images/ScrappedImages/" + newItem.Title;
            Random rnd = new Random();
            while (System.IO.File.Exists(location + ".jpg")) // dont want to overwrite any pictures, so we keep adding random numbers until we get an unused name
            {
                location += rnd.Next(0, 10).ToString();
            }

            location += ".jpg";

            try
            {
                url = AddAuthentication(url, userName, password);

                ChromeOptions op = new ChromeOptions();
                op.AddArguments("headless",
                    "window-size=3000,3000"); // what specific window size should be used?

                By locator = By.CssSelector(cssSelector);

                IWebDriver driver =
                    new ChromeDriver( //Todo switch location of Selenium Driver
                        "..\\IntranetApplication\\bin\\Debug\\netcoreapp2.0",
                        op);
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(TimeSpan.FromSeconds(5));

                IWebElement element = driver.FindElement(locator);

                Screenshot sc = ((ITakesScreenshot)driver).GetScreenshot();

                var img = (Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap);

                img.Clone(new Rectangle(element.Location, element.Size), img.PixelFormat)
                    .Save(location, System.Drawing.Imaging.ImageFormat.Jpeg);

                location = location.Replace("wwwroot", "");

                return location;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }

        }




        // Returns an error Image location if the conveter failed
        public string ConvertOneTEMP(string userName, string password, string url, string cssSelector )
        {
            string storageLocation = "wwwroot/images/TEMP.jpg";
            try
            {
                url = AddAuthentication(url, userName, password);

                ChromeOptions op = new ChromeOptions();
                op.AddArguments("headless",
                    "window-size=3000,3000"); // what specific window size should be used?

                By locator = By.CssSelector(cssSelector);

                IWebDriver driver =
                    new ChromeDriver( //Todo switch location of Selenium Driver
                        "..\\IntranetApplication\\bin\\Debug\\netcoreapp2.0",
                        op);
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(TimeSpan.FromSeconds(5));

                IWebElement element = driver.FindElement(locator);

                Screenshot sc = ((ITakesScreenshot)driver).GetScreenshot();

                var img = (Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap);
                         
                    img.Clone(new Rectangle(element.Location, element.Size), img.PixelFormat)
                        .Save(storageLocation, System.Drawing.Imaging.ImageFormat.Jpeg);
                

                return "/images/TEMP.jpg";
            }
            catch (Exception ex)
            {
                return "/images/Error.png";
            }


        }

        public void HtmlToJpegConverter()
        {
            _Log.LogInformation("HtmlToJpegConverter Started");
            try
            {
                List<HtmlTarget> htmlTargets = _htmlTargetContext.HtmlTargets.ToList();


                foreach (HtmlTarget resource in htmlTargets)
                {

                    try
                    {
                        //dotnet user-secrets set ConverterPW <PW>                       
                       //var testUserPw = _configuration["ConverterPW"];
                        // dont hack 'em
                        string userName = "nullman"; // place your TFS UserName Here
                        string password = ""; // place your TFS Password Here

                        string url = resource.Url;
                        string cssSelector = resource.CssSelector;
                        string storageLocation = resource.StorageLocation;

                        url = AddAuthentication(url, userName, password);

                        ChromeOptions op = new ChromeOptions();
                        op.AddArguments("headless",
                            "window-size=3000,3000"); // what specific window size should be used?

                        By locator = By.CssSelector(cssSelector);

                        IWebDriver driver =
                            new ChromeDriver( //Todo switch location of Selenium Driver
                                "..\\IntranetApplication\\bin\\Debug\\netcoreapp2.0",
                                op);
                        driver.Navigate().GoToUrl(url);
                        Thread.Sleep(TimeSpan.FromSeconds(5));

                        IWebElement element = driver.FindElement(locator);

                        Screenshot sc = ((ITakesScreenshot) driver).GetScreenshot();

                       var img = (Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap);


                        // custom size for crop can be specified by json file
                        if (resource.CroppingWidth != 0 & resource.CroppingHeight != 0
                        ) // custom size was specified in json file
                        {
                            Size customSize = new Size(resource.CroppingWidth ?? 0, resource.CroppingHeight ?? 0);
                            img.Clone(new Rectangle(element.Location, customSize), img.PixelFormat)
                                .Save(storageLocation, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        else
                        {
                            img.Clone(new Rectangle(element.Location, element.Size), img.PixelFormat)
                                .Save(storageLocation, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        _Log.LogInformation("Image at Location {0} has been successfully updated",storageLocation);

                    }
                    catch (Exception ex)
                    {
                        _Log.LogWarning(ex, "Failure in an HTML Image scrapping");
                    }
                }
            }
            catch (Exception ex)
            {
                _Log.LogCritical(ex, "A Failure With Transform Engine has occured");
            }


        }
        private static string AddAuthentication(string url, string userName, string password)
        {

            url = url.Replace("https://", "https://" + userName + ":" + password + "@");

            return url;
        }

    }
}

