using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using Keys = OpenQA.Selenium.Keys;

namespace Autotests.WebPages.Root
{
    public class Bank_Statement : PageBase
    {
        #region Data
        int count = 0;
        public int result;
        string param, firstdate, enddate,docnumber, summa, accnumber, home;
        static string element = "7451990794_40703810809280004826_287_y97_ufo" + "." + "zip";
        string loaddate = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка данных']";
        string waitdistribution = "//div[starts-with(@id,'loadmask')]/div[text()='Распределение']";
        string waitLoad = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        string waitpls = "//div[starts-with(@id,'loadmask')]/div[text()='Пожалуйста подождите...']";
        string waitUpdate = "//div[starts-with(@id,'loadmask')]/div[contains(text(),'Обновление')]";
        private WebElement AccountNum { get { return new WebElement().ByXPath(string.Format("//input[@name='RecipientAccountNum']")); } }
        private WebElement address { get { return new WebElement().ByXPath(string.Format("//label[text()='Адрес:']/../../td[2]/input")); } }
        private WebElement ApplySpread { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'distributionobjectseditwindow')]//span[text()='Применить распределение']")); } }
        private WebElement billPay { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjNavigationPanel')]/div[starts-with(@id,'menutreepanel')]//div[text()='Счет оплат']")); } }
        private WebElement buttonAdd { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//span[text()='Добавить']")); } }
        private WebElement buttonContinue { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementdistrselectwin')]//span[text()='Продолжить']")); } }
        private WebElement buttonContinueChooseHouse { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realtyaccdistribpanel')]//span[text()='Продолжить']")); } }
        private WebElement buttonContinueTickLS { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'suspaccpersaccdistribpanel')]//span[text()='Продолжить']")); } }
        private WebElement buttonOk { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='OK']")); } }
        private WebElement buttonSave { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//button[@data-qtip='Сохранить']/span[text()='Сохранить']")); } }
        private WebElement buttonSaveStatement { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementaddwin')]//button[@data-qtip='Сохранить']/span[text()='Сохранить']")); } }
        private WebElement buttonSpread { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'distributionobjectseditwindow')]//span[text()='Распределить']")); } }
        private WebElement buttonUpdate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//button[@data-qtip='Обновить']/span[text()='Обновить']")); } }
        private WebElement buttonUpdateAccPay { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjGrid')]//button[@data-qtip='Обновить']/span[text()='Обновить']")); } }
        private WebElement buttonYes { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='Да']")); } }
        private WebElement check { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//tr[2]/td[1]/div/div")); } }
        private WebElement checkResidentialHouse { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realtyaccdistribpanel')]//tr[2]/td[1]/div/div")); } }
        private WebElement checkTickLs { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'suspaccpersaccdistribpanel')]//tr[2]/td[4]/div[contains(text(),'{0}')]/../../../tr[2]/td[1]/div/div", param)); } }
        private WebElement choose { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realtyaccdistribpanel')]//tr[2]/td[1]/div/div")); } }
        private WebElement date { get { return new WebElement().ByXPath(string.Format("//div/div[starts-with(@id,'taskentrygrid')]//div[4]/table/tbody/tr/td/table[starts-with(@id,'datefield')]/tbody/tr/td/div")); } }
        private WebElement dateInput { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'papaymentgrid')]//td[1]/label[text()='Дата операции:']/../../td[2]/table/tbody/tr/td/input")); } }
        private WebElement dateLs { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'papaymentgrid')]//td[1]/label[text()='Дата операции:']/../../td[2]/table/tbody/tr/td/div")); } }
        private WebElement dateReceipt { get { return new WebElement().ByXPath(string.Format("//input[@name='DateReceipt']")); } }
        private WebElement docDate { get { return new WebElement().ByXPath(string.Format("//input[@name='DocumentDate']")); } }
        private WebElement docNum { get { return new WebElement().ByXPath(string.Format("//input[@name='DocumentNum']")); } }
        private WebElement edit { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjGrid')]//tr[2]/td[1]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement equiteMKD { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Средства собственников помещений в МКД']")); } }
        private WebElement equitePaymentOfRent { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Поступление оплат аренды']")); } }
        private WebElement import { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//button[starts-with(@id,'gkhbuttonimport')]/span[text()='Импорт']")); } }
        private WebElement importBanking { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]//a[starts-with(@id,'menuitem')]/span[text()='Импорт банковских выписок']")); } }
        private WebElement inputLs { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'gridcolumn')][3]//tr/td/input")); } }
        private WebElement messageSuccess { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='Успешная загрузка']")); } }
        private WebElement name { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'taskentrygrid')]//div[starts-with(@id,'gridcolumn')][3]/div[starts-with(@id,'container')][2]/table/tbody/tr/td/input")); } }
        private WebElement numDoc { get { return new WebElement().ByXPath(string.Format("//label[text()='Р/с получателя:']/../../td[2]/input")); } }
        private WebElement openLs { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[2]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement payment { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//span[text()='Оплаты']")); } }
        private WebElement residentialBuilding { get { return new WebElement().ByXPath(string.Format("//label[text()='Жилой дом:']/../../td[2]/input")); } }
        private WebElement select { get { return new WebElement().ByXPath(string.Format("//input[@name='DistributionType']/../../td[2]/div")); } }
        private WebElement spread { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//span[text()='Распределить']")); } }
        private WebElement sum { get { return new WebElement().ByXPath(string.Format("//input[@name='Sum']")); } }
        private WebElement today { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'datepicker')]/div/em/button/span[text()='Сегодня']")); } }
        private WebElement upload { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//div[starts-with(@id,'ext-comp')]/table/tbody/tr/td/table/tbody/tr/td[2]/div")); } }
        private WebElement wait { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'distributionobjectseditwindow')]//tr[2]/td[1]/div")); } }
        private WebElement waitaddress { get { return new WebElement().ByXPath(String.Format("//div[starts-with(@id,'suspaccpersaccdistribpanel')]//tr[9]/td[4]/div[contains(text(),'{0}')]", param)); } }
        private WebElement waitelement { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'b4grid')][2]/div/div/table/tbody/tr[2]/td[1]/div")); } }
        private WebElement waitLs { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[4]/div[contains(text(),'{0}')]", param)); } }
        private WebElement waitoperation { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'rbankstatementgrid')]//tr[2]/td[3]/div")); } }
        private WebElement waitTickLS { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'distributionobjectseditwindow')]//tr[2]/td[2]/div")); } }
        public WebElement debtTotal { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realtypaymentaccpanel')]//input[@name='DebtTotal']")); } }
        public WebElement messageSuccessSave { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Успешно сохранено!']")); } }
        public WebElement summaLs { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//div[starts-with(@id,'papaymentgrid')]//tr[2]/td[5]/div")); } }
        public WebElement waitMessageSuccess { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'taskentrygrid')]//tr[3]//td[5]/div[text()='Импорт банковских выписок']")); } }
        #endregion

        #region DataSort
        string view = "//span[text()='Р/с получателя']";
        string viewdate = "//div[starts-with(@id,'personalaccounteditpanel')]//span[text()='Дата операции']";
        string viewId = "//span[text()='Внешний Id']";
        private WebElement sortDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//span[text()='Дата операции']/../div")); } }
        private WebElement sortdesc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]//a[starts-with(@id,'menuitem')]/span[text()='Сортировать по убыванию']")); } }
        private WebElement sortValidate { get { return new WebElement().ByXPath(string.Format("//span[text()='Р/с получателя']/../div")); } }
        private WebElement sortValueId { get { return new WebElement().ByXPath(string.Format("//span[text()='Внешний Id']/../div")); } }
        #endregion

        public void ClickImportBankStatement()
        {
            Browser.WaitUntil(waitoperation.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            importClick:
            import.Click(false);
            try
            {
                importBanking.Click(false);
            }
            catch
            {
                goto importClick;
            }
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            upload.Click(false);
            Thread.Sleep(2000);
            Thread.Sleep(2000);
            System.Windows.Forms.SendKeys.SendWait(element);
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

        public void TaskAfterImport(string importbank)
        {
            this.param = importbank;
            Browser.WaitHideElement(By.XPath(waitLoad));
            name.Text = param;
            name.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitLoad));
            date.Click(false);
            today.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
        }

        public void ClickAdd()
        {
            Browser.WaitUntil(waitoperation.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            buttonAdd.Click(false);
        }

        public void FillRequiredFields(string datebegin, string dateend, string numdoc, string summ, string accnum)
        {
            this.firstdate = datebegin;
            this.enddate = dateend;
            this.docnumber = numdoc;
            this.summa = summ;
            this.accnumber = accnum;
            docDate.Text = firstdate;
            dateReceipt.Text = enddate;
            docNum.Text = docnumber;
            sum.Text = summa;
            AccountNum.Text = accnumber;
            buttonSaveStatement.Click(false);
        }

        public void Spread(string accnum)
        {
            this.accnumber = accnum;
            numDoc.Text = accnumber;
            buttonUpdate.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(view))).Build().Perform();
            sortValidate.Click(false);
            sortdesc.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            check.Click(false);
            spread.Click(false);
            select.Click(false);
            equiteMKD.Click(false);
            buttonContinue.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
        }

        public void SpreadReceiptOfPaymentOfRent(string accnum)
        {
            this.accnumber = accnum;
            numDoc.Text = accnumber;
            buttonUpdate.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(view))).Build().Perform();
            sortValidate.Click(false);
            sortdesc.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            check.Click(false);
            spread.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            select.Click(false);
            equitePaymentOfRent.Click(false);
            buttonContinue.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
        }

        public void ChooseHouse(string house)
        {
            this.home = house;
            residentialBuilding.Text = home;
            residentialBuilding.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            choose.Click(false);
        }

        public void ChooseAddress(string house)
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            this.param = house;
            address.Text = param;
            address.SendKeys(Keys.Enter);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitUntil(waitaddress.Enabled);
            bool waitelem;
            do
            {
                try
                {
                    checkTickLs.Click(false);
                    waitelem = waitelement.Displayed;
                }
                catch
                {
                    waitelem = false;
                }
                
            } while (waitelem == false);
        }

        public void ClickButtons()
        {
            buttonContinueChooseHouse.Click(false);
            Browser.WaitHideElement(By.XPath(waitdistribution));
            Browser.WaitUntil(wait.Exists());
            buttonSpread.Click(false);
            Browser.WaitHideElement(By.XPath(waitdistribution));
            ApplySpread.Click(false);
            buttonOk.Click(false);
        }

        public void ClickButton()
        {
            buttonContinueTickLS.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitdistribution));
            Browser.WaitUntil(waitTickLS.Exists());
            buttonSpread.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitUpdate));
            Browser.WaitHideElement(By.XPath(waitdistribution));
            ApplySpread.Click(false);
            Browser.WaitHideElement(By.XPath(waitdistribution));
            buttonOk.Click(false);
        }

        public void AccountPayment(string house)
        {
            this.home = house;
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            address.Last().Text = home;
            address.Last().SendKeys(Keys.Enter);
            buttonUpdateAccPay.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(viewId))).Build().Perform();
            sortValueId.Click(false);
            sortdesc.Last().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitLoad));
            edit.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitLoad));
            billPay.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void SelectTickLS(string house)
        {
            this.home = house;
            Browser.WaitHideElement(By.XPath(waitLoad));
            residentialBuilding.Last().Text = home;
            residentialBuilding.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitLoad));
            checkResidentialHouse.Click(false);
            buttonContinueChooseHouse.Click(false);
            Browser.WaitHideElement(By.XPath(waitdistribution));
            Browser.WaitUntil(waitTickLS.Exists());
        }

        public void DestributeApplyDistribution()
        {
            buttonSpread.Click(false);
            Browser.WaitHideElement(By.XPath(waitUpdate));
            ApplySpread.Click(false);
            Browser.WaitHideElement(By.XPath(waitdistribution));
            buttonOk.Click(false);
        }

        public void GotoRegLs(string house)
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitLoad));
            openLs.WaitIsClickable();
            this.param = house;
            inputLs.Text = param;
            inputLs.SendKeys(Keys.Enter);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitUntil(waitLs.Displayed);
            openLs.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitHideElement(By.XPath(waitpls));
            payment.WaitIsClickable();
            payment.Click(false);
            Browser.WaitHideElement(By.XPath(waitLoad));
            dateLs.Click(false);
            today.Click(false);
            dateInput.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitLoad));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(viewdate))).Build().Perform();
            sortDate.Click(false);
            sortdesc.Last().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitLoad));
            try
            {
                summa = summaLs.Text;
                result = summa.CompareTo("1000,00");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Записей нет, оплата не распределилась");
            }
        }
    }
}