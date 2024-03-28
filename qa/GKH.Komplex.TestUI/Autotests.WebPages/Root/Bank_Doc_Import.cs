using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using System.IO;

namespace Autotests.WebPages.Root
{
    public class Bank_Doc_Import : PageBase
    {
        #region Data
        int result;
        public static int resultsum;
        static string element = "7451990794_40703810809280004826_287_y97_ufo" + "." + "zip";
        string ls, date, sum, paymentdate, sumpay;
        string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        string waitpaymentconfirm = "//div[starts-with(@id,'loadmask')]/div[text()='Подтверждение оплат...']";
        private WebElement buttonOK { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='OK']")); } }
        private WebElement buttonSave { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//button[@data-qtip='Сохранить']/span[text()='Сохранить']")); } }
        private WebElement buttonUpdate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//button[@data-qtip='Обновить']/span[contains(@id,'btnIcon')]")); } }
        private WebElement buttonUpdateEditPanel { get { return new WebElement().ByXPath(string.Format("//span[contains(@id,'Icon')]")); } }
        private WebElement buttonYes { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='Да']")); } }
        private WebElement confirm { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//button[contains(@data-qtip,'подтверждение начислений')]/span[contains(@id,'btnIcon')]")); } }
        private WebElement datepay { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocimporteditpanel')]//tr[2]/td[12]/div")); } }
        private WebElement editElement { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//tr[2]/td[2]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement import { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//button[starts-with(@id,'gkhbuttonimport')]/span[text()='Импорт']")); } }
        private WebElement importPayments { get { return new WebElement().ByXPath(string.Format("//a[starts-with(@id,'menuitem')]/span[text()='Импорт реестров оплат']")); } }
        private WebElement litypeImport { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Импорт оплат от Сбербанка (txt)']")); } }
        private WebElement lsFale { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocimporteditpanel')]//tr[2]/td[2]/div")); } }
        private WebElement messageSuccess { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='Успешная загрузка']")); } }
        private WebElement openLS { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[2]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement papaymentDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'papaymentgrid')]/div/table/tbody/tr[2]/td[3]/div")); } }
        private WebElement payment { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//span[text()='Оплаты']")); } }
        private WebElement searchLS { get { return new WebElement().ByXPath(string.Format("//label[text()='Номер:']/../../td[2]/input")); } }
        private WebElement selectElement { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//tr[2]/td[1]/div/div")); } }
        private WebElement summa { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocimporteditpanel')]//tr[2]/td[13]/div")); } }
        private WebElement sumpayment { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'papaymentgrid')]/div/table/tbody/tr[2]/td[5]/div")); } }
        private WebElement typeImport { get { return new WebElement().ByXPath(string.Format("//input[@name='providerCode']/../../td[2]/div")); } }
        private WebElement typeUniversalImport { get { return new WebElement().ByXPath(string.Format("//input[@name='fsGorodCode']/../../td[2]/div")); } }
        private WebElement universalImport { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[contains(text(),'Универсальный импорт')]")); } }
        private WebElement upload { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//span[text()='Добавить']")); } }
        private WebElement waitoperation { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//tr[2]/td[3]/div")); } }
        private WebElement waitUfo { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//tr[2]/td[12]/div[contains(text(),'ufo')]")); } }
        #endregion

        #region DataSort
        string dateoperation = "//div[starts-with(@id,'bankdocumentimportgrid')]//span[text()='Дата операции']";
        string sort = "//div[starts-with(@id,'personalaccounteditpanel')]//div[starts-with(@id,'papaymentgrid')]//span[text()='Дата оплаты']";
        private WebElement clickDateOperation { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'bankdocumentimportgrid')]//span[text()='Дата операции']/../div"));} }
        private WebElement clickSort { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//div[starts-with(@id,'papaymentgrid')]//span[text()='Дата оплаты']/../div"));} }
        private WebElement sortdesc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]//a[starts-with(@id,'menuitem')]/span[text()='Сортировать по убыванию']"));} }
        #endregion

        public void Import()
        {
            Browser.WaitUntil(waitoperation.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            importClick:
            import.Click(false);
            try
            {
                importPayments.Click(false);
            }
            catch
            {
                goto importClick;
            }
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            typeImport.Click(false);
            universalImport.Click(false);
            typeUniversalImport.Click(false);
            litypeImport.Click(false);
            upload.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            var path1 = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\";
            var pathDirectory = Path.GetFullPath(string.Format(@"{0}\downloading", path1));
            Thread.Sleep(2000);
            System.Windows.Forms.SendKeys.SendWait(pathDirectory+ @"\" + element);
            System.Windows.Forms.SendKeys.SendWait(@"{Enter}");
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            buttonSave.Click(false);
            try
            {
                Browser.WaitUntil(messageSuccess.Exists());
                buttonYes.Click(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Ошибка загрузки: Файл не выбран");
            }
        }

        public void DisplayedLine()
        {
            buttonUpdate.Click(false);
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(dateoperation))).Build().Perform();
            clickDateOperation.Click(false);
            sortdesc.Click(false);
            try
            {
                Browser.WaitUntil(waitUfo.Exists());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Отсутствуют записи");
            }
        }

        public void ChooseOneTickLineAndConfirm()
        {
            selectElement.Click(false);
            confirm.Click(false);
            buttonOK.Click(false);
            try
            {
                Browser.WaitHideElement(By.XPath(waitpaymentconfirm));
                buttonOK.Click(false);
                buttonOK.Click(false);
            }
            catch 
            {
            }
        }

        public void RegisterLS()
        {
            editElement.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            var visible = false;
            var i = 0;
            while (visible == false || i < 2)
            {
                try
                {
                    Browser.WaitUntil(lsFale.Exists());
                    visible = true;
                }
                catch 
                {
                    buttonUpdateEditPanel.Last().Click(false);
                    Browser.WaitHideElement(By.XPath(waitload));
                    visible = false;
                    i++;
                }
            }
            ls = lsFale.Text;
            date = datepay.Text;
            sum = summa.Text;
            Pages.Menu.RegLS();
            Browser.WaitHideElement(By.XPath(waitload));
            searchLS.Text = ls;
            searchLS.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitload));
            openLS.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
        }

        public void DisplayedLoadedPayment()
        {
            payment.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(sort))).Build().Perform();
            clickSort.Click(false);
            sortdesc.Last().Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            try
            {
                paymentdate = papaymentDate.Text;
                result = paymentdate.CompareTo(date);
                sumpay = sumpayment.Text;
                resultsum = sumpay.CompareTo(sum);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Оплата не подтвердилась или не прошла");
            }
        }
    }
}