namespace WhatsappAutomation;

public class Selenium
{
    public static IWebDriver DriverInit()
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        IWebDriver driver;
        if (Settings.Default.chromeOptions == 0)
        {
            var chrome = Process.GetProcessesByName("chrome");
            foreach (var kapa in chrome)
            {
                try
                {
                    kapa.CloseMainWindow();
                }
                catch { }
            }
            string yol = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data";
            ChromeOptions ayar = new();
            ayar.AddExcludedArgument("enable-automation");
            ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService();
            ayar.AddArguments("--disable-notifications", "--user-data-dir=" + yol);
            defaultService.HideCommandPromptWindow = true;
            driver = new ChromeDriver(defaultService, ayar);
        }
        else
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            driver = new ChromeDriver(chromeDriverService, new ChromeOptions());
        }
        driver.Manage().Window.Maximize();
        driver.Navigate().GoToUrl("https://web.whatsapp.com");
        Main.log("Chrome çalıştırıldı!");
        return driver;
    }
    public static void NewChat(IWebDriver driver)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        if (!Settings.Default.startNewChat)
        {
            return;
        }
        int i = 1;
        while (i <= 3)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).Click();
                Main.log("Yeni sohbet penceresi çalıştırıldı!");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void CancelSearching(IWebDriver driver)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).Click();
                Main.log("Eski aranan numara temizlendi!");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void SearchInput(IWebDriver driver, string sendKeys)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).SendKeys(sendKeys);
                Main.log($"{sendKeys} numarası arandı!");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static bool VeliClick(IWebDriver driver, bool state, string alertText)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        string xpath = "HİDDEN";
        bool return_state = false;
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;

                if (Settings.Default.startNewChat)
                {
                    try
                    {
                        IWebElement element = driver.FindElement(By.XPath("HİDDEN"));
                        xpath = "HİDDEN";
                    }
                    catch (OpenQA.Selenium.NoSuchElementException)
                    {
                        try
                        {
                            IWebElement _element = driver.FindElement(By.XPath("HİDDEN"));
                            xpath = "HİDDEN";
                        }
                        catch (OpenQA.Selenium.NoSuchElementException)
                        {
                            Main.log("Aranan numarada bir sonuç çıkmadı");
                            if (state)
                            {
                                MessageBox.Show(alertText);
                            }
                            return_state = true;
                            break;
                        }
                    }
                }
                driver.FindElement(By.XPath(xpath)).Click();
                Main.log("Aranan numarada çıkan kişiye tıklanıldı");
                return_state = false;
                break;
            }
            catch (Exception)
            {
                if (state)
                {
                    MessageBox.Show(alertText);
                }
                Main.log("Aranan numarada bir sonuç çıkmadı");
                return_state = true;
                break;
            }
        }
        return return_state;
    }
    public static void WriteMessageContent(IWebDriver driver, string text)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        Thread.Sleep(Settings.Default.processDelay);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).SendKeys(text);
                Main.log($"Tıklanan kişiye ({text}) mesajı yazıldı.");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void SendMessageClick(IWebDriver driver)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).Click();
                Main.log("Tıklanan kişiye mesaj gönderildi!");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void ClickFileDiv(IWebDriver driver)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).Click();
                Main.log("PDF gönderme butonuna (+) tıklandı");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void SendImagePath(IWebDriver driver, string path)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).SendKeys(path);
                Main.log("RESİM dosyası seçildi");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void SendPdfPath(IWebDriver driver, string path)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                driver.FindElement(By.XPath("HİDDEN")).SendKeys(path);
                Main.log("PDF dosyası seçildi");
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void SendPdf(IWebDriver driver, ref List<string> sendedPdf, string pdfFile, string? pdfTitle)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;
                if (pdfTitle != null)
                {
                    driver.FindElement(By.XPath("HİDDEN")).SendKeys(pdfTitle);
                    Main.log("PDF başlığı yazıldı",false);
                }
                driver.FindElement(By.XPath("HİDDEN")).Click();
                Main.log("PDF dosyası gönderildi");
                if (!sendedPdf.Contains(pdfFile))
                {
                    sendedPdf.Add(pdfFile);
                }
                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }
    public static void SendImage(IWebDriver driver, string imageTitle)
    {
        if(!Main.sw.IsRunning){Main.sw.Start();}
        isDriverClosed(driver);
        int i = 1;
        while (i < 50)
        {
            try
            {
                i++;

                driver.FindElement(By.XPath("HİDDEN")).SendKeys(imageTitle);
                Main.log("RESİM başlığı yazıldı",false);

                driver.FindElement(By.XPath("HİDDEN")).Click();
                Main.log("RESİM dosyası gönderildi");

                break;
            }
            catch { }
        }
        Thread.Sleep(Settings.Default.processDelay);
    }

    private static void isDriverClosed(IWebDriver driver)
    {
        try
        {
            _ = driver.Url;
        }
        catch (Exception)
        {   
            Main.log("İşlem Penceresi Kapaıldı");         
            throw new DriverAlreadyClosed();
        }
    }
}