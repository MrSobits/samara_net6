using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;

namespace Autotests.WebPages.Root
{
    public class Subsidy : PageBase
    {
        #region Data
        public static bool verify;
        string param;
        string getsub = "//div[starts-with(@id,'loadmask')]/div[text()='Получение субсидий...']";
        string wait = "//div[starts-with(@id,'loadmask')]/div[text()='Расчет показателей']";
        string waitcollect = "/div[starts-with(@id,'loadmask')]/div[text()='Расчет собираемости']";
        string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        string waitpubl = "//div[starts-with(@id,'loadmask')]/div[text()='Публикация программы']";
        private WebElement buttonOk { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='OK']")); } }
        private WebElement buttonYes { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='Да']")); } }
        private WebElement CalcCollect { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'subsidypanel')]//span[text()='Рассчитать собираемость']")); } }
        private WebElement CalculateIndicator { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'subsidypanel')]//span[text()='Рассчитать показатели']")); } }
        private WebElement MessageSuccess { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//div[text()='{0}']", param)); } }
        private WebElement RecordMinSize { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'subsidyrecordgrid')]//tr[3]/td/table/tbody/tr[2]/td[4]/div")); } }
        private WebElement RecordPlannedCollection { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'subsidyrecordgrid')]//tr[3]/td/table/tbody/tr[2]/td[3]/div")); } }
        private WebElement RecordRegBudget { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'subsidyrecordgrid')]//tr[3]/td/table/tbody/tr[2]/td[6]/div")); } }
        private WebElement result { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'subsidytabpanel')]//span[text()='Результат корректировки']")); } }
        private WebElement typeMinSize { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'ext-comp')][2]/table[starts-with(@id,'numberfield')]//input")); } }
        private WebElement typePlannedCollection { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'ext-comp')]/table[starts-with(@id,'numberfield')]//input")); } }
        private WebElement typeRegBudget { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'ext-comp')][3]/table[starts-with(@id,'numberfield')]//input")); } }
        private WebElement versionPublication { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'subsidytabpanel')]/div[starts-with(@id,'programfourthstagegrid')]//span[text()='Версия для публикации']")); } }
        private WebElement waitProgram { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programfourthstagegrid')]//tr[2]/td[2]/div")); } }
        #endregion

        public void WaitSubsides()
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(getsub));
            Browser.WaitHideElement(By.XPath(waitload));
        }

        public void CalculateTheCollection(string cacl)
        {
            this.param = cacl;
            WaitSubsides();
            CalcCollect.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitcollect));
            try
            {
                verify = Browser.WaitUntil(MessageSuccess.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Расчет собираемости произведен не успешно");
            }
            buttonOk.Click(false);
        }

        public void CompleteThePlannedCollection(string number)
        {
            this.param = number;
            WaitSubsides();
            RecordPlannedCollection.Click(false);
            Browser.WaitUntil(typePlannedCollection.Displayed);
            typePlannedCollection.Text = param;
        }

        public void MinimumSizeFundReg(string number)
        {
            this.param = number;
            RecordMinSize.Click(false);
            Browser.WaitUntil(typeMinSize.Displayed);
            typeMinSize.Text = param;
        }

        public void RegionalBudget(string number)
        {
            this.param = number;
            RecordRegBudget.Click(false);
            Browser.WaitUntil(typeRegBudget.Displayed);
            typeRegBudget.Text = param;
        }

        public void CalculateIndicators(string calc)
        {
            this.param = calc;
            CalculateIndicator.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(wait));
            try
            {
                verify = Browser.WaitUntil(MessageSuccess.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Расчет показателей произведен не успешно");
            }
            buttonOk.Click(false);
        }

        public void versionforpublication(string calc)
        {
            WaitSubsides();
            this.param = calc;
            result.Click(false);
            Browser.WaitUntil(waitProgram.Displayed);
            versionPublication.Click(false);
            Browser.WaitUntil(buttonYes.Enabled);
            buttonYes.Click(false);
            Browser.WaitHideElement(By.XPath(waitpubl));
            try
            {
                verify = Browser.WaitUntil(MessageSuccess.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Программа не опубликована");
            }
            buttonOk.Click(false);
        }
    }
}