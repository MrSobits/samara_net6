using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Autotests.WebPages.Root
{
    public class RegisterOfAplications : PageBase
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

        public void AddApplication()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            AddButton.Click(false);
            WebElement.ByXPath("//*[@name='NumberGji']").SendKeys("1");
            WebElement.ByXPath("//*[@name='ZonalInspection']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            ChooseButton.Last().Click(false);
            SaveButton.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Browser.WaitHideElement(By.XPath("//*[text()='Загрузка']"));
        }

        public void FormationOfChecking()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            WebElement.ByXPath("//*[text()='Дата обращения']/../../div[3]//tr/td[2]/div[contains(@class,'date')]").Click(false);
            TodayButton.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            editButton.First().Click(false);
            WebElement.ByXPath("//*[@name='DocumentNumber']").SendKeys("1");
            WebElement.ByXPath("//*[@name='CheckTime']/../../td[2]").Click(false);
            TodayButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='ExtensTime']/../../td[2]").Click(false);
            TodayButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='Correspondent']").Text = "test";
            WebElement.ByXPath("//*[@name='CorrespondentAddress']").Text = "test";
            WebElement.ByXPath("//*[@name='Email']").Text = "test@test.ru";
            WebElement.ByXPath("//*[@name='FlatNum']").Text = "1";
            WebElement.ByXPath("//*[@name='Phone']").Text = "899999999";
            WebElement.ByXPath("//*[@name='KindStatement']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            ChooseButton.Click(false);
            WebElement.ByXPath("//*[text()='Тематики']").Click(false);
            AddButton.Last().Click(false);
            WebElement.ByXPath("//input[@role='checkbox']").First().Click(false); acceptButton.Click(false);
            WebElement.ByXPath("//*[text()='Место возникновения проблемы']").Click(false);
            AddButton.Last().Click(false);
            checkBox.First().Click(false);
            acceptButton.Click(false);
            SaveButton.Last().Click(false);
            WebElement.ByXPath("//*[text()='Сформировать проверку']/..").Click(false);
            WebElement.ByXPath("//*[@data-errorqtip='<ul><li>Это поле обязательно для заполнения</li></ul>']/../../td[2]").First().Click(false);
            checkBox.First().Click(false);
            ChooseButton.Last().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            SaveButton.Last().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            SaveButton.Last().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//span[text()='Сформировать']").Click(false);
            WebElement.ByXPath("//span[text()='Распоряжение']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//*[@name='DocumentDate']").Text = "19.12.2016";
            WebElement.ByXPath("//*[@name='IssuedDisposal']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            ChooseButton.Last().Click(false);
            WebElement.ByXPath("//*[@name='disposalInspectors']/../../td[2]").Click(false);
            checkBox.First().Click(false);
            acceptButton.Last().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//span[text()='OK']").Click(false);
            WebElement.ByXPath("//*[@name='DateStart']").Text = "19.12.2016";
            WebElement.ByXPath("//*[@name='DateEnd']").Text = "19.12.2016";
            SaveButton.Last().Click(false);
            WebElement.ByXPath("//span[text()='Да']").Click(false);

        }

    }
}