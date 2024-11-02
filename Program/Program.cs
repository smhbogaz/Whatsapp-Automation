global using OpenQA.Selenium.Chrome;
global using System.ComponentModel;
global using MaterialSkin.Controls;
global using System.Diagnostics;
global using ClosedXML.Excel;
global using OpenQA.Selenium;
global using System.Net;

namespace WhatsappAutomation
{
    public class Program
    {
        [STAThread]
        static void Main()
        {            
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }        
    }
}