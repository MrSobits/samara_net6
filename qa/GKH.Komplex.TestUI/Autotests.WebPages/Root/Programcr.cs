using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Autotests.WebPages.Root
{
    public class Programcr : PageBase
    {
        #region Data
        public static bool verify;
        string param;
        string waitForming = "//div[starts-with(@id,'loadmask')]/div[text()='Формирование программы на основе региональной программы КР']";
        string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        private WebElement buttonAdd { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programCrGrid')]//span[text()='Добавить']")); } }
        private WebElement buttonApply { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'objectCrPanel')]/div[starts-with(@id,'ext-comp')]//button[@data-qtip='Сохранить']/span[text()='Применить']")); } }
        private WebElement buttonClose { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programCrEditWindow')]//button[@data-qtip='Закрыть']/span[text()='Закрыть']")); } }
        private WebElement buttonSave { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programCrEditWindow')]//button[@data-qtip='Сохранить']//span[text()='Сохранить']")); } }
        private WebElement check { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]//tr[3]/td[1]/div/div")); } }
        private WebElement checkInput { get { return new WebElement().ByXPath(string.Format("//label[contains(text(),'Сформировать на основании Региональной программы КР')]/../input")); } }
        private WebElement checkObjectCr { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'objectCrPanel')]/div[starts-with(@id,'ext-comp')]//tr[2]/td[1]/div/div")); } }
        private WebElement full { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Полная']")); } }
        private WebElement Message { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'objectcrgrid')]//tr[2]/td[5]/div[text()='{0}']", param)); } }
        private WebElement name { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programCrEditWindow')]//input[@name='Name']")); } }
        private WebElement nameObjectCr { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'objectCrPanel')]//td[1]/label[text()='Наименование:']/../../td[2]/input")); } }
        private WebElement newState { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Новая']")); } }
        private WebElement period { get { return new WebElement().ByXPath(string.Format("//div[@id='programCrEditWindow-1473']//td[1]/input[@name='Period']/../../td[2]/div")); } }
        private WebElement programKr { get { return new WebElement().ByXPath(string.Format("//input[@name='tfProgramCr']/../../td[2]/div")); } }
        private WebElement select { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]//span[text()='Выбрать']")); } }
        private WebElement statecr { get { return new WebElement().ByXPath(string.Format("//input[@name='TypeProgramStateCr']/../../td[2]/div")); } }
        private WebElement update { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'objectCrPanel')]//button[@data-qtip='Обновить']/span[text()='Обновить']")); } }
        private WebElement visability { get { return new WebElement().ByXPath(string.Format("//input[@name='TypeVisibilityProgramCr']/../../td[2]/div")); } }
        private WebElement wait { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programCrGrid')]//tr[2]/td[3]/div")); } }
        private WebElement waitObject { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'objectCrPanel')]/div[starts-with(@id,'ext-comp')]//tr[2]/td[2]/div")); } }
        private WebElement waitTest { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'objectcrgrid')]//tr[2]/td[5]/div[text()='{0}']", param)); } }
        #endregion

        #region DataSort
        string sort = "//div[starts-with(@id,'b4grid')][1]//span[text()='Наименование']";
        private WebElement clicksort { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'b4grid')][1]//span[text()='Наименование']/../div"));} }
        private WebElement sortDesc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]/div/a[starts-with(@id,'menuitem')]/span[text()='Сортировать по убыванию']"));} }
        #endregion

        public void Create()
        {
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(wait.Displayed);
            buttonAdd.Click(false);
        }

        public void FillingDate(string Name)
        {
            this.param = Name;
            Browser.WaitUntil(name.Enabled);
            name.Text = param;
            period.Click(false);
            check.Click(false);
            select.Click(false);
            checkInput.Click(false);
            visability.Click(false);
            Browser.WaitUntil(full.Displayed);
            full.Click(false);
            statecr.Click(false);
            Browser.WaitUntil(newState.Displayed);
        }

        public void ClickSave()
        {
            buttonSave.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
        }

        public void ObjectCr(string Name)
        {
            this.param = Name;
            buttonClose.Click(false);
            Browser.WaitHideElement(By.XPath(waitForming));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Pages.Menu.GotoObjectKr();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(waitTest.Displayed);
            programKr.Click(false);
            Browser.WaitUntil(waitObject.Displayed);
            nameObjectCr.Text = param;
            nameObjectCr.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitload));
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(sort))).Build().Perform();
            clicksort.Click(false);
            sortDesc.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            checkObjectCr.Click(false);
            buttonApply.Click(false);
            update.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            try
            {
                verify = Browser.WaitUntil(Message.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Сформированной программы нет");
            }
        }
    }
}