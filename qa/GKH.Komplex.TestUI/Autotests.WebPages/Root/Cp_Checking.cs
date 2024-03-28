using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Autotests.WebPages.Root
{
    public class Cp_Checking : PageBase
    {
        #region Data
        int day, month, year;
        public string datetoday, currentdate;
        private string waitrun = "//div[starts-with(@id,'loadmask')]/div[text()='Запуск...']";
        private WebElement closeMonth { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'periodclosecheckresultsgrid')]//span[text()='Закрыть месяц']")); } }
        private WebElement messageValidate { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Задачи поставлены в очередь на обработку']")); } }
        private WebElement runVerification { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'periodclosecheckresultsgrid')]//span[text()='Выполнить проверки']")); } }
        private WebElement verifycurrentDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'periodclosecheckresultsgrid')]//tr[2]/td[6]/div")); } }
        private WebElement waitVerify { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p/../h3[text()='Проверка']")); } }
        public WebElement messageError { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Не по всем лицевым счетам проведены начисления']")); } }
        #endregion

        #region DataSort
        string viewValidate = "//div[starts-with(@id,'periodclosecheckresultsgrid')]//span[text()='Дата проверки']";
        private WebElement sortdesc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]//a[starts-with(@id,'menuitem')]/span[text()='Сортировать по убыванию']")); } }
        private WebElement sortValidate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'periodclosecheckresultsgrid')]//span[text()='Дата проверки']/../div")); } }
        #endregion
        
        public void ClickValidate()
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            runVerification.Click(false);
            Browser.WaitHideElement(By.XPath(waitrun));
            Browser.WaitUntil(messageValidate.Displayed);
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(viewValidate))).Build().Perform();
            sortValidate.Click(false);
            sortdesc.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            day = DateTime.Now.Day;
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            if (day < 10)
            {
                datetoday = "0" + day + "." + month + "." + year;
            }
            else
            {
                datetoday = day + "." + month + "." + year;
            }
            currentdate = verifycurrentDate.Text;
            currentdate = currentdate.Substring(0, 10);
        }

        public void CloseCurrentPeriod()
        {
            closeMonth.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitrun));
            Browser.WaitUntil(waitVerify.Displayed);
        }
    }
}