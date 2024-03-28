using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Autotests.WebPages.Root.Housing_Inspectorate
{
    public class AppealToRequestLicense : PageBase
    {
        #region Data
        private WebElement AddButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Добавить']/..")); } }
        private WebElement ChooseButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Выбрать']")); } }
        private WebElement TodayButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Сегодня']")); } }
        private WebElement SaveButton { get { return new WebElement().ByXPath(string.Format("//button[@data-qtip='Сохранить']")).Last(); } }
        private WebElement editButton { get { return new WebElement().ByXPath(string.Format("//img[@src='/stable-chelyabinsk/content/img/icons/pencil.png']")); } }
        private WebElement generateInspection { get { return new WebElement().ByXPath(string.Format("//span[text()='Сформировать проверку']/..")); } }
        private WebElement checkBox { get { return new WebElement().ByXPath(string.Format("//div[@class='x-grid-row-checker']")); } }
        private WebElement acceptButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Применить']/..")); } }

        #endregion

        public void AddRequest()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            AddButton.WaitUntilEnabled().Click();
            WebElement.ByXPath("//*[@name='Contragent']/../../td[2]/div").Click(false);
            WebElement.ByXPath("//div[@class='x-grid-row-checker']/../../../../tr[2]//div[@class='x-grid-row-checker']").Last().Click(false);
            ChooseButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='DateRequest']/../../td[2]/div").Click(false);
            TodayButton.Last().Click(false);
            SaveButton.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//*[@name='ConfirmationOfDuty']").SendKeys("1");
            WebElement.ByXPath("//*[@name='ReasonOffers']").SendKeys("1");
            WebElement.ByXPath("//*[@name='ReasonRefusal']").SendKeys("1");
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            SaveButton.WaitUntilEnabled().Click(false);
        }

        public void AddInspection()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            WebElement.ByXPath("//*[text()='Номер заявки']").Click(false);
            WebElement.ByXPath("//*[text()='Номер заявки']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            editButton.First().Click(false);
            generateInspection.WaitUntilEnabled().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            AddButton.Last().WaitUntilEnabled().Click(false);
            checkBox.First().Click(false);
            acceptButton.Last().Click(false);
            WebElement.ByXPath("//span[text()='Сформировать']").Click(false);
            WebElement.ByXPath("//span[text()='Распоряжение']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//*[@name='DocumentDate']").SendKeys("19.12.2016");
            WebElement.ByXPath("//*[@name='IssuedDisposal']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            ChooseButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='disposalInspectors']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            acceptButton.Last().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//span[text()='OK']").Click(false);
            WebElement.ByXPath("//*[@name='DateStart']").SendKeys("19.12.2016");
            WebElement.ByXPath("//*[@name='DateEnd']").SendKeys("19.12.2016");
            SaveButton.Last().Click(false);
            WebElement.ByXPath("//span[text()='Да']").Click(false);
        }

        
    }
}