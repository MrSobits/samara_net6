using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Autotests.WebPages.Root.Housing_Inspectorate
{
    public class ResolutionOfProsecutors : PageBase
    {

        #region Data
        private WebElement AddButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Добавить']/..")); } }
        private WebElement ChooseButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Выбрать']")); } }
        private WebElement TodayButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Сегодня']")); } }
        private WebElement SaveButton { get { return new WebElement().ByXPath(string.Format("//button[@data-qtip='Сохранить']")).Last(); } }
        private WebElement msgIconSuccess { get { return new WebElement().ByXPath(string.Format("//span[@class='b4-msg-icon-success']")); } }
        private WebElement editButton { get { return new WebElement().ByXPath(string.Format("//img[@src='/stable-chelyabinsk/content/img/icons/pencil.png']")); } }
        private WebElement checkBox { get { return new WebElement().ByXPath(string.Format("//div[@class='x-grid-row-checker']")); } }
        private WebElement acceptButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Применить']/..")); } }

        #endregion


        public void AddDecision()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            AddButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='Municipality']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            ChooseButton.Click(false);
            WebElement.ByXPath("//*[@name='DocumentDate']/../../td[2]").Click(false);
            TodayButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='Executant']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//li[text()='Управляющая компания']").Last().Click(false);
            SaveButton.Click(false);
            WebElement.ByXPath("//*[@name='Contragent']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            ChooseButton.Last().Click(false);
            SaveButton.Click(false);
        }

        public void FormationDecision()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            WebElement.ByXPath("//*[text()='Дата']").Click(false);
            WebElement.ByXPath("//*[text()='Дата']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            editButton.First().Click(false);
            WebElement.ByXPath("//*[text()='Сформировать']/..").WaitUntilEnabled().Click(false);
            WebElement.ByXPath("//*[text()='Постановление']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//*[@name='DocumentDate']/../../td[2]").WaitUntilEnabled().Click(false);
            TodayButton.Click(false);
            WebElement.ByXPath("//*[@name='FineMunicipality']/../../td[2]").Click(false);
            WebElement.ByXPath("//input[@role='checkbox']").First().Click(false);
            ChooseButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='Sanction']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//li[1]").Last().Click(false);
            WebElement.ByXPath("//*[@name='DeliveryDate']/../../td[2]").Click(false);
            TodayButton.Click(false);
            SaveButton.Last().Click(false);
        }

        public void FormationOfRepresentations()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            WebElement.ByXPath("//*[text()='Дата']").Click(false);
            WebElement.ByXPath("//*[text()='Дата']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            editButton.First().Click(false);
            WebElement.ByXPath("//*[text()='Постановление']").WaitUntilEnabled().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//*[text()='Сформировать']/..").WaitUntilEnabled().Click(false);
            WebElement.ByXPath("//*[text()='Представление']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//*[@name='DocumentDate']/../../td[2]").WaitUntilEnabled().Click(false);
            TodayButton.Click(false);
            SaveButton.Last().Click(false);
        }

        public bool FormationProtocol()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            WebElement.ByXPath("//*[text()='Дата']").Click(false);
            WebElement.ByXPath("//*[text()='Дата']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            editButton.First().Click(false);
            WebElement.ByXPath("//*[text()='Постановление']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//*[text()='Сформировать']/..").WaitUntilEnabled().Click(false);
            WebElement.ByXPath("//*[text()='Протокол']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            return Browser.WaitUntil(WebElement.ByXPath("//div[text()='Протокол']").Exists());
        }

    }
}