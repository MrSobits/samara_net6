using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Autotests.WebPages.Root
{
    public class Unaccepted_Charges : PageBase
    {
        #region Data
        private string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        private WebElement buttonConfirm { get { return new WebElement().ByXPath(string.Format("//button[@data-qtip='По нажатию произойдет подтверждение начислений выбранной записи']/span[text()='Подтвердить']")); } }
        private WebElement buttonOK { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='OK']")); } }
        private WebElement check { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargepacketgrid')]/div/table/tbody/tr[2]/td[5]/div[text()]/../../td[1]/div/div")); } }
        public WebElement messageSuccessfulConfirm { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Успешно']")); } }
        #endregion

        #region DataSort
        string viewFormedSection = "//div[starts-with(@id,'unacceptedchargepacketgrid')]//span[text()='Дата формирования']";
        private WebElement sortdesc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]//a[starts-with(@id,'menuitem')]/span[text()='Сортировать по убыванию']")); } }
        private WebElement sortFormedSection { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargepacketgrid')]//span[text()='Дата формирования']/../div")); } }
        #endregion
        
        public void CheckFormedSection()
        {
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(viewFormedSection))).Build().Perform();
            sortFormedSection.Click(false);
            sortdesc.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            check.Click(false);
        }

        public void ClickOnConfirm()
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            buttonConfirm.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            buttonOK.Click(false);
            Browser.WaitUntil(messageSuccessfulConfirm.Displayed);
        }
    }
}