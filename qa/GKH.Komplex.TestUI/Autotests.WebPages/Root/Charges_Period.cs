using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;

namespace Autotests.WebPages.Root
{
    public class Charges_Period : PageBase
    {
        #region Data
        public static bool verify;
        string waitLoad = "//div[starts-with(@id,'ext-gen')]/div[text()='Загрузка...']";
        string waitrun = "//div[starts-with(@id,'loadmask')]/div[text()='Запуск...']";
        private WebElement closePeriod { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'periodclosecheckresultsgrid')]//span[text()='Закрыть месяц'][1]")); } }
        private WebElement messageError { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p/../h3[text()='Ошибка']")); } }
        public WebElement badMessage { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Не по всем лицевым счетам проведены начисления']")); } }
        public WebElement ClosePeriod { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'chargeperiodgrid')]//span[text()='Закрыть текущий период'][1]")); } }
        public WebElement messageSuccessfulConfirm { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Успешно']")); } }
        #endregion

        public void CloseTheCurrentPeriod()
        {
            ClosePeriod.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            Browser.WaitHideElement(By.XPath(waitLoad));
            try
            {
                Browser.WaitUntil(messageError.Exists());
                throw new Exception("Не по всем лицевым счетам проведены начисления");
            }
            catch 
            {
            }
        }

        public void CloseMonth()
        {
            closePeriod.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitrun));
            try
            {
                Browser.WaitUntil(messageError.Exists());
                verify = messageError.Exists();
                throw new Exception("Не по всем лицевым счетам проведены начисления");
            }
            catch 
            {
            }
        }
    }
}