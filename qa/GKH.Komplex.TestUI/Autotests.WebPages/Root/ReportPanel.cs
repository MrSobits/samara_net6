using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;

namespace Autotests.WebPages.Root
{
    public class ReportPanel : PageBase
    {
        #region Data
        public bool verify;
        string param;
        string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        string waitpls = "//div[starts-with(@id,'loadmask')]/div[text()='Пожалуйста подождите...']";
        private WebElement controlReport { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'reportpanel')]//span[text()='Контрольный отчет по суммам банковских выписок']")); } }
        private WebElement ControlReport { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'reportpanel')]//span[text()='Региональный фонд']")); } }
        private WebElement controlReportDebate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'reportpanel')]//span[text()='Контрольный отчет по дебатам']")); } }
        private WebElement controlReportPaymentDoc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'reportpanel')]//span[text()='Контрольный отчет по суммам реестр оплат платежных агентов']")); } }
        private WebElement newReport { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'reportpanel')]//span[contains(text(),'Новые отчеты')]")); } }
        private WebElement searchfield { get { return new WebElement().ByXPath(string.Format("//input[@name='searchfield']")); } }
        private WebElement waitControlReport { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'tabpanel')]/div/div/div/h1[text()='Региональный фонд']")); } }
        #endregion

        public void BankStatement(string bankstatement)
        {
            this.param = bankstatement;
            var WebElement = new WebElement();
            Pages.Menu.GotoPanelReport();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            WebElement.ByName("searchfield").Text = param;
            WebElement.ByName("searchfield").SendKeys(Keys.Enter);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            try
            {
                try
                {
                    Browser.WaitUntil(controlReport.Displayed);
                    controlReport.Click(false);
                }
                catch
                {
                    newReport.Click(false);
                    controlReport.Click(false);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message+ " Отсутствует контрольный отчет по суммам банковских выписок");
            }
        }

        public void PayingAgent(string payingagent)
        {
            this.param = payingagent;
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            WebElement.ByName("searchfield").Text = param;
            WebElement.ByName("searchfield").SendKeys(Keys.Enter);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            try
            {
                try
                {
                    Browser.WaitUntil(controlReportPaymentDoc.Displayed);
                    controlReportPaymentDoc.Click(false);
                }
                catch
                {
                    newReport.Click(false);
                    controlReportPaymentDoc.Click(false);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Отсутствует контрольный отчет по суммам реестр оплат платежных агентов");
            }
        }

        public bool Debate(string debate)
        {
            this.param = debate;
            var WebElement = new WebElement();
            Pages.Menu.GotoPanelReport();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            WebElement.ByName("searchfield").Text = param;
            WebElement.ByName("searchfield").SendKeys(Keys.Enter);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            try
            {
                try
                {
                    Browser.WaitUntil(controlReportDebate.Displayed);
                    controlReportDebate.Click(false);
                }
                catch
                {
                    newReport.Click(false);
                    controlReportDebate.Click(false);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Отсутствует контрольный отчет по дебатам");
            }
            
            return true;
        }

        public void RunReport(string RegFund)
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            this.param = RegFund;
            searchfield.Text = param;
            searchfield.SendKeys(Keys.Enter);
            try
            {
                Browser.WaitUntil(ControlReport.Displayed);
            }
            catch 
            {
                throw new Exception("Отсутствует: " + param);
            }
            newReport.Click(false);
            ControlReport.Click(false);
            try
            {
                verify = Browser.WaitUntil(waitControlReport.Exists());
            }
            catch
            {
                verify = false;
            }
        }
    }
}